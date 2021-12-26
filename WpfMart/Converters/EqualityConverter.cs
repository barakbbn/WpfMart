using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// <para>Check equality of converted value with <see cref="EqualityConverter.CompareTo" /> property.</para>
    /// <para>In case they are equal, return the value of <see cref="EqualityConverter.TrueValue" /> property otherwise <see cref="EqualityConverter.FalseValue" />.</para>
    /// </summary>
    /// <remarks>
    /// <para>Resulting value can also depends if converter is setup to be negative using the <see cref="EqualityConverter.IsNegative" /></para>
    /// <para>and if comparing to null is also supported by using <see cref="EqualityConverter.NullValue" /> property of <see cref="EqualityConverter.IsNullable" /> property</para>
    /// <para/>
    /// <para>When used as markup-extension it's possible to chain another converter to convert from,</para>
    /// <para>using the <see cref="ChainedConverterExtensionBase.FromConverter"/></para>
    /// <para>When not used as markup-extension, wrap with <see cref="GroupConverter"/> to chain several converters.</para>
    /// </remarks>
    /// <example>
    /// <para>1. If enum value is MachineState.On, set CheckBox.IsChecked to true, otherwise false.</para>
    /// <para>2. If enum value is MachineState.None, ComboBox shouldn't have selected value,</para>
    /// <para>   otherwise the enum value is the selected value.</para>
    /// <code>
    /// enum MachineState { None, On, Off, }
    /// </code>
    /// <code>
    /// <![CDATA[
    /// <CheckBox IsChecked="{Binding SelectedMachineState, Converter={conv:EqualityConverter
    ///             CompareTo={x:Static local:MachineState.On}}}" />
    ///             
    /// <ComboBox SelectedItem="{Binding SelectedMachineState, Converter={conv:EqualityConverter 
    ///             CompareTo={x:Static local:MachineState.None}
    ///             TrueValue={x:Null}
    ///             FalseValue={conv:UseInputValue}}}">
    ///     <x:Static local:MachineState.On />
    ///     <x:Static local:MachineState.Off />
    /// </ComboBox>
    /// ]]>
    /// </code>
    /// </example>
    public class EqualityConverter : ChainedConverterExtensionBase, IValueConverter
    {
        #region Predefined

        public static EqualityConverter ToVisibility => new EqualityConverter { TrueValue = Boxed.Visible, FalseValue = Boxed.Collapsed };
        public static EqualityConverter Negative => new EqualityConverter { IsNegative = true };

        #endregion

        private object _nullValue;

        /// <summary>
        /// C'tor
        /// </summary>
        public EqualityConverter()
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="compareTo">The value to compare to</param>
        public EqualityConverter(object compareTo)
        {
            CompareTo = compareTo;
        }

        /// <summary>
        /// Value to return when input value is null.
        /// </summary>
        /// <remarks>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// <para/>
        /// <para>Only when setting a value the converter supports conversion from null value.</para>
        /// <para>(or alternatively, if setting IsNullable property to true)</para>
        /// </remarks>
        public object NullValue
        {
            get { return _nullValue; }
            set
            {
                _nullValue = value;
                IsNullable = true;
            }
        }

        /// <summary>
        /// Determines if to support comparing to null and convert a null value to NullValue property
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// <para>The value to return if input value equals to <see cref="EqualityConverter.CompareTo"/> property.</para>
        /// <para>Unless <see cref="EqualityConverter.IsNegative"/> is true,</para>
        /// <para>or if value is null and <see cref="EqualityConverter.IsNullable"/> is true.</para>
        /// </summary>
        /// <remarks>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object TrueValue { get; set; } = Boxed.True;

        /// <summary>
        /// <para>The value to return if input value NOT equals to <see cref="EqualityConverter.CompareTo"/> property.</para>
        /// <para>Unless <see cref="EqualityConverter.IsNegative"/> is true,</para>
        /// <para>or if value is null and <see cref="EqualityConverter.IsNullable"/> is true.</para>
        /// </summary>
        /// <remarks>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object FalseValue { get; set; } = Boxed.False;

        /// <summary>
        /// The value to compare to the input value passed to the Convert method.
        /// </summary>
        /// <remarks>
        /// <para>Property can be set to special value as follow:</para>
        /// <para><see cref="ConverterSpecialValue.UseConverterParameter"/> to compare to the converter-parameter value of the Convert method.</para>
        /// </remarks>
        public object CompareTo { get; set; }

        /// <summary>
        /// Determine if to treat the equality check as not equals.
        /// </summary>
        [ConstructorArgument("isNegative")]
        public bool IsNegative { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result;
            if (IsNullable && ReferenceEquals(null, value))
            {
                result = NullValue;
            }
            else
            {
                var compareTo = CompareTo;
                compareTo = Equals(compareTo, ConverterSpecialValue.UseConverterParameter) ? parameter : compareTo;
                bool equals = Equals(value, compareTo);

                result = (equals != IsNegative) ? TrueValue : FalseValue;
            }
            return ConverterSpecialValue.Apply(result, value, parameter);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result;
            var compareTo = IsNegative ? FalseValue: TrueValue;
            compareTo = ConverterSpecialValue.Apply(compareTo, value, parameter);
            if (Equals(value, compareTo))
            {
                result = CompareTo;
            }
            else if (IsNullable && Equals(value, ConverterSpecialValue.Apply(NullValue, value, parameter)))
            {
                result = null;
            }
            else
            {
                result = value;
            }

            return ConverterSpecialValue.Apply(result, value, parameter);
        }

        protected override IValueConverter Converter => this;
    }

    /// <summary>
    /// <para>Check equality of converted value with <see cref="EqualityConverter.CompareTo" /> property</para>
    /// <para>and return <see cref="System.Windows.Visibility"/> as result.</para>
    /// </summary>
    public class EqualityToVisibilityConverterExtension: ChainedConverterExtensionBase
    {
        private Lazy<EqualityConverter> _innerConverter = new Lazy<EqualityConverter>();

        private object _nullValue;

        /// <summary>
        /// C'tor
        /// </summary>
        public EqualityToVisibilityConverterExtension()
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="useHidden">Should use Visibility.Hidden instead of Visibility.Collapsed</param>
        public EqualityToVisibilityConverterExtension(bool useHidden)
        {
            UseHidden = useHidden;
        }

        /// <summary>
        /// Value to return when input value is null.
        /// </summary>
        /// <remarks>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// <para/>
        /// <para>Only when setting a value the converter supports conversion from null value.</para>
        /// <para>(or alternatively, if setting IsNullable property to true)</para>
        /// </remarks>
        public object NullValue
        {
            get { return _nullValue; }
            set
            {
                _nullValue = value;
                IsNullable = true;
            }
        }

        /// <summary>
        /// Determines if to support comparing to null and convert a null value to NullValue property
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// The value to compare to the input value passed to the Convert method.
        /// </summary>
        /// <remarks>
        /// <para>Property can be set to special value as follow:</para>
        /// <para><see cref="ConverterSpecialValue.UseConverterParameter"/> to compare to the converter-parameter value of the Convert method.</para>
        /// </remarks>
        public object CompareTo { get; set; }

        /// <summary>
        /// Determine if to treat the equality check as not equals.
        /// </summary>
        public bool IsNegative { get; set; }

        /// <summary>
        /// When true, <see cref="Visibility.Hidden"/> is used instead of <see cref="Visibility.Collapsed"/>
        /// </summary>
        [ConstructorArgument("useHidden")]
        public bool UseHidden { get; set; }

        protected override IValueConverter Converter
        {
            get
            {
                var converter = _innerConverter.Value;
                converter.IsNegative = IsNegative;
                if (IsNullable) converter.NullValue = NullValue;
                converter.TrueValue = Boxed.Visible;
                converter.FalseValue = UseHidden ? Boxed.Hidden : Boxed.Collapsed;

                return converter;
            }
        }
    }

    /// <summary>
    /// Checks if converted value is null and returns the <see cref="TargetNullValueConverterExtension.Value"/> property, otherwise returns the converted value as is.
    /// </summary>
    /// <remarks>
    /// <para>The converter behavior is resemble to <see cref="System.Windows.Data.BindingBase.TargetNullValue"/> in that it only convert null value,</para>
    /// <para>and keep the value untouched otherwise</para>
    /// </remarks>
    public class TargetNullValueConverterExtension : ChainedConverterExtensionBase
    {
        private Lazy<EqualityConverter> _innerConverter = new Lazy<EqualityConverter>();

        /// <summary>
        /// C'tor
        /// </summary>
        public TargetNullValueConverterExtension()
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">Value to return when converting null</param>
        public TargetNullValueConverterExtension(object value)
        {
            Value = value;
        }

        /// <summary>
        /// The value to return if input value is null.
        /// </summary>
        /// <remarks>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        [ConstructorArgument("value")]
        public object Value { get; set; }

        protected override IValueConverter Converter
        {
            get
            {
                var converter = _innerConverter.Value;
                converter.TrueValue = Value;
                converter.FalseValue = ConverterSpecialValue.UseInputValue;

                return converter;
            }
        }
    }

    /// <summary>
    /// Convert a null to false and non-null value to true
    /// </summary>
    public class IsNotNullConverterExtension : ChainedConverterExtensionBase
    {
        private static EqualityConverter _converter = EqualityConverter.Negative;
        protected override IValueConverter Converter => _converter;
    }

    /// <summary>
    /// Converts null value to <see cref="Visibility.Collapsed"/> and non-null value to <see cref="Visibility.Visible"/>
    /// </summary>
    public class NullToInvisibleConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly EqualityConverter _converter = new EqualityConverter { TrueValue = Boxed.Collapsed, FalseValue = Boxed.Visible };
        protected override IValueConverter Converter => _converter;
    }

}
