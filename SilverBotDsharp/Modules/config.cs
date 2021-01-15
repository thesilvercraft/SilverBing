using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SilverBotDsharp.Modules.infoclasses
{
    public class Config
    {
        public string Prefix { get; set; } = "sb!";
        public string Token { get; set; } = "UR TOKEN HERE";
        public string Invite { get; set; } = "https://discord.com/api/oauth2/authorize?client_id={0}&permissions=2147483639&scope=bot";
        public timespan Timespan { get; set; } = new timespan();

        public string Message { get; set; } = "<:micorsoft_bing_0_0:779071679271010324><:micorsoft_bing_1_0:779071679125127190><:micorsoft_bing_2_0:779071680064126999><:micorsoft_bing_3_0:779071679598034945><:micorsoft_bing_4_0:779071680017858590><:micorsoft_bing_5_0:779071680383156264><:micorsoft_bing_6_0:779071679908937749><:micorsoft_bing_7_0:779071680131629086>" + Environment.NewLine +
"<:micorsoft_bing_0_1:779071679019876393><:micorsoft_bing_1_1:779071679862538313><:micorsoft_bing_2_1:779071679879446628><:micorsoft_bing_3_1:779071679870795847><:micorsoft_bing_4_1:779071680114196480><:micorsoft_bing_5_1:779071680064258079><:micorsoft_bing_6_1:779071680277774336><:micorsoft_bing_7_1:779071680151683083>";

        public string Emote { get; set; } = ":MICROSOFT:";

        public static TimeSpan timeSpan(timespan timespan)
        {
            return new TimeSpan(timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.MilliSeconds);
        }

        public static Config Get()
        {
            try
            {
                using Stream stream = File.OpenRead("bingbt.json");
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                reader.Dispose();
                Config asdf = JsonSerializer.Deserialize<Config>(content);
                return asdf;
            }
            catch (FileNotFoundException e)
            {
                using (StreamWriter streamWriter = new StreamWriter("bingbt.json"))
                {
                    Config config = new Config();
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                    streamWriter.Write(JsonSerializer.Serialize(config, options));
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You should have a bingbt.json file where you store the exe it should open in your prefered text editor, fill it out thanks");
                Process.Start("notepad.exe", Environment.CurrentDirectory + "\\bingbt.json");
                Console.WriteLine("Press any key to continue...");
                Console.WriteLine(e);
                Console.ReadKey();
                System.Environment.Exit(420);
                Config asdf = new Config();
                return asdf;
            }
        }
    }

    public class timespan
    {
        public int Days { get; set; } = 0;
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 5;
        public int Seconds { get; set; } = 0;
        public int MilliSeconds { get; set; } = 0;
    }
}