using System;
using System.Security.Cryptography;
using System.Text;

namespace AnalyticsNET.API.Entity
{
    public static class StringExtensions
    {
        public static string ToMD5String(this string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
        public static string ToSHA256String(this string datastring, string hashkey)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] keyByte = encoding.GetBytes(hashkey);
                HMACSHA256 hmacsha1 = new HMACSHA256(keyByte);
                byte[] messageBytes = encoding.GetBytes(datastring);
                byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
                string final_hash = string.Empty;
                for (int i = 0; i < hashmessage.Length; i++)
                {
                    final_hash += hashmessage[i].ToString("X2"); // hex format
                }
                return final_hash.ToLower();
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
    }
}
