using DSharpPlus.Entities;
using System.IO;
using System.Text.Json;

namespace SilverBotDsharp.Modules
{
    internal static class Splashes
    {
        private static DiscordActivity[] Internal { get; } = {
        new ("Bingers", ActivityType.Watching),
        };

        private static DiscordActivity[] Cache;

        public static DiscordActivity[] Get(bool ignorecache = false)
        {
            if (ignorecache || Cache == null)
            {
                if (File.Exists("splashes.json"))
                {
                    using StreamReader reader = new("splashes.json");
                    var arrays = JsonSerializer.Deserialize<DiscordActivity[]>(reader.ReadToEnd());
                    Cache = arrays;
                    return Cache;
                }
                else
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    using StreamWriter writer = new("splashes.json");
                    writer.Write(JsonSerializer.Serialize(Internal, options));
                    return Internal;
                }
            }
            else
            {
                return Cache;
            }
        }

        public static DiscordActivity GetSingle(bool ignorecache = false)
        {
            RandomGenerator rg = new();
            var arr = Get(ignorecache);
            return arr[rg.Next(0, arr.Length)];
        }
    }
}