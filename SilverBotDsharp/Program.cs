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

        private static void Main(string[] args)
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
            using (SentrySdk.Init(***REMOVED***))
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
                commands.RegisterCommands<bing>();
                await discord.ConnectAsync();
                await Task.Delay(2000);
                DiscordActivity activity = new DiscordActivity
                {
                    ActivityType = ActivityType.Watching,
                    Name = "Bingers",
                };
                await discord.UpdateStatusAsync(activity);
                interactivity = discord.UseInteractivity();
                Console.WriteLine("Logged in as " + discord.CurrentUser.Username);
                bing.sbing(discord);
                await Task.Delay(-1);
            }
        }
    }
}