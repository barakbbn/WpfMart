using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// Defines options to set for <see cref="NullOrEmptyConverter.Mode"/> property
    /// </summary>
    [TypeConverter(typeof(EnumTypeConverter<ComparisonMode>))]
    public enum ComparisonMode
    {
        /// <summary>
        /// Check or either null or empty
        /// </summary>
        IsNullOrEmpty,
        /// <summary>
        /// Check only for empty value
        /// </summary>
        IsEmpty
    }

    /// <summary>
    /// <para>Convert from either null or empty IEnumerable or string.</para>
    /// <para>Can set Mode to convert only if empty.</para>
    /// </summary>
    /// <remarks>
    /// <para>Converter can be negative, meaning comparing value is not null and also not empty string of collection.</para>
    /// <para>There is also a predefined <see cref="NegativeBoolConverterExtension"/> used to perform negative boolean conversion.</para>
    /// <para/>
    /// <para>When used as markup-extension it's possible to chain another converter to convert from,</para>
    /// <para>using the <see cref="ChainedConverterExtensionBase.FromConverter"/></para>
    /// <para>When not used as markup-extension, wrap with <see cref="GroupConverter"/> to chain several converters.</para>
    /// </remarks>
    /// <example>
    /// Make combo-box read-only in case the DevicesList property on view-model is either null or empty
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{Binding DevicesList}"
    ///           IsReadOnly="{Binding DevicesList, Converter={conv:NullOrEmptyConverter}}}"/>
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// <para>Make combo-box read-only in case the DevicesList property on view-model is either null or empty.</para>
    /// <para>Note: for simpler usage of the example, use <see cref="NullOrEmptyToInvisibleConverterExtension"/> converter.</para>
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{Binding DevicesList}"
    ///           Visibility="{Binding DevicesList, Converter={conv:NullOrEmptyConverter 
    ///             TrueValue={x:Static Visibility.Collapsed}
    ///             FalseValue={x:Static Visibility.Visible}}}"/>
    /// ]]>
    /// </code>
    /// </example>
    public class NullOrEmptyConverter : ChainedConverterExtensionBase, IValueConverter, ICantConvertBack
    {
        #region Predefined

        /// <summary>
        /// Predefined NullOrEmptyConverter configured as Negative converter
        /// </summary>
        public static NullOrEmptyConverter NotNullOrEmpty => new NullOrEmptyConverter(true);

        /// <summary>
        /// Predefined converter that bull or empty value to <see cref="System.Windows.Visibility.Collapsed"/> and non null or empty to <see cref="System.Windows.Visibility.Visible"/>
        /// </summary>
        public static NullOrEmptyConverter CollpasedWhenNullOrEmpty => new NullOrEmptyConverter
        {
            TrueValue = Boxed.Collapsed,
            FalseValue = Boxed.Visible
        };

        #endregion

        /// <summary>
        /// C'tor
        /// </summary>
        public NullOrEmptyConverter() {}

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="NullOrEmptyConverter.IsNegative"/> property</param>
        public NullOrEmptyConverter(bool isNegative)
        {
            IsNegative = isNegative;
        }

        /// <summary>
        /// Value to return when input value is empty (or also null, depended on Mode property)
        /// </summary>
        /// <remarks>
        /// <para>Property can be set to special value as follow:</para>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object TrueValue { get; set; } = Boxed.True;

        /// <summary>
        /// Value to return when input value is not empty (and also not null, depended on Mode property)
        /// </summary>
        /// <remarks>
        /// <para>Property can be set to special value as follow:</para>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object FalseValue { get; set; } = Boxed.False;

        /// <summary>
        /// Determine if to compare to only empty IEnumerable/string, or also for null value
        /// </summary>
        public ComparisonMode Mode { get; set; } = ComparisonMode.IsNullOrEmpty;

        /// <summary>
        /// Determine if to compare to False instead of True when converting.
        /// (switch between TrueValue and FalseValue)
        /// </summary>
        [ConstructorArgument("isNegative")]
        public bool IsNegative { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = ReferenceEquals(null, value);
            var isEmpty = false;

            if(!isNull)
            {
                var enumerable = (value as IEnumerable)?.GetEnumerator();
                using (enumerable as IDisposable)
                {
                    bool hasValue = enumerable?.MoveNext() ?? false;
                    isEmpty = !hasValue;
                }
            }
            bool isNullOrEmpty = (isNull && Mode == ComparisonMode.IsNullOrEmpty || isEmpty);
            var result = (isNullOrEmpty != IsNegative) ? TrueValue : FalseValue;

            return ConverterSpecialValue.Apply(result, value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("NullOrEmptyConverter.ConvertBack is not supported");
        }

        /// <summary>
        /// Chained converter to use first when converting and last when converting-back.
        /// </summary>
        protected override IValueConverter Converter => this;
    }

    /// <summary>
    /// Converts non null or empty value to boolean true, otherwise false
    /// </summary>
    public class IsNotNullOrEmptyConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly NullOrEmptyConverter _converter = NullOrEmptyConverter.NotNullOrEmpty;
        protected override IValueConverter Converter => _converter;
    }

    /// <summary>
    /// Converts null or empty value to <see cref="System.Windows.Visibility.Collapsed"/> otherwise to <see cref="System.Windows.Visibility.Visible"/>
    /// </summary>
    /// <example>
    /// Make combo-box read-only in case the DevicesList property on view-model is either null or empty.
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{Binding DevicesList}"
    ///           Visibility="{Binding DevicesList, Converter={conv:NullOrEmptyToInvisibleConverter}}"/>
    /// ]]>
    /// </code>
    /// </example>
    public class NullOrEmptyToInvisibleConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly NullOrEmptyConverter _converter = NullOrEmptyConverter.CollpasedWhenNullOrEmpty;
        protected override IValueConverter Converter => _converter;
    }
}
