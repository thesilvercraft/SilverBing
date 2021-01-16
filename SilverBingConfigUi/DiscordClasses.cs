using System;
using System.Collections.Generic;
using System.Text;

namespace SilverBingConfigUi
{
    public class Status
    {
        public string Name { get; set; }
        public object StreamUrl { get; set; }
        public int ActivityType { get; set; }
        public object RichPresence { get; set; }
        public object CustomStatus { get; set; }
    }
}