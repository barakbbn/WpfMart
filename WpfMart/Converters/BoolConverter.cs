using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// Converts a bool or nullable bool value to another value specified by properties TrueValue, FalseFalue and NullValue
    /// </summary>
    /// <remarks>
    /// <para>by default converts a boolean true to a value specified by the TrueValue property,</para>
    /// <para>otherwise to a value specified in the FalseValue property.</para>
    /// <para/>
    /// <para>by default converting null is not supported and considered as false.</para>
    /// <para/>
    /// <para>Converter can be negative, meaning comparing to false instead of true.</para>
    /// <para>There is also a predefined <see cref="NegativeBoolConverterExtension"/> used to perform negative boolean conversion.</para>
    /// <para/>
    /// <para>To support nullable bool, either set NullValue to a desired value (can also set null),</para>
    /// <para>or set IsNullable to true and by that, NullValue property will have its default value of null.</para>
    /// <para/>
    /// <para>When used as markup-extension it's possible to chain another converter to convert from,</para>
    /// <para>using the <see cref="ChainedConverterExtensionBase.FromConverter"/> property.</para>
    /// <para>* When not used as markup-extension, wrap with <see cref="GroupConverter"/> to chain several converters.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// enum MachineState
    /// {
    ///    None,
    ///    [Description("Wax On")]
    ///    On,
    ///    [Description("Wax Off")]
    ///    Off,
    /// }
    /// </code>
    /// <code>
    /// <![CDATA[
    /// <Control.Resources>
    ///     <!-- Convert from bool to string value. -->
    ///     <conv:BoolConverter x:Key="BoolToOnOffConverter" TrueValue="On" FalseValue="Off" />
    ///                           
    ///    <!-- Convert from bool to enum value. -->
    ///    <conv:BoolConverter x:Key="IsCheckedToMachineStateEnum" 
    ///                           TrueValue="{x:Static local:MachineState.On}"
    ///                           FalseValue="{x:Static local:MachineState.Off}" />
    ///                           
    ///    <!-- Convert from true to false, false to true and keep null value unchanged. -->
    ///    <conv:BoolConverter x:Key="NegativeIsChecked" IsNegative="True" IsNullable="True"/>
    ///    
    ///    <!-- reversed conversion. From Combobox.SelectedItem enum value to Checkbox.IsChecked (nullable) -->
    ///    <conv:BoolConverter x:Key="EnumToIsCheckedConverter" 
    ///                            IsReversed="True"
    ///                            TrueValue="{x:Static local:MachineState.On}" 
    ///                            FalseValue="{x:Static local:MachineState.Off}"
    ///                            NullValue="{x:Static local:MachineState.None}" />
    /// </Control.Resources>
    /// <StackPanel>
    ///     <ComboBox x:Name="combobox" ItemsSource="{z:EnumValues local:MachineState}" SelectedIndex="0" />
    ///     <CheckBox IsThreeState="True" 
    ///               IsChecked="{Binding SelectedItem, ElementName=combobox, Converter={StaticResource EnumToIsCheckedConverter}}"/>
    /// </StackPanel>     
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="Markup.EnumValuesExtension"/>    
    [ValueConversion(typeof(bool?), typeof(object))]
    public class BoolConverter : ChainedConverterExtensionBase, IValueConverter
    {
        #region Predefined

        /// <summary>
        /// Predefined BoolConverter configured as Negative bool converter
        /// </summary>
        public static BoolConverter Negative => new BoolConverter
        {
            IsNegative = true
        };

        #endregion

        private object _nullValue;

        /// <summary>
        /// C'tor
        /// </summary>
        public BoolConverter() {}

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="BoolConverter.IsNegative"/> property</param>
        public BoolConverter(bool isNegative)
        {
            IsNegative = isNegative;
        }

        /// <summary>
        /// Determine if to reverse the conversion. Convert become ConvertBack and vise-versa
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// Determines if to compare to false instead of true when converting.
        /// (switch between TrueValue and FalseValue)
        /// </summary>
        [ConstructorArgument("isNegative")]
        public bool IsNegative { get; set; }

        /// <summary>
        /// Value to return when converting boolean true value. (of false if IsNegative property is set to true)
        /// <see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.
        /// <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property
        /// <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception
        /// </summary>
        public object TrueValue { get; set; } = Boxed.True;

        /// <summary>
        /// Value to return when for false value. (of true if IsNegative property is set to true).
        /// </summary>
        /// <remarks>
        /// <para><see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para><see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para><see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object FalseValue { get; set; } = Boxed.False;

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
        /// Determines if to support <see cref="System.Nullable{bool}" /> and convert null value to NullValue property.
        /// </summary>
        public bool IsNullable { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (IsReversed)
                ? ConvertBackInternal(value, targetType, parameter, culture)
                : ConvertInternal(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (IsReversed)
                ? ConvertInternal(value, targetType, parameter, culture)
                : ConvertBackInternal(value, targetType, parameter, culture);
        }

        private object ConvertInternal(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            if (IsNullable && ReferenceEquals(null, value))
            {
                result = NullValue;
            }
            else
            {
                bool isNegative = (parameter is bool) ? (bool)parameter : IsNegative;
                bool equals = Equals(value, Boxed.True);
                result = (equals != isNegative) ? TrueValue : FalseValue;
            }

            return ConverterSpecialValue.Apply(result, value, parameter);
        }

        private object ConvertBackInternal(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNegative = (parameter is bool) ? (bool)parameter : IsNegative;

            if (Equals(value, TrueValue))
            {
                return !isNegative;
            }
            if (IsNullable && Equals(value, NullValue))
            {
                return null;
            }

            return isNegative;
        }

        protected override IValueConverter Converter => this;
    }

    /// <summary>
    /// The familiar NegativeBooleanConverter, but with ability to be used as MarkupExtension and chain another converter
    /// </summary>
    public class NegativeBoolConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly BoolConverter _converter = BoolConverter.Negative;
        protected override IValueConverter Converter => _converter;
    }
}
