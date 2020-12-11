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

        public timespan Timespan { get; set; } = new timespan();

        public static TimeSpan timeSpan(timespan timespan)
        {
            return new TimeSpan(timespan.Days, timespan.Hours, timespan.Minutes, timespan.Secounds, timespan.MiliSecounds);
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
                    var options = new JsonSerializerOptions();
                    options.WriteIndented = true;

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
        public int Secounds { get; set; } = 0;
        public int MiliSecounds { get; set; } = 0;
    }
}