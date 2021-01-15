using SilverBotDsharp.Modules.infoclasses;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using SilverBotDsharp.Modules;
using System;
using System.Threading.Tasks;
using System.Reflection;
using DSharpPlus.Entities;
using System.Diagnostics;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using LiteDB;
using Sentry;

namespace SilverBotDsharp
{
    internal class Program
    {
        private static DiscordClient discord;
        private static CommandsNextExtension commands;
        private static Config config = new Config();
        private static InteractivityExtension interactivity;

        public static InteractivityExtension Interactivity { get => interactivity; set => interactivity = value; }

        private static void Main()
        {
            config = Config.Get();
            MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static Config GetConfig()
        {
            return config;
        }

        private static async Task MainAsync()
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information,
            });
            string[] v = { config.Prefix };
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                EnableMentionPrefix = true,
                StringPrefixes = v,
            });
            commands.RegisterCommands<Bing>();
            await discord.ConnectAsync();
            await Task.Delay(2000);
            DiscordActivity activity = new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = "Loading statuses",
            };
            await discord.UpdateStatusAsync(activity);
            Interactivity = discord.UseInteractivity();
            Console.WriteLine("Logged in as " + discord.CurrentUser.Username);
            binglist.load_config();
            await Bing.Sbing(discord);
            while (true)
            {
                await discord.UpdateStatusAsync(Splashes.GetSingle());
            }
            await Task.Delay(-1);
        }
    }
}