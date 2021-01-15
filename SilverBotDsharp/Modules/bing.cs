using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using LiteDB;
using SilverBotDsharp.Modules.infoclasses;

namespace SilverBotDsharp.Modules
{
    internal class Bing : BaseCommandModule
    {
        public class Person
        {
            public int Id { get; set; }
            public ulong UserId { get; set; }
            public ulong ServerId { get; set; }
            public ulong Bingtimes { get; set; }
        }

        public class Guildthingy
        {
            public int Id { get; set; }
            public ulong ChannelId { get; set; }
            public ulong ServerId { get; set; }
        }

        public static DateTime EasterSunday(int year)
        {
            int g = year % 19;
            int c = year / 100;
            int h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            int i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));

            int day = i - (year + year / 4 + i + 2 - c + c / 4) % 7 + 28;
            int month = 3;
            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Sends a bing i guess
        /// </summary>
        /// <param name="ch">the channel to send a bing to</param>
        /// <param name="cl">the client for interactivity</param>
        private static async Task SendbingAsync([Description("the channel to send to")] DiscordChannel ch, [Description("client")] DiscordClient cl)
        {
            var interactivity = cl.GetInteractivity();
            var conf = Program.GetConfig();
            DiscordMessage e = await ch.SendMessageAsync(conf.Message);
            await e.CreateReactionAsync(DiscordEmoji.FromName(cl, conf.Emote));

            var react = await interactivity.WaitForReactionAsync((x => x.Emoji == DiscordEmoji.FromName(cl, conf.Emote) && x.Message == e && x.User != cl.CurrentUser), Config.timeSpan(conf.Timespan)); ;
            if (!react.TimedOut)
            {
                using var db = new LiteDatabase(@"Filename=bingers.db; Connection=shared");
                var col = db.GetCollection<Person>();
                col.EnsureIndex(x => x.ServerId);
                var thing = col.FindOne(x => x.ServerId == ch.GuildId && x.UserId == react.Result.User.Id);
                if (thing != null)
                {
                    thing.Bingtimes++;
                    var list = binglist.bingtexts.Where(b => (b.day == DateTime.Today.Day || b.day == null) && (b.month == DateTime.Today.Month || b.month == null) && (b.year == DateTime.Today.Year || b.year == null) && (b.hour == DateTime.Now.Hour || b.hour == null) && (b.minute == DateTime.Now.Minute || b.minute == null) && (b.number_of_bings_of_user == thing.Bingtimes || b.number_of_bings_of_user == null) && (b.day_of_week == (int)DateTime.Today.DayOfWeek || b.day_of_week == null)).ToArray();
                    string message = "";
                    foreach (var o in list)
                    {
                        message += String.Format(o.text, react.Result.User.Mention, ((DiscordMember)react.Result.User).Nickname) + Environment.NewLine;
                    }
                    await ch.SendMessageAsync(message);
                    if (react.Result.User.Id == 687387957296103541 && DateTime.Today.Day == 8 && DateTime.Today.Month == 3)
                    {
                        await ch.SendMessageAsync(react.Result.User.Mention + " happy birthday Wbbubler https://cdn.discordapp.com/attachments/728360861483401240/781827459867344916/cooltext369813039532598.png");
                    }

                    try
                    {
                        col.Update(thing);
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    Person person = new Person
                    {
                        Bingtimes = 1,
                        ServerId = ch.Guild.Id,
                        UserId = react.Result.User.Id
                    };
                    col.Insert(person);
                    await ch.SendMessageAsync("Its the first time " + react.Result.User.Mention + " reacted to the Microsoft bing first! Everyone congratulate them and give em a pat on the back for acheving such an advancment!");
                }
            }
        }

        [Command("setupbing")]
        [Description("set up sie bing boi")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        [RequireGuild()]
        public async Task Setupbing(CommandContext ctx, DiscordChannel shit)
        {
            if (shit.GuildId == ctx.Guild.Id)
            {
                using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
                var col = db.GetCollection<Guildthingy>();
                Guildthingy guild = new Guildthingy
                {
                    ChannelId = shit.Id,
                    ServerId = shit.GuildId
                };
                col.Insert(guild);
                Task task = SendbingAsync(shit, cl);
            }
        }

        [Command("removebing")]
        [Description("DONT remove up sie bing boi")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        [RequireGuild()]
        public async Task Removebing(CommandContext ctx, DiscordChannel shit)
        {
            if (shit.GuildId == ctx.Guild.Id)
            {
                using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
                var col = db.GetCollection<Guildthingy>();
                col.EnsureIndex(x => x.ChannelId);
                var thing = col.FindOne(x => x.ServerId == shit.GuildId && x.ChannelId == shit.Id);
                if (thing != null)
                {
                    col.Delete(thing.Id);
                    await ctx.RespondAsync("well bois we deleted " + shit.Id);
                }
            }
        }

        [Command("fakebing")]
        [Description("makes fake bing")]
        [RequireGuild()]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task Fakebing(CommandContext ctx)
        {
            cl = ctx.Client;

            using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
            var col = db.GetCollection<Guildthingy>();
            foreach (var thing in col.Find(x => x.ServerId == ctx.Guild.Id))
            {
                Task bing = SendbingAsync(await ctx.Client.GetChannelAsync(thing.ChannelId), cl);
            }
        }

        [Command("bingcount")]
        [Description("tells you how many bings exist (1)")]
        [RequireGuild()]
        public async Task Bingcount(CommandContext ctx)
        {
            DiscordEmbedBuilder bob = new DiscordEmbedBuilder();
            bob.WithTitle("There is only one bing: MICROSOFT BING");
            bob.WithDescription("But on the topic of microsoft bing you caught: none");
            using (var db = new LiteDatabase(@"Filename=bingers.db; Connection=shared"))
            {
                var col = db.GetCollection<Person>();
                col.EnsureIndex(x => x.ServerId);
                var thing = col.FindOne(x => x.ServerId == ctx.Guild.Id && x.UserId == ctx.User.Id);
                if (thing != null)
                {
                    bob.WithDescription($"But on the topic of microsoft bing you caught: {thing.Bingtimes}");
                }
            }
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync(embed: bob.Build());
        }

        [Command("bingcount")]
        [Description("tells you how many bings exist (1)")]
        [RequireGuild()]
        public async Task Bingcount(CommandContext ctx, DiscordUser a)
        {
            DiscordEmbedBuilder bob = new DiscordEmbedBuilder();
            bob.WithTitle("There is only one bing: MICROSOFT BING");
            bob.WithDescription($"But on the topic of microsoft bing {a.Mention} caught: none");
            using (var db = new LiteDatabase(@"Filename=bingers.db; Connection=shared"))
            {
                var col = db.GetCollection<Person>();
                col.EnsureIndex(x => x.ServerId);
                var thing = col.FindOne(x => x.ServerId == ctx.Guild.Id && x.UserId == a.Id);
                if (thing != null)
                {
                    bob.WithDescription($"But on the topic of microsoft bing {a.Mention} caught: {thing.Bingtimes}");
                }
            }
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync(embed: bob.Build());
        }

        private static readonly IEnumerable<int> range = Enumerable.Range(1900, 2000);

        [Command("git")]
        [Aliases("github", "code")]
        [Description("ehh some nerd shit")]
        public async Task git(CommandContext ctx)
        {
            DiscordEmbedBuilder bob = new DiscordEmbedBuilder();
            bob.WithTitle("thesilvercraft/SilverBing");
            bob.WithUrl("https://github.com/thesilvercraft/SilverBing");
            bob.WithDescription($"the bot we all love the one the only BING BOT. Contribute to thesilvercraft/SilverBing development by creating an account on GitHub.");
            bob.WithAuthor("GitHub");
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync("<https://github.com/thesilvercraft/SilverBing>", embed: bob.Build());
        }

        [Command("bingsreload")]
        [Description("reload the config for bings")]
        [RequireOwner()]
        public async Task reload(CommandContext ctx)
        {
            binglist.load_config();
            DiscordEmbedBuilder bob = new DiscordEmbedBuilder();
            bob.WithTitle("Reloaded binglist for ya.");
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync(embed: bob.Build());
        }

        [Command("leaderbing")]
        [Description("Shows you the leaderbing of this fine establishment(guild)")]
        [RequireGuild()]
        public async Task Leaderbing(CommandContext ctx)
        {
            DiscordEmbedBuilder bob = new DiscordEmbedBuilder();
            List<string> list = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            List<Page> pages = new List<Page>();
            bob.WithTitle("Microsoft bing caught in this server:");
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            using (var db = new LiteDatabase(@"Filename=bingers.db; Connection=shared"))
            {
                var col = db.GetCollection<Person>();
                col.EnsureIndex(x => x.ServerId);

                var thing = col.Find(x => x.ServerId == ctx.Guild.Id).OrderByDescending(x => x.Bingtimes).ToList();
                if (thing != null)
                {
                    foreach (Person person in thing)
                    {
                        if (range.Contains(stringBuilder.Length))
                        {
                            bob.WithDescription(stringBuilder.ToString());

                            pages.Add(new Page(embed: bob));
                            stringBuilder.Clear();
                        }
                        else
                        {
                            stringBuilder.Append("<@!" + person.UserId + "> reacted to the bing first " + person.Bingtimes + " times" + Environment.NewLine);
                        }
                    }
                    bob.WithDescription(stringBuilder.ToString());
                    pages.Add(new Page(embed: bob));
                }
            }
            var interactivity = ctx.Client.GetInteractivity();
            for (int a = 0; a < pages.Count; a++)
            {
                var embedbuilder = new DiscordEmbedBuilder(pages[a].Embed);
                embedbuilder.WithAuthor("page " + (a + 1) + " out of " + (pages.Count + 1));
                pages[a].Embed = embedbuilder.Build();
            }
            await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.User, pages);
        }

        private static Timer tim = new Timer();
        private static DiscordClient cl;

        public static async Task Sbing(DiscordClient e)
        {
            tim = new Timer();
            var conf = Program.GetConfig();
            tim.Interval = Config.timeSpan(conf.Timespan).TotalMilliseconds;//time in milisecounds
            tim.Elapsed += Tim_ElapsedAsync;
            cl = e;
            tim.Start();
        }

        private static List<Guildthingy> todel = new List<Guildthingy>();

        internal static List<Guildthingy> Todel { get => todel; set => todel = value; }

        private static void Remove_bad_things()
        {
            using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
            var col = db.GetCollection<Guildthingy>();

            foreach (var fuck in Todel.ToList())
            {
                Console.WriteLine("Deleting " + fuck.Id);
                if (col.Delete(fuck.Id))
                {
                    Debug.Write("Deleted " + fuck.Id);
                }
            }
            Todel.Clear();
        }

        private static async void Tim_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            using (var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared"))
            {
                var col = db.GetCollection<Guildthingy>();

                var findall = col.FindAll().ToArray();
                try
                {
                    Console.WriteLine("Sending bing");
                    foreach (var thing in findall)
                    {
                        try
                        {
                            Console.WriteLine("Trying to find channel by id: " + thing.ChannelId);
                            var chan = await cl.GetChannelAsync(thing.ChannelId);
                            Console.WriteLine("Found " + chan.Name);
                            Task bing = SendbingAsync(chan, cl);
                        }
                        catch (NotFoundException)
                        {
                            Debug.WriteLine("Channnel not found :(");
                            Todel.Add(thing);
                        }
                        catch (UnauthorizedException)
                        {
                            Debug.WriteLine("I HAS NO PERMS FUCK");
                            Todel.Add(thing);
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("fuck");
                    throw;
                }
            }
            Remove_bad_things();
        }
    }
}