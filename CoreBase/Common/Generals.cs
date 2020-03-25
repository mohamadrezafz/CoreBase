using CoreBase.Common.Constant;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CoreBase.Common
{
    public class GeneralHelper
    {
        public virtual Type FindBaseType<TBaseType>(Type driveType)
        {
            return this.FindBaseType(typeof(TBaseType), driveType);
        }
        public virtual Type FindBaseType(Type baseType, Type driveType)
        {
            if (baseType.IsAssignableFrom(driveType))
                return baseType;
            if (baseType.IsGenericTypeDefinition)
            {
                var bt = driveType.BaseType;
                while (bt != null && bt != typeof(object))
                {
                    if (bt.IsGenericType)
                    {
                        if (bt.GetGenericTypeDefinition() == baseType.GetGenericTypeDefinition())
                        {
                            return bt;
                        }
                    }
                    bt = bt.BaseType;
                }
            }
            return null;
        }
        public virtual bool IsAssignableFrom(Type baseType, Type driveType)
        {
            var bt = FindBaseType(baseType, driveType);
            return bt != null;
        }
        public virtual bool IsAssignableFrom<TBaseType>(Type driveType)
        {
            return this.IsAssignableFrom(typeof(TBaseType), driveType);
        }

        public virtual void EnforceType<TBaseType>(Type drivedType, string parameterName)
        {
            EnforceType(typeof(TBaseType), drivedType, parameterName);
        }
        public virtual void EnforceObjectType<TBaseType>(object drivedObject, string parameterName)
        {
            EnforceObjectType(typeof(TBaseType), drivedObject, parameterName);
        }
        public virtual void EnforceType(Type baseType, Type drivedType, string parameterName)
        {
            if (!this.IsAssignableFrom(baseType, drivedType))
                throw new InvalidOperationException(string.Format("Passed {0} argument must be drived from '{1}'.", parameterName, baseType));
        }
        public virtual void EnforceObjectType(Type baseType, object drivedObject, string parameterName)
        {
            if (!baseType.IsInstanceOfType(drivedObject))
                throw new InvalidOperationException(string.Format("Passed {0} argument must be drived from '{1}'.", parameterName, baseType));
        }
        public virtual void EnforceNullArgument(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        public virtual bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public virtual bool IsNotNullOrWhiteSpace(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public virtual bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public virtual bool IsNotNullOrEmpty(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public virtual string ToStringForLike(string value)
        {
            return $"%{value}%";
        }

        public virtual string SerializeAsXml(object value)
        {
            if (value == null)
                return null;
            if (value is string)
                return value as string;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serilizer = new XmlSerializer(value.GetType());
                serilizer.Serialize(ms, value);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public virtual T DeserializeFormXml<T>(string value)
        {
            if (value == null)
                return default;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serilizer = new XmlSerializer(typeof(T));
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write(value);
                }
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serilizer.Deserialize(ms);
            }
        }

        public virtual string SerializeAsJson(object value, params JsonConverter[] converters)
        {
            if (value == null)
                return null;
            return JsonConvert.SerializeObject(value, converters);
        }

        public virtual string SerializeAsJson(object value, JsonSerializerSettings settings = null, bool exceptionalHandling = false, bool applyCustomConverter = false, bool applyEnumConverter = true, bool applyLogPolicyConverter = false)
        {
            if (value == null)
                return null;
            if (value is string)
            {
                return value as string;
            }
            return this.SerializeAsJson(value, settings);
        }

        public virtual string SerializeAsJson(object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, settings);
        }

        public virtual T DeserializeFromJson<T>(string value, JsonSerializerSettings settings = null, bool exceptionalHandling = false, bool applyCustomConverter = false, bool applyEnumConverter = true, bool applyLogPolicyConverter = false)
        {
            if (value == null)
                return default;

            if (typeof(T) == typeof(string))
            {
                return (T)((object)value);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public virtual object DeserializeFromJson(string value, Type type, JsonSerializerSettings settings = null, bool exceptionalHandling = false, bool applyCustomConverter = false, bool applyEnumConverter = true, bool applyLogPolicyConverter = false)
        {
            if (value == null)
                return null;

            if (type.IsAssignableFrom(typeof(string)))
            {
                return (string)value;
            }
            return JsonConvert.DeserializeObject(value, type);
        }

        public virtual ICollection<T> AddRange<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null || collection == null)
                return collection;
            foreach (var item in items)
            {
                collection.Add(item);
            }
            return collection;
        }

        public virtual void ForEach<T>(IEnumerable<T> collection, Action<T> action)
        {
            if (action == null || collection == null)
                return;
            collection.ToList().ForEach(action);
        }

        public virtual void ForEachEx<T>(IEnumerable<T> collection, Action<int, T> action)
        {
            if (action == null || collection == null)
                return;

            var list = collection.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                action(i, list[i]);
            }
        }

        public virtual bool IsGreaterThanZero(int i)
        {
            return i > 0;
        }

        public virtual bool IsGreaterThanZero(int? i)
        {
            return i.HasValue && IsGreaterThanZero(i.Value);
        }

        public virtual bool IsGreaterThanZero(long i)
        {
            return i > 0;
        }

        public virtual bool IsGreaterThanZero(long? i)
        {
            return i.HasValue && IsGreaterThanZero(i.Value);
        }

        public virtual bool IsGreaterThanZero(decimal? i)
        {
            return i.HasValue && IsGreaterThanZero(i.Value);
        }

        public virtual bool IsGreaterThanZero(decimal i)
        {
            return i > 0;
        }

        public virtual bool IsGreaterThan(int i, int j)
        {
            return i > j;
        }

        public virtual bool IsGreaterThan(int? i, int j)
        {
            return i.HasValue && IsGreaterThan(i.Value, j);
        }

        public virtual bool IsGreaterThan(long i, long j)
        {
            return i > j;
        }

        public virtual bool IsGreaterThan(long? i, long j)
        {
            return i.HasValue && IsGreaterThan(i.Value, j);
        }

        public virtual bool IsGreaterThan(decimal i, decimal j)
        {
            return i > j;
        }

        public virtual bool IsGreaterThan(decimal? i, decimal j)
        {
            return i.HasValue && IsGreaterThan(i.Value, j);
        }

        public virtual bool IsNotGreaterThanZero(int i)
        {
            return !IsGreaterThanZero(i);
        }

        public virtual bool IsNotGreaterThanZero(int? i)
        {
            return !IsGreaterThanZero(i);
        }

        public virtual bool IsNotGreaterThanZero(long i)
        {
            return !IsGreaterThanZero(i);
        }

        public virtual bool IsNotGreaterThanZero(long? i)
        {
            return !IsGreaterThanZero(i);
        }

        public virtual bool IsNotGreaterThanZero(decimal i)
        {
            return !IsGreaterThanZero(i);
        }

        public virtual bool IsNotGreaterThanZero(decimal? i)
        {
            return !IsGreaterThanZero(i);
        }

        public virtual bool IsNotGreaterThan(int i, int j)
        {
            return !IsGreaterThan(i, j);
        }

        public virtual bool IsNotGreaterThan(int? i, int j)
        {
            return !IsGreaterThan(i, j);
        }

        public virtual bool IsNotGreaterThan(long i, long j)
        {
            return !IsGreaterThan(i, j);
        }

        public virtual bool IsNotGreaterThan(long? i, long j)
        {
            return !IsGreaterThan(i, j);
        }

        public virtual bool IsNotGreaterThan(decimal i, decimal j)
        {
            return !IsGreaterThan(i, j);
        }

        public virtual bool IsNotGreaterThan(decimal? i, decimal j)
        {
            return !IsGreaterThan(i, j);
        }

        public virtual bool IsNullOrZero(int i)
        {
            return i == 0;
        }

        public virtual bool IsNullOrZero(int? i)
        {
            return !i.HasValue || i.Value == 0;
        }

        public virtual bool IsNullOrZero(long i)
        {
            return i == 0;
        }

        public virtual bool IsNullOrZero(long? i)
        {
            return !i.HasValue || i.Value == 0;
        }

        public virtual bool IsNullOrZero(decimal i)
        {
            return i == 0;
        }

        public virtual bool IsNullOrZero(decimal? i)
        {
            return !i.HasValue || i.Value == 0;
        }

        public virtual bool IsNotNullOrZero(int i)
        {
            return !IsNullOrZero(i);
        }

        public virtual bool IsNotNullOrZero(int? i)
        {
            return !IsNullOrZero(i);
        }

        public virtual bool IsNotNullOrZero(long i)
        {
            return !IsNullOrZero(i);
        }

        public virtual bool IsNotNullOrZero(long? i)
        {
            return !IsNullOrZero(i);
        }

        public virtual bool IsNotNullOrZero(decimal i)
        {
            return !IsNullOrZero(i);
        }

        public virtual bool IsNotNullOrZero(decimal? i)
        {
            return !IsNullOrZero(i);
        }

        public virtual bool IsGreaterThanToday(DateTime value)
        {
            return value.Date >= DateTime.Now.Date;
        }

        public virtual bool IsNullOrGreaterThanToday(DateTime? value)
        {
            return !value.HasValue || IsGreaterThanToday(value.Value);
        }

        public virtual bool IsGreaterThan(DateTime value1, DateTime value2)
        {
            return value1.Date >= value2.Date;
        }

        public virtual bool IsNullOrGreaterThan(DateTime? value1, DateTime value2)
        {
            return !value1.HasValue || IsGreaterThan(value1.Value, value2);
        }

        public virtual bool IsFewerThanToday(DateTime value)
        {
            return value.Date <= DateTime.Now.Date;
        }

        public virtual bool IsNullOrFewerThanToday(DateTime? value)
        {
            return !value.HasValue || IsFewerThanToday(value.Value);
        }

        public virtual bool IsFewerThan(DateTime value1, DateTime value2)
        {
            return value1.Date <= value2.Date;
        }

        public virtual bool IsNullOrFewerThan(DateTime? value1, DateTime value2)
        {
            return !value1.HasValue || IsFewerThan(value1.Value, value2);
        }

        public virtual bool IsInLastNHour(DateTime value, int n)
        {
            if (n < 1)
                return false;
            var now = DateTime.Now;
            return value > now.AddHours(-n) && value <= now;

        }

        public virtual string SafeToString(object obj)
        {
            return obj?.ToString();
        }

        public virtual string GetCardNumberIssuerBin(string cardNumberOrTrack2)
        {
            if (string.IsNullOrEmpty(cardNumberOrTrack2) || cardNumberOrTrack2.Length < 6)
                return "";
            return cardNumberOrTrack2.Substring(0, 6);
        }

        public virtual string TruncateCardNumber(string cardNumber, char replaceChar = '*')
        {
            if (string.IsNullOrEmpty(cardNumber)) return cardNumber;

            var indexOf = cardNumber.IndexOf('=');
            if (indexOf >= 0)
            {
                cardNumber = cardNumber.Substring(0, indexOf);
            }
            var len = cardNumber.Length;
            if (cardNumber.Length < 16)
            {
                cardNumber = cardNumber.PadRight(16, ' ');
            }
            if (cardNumber.Length > 19)
                cardNumber = cardNumber.Substring(0, 19);
            return string.Format("{0}******{1}", cardNumber.Substring(0, 6), cardNumber.Substring(12)).Substring(0, len);
        }

        public virtual string EncryptCardNumber(string cardNumber, int? generation = null)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return cardNumber;
            return cardNumber;
        }
        public virtual string DecryptCardNumber(string encCardNumber, int? generation = null)
        {
            if (string.IsNullOrEmpty(encCardNumber))
                return encCardNumber;
            return encCardNumber;
        }

        public virtual string TruncateTrack2(string track2, char replaceChar = '*')
        {
            return this.TruncateCardNumber(track2, replaceChar);
        }

        public virtual bool IsNull(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            return false;
        }

        public virtual bool IsNullableType(Type t)
        {
            EnforceNullArgument(t, "t");
            return (t.IsValueType && !t.IsPrimitive && t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public virtual string FormatWith(string format, IFormatProvider provider, object arg0)
        {
            return format.FormatWith(provider, new object[] { arg0 });
        }

        public virtual bool IsGenericType(Type type)
        {
            return type.IsGenericType;
        }

        public virtual Type GetObjectType(object v)
        {
            if (v == null)
            {
                return null;
            }
            return v.GetType();
        }

        public virtual bool IsEnum(Type objectType)
        {
            return
                objectType.IsEnum ||
                (this.IsNullableType(objectType) && objectType.GetGenericArguments().First().IsEnum);
        }

        public virtual string SerializeToJson(object value)
        {
            try
            {
                var r = value.SerializeAsJson(
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        DateFormatString = "yyyy-MM-dd HH:mm:ss.fffffff",
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),

                    },
                    true, true, true, true);
                return r;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return string.Empty;
            }
        }

    }
    public static class Generals
    {
        private static readonly object lockThis = new object();

        private static GeneralHelper helper;
        public static GeneralHelper Helper
        {
            get
            {
                if (helper == null)
                {
                    lock (lockThis)
                    {
                        if (helper == null)
                        {
                            helper = new GeneralHelper();
                        }
                    }
                }
                return helper;
            }
            set
            {
                helper = value;
            }
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return Generals.Helper.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return Generals.Helper.IsNotNullOrWhiteSpace(value);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return Generals.Helper.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return Generals.Helper.IsNotNullOrEmpty(value);
        }

        public static string ToStringForLike(this string value)
        {
            return value.IsNotNullOrWhiteSpace() ? Generals.Helper.ToStringForLike(value) : value;
        }

        public static bool IsGreaterThanZero(this int value)
        {
            return Generals.Helper.IsGreaterThanZero(value);
        }

        public static bool IsGreaterThanZero(this int? value)
        {
            return Generals.Helper.IsGreaterThanZero(value);
        }

        public static bool IsGreaterThanZero(this long value)
        {
            return Generals.Helper.IsGreaterThanZero(value);
        }

        public static bool IsGreaterThanZero(this long? value)
        {
            return Generals.Helper.IsGreaterThanZero(value);
        }

        public static bool IsGreaterThanZero(this decimal value)
        {
            return Generals.Helper.IsGreaterThanZero(value);
        }

        public static bool IsGreaterThanZero(this decimal? value)
        {
            return Generals.Helper.IsGreaterThanZero(value);
        }

        public static bool IsGreaterThan(this int value1, int value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsGreaterThan(this int? value1, int value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsGreaterThan(this long value1, long value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsGreaterThan(this long? value1, long value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsGreaterThan(this decimal value1, decimal value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsGreaterThan(this decimal? value1, decimal value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsNotGreaterThanZero(this int value)
        {
            return Generals.Helper.IsNotGreaterThanZero(value);
        }

        public static bool IsNotGreaterThanZero(this int? value)
        {
            return Generals.Helper.IsNotGreaterThanZero(value);
        }

        public static bool IsNotGreaterThanZero(this long value)
        {
            return Generals.Helper.IsNotGreaterThanZero(value);
        }

        public static bool IsNotGreaterThanZero(this long? value)
        {
            return Generals.Helper.IsNotGreaterThanZero(value);
        }

        public static bool IsNotGreaterThanZero(this decimal value)
        {
            return Generals.Helper.IsNotGreaterThanZero(value);
        }

        public static bool IsNotGreaterThanZero(this decimal? value)
        {
            return Generals.Helper.IsNotGreaterThanZero(value);
        }

        public static bool IsNotGreaterThan(this int value1, int value2)
        {
            return Generals.Helper.IsNotGreaterThan(value1, value2);
        }

        public static bool IsNotGreaterThan(this int? value1, int value2)
        {
            return Generals.Helper.IsNotGreaterThan(value1, value2);
        }

        public static bool IsNotGreaterThan(this long value1, long value2)
        {
            return Generals.Helper.IsNotGreaterThan(value1, value2);
        }

        public static bool IsNotGreaterThan(this long? value1, long value2)
        {
            return Generals.Helper.IsNotGreaterThan(value1, value2);
        }

        public static bool IsNotGreaterThan(this decimal value1, decimal value2)
        {
            return Generals.Helper.IsNotGreaterThan(value1, value2);
        }

        public static bool IsNotGreaterThan(this decimal? value1, decimal value2)
        {
            return Generals.Helper.IsNotGreaterThan(value1, value2);
        }

        public static bool IsNullOrZero(this int value)
        {
            return Generals.Helper.IsNullOrZero(value);
        }

        public static bool IsNullOrZero(this int? value)
        {
            return Generals.Helper.IsNullOrZero(value);
        }

        public static bool IsNullOrZero(this long value)
        {
            return Generals.Helper.IsNullOrZero(value);
        }

        public static bool IsNullOrZero(this long? value)
        {
            return Generals.Helper.IsNullOrZero(value);
        }

        public static bool IsNullOrZero(this decimal value)
        {
            return Generals.Helper.IsNullOrZero(value);
        }

        public static bool IsNullOrZero(this decimal? value)
        {
            return Generals.Helper.IsNullOrZero(value);
        }

        public static bool IsNotNullOrZero(this int value)
        {
            return Generals.Helper.IsNotNullOrZero(value);
        }

        public static bool IsNotNullOrZero(this int? value)
        {
            return Generals.Helper.IsNotNullOrZero(value);
        }

        public static bool IsNotNullOrZero(this long value)
        {
            return Generals.Helper.IsNotNullOrZero(value);
        }

        public static bool IsNotNullOrZero(this long? value)
        {
            return Generals.Helper.IsNotNullOrZero(value);
        }

        public static bool IsNotNullOrZero(this decimal value)
        {
            return Generals.Helper.IsNotNullOrZero(value);
        }

        public static bool IsNotNullOrZero(this decimal? value)
        {
            return Generals.Helper.IsNotNullOrZero(value);
        }

        public static string SerializeAsJson(this object value, params JsonConverter[] converters)
        {
            return Generals.Helper.SerializeAsJson(value, converters);
        }

        public static string SerializeAsJson(this object value, JsonSerializerSettings settings)
        {
            return Generals.Helper.SerializeAsJson(value, settings);
        }

        public static string SerializeAsJson(this object value, JsonSerializerSettings settings = null, bool exceptionalHandling = false, bool applyCustomConverter = false, bool applyEnumConverter = true, bool applyLogPolicyConverter = false)
        {
            return Generals.Helper.SerializeAsJson(value, settings, exceptionalHandling, applyCustomConverter, applyEnumConverter, applyLogPolicyConverter);
        }

        public static T DeserializeFromJson<T>(this string value, JsonSerializerSettings settings = null, bool exceptionalHandling = false, bool applyCustomConverter = false, bool applyEnumConverter = true, bool applyLogPolicyConverter = false)
        {
            return Generals.Helper.DeserializeFromJson<T>(value, settings, exceptionalHandling, applyCustomConverter, applyEnumConverter, applyLogPolicyConverter);
        }
        public static object DeserializeFromJson(this string value, Type type, JsonSerializerSettings settings = null, bool exceptionalHandling = false, bool applyCustomConverter = false, bool applyEnumConverter = true, bool applyLogPolicyConverter = false)
        {
            return Generals.Helper.DeserializeFromJson(value, type, settings, exceptionalHandling, applyCustomConverter, applyEnumConverter, applyLogPolicyConverter);
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            return Generals.Helper.AddRange<T>(collection, items);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Generals.Helper.ForEach<T>(collection, action);
        }

        public static void ForEachEx<T>(this IEnumerable<T> collection, Action<int, T> action)
        {
            Generals.Helper.ForEachEx<T>(collection, action);
        }

        public static bool IsGreaterThanToday(this DateTime value)
        {
            return Generals.Helper.IsGreaterThanToday(value);
        }

        public static bool IsNullOrGreaterThanToday(this DateTime? value)
        {
            return Generals.Helper.IsNullOrGreaterThanToday(value);
        }

        public static bool IsGreaterThan(this DateTime value1, DateTime value2)
        {
            return Generals.Helper.IsGreaterThan(value1, value2);
        }

        public static bool IsNullOrGreaterThan(this DateTime? value1, DateTime value2)
        {
            return Generals.Helper.IsNullOrGreaterThan(value1, value2);
        }

        public static bool IsFewerThanToday(this DateTime value)
        {
            return Generals.Helper.IsFewerThanToday(value);
        }

        public static bool IsNullOrFewerThanToday(this DateTime? value)
        {
            return Generals.Helper.IsNullOrFewerThanToday(value);
        }

        public static bool IsFewerThan(this DateTime value1, DateTime value2)
        {
            return Generals.Helper.IsFewerThan(value1, value2);
        }

        public static bool IsNullOrFewerThan(this DateTime? value1, DateTime value2)
        {
            return Generals.Helper.IsNullOrFewerThan(value1, value2);
        }

        public static bool IsInLast24Hour(this DateTime value)
        {
            return Generals.Helper.IsInLastNHour(value, 24);
        }

        public static bool IsInLastNHour(this DateTime value, int n)
        {
            return Generals.Helper.IsInLastNHour(value, n);
        }



        public static string SafeToString(this object obj)
        {
            return Generals.Helper.SafeToString(obj);
        }

        public static string TruncateCardNumber(string cardNumber, char replaceChar = '*')
        {
            return Generals.Helper.TruncateCardNumber(cardNumber);
        }
        public static string GetCardNumberIssuerBin(string cardNumber)
        {
            return Generals.Helper.GetCardNumberIssuerBin(cardNumber);
        }

        public static string TruncateTrack2(string track2, char replaceChar = '*')
        {
            return Generals.Helper.TruncateTrack2(track2);
        }

        public static string EncryptCardNumber(string cardNumber, int? generation = null)
        {
            return Generals.Helper.EncryptCardNumber(cardNumber, generation);
        }

        public static string DecryptCardNumber(string cardNumber, int? generation = null)
        {
            return Generals.Helper.DecryptCardNumber(cardNumber, generation);
        }

        public static bool IsNull(this object obj)
        {
            return Generals.Helper.IsNull(obj);
        }

        public static string FormatWith(this string format, IFormatProvider provider, object arg0)
        {
            return Generals.Helper.FormatWith(format, provider, arg0);
        }

        public static bool IsGenericType(this Type type)
        {
            return Generals.Helper.IsGenericType(type);
        }

        public static Type GetObjectType(object v)
        {
            return Generals.Helper.GetObjectType(v);
        }

        public static bool IsEnum(Type objectType)
        {
            return Generals.Helper.IsEnum(objectType);
        }

        public static bool IsNullableType(this Type type)
        {
            return Generals.Helper.IsNullableType(type);
        }

        public static string SerializeToJson(this object value)
        {
            return Generals.Helper.SerializeToJson(value);
        }
    }
}
