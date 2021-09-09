using System;

namespace AnalyticsNET
{
    public class Trait
    {
        public Trait()
        {
            Id = $"{Guid.NewGuid().ToString().ToUpper()}-{DateTime.Now.Ticks}";
            SentSuccesfully = false;
            NextSending = DateTime.Now;
            FailedCount = 0;
        }
        public string Id { get; internal set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool SentSuccesfully { get; internal set; }
        public DateTime NextSending { get; internal set; }
        public int FailedCount { get; internal set; }
    }
}
