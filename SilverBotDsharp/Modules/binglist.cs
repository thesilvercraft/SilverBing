using System;
using System.IO;
using System.Text.Json;

namespace SilverBotDsharp.Modules
{
    internal static class BingList
    {
        public static BingText[] BingTexts { get; set; } = { new BingText() };

        public static void LoadConfig()
        {
            Console.WriteLine("Loading bing texts");
            if (File.Exists("BINGSYEAH.json"))
            {
                using StreamReader Filejosn = new("BINGSYEAH.json");
                BingTexts = JsonSerializer.Deserialize<BingText[]>(Filejosn.ReadToEnd());
            }
            else
            {
                using StreamWriter writer = new("BINGSYEAH.json");
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                writer.Write(JsonSerializer.Serialize(BingTexts, options));
            }
        }
    }

    public class BingText
    {
        public string text { get; set; } = "{0} reacted to the Microsoft bing first.";
        public int? day { get; set; } = null;
        public int? month { get; set; } = null;
        public int? year { get; set; } = null;
        public int? hour { get; set; } = null;
        public int? minute { get; set; } = null;
        public int? day_of_week { get; set; } = null;
        public ulong? number_of_bings_of_user { get; set; } = null;
    }
}