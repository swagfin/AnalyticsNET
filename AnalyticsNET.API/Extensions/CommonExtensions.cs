using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;

namespace AnalyticsNET.API
{
    public static class CommonExtensions
    {
        public static string GetClientIpAddress(this HttpRequest httpRequest)
        {
            if (httpRequest.Headers.ContainsKey("X-Forwarded-For") && IsValidIpAddress(httpRequest.Headers["X-Forwarded-For"]))
                return httpRequest.Headers["X-Forwarded-For"];
            return httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        private static bool IsValidIpAddress(string ipString)
        {
            try
            {
                if (string.IsNullOrEmpty(ipString)) return false;
                if (ipString.Count(c => c == '.') != 3) return false;
                return IPAddress.TryParse(ipString, out IPAddress _);
            }
            catch { return false; }
        }
    }
}
