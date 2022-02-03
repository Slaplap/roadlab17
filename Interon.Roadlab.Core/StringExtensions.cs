using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Interon.Roadlab.Core
{
    public static class StringExtensions
    {
        public static string AsUriPath(this string filePath)
        {
            return WebUtility.UrlEncode(filePath.Replace('\\', '/'));
        }
        public static string AsFilePath(this string uriPath)
        {
            return WebUtility.UrlDecode(uriPath.Replace('/', '\\'));

        }
        public static string URLEncode(this string str)
        {
            return WebUtility.UrlEncode(str);
        }
        public static string URLDecode(this string str)
        {
            return WebUtility.UrlDecode(str);
        }
        public static bool  IsValidEmail(this string strIn)
        {
            if (strIn == null)
            {

                return false;
            }
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        public static bool IsNotEmpty(this string strIn)
        {
            return !string.IsNullOrWhiteSpace(strIn);
        }
        // Convert the string to Pascal case.
        public static string ToPascalCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }
        public static string ToCamelCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null || the_string.Length < 2)
                return the_string;

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }
        public static string ToProperCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Start with the first character.
            string result = the_string.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (int i = 1; i < the_string.Length; i++)
            {
                if (char.IsUpper(the_string[i])) result += " ";
                result += the_string[i];
            }

            return result;
        }
    }
}
