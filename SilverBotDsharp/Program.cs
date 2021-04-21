using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity.Extensions;
using SilverBotDsharp.Modules;
using SilverBotDsharp.Modules.infoclasses;
using System;
using System.Threading.Tasks;

namespace SilverBotDsharp
{
    internal static class Program
    {
        private static Config config = new();

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
            DiscordClient discord = new(new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information,
            });
            string[] v = { config.Prefix };
            CommandsNextExtension commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                EnableMentionPrefix = true,
                StringPrefixes = v,
            });
            commands.RegisterCommands<Bing>();
            await discord.ConnectAsync();
            discord.UseInteractivity();
            Console.WriteLine("Logged in as " + discord.CurrentUser.Username);
            await Task.Delay(4000);
            BingList.LoadConfig();
            await Task.Delay(4000);
            await Bing.Sbing(discord);
            while (true)
            {
                await discord.UpdateStatusAsync(Splashes.GetSingle());
                await Task.Delay((int)Config.SBTimespanToTimeSpan(config.SplashDelay).TotalMilliseconds);
            }
        }
    }
}