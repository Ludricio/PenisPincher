using System;
using System.Collections.Generic;
using System.Text;

namespace PenisPincher.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        public static bool IsNullOrWhitespace(this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        public static void ThrowIfNullOrEmpty(this string @this, string paramName)
        {
            @this.ThrowIfNull(paramName);
            if (@this.IsNullOrEmpty()) throw new ArgumentException("Argument cannot be empty", paramName);
        }

        public static void ThrowIfNullOrWhitespace(this string @this, string paramName)
        {
            @this.ThrowIfNull(paramName);
            if (@this.IsNullOrWhitespace())
                throw new ArgumentException("Argument cannot consist only of whitespace", paramName);
        }

        public static bool EqualsIgnoreCase(this string @this, string other)
        {
            return @this.Equals(other, StringComparison.OrdinalIgnoreCase);
        }
    }
}
