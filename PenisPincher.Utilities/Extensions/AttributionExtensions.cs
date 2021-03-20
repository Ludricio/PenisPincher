using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PenisPincher.Utilities.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAttribute<T>(this ParameterInfo @this) where T : Attribute
        {
            return @this.GetCustomAttribute<T>() != null;
        }

        public static bool HasAttribute<T>(this MemberInfo @this) where T : Attribute
        {
            return @this.GetCustomAttribute<T>() != null;
        }
    }
}
