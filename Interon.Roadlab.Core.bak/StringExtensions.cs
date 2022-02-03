using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
