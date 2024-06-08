using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IntegrationExcelImporter.Common.Utility
{
    public static class EnumHelper
    {
        /// <summary>
        /// Flag Enum을 리스트로 가져옴
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<T> GetFlagEnumToList<T>(this Enum value)
            where T : struct
        {
            List<T> list = new List<T>();

            foreach (T r in Enum.GetValues(typeof(T)))
            {
                if (((int)(object)value & (int)(object)r) > 0)
                    list.Add(r);
            }

            return list;
        }

        /// <summary>
        /// 선언된 Enum값의 Description를 가져옴
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description as string;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 선언된 Enum의 Description에서 Enum값을 가져온다
        /// </summary>
        /// <typeparam name="T">정의된 Enum</typeparam>
        /// <param name="description">Enum Description</param>
        /// <returns>Enum 값</returns>
        public static T GetEnumFromDescription<T>(this string description)
        {
            var type = typeof(T);

            if (type.IsEnum == false)
            {
                throw new ArgumentException();
            }

            FieldInfo[] fields = type.GetFields();

            var field = fields.SelectMany(p => p.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Attribute = a }).Where(a => ((DescriptionAttribute)a.Attribute).Description == description).SingleOrDefault();

            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }


        /// <summary>선언된 Enum값의 DefaultValue를 가져옴</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Value(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DefaultValueAttribute attr = Attribute.GetCustomAttribute(field, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                    if (attr != null && attr.Value != null)
                    {
                        return attr.Value.ToString();
                    }
                }
            }
            return null;
        }

        /// <summary>선언된 Enum값의 Browsable을 가져옴</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetBrowsable(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    BrowsableAttribute attr = Attribute.GetCustomAttribute(field, typeof(BrowsableAttribute)) as BrowsableAttribute;
                    if (attr != null)
                    {
                        return attr.Browsable;
                    }
                }
            }
            return true;
        }

        public static List<string> GetStrings(this Enum value)
        {
            var temp = value.ToString().Split(',');
            var result = new List<string>();

            foreach (var t in temp)
                result.Add(t.Trim());

            return result;
        }
    }

}
