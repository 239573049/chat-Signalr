using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace Chat.Uitl.Util
{
    public class EnumExtensionUtil
    {
        public static T GetEnumVal<T>(string enumValue)
        {
            var enumType = typeof(T);
            var fields = enumType.GetFields().ToList();
            foreach (var field in fields)
            {
                var fieldValue = field.CustomAttributes.FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value;
                if (fieldValue == null) continue;
                var enumStringValue = fieldValue.ToString();
                if (enumStringValue== enumValue)
                {
                    return (T)Enum.Parse(typeof(T), field.GetValue(null)?.ToString() ?? string.Empty);
                }
            }
            return default;
        }
        public static string GetEnumStringVal(Enum enumValue)
        {
            try
            {
                return enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .FirstOrDefault()?
                    .GetCustomAttribute<DescriptionAttribute>()?
                    .Description;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<int, string> GetEnumKeyValue<T>()
        {
            var enumType = typeof(T);
            var fields = enumType.GetFields().ToList();
            var dictionary = new Dictionary<int, string>();
            foreach (var field in fields)
            {
                var fieldValue = field.CustomAttributes.FirstOrDefault()?.ConstructorArguments.FirstOrDefault().Value;
                if (fieldValue == null) continue;
                var enumStringValue = fieldValue.ToString();
                dictionary.TryAdd((int)field.GetValue(null), enumStringValue);
            }
            return dictionary;
        }
    }
}
