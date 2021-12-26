using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// Groups several value-converters together.
    /// </summary>
    /// <remarks>
    /// <para>Performs Convert from first to last converter and ConvertBack from last to first-converter.</para>
    /// <para>If one of the converter's return value is either Binding.DoNothing or DependencyProperty.UnsetValue, the conversion stop.</para>
    /// <para>When converting back, if one of the converter implement interface ICantConvertBack, it's skipped from conversion</para>
    /// </remarks>
    /// <example>
    /// Usage in XAML
    /// <code>
    /// <![CDATA[
    /// <!-- Convert from nullable int property. 
    ///      First converter, converts null to zero, otherwise keep the same value.
    ///      Second converter, converts the result of previous converter (int), by casting to specified Enum type
    /// -->
    /// <Control.Resources>
    ///     <conv:GroupConverter x:Key="NullSafeNumberToMyEnum">
    ///         <conv:NullConverter TrueValue="{z:Int 0}" FalseValue="{conv:UseInputValue}" />
    ///         <conv:CastConverter ToType="local:MyEnum" />
    ///     </conv:GroupConverter>
    /// </Control.Resources>
    /// <ContentControl Content="{Binding NullableInt, Converter={StaticResouces NullSafeNumberToMyEnum}" />
    /// ]]>
    /// </code>
    /// </example>
    [ContentProperty("Converters")]
    public class GroupConverter : IValueConverter, IEnumerable, IEnumerable<IValueConverter>
    {
        private Lazy<Collection<IValueConverter>> _converters = new Lazy<Collection<IValueConverter>>();

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!HasAnythingToConvert) return value;

            foreach (var converter in _converters.Value ?? Enumerable.Empty<IValueConverter>())
            {
                value = converter.Convert(value, targetType, parameter, culture);

                if (Equals(value, Binding.DoNothing) || Equals(value, DependencyProperty.UnsetValue)) break;
            }
            return value;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!HasAnythingToConvert) return Binding.DoNothing;

            for (int i = Converters.Count - 1; i >= 0; i--)
            {
                var converter = Converters[i];
                if (!(converter is ICantConvertBack))
                {
                    value = converter.ConvertBack(value, targetType, parameter, culture);

                    if (Equals(value, Binding.DoNothing) || Equals(value, DependencyProperty.UnsetValue)) break;
                }
            }
            return value;
        }

        public Collection<IValueConverter> Converters => _converters.Value;

        public void Add(IValueConverter converter) => Converters.Add(converter);

        public void Insert(int index, IValueConverter converter) => Converters.Insert(index, converter);

        private bool HasAnythingToConvert => _converters.IsValueCreated && _converters.Value.Count > 0;

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<IValueConverter>).GetEnumerator();

        IEnumerator<IValueConverter> IEnumerable<IValueConverter>.GetEnumerator() => (HasAnythingToConvert) 
            ? Converters.GetEnumerator() 
            : Enumerable.Empty<IValueConverter>().GetEnumerator();
    }
}
