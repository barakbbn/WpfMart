using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using WpfMart.Converters;
using WpfMart.Utils;

namespace WpfMart.Converters
{
    /// <summary>
    /// Checks whether the first value in the array of values to convert, is within range specified by the other values in the array.
    /// </summary>
    /// <remarks>
    /// <para>The value to convert is the item in values[0] of the values parameter passed to the <see cref="InRangeMultiConverter.Convert"/> method.</para>
    /// <para>By default values[1] is the lower bound of the range and values[2] is the upper bound of the range</para>
    /// <para />
    /// <para>To specify if lower and/or upper bounds inclusive or exclusive,</para>
    /// <para>set properties <see cref="InRangeMultiConverter.LowerInclusive"/> and/or <see cref="InRangeMultiConverter.LowerInclusive"/>.</para>
    /// <para />
    /// <para>To use only lower or only upper bounds, ensure only 2 items provided to be converter (instead of 3),</para>
    /// <para>and set only one of the properties </para>
    /// <para><see cref="InRangeMultiConverter.LowerInclusive"/> or <see cref="InRangeMultiConverter.LowerInclusive"/></para>
    /// <para>(otherwise it defaults to lower inclusive)</para>
    /// <para />
    /// </remarks>
    /// <example>
    /// Show warning TextBlock in case selected From-date is not less than To-date
    /// <code>
    /// <![CDATA[
    /// <StackPanel Orientation="Horizontal">
    ///     <DatePicker x:Name="FromDatePicker" />
    ///     <DatePicker x:Name="ToDatePicker" />
    ///     <TextBlock Text="From date should be less than To date">
    ///         <TextBlock.Visibility>
    ///             <MultiBinding Converter="{conv:InRangeMultiConverter UpperInclusive=True, TrueValue=Collapsed, FalseValue=Visible}" >
    ///                 <Binding Path="SelectedDate" ElementName="FromDatePicker" />
    ///                 <Binding Path="SelectedDate" ElementName="ToDatePicker" />
    ///             </MultiBinding>
    ///         </TextBlock.Visibility>
    ///     </TextBlock>
    /// </StackPanel>
    /// ]]>
    /// </code>
    /// </example>
    [MarkupExtensionReturnType(typeof(IMultiValueConverter))]
    public class InRangeMultiConverter : MarkupExtension, IMultiValueConverter, ICantConvertBack
    {
        private Lazy<InRangeConverter> _innerConverter = new Lazy<InRangeConverter>();

        /// <summary>
        /// C'tor
        /// </summary>
        public InRangeMultiConverter()
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="isNegative">Specify if to construct the converter as negative or not, by setting the <see cref="InRangeMultiConverter.IsNegative"/> property</param>
        public InRangeMultiConverter(bool isNegative)
        {
            IsNegative = isNegative;
        }

        /// <summary>
        /// <para>Determine if lower limit is inclusive (greater than or equals to), otherwise exclusive (greater than).</para>
        /// <para>If set to null (default), it means there is no lower limit.</para>
        /// </summary>
        public bool? LowerInclusive { get; set; }

        /// <summary>
        /// <para>Determine if upper limit is inclusive (less than or equals to), otherwise exclusive (less than).</para>
        /// <para>If set to null (default), it means there is no upper limit.</para>
        /// </summary>
        public bool? UpperInclusive { get; set; }

        /// <summary>
        /// Value to return when converted value is within range (or not in range if IsNegative property is set to true).
        /// </summary>
        /// <remarks>
        /// <para>Can use special values such as:</para>
        /// <para>- <see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para>- <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para>- <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object TrueValue
        {
            get { return _innerConverter.Value.TrueValue; }
            set { _innerConverter.Value.TrueValue = value; }
        }

        /// <summary>
        /// Value to return when converted value is not within range (or in range if IsNegative property is set to true).
        /// </summary>
        /// <remarks>
        /// <para>Can use special values such as:</para>
        /// <para>- <see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.</para>
        /// <para>- <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property.</para>
        /// <para>- <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception.</para>
        /// </remarks>
        public object FalseValue
        {
            get { return _innerConverter.Value.FalseValue; }
            set { _innerConverter.Value.FalseValue = value; }
        }

        /// <summary>
        /// Determines if to check that converted value is not in range.
        /// (switch between TrueValue and FalseValue)
        /// </summary>
        [ConstructorArgument("isNegative")]
        public bool IsNegative
        {
            get { return _innerConverter.Value.IsNegative; }
            set { _innerConverter.Value.IsNegative = value; }
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
            get { return _innerConverter.Value.NullValue; }
            set { _innerConverter.Value.NullValue = value; }
        }

        /// <summary>
        /// Determines if to support comparing to null and convert a null value to NullValue property
        /// </summary>
        public bool IsNullable
        {
            get { return _innerConverter.Value.IsNullable; }
            set { _innerConverter.Value.IsNullable = value; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length > 2)
            {
                SetLower(values[1]);
                SetUpper(values[2]);
            }
            else if(LowerInclusive != null || UpperInclusive == null)
            {
                SetLower(values[1]);
            }
            else
            {
                SetUpper(values[1]);
            }

            var result = _innerConverter.Value.Convert(values[0], targetType, parameter, culture);

            //It seems that with multi-binding (as opposed to regular binding),
            //WPF doesn't convert result to target-type automatically.
            object castedResult;
            ConvertHelper.TryChangeType(result, targetType, out castedResult, culture);

            return castedResult;
        }

        private void SetLower(object limit)
        {
            if (ReferenceEquals(limit, null)) return;
            if (LowerInclusive != false)
            {
                _innerConverter.Value.From = limit;
            }
            else
            {
                _innerConverter.Value.After = limit;
            }
        }

        private void SetUpper(object limit)
        {
            if (ReferenceEquals(limit, null)) return;
            if (UpperInclusive != false)
            {
                _innerConverter.Value.To = limit;
            }
            else
            {
                _innerConverter.Value.Before = limit;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { Binding.DoNothing };
        }

        /// <summary>
        /// <see cref="System.Windows.Markup.MarkupExtension.ProvideValue(IServiceProvider)"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

    }
}
