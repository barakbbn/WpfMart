using System;
using System.Collections.Generic;
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
    /// Checks equality of two values provided by the 'values' parameter of the <see cref="EqualityMultiConverter.Convert(object[], Type, object, CultureInfo)"/> method.
    /// </summary>
    /// <remarks>
    /// <para>The first value is the item in values[0] and second value is the item in values[1].</para>
    /// <para/>
    /// <para>If special value <see cref="ConverterSpecialValue.UseInputValue"/> is used,</para>
    /// <para>It will refer to the value in values[0].</para>
    /// </remarks>
    /// <example>
    /// Show warning TextBlock in case selected From-date is not less than To-date
    /// <code>
    /// <![CDATA[
    /// <StackPanel>
    ///     <ComboBox x:Name="SaladDressing1" ItemsSource="{Binding AvailableSaladDressing}"/>
    ///     <ComboBox x:Name="SaladDressing2" ItemsSource="{Binding AvailableSaladDressing} />
    ///     
    ///     <TextBlock Text="Please notice you picked the same salad dressing for both options">
    ///         <TextBlock.Visibility>
    ///             <MultiBinding Converter="{conv:EqualityMultiConverter TrueValue=Collapsed, FalseValue=Visible}" >
    ///                 <Binding Path="SelectedItem" ElementName="SaladDressing1" />
    ///                 <Binding Path="SelectedItem" ElementName="SaladDressing2" />
    ///             </MultiBinding>
    ///         </TextBlock.Visibility>
    ///     </TextBlock>
    /// </StackPanel>
    /// ]]>
    /// </code>
    /// </example>
    public class EqualityMultiConverter: MarkupExtension, IMultiValueConverter, ICantConvertBack
    {
        /// <summary>
        /// The value to return if input value equals to <see cref="EqualityConverter.CompareTo"/> property.
        /// Property can be set to special value as follow:
        /// <see cref="DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.
        /// <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property
        /// <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception
        /// </summary>
        public object TrueValue { get; set; } = Boxed.True;

        /// <summary>
        /// The value to return if input value not equals to CompareToValue property.
        /// Property can be set to special value as follow:
        /// <see cref="DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.
        /// <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property
        /// <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception
        /// </summary>
        public object FalseValue { get; set; } = Boxed.False;

        /// <summary>
        /// Determine if to treat the equality check as not equals.
        /// </summary>
        [ConstructorArgument("isNegative")]
        public bool IsNegative { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object value = (values.Length > 0)? values[0]: null;
            bool equals = false;
            if (values.Length > 1)
            {
                var compareTo = ConverterSpecialValue.Apply(value, value, parameter);

                for (int i = 1; i < values.Length; i++)
                {
                    var value2 = values[i];
                    value2 = ConverterSpecialValue.Apply(value2, value2, parameter);
                    equals = Equals(compareTo, value2);
                    if (!equals) break;
                }
            }

            var result = (equals != IsNegative) ? TrueValue : FalseValue;

            result = ConverterSpecialValue.Apply(result, value, parameter);

            //It seems that with multi-binding (as opposed to regular binding),
            //WPF doesn't convert result to target-type automatically.
            object castedResult;
            ConvertHelper.TryChangeType(result, targetType, out castedResult, culture);

            return castedResult;
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
