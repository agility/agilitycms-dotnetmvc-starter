﻿using System.Text.RegularExpressions;

namespace Agility.NET5.Starter.Util
{
    public static class Utility
    {
        public static string CleanHtml(string html)
        {
            var pattern = @"href=""~\/";
            var replacement = "href=\"/";
            return Regex.Replace(html, pattern, replacement);
        }
    }
}
