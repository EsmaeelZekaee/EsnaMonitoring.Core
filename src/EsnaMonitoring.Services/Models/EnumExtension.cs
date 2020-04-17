namespace EsnaMonitoring.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtension
    {
        public static string GetEnumDisplay<T>(object value)
            where T : struct, Enum
        {
            var type = typeof(T);
            return type.GetField(type.GetEnumName(value)).GetCustomAttribute<DisplayAttribute>().Name;
        }

        public static IEnumerable<T> GetValues<T>(Type @enum)
            where T : struct, Enum
        {
            return Enum.GetNames(@enum).Select(Enum.Parse<T>);
        }
    }
}