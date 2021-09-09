using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnalyticsNET
{
    internal static class CoreLib
    {
        public static async Task<string> RequestAsync(string url, HttpMethod httpMethod, Dictionary<string, string> parameters = null, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            request.Content = new FormUrlEncodedContent(parameters);
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            //Request Headers
            if (headers != null)
                foreach (var item in headers)
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
            //End of Headers
            var response = await client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }


}
