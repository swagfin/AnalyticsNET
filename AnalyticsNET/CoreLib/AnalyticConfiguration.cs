using System.Collections.Generic;

namespace AnalyticsNET
{
    internal class AnalyticConfiguration
    {
        public AnalyticConfiguration()
        {
            AdditionalRequestParameters = new Dictionary<string, string>();
            AdditionalRequestHeaders = new Dictionary<string, string>();
            NextCallBackInMilliseconds = 3000;
            EndPointUrl = "https://analytics.crudsofttechnologies.com/";
        }
        public string EndPointUrl { get; internal set; }
        public int NextCallBackInMilliseconds { get; internal set; }
        public Dictionary<string, string> AdditionalRequestParameters { get; set; }
        public Dictionary<string, string> AdditionalRequestHeaders { get; set; }
    }
}
