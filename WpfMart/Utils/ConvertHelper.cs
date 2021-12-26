using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace WpfMart.Utils
{
    internal sealed class ConvertHelper
    {
        private static readonly HashSet<Type> _charInderectlySupportedConvertions = new HashSet<Type>
        {
            typeof(float), typeof(double), typeof(decimal)
        };
        private static readonly HashSet<Type> _convertibleInterfaceTypes = new HashSet<Type>
        {
            typeof(bool), typeof(byte), typeof(char), typeof(decimal), typeof(DateTime),
            typeof(double), typeof(int), typeof(short), typeof(long), typeof(sbyte),
            typeof(float), typeof(string), typeof(ushort), typeof(uint), typeof(ulong)
        };

        public static bool TryChangeType(object value, Type targetType, out object result, IFormatProvider provider = null)
        {
            result = value;
            Type typeofValue = value?.GetType();
            if (ReferenceEquals(value, null) || targetType.IsAssignableFrom(typeofValue)) return true;

            targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            var typeConverter = TypeDescriptor.GetConverter(targetType);
            if (typeConverter != null && typeConverter.CanConvertFrom(typeofValue))
            {
                try
                {
                    result = typeConverter.ConvertFrom(value);
                }
                catch (FormatException)
                {
                    return false;
                }
                return true;
            }

            if (targetType.IsEnum)
            {
                try
                {
                    result = Enum.ToObject(targetType, value);
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            if (!(value is IConvertible))
            {
                if (targetType == typeof(string))
                {
                    result = value.ToString();
                    return true;
                }
                return false;
            }

            if(value is char && _charInderectlySupportedConvertions.Contains(targetType))
            {
                value = (int)(char)value;
            }
            var convertible = value as IConvertible;
            if (convertible != null && _convertibleInterfaceTypes.Contains(targetType))
            {
                try
                {
                    result = convertible.ToType(targetType, provider ?? CultureInfo.CurrentCulture);
                    return true;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }

            return false;
        }

        public static object ChangeType(object value, Type targetType, IFormatProvider provider = null)
        {
            if (ReferenceEquals(value, null)) return null;
            if (targetType.IsAssignableFrom(value.GetType())) return value;

            targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            var typeConverter = TypeDescriptor.GetConverter(targetType);
            if (typeConverter != null && typeConverter.CanConvertFrom(value.GetType()))
            {
                try
                {
                    return typeConverter.ConvertFrom(value);
                }
                catch (FormatException ex)
                {
                    throw new InvalidCastException(
                        string.Format("Cannot cast value '{0}' to enum type {1}",
                        value.GetType().Name,
                        targetType), ex);
                }
            }

            if (targetType.IsEnum)
            {
                try
                {
                    return Enum.ToObject(targetType, value);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidCastException(
                        string.Format("Cannot cast value of type {0} to {1} enum type",
                        value.GetType().Name,
                        targetType), ex);
                }
            }

            if (!(value is IConvertible))
            {
                if (targetType == typeof(string)) value.ToString();
            }

            if (value is char && _charInderectlySupportedConvertions.Contains(targetType))
            {
                value = (int)(char)value;
            }

            return System.Convert.ChangeType(value, targetType, provider ?? CultureInfo.CurrentCulture);
        }
    }
}
