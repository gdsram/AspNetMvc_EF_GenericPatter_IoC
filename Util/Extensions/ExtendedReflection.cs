using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Util.Extensions
{
    public static class ExtendedReflection
    {
        public static bool IsValidEmail(this string strIn)
        {
            var invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", EmailDomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string EmailDomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;

            domainName = idn.GetAscii(domainName);

            return match.Groups[1].Value + domainName;
        }

        public static void CallMethod(this object objRef, string methodName, params object[] parameters)
        {
            var objType = objRef.GetType();
            var methodInfo = objType.GetMethod(methodName);

            methodInfo.Invoke(objRef, parameters);

        }

        public static T CallMethod<T>(this object objRef, string methodName, params object[] parameters)
        {
            var objType = objRef.GetType();
            var methodInfo = objType.GetMethod(methodName);

            var result = methodInfo.Invoke(objRef, parameters);
            if (result == null)
            {
                return default(T);
            }

            if (result.GetType().IsPrimitive)
            {
                var typedResult = Convert.ChangeType(result, typeof(T));
                return (T)typedResult;
            }

            return (T)result;
        }

        public static bool IsPrimitive(this Type typeRef)
        {
            return typeRef.IsPrimitive || IsNumericType(typeRef) || typeRef == typeof(String);
        }

        public static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }
        public static bool IsDateTimeType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DateTime:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsDateTimeType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

        public static PropertyInfo[] GetPropertyInfos<T>(this T objRef)
        {
            Type type = typeof(T);
            if (objRef != null)
            {
                type = objRef.GetType();
            }

            return type.GetProperties();
        }


        public static T GetPropertyValue<T>(this object objRef, string propertyName) where T : class
        {
            var propertyInfo = objRef.GetPropertyInfos().FirstOrDefault(o => o.Name == propertyName);
            return objRef.GetPropertyValue<T>(propertyInfo);
        }


        public static T GetPropertyValue<T>(this object objRef, PropertyInfo pInfo) where T : class
        {
            var raw = pInfo.GetValue(objRef);
            if (raw == null)
            {
                return (T)null;
            }
            else if (typeof(T).Name == "Object")
            {
                return raw as T;
            }
            else if (typeof(T).Name == "String")
            {
                return raw.ToString() as T;
            }
            else
            {
                var convertedTypeRaw = Convert.ChangeType(raw, typeof(T));
                return (T)convertedTypeRaw;
            }
        }

        public static string BinarySerialize<T>(T obj)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            var memStream = new MemoryStream();
            formatter.Serialize(memStream, obj);

            return System.Text.Encoding.UTF8.GetString(memStream.GetBuffer());
        }

        public static T BinaryDeserialize<T>(string serializedObj)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(serializedObj);
            var memStream = new MemoryStream(buffer);

            BinaryFormatter formatter = new BinaryFormatter();

            var rawObject = formatter.Deserialize(memStream);

            return (T)rawObject;
        }

        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value + "");

            var attributes = fi.GetCustomAttributes<DescriptionAttribute>(inherit: false).FirstOrDefault();

            return attributes == null ? value.ToString() : (attributes as DescriptionAttribute).Description;
        }

        /// <summary>
        /// Tries to parse the given object into the type T. It tries to convert the object into a string.
        /// If that succeeds, then it will call the corresponding TryParse for the given type (if it exists).
        /// For number types that don't have decimal points, it will call TryParse on the string from the first position up to before the decimal point
        /// </summary>
        /// <typeparam name="T">The primitive struct type. If it does not have a TryParse static method, then it will return a default value</typeparam>
        /// <param name="value">The value that will be converted</param>
        /// <returns>If successful, the value converted into the type, otherwise the default value for that type</returns>
        public static T ToPrimitive<T>(this object value) where T : struct
        {

            if (value != null && typeof(T).AssemblyQualifiedName == value.GetType().AssemblyQualifiedName)
            {
                return (T)value;
            }
            string str = value as string;
            Type t = typeof(T);
            int indexDot = str == null ? -1 : str.IndexOf(".", StringComparison.Ordinal);

            if (str != null
                && indexDot != -1
                &&
                (t == typeof(byte)
                 || t == typeof(sbyte)
                 || t == typeof(short)
                 || t == typeof(ushort)
                 || t == typeof(int)
                 || t == typeof(uint)
                 || t == typeof(long)
                 || t == typeof(ulong)))
            {
                str = str.Substring(0, indexDot);
            }
            object tmp = value != null && str != null ? str : value;

            T result;

            MethodInfo methodInfo = t.GetMethod("TryParse", new Type[] { typeof(string), typeof(T).MakeByRefType() });

            if (methodInfo != null)
            {
                object[] parameters = { tmp, null };
                bool parsed = (bool)methodInfo.Invoke(obj: null, parameters: parameters);
                result = !parsed ? default(T) : (T)parameters[1];
            } //if
            else result = default(T);

            return result;
        }

        public static T[] SubArray<T>(this T[] data, int index, int? length = null)
        {
            if (!length.HasValue)
            {
                length = (data.Length - index);
            }

            T[] result = new T[length ?? (data.Length - index)];
            Array.Copy(data, index, result, 0, length.Value);
            return result;
        }

        public static string EnumMemberValue<T>(this T enumValue)
        {
            var type = typeof(T);
            var memInfo = type.GetMember(enumValue.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
            var description = ((EnumMemberAttribute)attributes[0]).Value;

            return description;
        }

    }
}
