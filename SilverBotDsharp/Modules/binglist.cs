using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SilverBotDsharp.Modules
{
    internal class binglist
    {
        public static bingtext[] bingtexts { get; set; } = { new bingtext() };

        public static void load_config()
        {
            Console.WriteLine("lodain config");
            if (File.Exists("BINGSYEAH.json"))
            {
                using (StreamReader Filejosn = new StreamReader("BINGSYEAH.json"))
                {
                    bingtexts = JsonSerializer.Deserialize<bingtext[]>(Filejosn.ReadToEnd());
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter("BINGSYEAH.json"))
                {
                    var options = new JsonSerializerOptions();
                    options.WriteIndented = true;
                    writer.Write(JsonSerializer.Serialize(bingtexts, options));
                }
            }
        }
    }

    public class bingtext
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