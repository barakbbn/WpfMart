using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// Flag interface for value-converter to indicate it doesn't support ConvertBack.
    /// </summary>
    public interface ICantConvertBack { }

    /// <summary>
    /// Type converter for enumeration types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumTypeConverter<T> : TypeConverter where T : struct
    {
        private readonly EnumConverter _enumConverter = new EnumConverter(typeof(T));

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return _enumConverter.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return _enumConverter.CanConvertTo(destinationType) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return _enumConverter.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return _enumConverter.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Common value types provided as boxed objects
    /// </summary>
    public static class Boxed
    {
        public static readonly object True = true;
        public static readonly object False = false;
        public static readonly object Visible = Visibility.Visible;
        public static readonly object Collapsed = Visibility.Collapsed;
        public static readonly object Hidden = Visibility.Hidden;
        public static readonly object Zero = 0;
        public static readonly object EmptyString = "";
    }

    /// <summary>
    /// Special values used by converters to provide special behaviors of the converter.
    /// </summary>
    public class ConverterSpecialValue
    {
        private string _name;

        /// <summary>
        /// <para>Indicates that the converter should use the 'value' parameter passed to the Convert method.</para>
        /// <see cref="System.Windows.Data.IValueConverter.Convert(object, Type, object, CultureInfo)"/>
        /// </summary>
        public static readonly ConverterSpecialValue UseInputValue = new ConverterSpecialValue();

        /// <summary>
        /// <para>Indicates that the converter should use the ConverterParameter value use in binding to the Convert method.</para>
        /// <para><see cref="System.Windows.Data.IValueConverter.Convert(object, Type, object, CultureInfo)"/></para>
        /// <see cref="System.Windows.Data.Binding.ConverterParameter"/>
        /// </summary>
        public static readonly ConverterSpecialValue UseConverterParameter = new ConverterSpecialValue();

        /// <summary>
        /// <para>Indicates that the converter should throw <see cref="System.InvalidOperationException"/>.</para>
        /// <para>Used only as rare option to cause exception in case developer requires that certain conditions won't go unnoticed.</para>
        /// <para>Such as to trigger the <see cref="System.Windows.Data.Binding.ValidatesOnExceptions"/> property</para>
        /// </summary>
        /// <example>
        /// Cause validation error of the TextBox if using null as text. (Prefer doing it in view-model, or using validation rules)
        /// <code>
        /// <![CDATA[
        /// <TextBox Text="{Binding Name, ValidatesOnExceptions=True,
        ///                         Converter={conv:NullConverter TrueValue={conv:ThrowException}, FalseValue={conv:UseInputValue}}}" />
        /// ]]>
        /// </code>
        /// </example>
        /// <example>
        /// Ensure to notify the developer NOT to set null (again) in the Name property.
        /// <code>
        /// <![CDATA[
        /// <TextBox Text="{Binding Name, Converter={conv:NullConverter TrueValue={conv:ThrowException}, FalseValue={conv:UseInputValue}}}" />
        /// ]]>
        /// </code>
        /// </example>
        public static readonly ConverterSpecialValue ThrowException = new ConverterSpecialValue();

        private ConverterSpecialValue([CallerMemberName]  string name = "")
        {
            _name = name;
        }

        /// <summary>
        /// Return the instance name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _name;

        /// <summary>
        /// Check if passed value is special value and act upon it, otherwise return the input value as is.
        /// </summary>
        /// <param name="value">The value that is potentially instance of <see cref="ConverterSpecialValue"/></param>
        /// <param name="converterValue">The 'value' parameter that was passed to the Convert method</param>
        /// <param name="converterParameter">The 'parameter' parameter that was passed to the Convert method</param>
        /// <returns></returns>
        public static object Apply(object value, object converterValue, object converterParameter)
        {
            if (Equals(value, ConverterSpecialValue.ThrowException)) throw new InvalidOperationException();
            if (Equals(value, ConverterSpecialValue.UseConverterParameter)) return converterParameter;
            if (Equals(value, ConverterSpecialValue.UseInputValue)) return converterValue;

            return value;
        }
    }

    /// <summary>
    /// Provide a value of <see cref="ConverterSpecialValue"/> in markup-extension syntax.
    /// </summary>
    /// <remarks>
    /// <para>Refer to the predefined markup-extensions for each special value, instead of using ConverterSpecialValue directly.</para>
    /// <para><see cref="UseInputValueExtension"/></para>
    /// <para><see cref="UseConvereterParameterExtension"/></para>
    /// <para><see cref="ThrowExceptionExtension"/></para>
    /// </remarks>
    [MarkupExtensionReturnType(typeof(ConverterSpecialValue))]
    public class ConverterSpecialExtension : MarkupExtension
    {
        public ConverterSpecialExtension(ConverterSpecialValue specialValue)
        {
            Value = specialValue;
        }

        public ConverterSpecialValue Value { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }

    /// <summary>
    /// Markup extension to provide <see cref="ConverterSpecialValue.UseInputValue"/>
    /// </summary>
    public class UseInputValueExtension : ConverterSpecialExtension
    {
        public UseInputValueExtension() : base(ConverterSpecialValue.UseInputValue) { }
    }

    /// <summary>
    /// Markup extension to provide <see cref="ConverterSpecialValue.UseConverterParameter"/>
    /// </summary>
    public class UseConvereterParameterExtension : ConverterSpecialExtension
    {
        public UseConvereterParameterExtension() : base(ConverterSpecialValue.UseConverterParameter) { }
    }

    /// <summary>
    /// Markup extension to provide <see cref="ConverterSpecialValue.ThrowException"/>
    /// </summary>
    public class ThrowExceptionExtension : ConverterSpecialExtension
    {
        public ThrowExceptionExtension() : base(ConverterSpecialValue.ThrowException) { }
    }

    /// <summary>
    /// Returns <see cref="DependencyProperty.UnsetValue"/>, in shorter syntax than
    /// {x:Static DependencyProperty.UnsetValue}
    /// </summary>
    [MarkupExtensionReturnType(typeof(DependencyProperty))]
    public class UnsetValueExtension: MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Returns <see cref="Binding.DoNothing"/>, in shorter syntax than
    /// {x:Static Binding.DoNothing}
    /// </summary>
    public class DoNothingExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Binding.DoNothing;
        }
    }

    internal static class ConverterExtensions
    {
        public static IValueConverter FromConverter(this IValueConverter converter, IValueConverter fromConverter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            if (fromConverter == null) return converter;

            var group =  ((converter as GroupConverter) ?? new GroupConverter { converter });
            group.Insert(0, fromConverter);

            return group;
        }
    }
}
