using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WpfMart.Utils;

namespace WpfMart.Converters
{
    /// <summary>
    /// Base class for converters that checks whether converted value is within specified range.
    /// </summary>
    /// <typeparam name="T">Type of value for the range</typeparam>
    public abstract class InRangeConverter<T>: ChainedConverterExtensionBase, IValueConverter, ICantConvertBack
    {
        internal protected struct Boundary
        {
            public Boundary(T value, bool isInclusive = true)
            {
                Value = value;
                IsInclusive = isInclusive;
            }

            public T Value { get; private set; }
            public bool IsInclusive { get; private set; }
        }

        private Boundary? _lower;
        private Boundary? _upper;
        private object _nullValue;

        /// <summary>
        /// C'tor
        /// </summary>
        public InRangeConverter()
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="InRangeConverter{T}.IsNegative"/> property</param>
        public InRangeConverter(bool isNegative)
        {
            IsNegative = isNegative;
        }

        /// <summary>
        /// <para>Lower bound of the range, including the specified value.</para>
        /// <para>Must be less than or equals to the value specified in either <see cref="InRangeConverter{T}.To"/> property or</para>
        /// <para><see cref="InRangeConverter{T}.Before"/> property</para>
        /// </summary>
        /// <remarks>
        /// <para>Converter will check if  the input value is equals or greater than this property value.</para>
        /// <para>This property cannot be set together with the <see cref="InRangeConverter{T}.After"/> property, as they're mutually exclusive. </para>
        /// <para>When not set (default) or set to null (if possible) there is no lower bound.</para>
        /// </remarks>
        public T From
        {
            get
            {
                return (_lower == null) ? default(T) : _lower.Value.Value;
            }
            set { _lower = ReferenceEquals(value, null)? (Boundary?)null : new Boundary(value); }
        }

        /// <summary>
        /// Upper bound of the range, including the specified value.
        /// </summary>
        /// <remarks>
        /// <para>Converter will check if the input value is equals or less than this property value.</para>
        /// <para>This property cannot be set together with the <see cref="InRangeConverter{T}.Before"/> property, as they're mutually exclusive.</para>
        /// <para>When not set (default) or set to null (if possible) there is no upper bound.</para>
        /// </remarks>
        public T To
        {
            get { return (_upper == null) ? default(T) : _upper.Value.Value; }
            set { _upper = ReferenceEquals(value, null) ? (Boundary?)null : new Boundary(value); }
        }

        /// <summary>
        /// <para>Lower bound of the range, excluding the specified value.</para>
        /// <para>Must be less than the value specified in either <see cref="InRangeConverter{T}.To"/> property or</para>
        /// <para><see cref="InRangeConverter{T}.Before"/> property</para>
        /// </summary>
        /// <remarks>
        /// <para>Converter will check if the input value is greater than this property value.</para>
        /// <para>This property cannot be set together with the <see cref="InRangeConverter{T}.From"/> property, as they're mutually exclusive. </para>
        /// <para>When not set (default) or set to null (if possible) there is no lower bound.</para>
        /// </remarks>
        public T After
        {
            get { return (_lower == null) ? default(T) : _lower.Value.Value; }
            set { _lower = ReferenceEquals(value, null) ? (Boundary?)null : new Boundary(value, false); }
        }

        /// <summary>
        /// Upper bound of the range, excluding the specified value.
        /// </summary>
        /// <remarks>
        /// <para>Converter will check if the input value is less than this property value.</para>
        /// <para>This property cannot be set together with the <see cref="InRangeConverter{T}.To"/> property, as they're mutually exclusive.</para>
        /// <para>When not set (default) or set to null (if possible) there is no upper bound.</para>
        /// </remarks>
        public T Before
        {
            get { return (_upper == null) ? default(T) : _upper.Value.Value; }
            set { _upper = ReferenceEquals(value, null)? (Boundary?)null : new Boundary(value, false); }
        }

        /// <summary>
        /// Determines if to check that converted value is not in range.
        /// (switch between TrueValue and FalseValue)
        /// </summary>
        [ConstructorArgument("isNegative")]
        public bool IsNegative { get; set; }

        /// <summary>
        /// Value to return when converted value is within range (or not in range if IsNegative property is set to true).
        /// </summary>
        /// <remarks>
        /// <para>Can use special values such as:</para>
        /// <para>- <see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para>- <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para>- <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object TrueValue { get; set; } = Boxed.True;

