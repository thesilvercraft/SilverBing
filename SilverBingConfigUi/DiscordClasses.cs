namespace SilverBingConfigUi
{
    public class Status
    {
        public string Name { get; set; }
        public string StreamUrl { get; set; }

        /// <summary>
        ///0 Playing
        ///1 Streaming
        ///2 Listening to
        ///3 Watching
        ///4 Custom
        ///5 Competing
        /// </summary>
        public int ActivityType { get; set; }

        //bots can't play minecraft
        //public object RichPresence { get; set; }
        //bots can't have custom statuses
        //public object CustomStatus { get; set; }
    }
}