using System;
using System.Collections.Generic;
using System.Text;

namespace Kyameru.Component.Ftp.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrEmptyPath(this string input)
        {
            if (input == "/" || string.IsNullOrWhiteSpace(input))
            {
                return true;
            }

            return false;
        }
    }
}