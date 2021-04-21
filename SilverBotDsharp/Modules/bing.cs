using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using LiteDB;
using SilverBotDsharp.Modules.infoclasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SilverBotDsharp.Modules
{
    internal class Bing : BaseCommandModule
    {
        public class Person
        {
            [BsonId]
            public ulong UserId { get; set; }

            public ulong ServerId { get; set; }
            public ulong Bingtimes { get; set; }
        }

        public class Guildthingy
        {
            [BsonId]
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
        private static async Task SendbingAsync([Description("the channel to send to")] DiscordChannel ch)
        {
            var conf = Program.GetConfig();
            DiscordMessage e = await ch.SendMessageAsync(conf.Message);
            await e.CreateReactionAsync(ReactionEmote);
            var react = await interactivity.WaitForReactionAsync(x => x.Emoji == ReactionEmote && x.Message == e && !x.User.IsBot, Config.SBTimespanToTimeSpan(conf.Timespan));
            if (!react.TimedOut)
            {
                using var db = new LiteDatabase(@"Filename=bingers.db; Connection=shared");
                var col = db.GetCollection<Person>();
                col.EnsureIndex(x => x.ServerId);
                var thing = col.FindOne(x => x.ServerId == ch.GuildId && x.UserId == react.Result.User.Id);
                if (thing != null)
                {
                    thing.Bingtimes++;
                    var list = BingList.BingTexts.Where(b => (b.day == DateTime.Today.Day || b.day == null) && (b.month == DateTime.Today.Month || b.month == null) && (b.year == DateTime.Today.Year || b.year == null) && (b.hour == DateTime.Now.Hour || b.hour == null) && (b.minute == DateTime.Now.Minute || b.minute == null) && (b.number_of_bings_of_user == thing.Bingtimes || b.number_of_bings_of_user == null) && (b.day_of_week == (int)DateTime.Today.DayOfWeek || b.day_of_week == null)).ToArray();
                    StringBuilder message = new();
                    foreach (var o in list)
                    {
                        message.Append($"{string.Format(o.text, react.Result.User.Mention, ((DiscordMember)react.Result.User).Nickname)}{Environment.NewLine}");
                    }
                    await ch.SendMessageAsync(message.ToString());
                    if (react.Result.User.Id == 687387957296103541 && DateTime.Today.Day == 8 && DateTime.Today.Month == 3)
                    {
                        await ch.SendMessageAsync($"{react.Result.User.Mention} happy birthday Wbbubler https://cdn.discordapp.com/attachments/728360861483401240/781827459867344916/cooltext369813039532598.png");
                    }
                    col.Update(thing);
                }
                else
                {
                    col.Insert(new Person
                    {
                        Bingtimes = 1,
                        ServerId = ch.Guild.Id,
                        UserId = react.Result.User.Id
                    });
                    await ch.SendMessageAsync($"Its the first time {react.Result.User.Mention} reacted to the Microsoft bing first! Everyone congratulate them and give em a pat on the back for making such an advancement!");
                }
            }
        }

        [Command("setupbing")]
        [Description("set up sie bing boi")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        [RequireGuild]
        public async Task Setupbing(CommandContext ctx, DiscordChannel shit)
        {
            if (shit.GuildId == ctx.Guild.Id)
            {
                using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
                db.GetCollection<Guildthingy>().Insert(new Guildthingy
                {
                    ChannelId = shit.Id,
                    ServerId = shit.GuildId
                });
                Channels.Add(shit);
                _ = SendbingAsync(shit);
            }
        }

        [Command("removebing")]
        [Description("DONT remove up sie bing boi")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        [RequireGuild]
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
                    col.Delete(thing.ChannelId);
                    Channels.RemoveAll(x => x.Id == thing.ChannelId);
                    await ctx.RespondAsync($"well we deleted {shit.Id}");
                }
            }
        }

        [Command("fakebing")]
        [Description("makes test bing")]
        [RequireGuild]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task Fakebing(CommandContext ctx)
        {
            using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
            var col = db.GetCollection<Guildthingy>();
            foreach (var thing in col.Find(x => x.ServerId == ctx.Guild.Id))
            {
                _ = SendbingAsync(await ctx.Client.GetChannelAsync(thing.ChannelId));
            }
        }

        [Command("bingcount")]
        [Description("tells you how many bings exist (1)")]
        [RequireGuild]
        public async Task Bingcount(CommandContext ctx)
        {
            DiscordEmbedBuilder bob = new();
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
            DiscordEmbedBuilder bob = new();
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
        public async Task Git(CommandContext ctx)
        {
            await ctx.RespondAsync("https://github.com/thesilvercraft/SilverBing");
        }

        [Command("bingsreload")]
        [Description("reload the config for bings")]
        [RequireOwner]
        public async Task Reload(CommandContext ctx)
        {
            BingList.LoadConfig();
            DiscordEmbedBuilder bob = new();
            bob.WithTitle("Reloaded binglist for ya.");
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync(embed: bob.Build());
        }

        [Command("splashesreload")]
        [Description("reload the config for splashes")]
        [RequireOwner]
        public async Task ReloadSplashes(CommandContext ctx)
        {
            Splashes.Get(true);
            DiscordEmbedBuilder bob = new();
            bob.WithTitle("Reloaded splashes for ya.");
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync(embed: bob.Build());
        }

        [Command("invite")]
        [Description("invite me to your server")]
        public async Task invite(CommandContext ctx)
        {
            DiscordEmbedBuilder bob = new();
            bob.WithTitle("Invite link:");
            bob.WithDescription(string.Format(Program.GetConfig().Invite, ctx.Client.CurrentUser.Id));
            bob.WithFooter("Requested by " + ctx.User.Username, ctx.User.GetAvatarUrl(ImageFormat.Png));
            await ctx.RespondAsync(embed: bob.Build());
        }

        [Command("leaderbing")]
        [Description("Shows you the leaderbing of this fine establishment(guild)")]
        [RequireGuild]
        public async Task LeaderBing(CommandContext ctx)
        {
            DiscordEmbedBuilder bob = new();
            StringBuilder stringBuilder = new();
            List<Page> pages = new();
            bob.WithTitle("Microsoft bing caught in this server:");
            bob.WithFooter($"Requested by {ctx.User.Username}", ctx.User.GetAvatarUrl(ImageFormat.Png));
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
                            stringBuilder.Append($"<@!{person.UserId}> reacted to the bing first {person.Bingtimes} times{Environment.NewLine}");
                        }
                    }
                    bob.WithDescription(stringBuilder.ToString());
                    pages.Add(new Page(embed: bob));
                }
            }
            for (int a = 0; a < pages.Count; a++)
            {
                var embedbuilder = new DiscordEmbedBuilder(pages[a].Embed);
                embedbuilder.WithAuthor("Page " + (a + 1) + "/" + pages.Count);
                pages[a].Embed = embedbuilder.Build();
            }
            await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.User, pages);
        }

        private static readonly List<DiscordChannel> Channels = new();
        private static DiscordEmoji ReactionEmote;
        private static InteractivityExtension interactivity;

        public static async Task Sbing(DiscordClient e)
        {
            using (var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared"))
            {
                var col = db.GetCollection<Guildthingy>();

                var findall = col.FindAll().ToArray();
                try
                {
                    Console.WriteLine("Trying to find all channels");
                    foreach (var thing in findall)
                    {
                        try
                        {
                            Console.WriteLine($"Trying to find channel by id: {thing.ChannelId}");
                            var chan = await e.GetChannelAsync(thing.ChannelId);
                            Console.WriteLine($"Found {chan.Name}");
                            Channels.Add(chan);
                        }
                        catch (NotFoundException)
                        {
                            Debug.WriteLine("Channel not found :(");
                            Todel.Add(thing.ChannelId);
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("fuck");
                    throw;
                }
            }
            Timer tim = new();
            var conf = Program.GetConfig();
            ReactionEmote = DiscordEmoji.FromName(e, conf.Emote);
            interactivity = e.GetInteractivity();
            tim.Interval = Config.SBTimespanToTimeSpan(conf.Timespan).TotalMilliseconds;
            tim.Elapsed += Tim_ElapsedAsync;
            tim.Start();
        }

        internal static List<ulong> Todel { get; set; } = new();

        private static void Remove_bad_things()
        {
            using var db = new LiteDatabase(@"Filename=bingloc.db; Connection=shared");
            var col = db.GetCollection<Guildthingy>();

            foreach (var fuck in Todel.ToList())
            {
                Console.WriteLine($"Deleting {fuck}");
                if (col.Delete(fuck))
                {
                    Channels.RemoveAll(x => x.Id == fuck);
                    Debug.Write($"Deleted {fuck}");
                }
            }
            Todel.Clear();
        }

        private static async void Tim_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("Sending bing");
                foreach (var thing in Channels)
                {
                    try
                    {
                        Console.WriteLine($"Sending bing in {thing.Name}");
                        _ = SendbingAsync(thing);
                    }
                    catch (NotFoundException)
                    {
                        Debug.WriteLine("Channel not found :(");
                        Todel.Add(thing.Id);
                    }
                    catch (UnauthorizedException)
                    {
                        Debug.WriteLine("I HAS NO PERMS FUCK");
                        Todel.Add(thing.Id);
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("fuck");
                throw;
            }

            Remove_bad_things();
        }
    }
}