        /// <summary>
        /// Value to return when converted value is not within range (or in range if IsNegative property is set to true).
        /// </summary>
        /// <remarks>
        /// <para>Can use special values such as:</para>
        /// <para>- <see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para>- <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para>- <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
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
        /// Determines if to support comparing to null and convert a null value to NullValue property
        /// </summary>
        public bool IsNullable { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            bool isNegative = (parameter is bool) ? (bool)parameter : IsNegative;
            if (ReferenceEquals(null, value))
            {
                result = (IsNullable) ? NullValue : IsNegative ? TrueValue : FalseValue;
            }
            else
            {
                result = (IsInRange(value, _lower, _upper, culture) != isNegative) ? TrueValue : FalseValue;
            }

            return ConverterSpecialValue.Apply(result, value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        protected override IValueConverter Converter => this;

        protected virtual bool IsInRange(object value, Boundary? lower, Boundary? upper, CultureInfo culture)
        {
            if (lower == null && upper == null) return false;

            object castedValue = null;
            bool lowerMatches = true, upperMatches = true;

            if (lower != null)
            {
                TryChangeType(value, lower.Value.Value.GetType(), out castedValue, culture);
                lowerMatches = IsValueWithinLowerBound(castedValue, lower.Value);
            }
            if (upper != null && lowerMatches)
            {
                var upperType = upper.Value.Value.GetType();
                if (castedValue == null || !upperType.IsAssignableFrom(castedValue.GetType()))
                {
                    TryChangeType(value, upperType, out castedValue, culture);
                }
                upperMatches = IsValueWithinUpperBound(castedValue, upper.Value);
            }

            return lowerMatches && upperMatches;
        }

        protected virtual int Compare(object valueAfterCastAttempt, T bound)
        {
            return Comparer.Default.Compare(valueAfterCastAttempt, bound);
        }

        protected virtual bool IsValueWithinLowerBound(object valueAfterCastAttempt, Boundary lower)
        {
            var compare = Compare(valueAfterCastAttempt, lower.Value);
            return (lower.IsInclusive) ? compare >= 0 : compare > 0;
        }

        protected virtual bool IsValueWithinUpperBound(object valueAfterCastAttempt, Boundary upper)
        {
            var compare = Compare(valueAfterCastAttempt, upper.Value);
            return (upper.IsInclusive) ? compare <= 0 : compare < 0;
        }

        protected virtual bool TryChangeType(object value, Type targetType, out object castedValue, CultureInfo culture)
        {
            return ConvertHelper.TryChangeType(value, targetType, out castedValue, culture);
        }
    }

    /// <summary>
    /// <para>Check if value is in range of lower and/or upper bounds and returns</para>
    /// <para><see cref="InRangeConverter{T}.TrueValue"/> property if in range, </para> 
    /// <para>otherwise returns <see cref="InRangeConverter{T}.FalseValue"/> property.</para>
    /// </summary>
    /// <remarks>
    /// <para>The converter's range of values to check, is specified by providing lower and upper bounds of the range,</para>
    /// <para>using properties:</para>
    /// <para><see cref="InRangeConverter{T}.From"/> for inclusive lower bound,</para>
    /// <para>or <see cref="InRangeConverter{T}.After"/> for exclusive lower bound.</para>
    /// <para><see cref="InRangeConverter{T}.To"/> for inclusive upper bound,</para>
    /// <para>or <see cref="InRangeConverter{T}.Before"/> for exclusive upper bound.</para>
    /// <para/>
    /// <para>The converter can be used to check if value is greater than or less than, by only specifying one of the bounds (lower or upper) as follow:</para>
    /// <para>For greater than or equals condition, use <see cref="InRangeConverter{T}.From"/> property.</para>
    /// <para>For greater than condition, use <see cref="InRangeConverter{T}.After"/> property.</para>
    /// <para>For less than or equals condition, use <see cref="InRangeConverter{T}.To"/> property.</para>
    /// <para>For less than condition, use <see cref="InRangeConverter{T}.Before"/> property.</para>
    /// <para/>
    /// <para>A special care can the done when converting from a null,</para>
    /// <para>either by setting <see cref="InRangeConverter{T}.NullValue"/> property to a desired value (can also set to {x:null}),</para>
    /// <para>or set <see cref="InRangeConverter{T}.IsNullable"/> property to true and by that, NullValue property will have its default value of null.</para>
    /// <para/>
    /// <para>Converter can be negative and thus check value is not in range.</para>
    /// <para>Note: if converting from a null and it's not supported (see above how to support null),</para>
    /// <para>then null is always considered as not in range.</para>
    /// <para/>
    /// <para>When used as markup-extension it's possible to chain another converter to convert from,</para>
    /// <para>using the <see cref="ChainedConverterExtensionBase.FromConverter"/> property.</para>
    /// <para>When not used as markup-extension, wrap with <see cref="GroupConverter"/> to chain several converters.</para>
    /// <para/>
    /// <para>* For numeric ranges, prefer using <see cref="NumInRangeConverter"/> instead.</para>
    /// <para>* For date-time ranges, prefer using <see cref="DateInRangeConverter"/> instead.</para>
    /// <para>* Avoid setting lower bound to be less than or equals to the upper bound, as the converter wo'nt function as expected.</para>
    /// <para>* in case of the value in <see cref="InRangeConverter{T}.From"/> property and <see cref="InRangeConverter{T}.To"/> property,</para>
    /// <para>  It's same as comparing to a specific value, therefore prefer using <see cref="WpfMart.Converters.EqualityConverter"/> converter.</para>
    /// </remarks>
    /// <example>
    /// <para>Using <see cref="NumInRangeConverter"/></para>
    /// <para>Indicate when Angle property (double) on view-model is not within [0 - 360) comfort zone.</para>
    /// <code>
    /// <![CDATA[
    /// <TextBlock Text="Angle will be reduced to value between 0 to 360 (not including)"
    ///     Visibility="{Binding Angle, Converter={conv:NumInRangeConverter 
    ///         IsNegative=True, From=0, Before=360,TrueValue=Visible, FalseValue=Collapsed}}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Using <see cref="DateInRangeConverter"/>
    /// <code>
    /// <![CDATA[
    /// <TextBlock Text="Windows XP updates are only available after October 25, 2001 and before April 8, 2014"
    ///     Visibility="{Binding OSVersionDate, Converter={conv:DateInRangeConverter 
    ///         IsNegative=True, After=2001-10-25, Before=2014-04-8, TrueValue=Visible, FalseValue=Collapsed}}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// <para>1. In case customer-type is between Loyal to VIP (CustomerType enum) allow it to redeem one-time 5% discount.</para>
    /// <para>2. Allow to enter a coupon-code only from software version 2.5 (before that it wasn't supported).</para>
    /// <para>2.1. Coupon code string must start with a digit between 1 and 9. otherwise show 'invalid' background color.</para>
    /// <para>2.1.1 if coupon code not entered (null or empty string) don't show as invalid background.</para>
    /// <code>
    /// <![CDATA[
    /// enum CustomerType { Regular, Repeating, Loyal, VIP, OurEmployee}
    /// ]]>
    /// </code>
    /// <code>
    /// <![CDATA[
    /// <ListBox HorizontalContentAlignment="Stretch">
    ///     <CheckBox IsChecked = "{Binding RedeemValuableCustomerDiscount}" Content="Redeem valuable customer 5% discount"
    ///         Visibility="{Binding CustomerType, Converter={conv:InRangeConverter 
    ///             From={x:Static local:CustomerType.Loyal}, 
    ///             To={x:Static local:CustomerType.VIP},
    ///             TrueValue=Visible, FalseValue=Collapsed}}" />
    ///             
    ///     <TextBlock Text="Coupon code:" />
    ///     
    ///     <TextBox Text="{Binding CouponCode, UpdateSourceTrigger=PropertyChanged}"
    ///         IsEnabled="{Binding SoftwareVersion, Converter={conv:InRangeConverter From={z:Version 2.5.0.0}}}" 
    ///         Background="{Binding CouponCode,Converter={conv:InRangeConverter NullValue = {conv:UnsetValue},
    ///             From=1, Before=a, TrueValue={conv:UnsetValue}, FalseValue=Coral,
    ///             FromConverter={conv:NullOrEmptyConverter TrueValue={x:Null}, FalseValue={conv:UseInputValue }}}}" />
    /// </ListBox>
    /// ]]>
    /// </code>
    /// <para>1. The discount CheckBox.Visibility bound to InRangeConverter which is set for a range of enum values for customer-type.</para>
    /// <para>   Looking at the enum possible values it appear that customer-types Regular and OurEmployee won't get the discount.</para>
    /// <para>2. The coupon TextBox.Background bound to InRangeConverter which is set for a range of strings which</para>
    /// <para>   equals or greater then "1" but less than "a".</para>
    /// <para>   It's not set to be less or equals to "9" since string "9" is less than string "9coupon" which is also valid coupon code,</para>
    /// <para>   So since string "a" is one order grater than string "9" the converter upper bound is before "a"</para>
    /// <para>2.1 In order to set background color only when condition met, BUT keep the default background (which can be anything) otherwise,</para>
    /// <para>    the converter uses {conv:UnsetValue} markup-extension which is simply shorthand for {x:Static DependencyProperty.UnsetValue}</para>
    /// <para>    that when returned by the converter, causes a dependency property to fallback to its default value</para>
    /// <para>2.1.1 Since coupon code can be null or empty, to which the converter will treat as out of range,</para>
    /// <para>      we manipulate the converter to treat null input value is default background,</para>
    /// <para>      And ensure that in case it's empty string, and inner converter will convert it to null, to satisfy the out converter.</para>
    /// </example>
    public class InRangeConverter : InRangeConverter<object>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public InRangeConverter()
        {
        }
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="BoolConverter.IsNegative"/> property</param>
        public InRangeConverter(bool isNegative): base(isNegative)
        {
        }
    }

    /// <summary>
    /// InRangeConverter for DateTime values
    /// </summary>
    public class DateInRangeConverter: InRangeConverter<DateTime>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public DateInRangeConverter()
        {
        }
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="BoolConverter.IsNegative"/> property</param>
        public DateInRangeConverter(bool isNegative): base(isNegative)
        {
        }
    }


    /// <summary>
    /// InRangeConverter for numeric values
    /// </summary>
    public class NumInRangeConverter : InRangeConverter<Decimal>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public NumInRangeConverter()
        {
        }
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="BoolConverter.IsNegative"/> property</param>
        public NumInRangeConverter(bool isNegative): base(isNegative)
        {
        }
    }

}
