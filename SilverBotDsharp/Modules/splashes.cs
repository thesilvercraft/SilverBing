using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SilverBotDsharp.Modules
{
    internal class Splashes
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