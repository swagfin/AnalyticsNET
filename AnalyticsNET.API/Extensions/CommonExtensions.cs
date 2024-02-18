using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

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

        public static string ToMD5String(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
        public static bool IsValidated(this ModelStateDictionary modelState, out string errorMessage)
        {
            errorMessage = string.Join(Environment.NewLine, modelState?.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return modelState?.IsValid ?? true;
        }
        public static void ValidateOrThrow(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValidated(out string validationErrors))
                throw new Exception(validationErrors);
        }

        public static string FormatToUrlStyle(this string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9]+", "-").Trim('-').ToLower().Trim();
        }
        public static string UrlEncoded(this string value)
        {
            return HttpUtility.UrlEncode(value ?? string.Empty, Encoding.UTF8).Trim();
        }

        //** stu encrypt
        public static string StuEncrypt(this string text, int k)
        {
            var response = string.Empty;
            int v;
            string vString;
            for (int loop = 0; loop < text.Length; loop++)
            {
                v = text.ToCharArray()[loop];
                v = v ^ k;
                vString = Convert.ToString(v, 16);
                vString = Padit(vString);
                response += vString;
            }
            response = response.ToUpper();
            return response;
        }

        public static string StuDecrypt(this string cipher, int k)
        {
            string response = string.Empty;
            string tmp;
            string tmp1;
            string v;
            int vInt;
            for (int loop = 0; loop < cipher.Length; loop += 2)
            {
                tmp = cipher[loop].ToString();
                tmp1 = cipher[loop + 1].ToString();
                v = tmp + tmp1;
                vInt = Convert.ToInt32(v, 16);
                vInt = vInt ^ k;
                v = Convert.ToChar(vInt).ToString();
                response += v;
            }
            return (response);
        }
        private static string Padit(string x)
        {
            string tmp = x;
            while (tmp.Length < 2)
            {
                tmp = '0' + x;
            }
            return tmp;
        }
    }
}
