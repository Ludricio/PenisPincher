using System;
using System.Collections.Generic;
using System.Text;

namespace PenisPincher.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowIfNull<T>(this T @this, string paramName) where T : class
        {
            if (@this == null) throw new ArgumentNullException(paramName);
        }

        public static void ThrowIf<T>(this T @this, Predicate<T> predicate, string paramName) where T : class
        {
            if (predicate(@this))
            {
                throw new ArgumentException();
            }
        }
    }
}
