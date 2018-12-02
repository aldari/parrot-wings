using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PW.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Возвращает атрибут если он есть, иначе null
        /// </summary>
        /// <typeparam name="TAttribute">тип атрибута</typeparam>
        /// <param name="enumValue">значение перечисления</param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Получает значение поля Description атрибута DisplayAttribute
        /// </summary>
        /// <param name="enumValue">значение перечисления</param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>()?.Description ?? string.Empty;
        }

        /// <summary>
        /// Получает значение поля Name атрибута DisplayAttribute
        /// </summary>
        /// <param name="enumValue">значение перечисления</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetAttribute<DisplayAttribute>()?.Name ?? string.Empty;
        }

        public static List<EnumValue> GetValues<T>()
        {
            List<EnumValue> values = new List<EnumValue>();
            foreach (var itemType in Enum.GetValues(typeof(T)))
            {
                //For each value of this enumeration, add a new EnumValue instance
                values.Add(new EnumValue()
                {
                    Name = Enum.GetName(typeof(T), itemType),
                    Value = (int)itemType
                });
            }
            return values;
        }
    }

    public class EnumValue
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}