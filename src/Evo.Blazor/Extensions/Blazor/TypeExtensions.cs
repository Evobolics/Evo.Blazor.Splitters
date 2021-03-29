using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static List<PropertyInfo> GetProperties<TAttributeType>(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(p => p.GetCustomAttributes(typeof(TAttributeType), false).Count() == 1).ToList();
        }
    }
}
