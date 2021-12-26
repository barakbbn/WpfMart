using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Converts a key to a mapped value.
    /// </summary>
    /// <remarks>
    /// <para>Define a dictionary of key-value pairs for which a converted value is considered a key in the dictionary </para>
    /// <para>and the matching value for the key is returned as result.</para>
    /// <para>The dictionary is defined in the same manner as a ResourceDictionary is defined in XAML.</para>
    /// </remarks>
    /// <example>
    /// Convert from MachineState enum value on view-model to CheckBox's IsChecked, Content and Foreground
    /// <code>
    /// <![CDATA[
    /// enum MachineState
    /// {
    ///    None,
    ///    [Description("Wax On")]
    ///    On,
    ///    [Description("Wax Off")]
    ///    Off,
    ///    [Description("Wax Burn")]
    ///    Faulted
    /// }
    /// ]]>
    /// </code>
    /// <code>
    /// <![CDATA[
    /// <Window.Resources>
    ///     <!-- Map two of the enum values to boolean true and false. have the rest of the enum values fallback to null .
    ///          When converting back, unmapped value is fallback to MachineState.None on the view-model's property -->
    ///    <conv:MapConverter x:Key="machineStateToIsChecked" FallbackValue="{x:Null}" ConvertBackFallbackValue="{x:Static local:MachineState.None}" >
    ///        <sys:Boolean x:Key="{x:Static local:MachineState.On}" >True</sys:Boolean>
    ///        <sys:Boolean x:Key="{x:Static local:MachineState.Off}" >False</sys:Boolean>
    ///    </conv:MapConverter>
    ///    
    ///     <!-- Map enum  values to brushes used by CheckBox.Foreground property.
    ///          unmapped enum values are fallback to the Foreground default value by using DependencyProperty.UnsetValue.
    ///    <conv:MapConverter x:Key="machineStateToForeground" FallbackValue="{x:Static DependencyProperty.UnsetValue}">
    ///        <SolidColorBrush x:Key="{x:Static local:MachineState.On}" Color="Green" />
    ///        <SolidColorBrush x:Key="{x:Static local:MachineState.Off}" Color="Red" />
    ///        <SolidColorBrush x:Key="{x:Static local:MachineState.Faulted}" Color="Orange" />
    ///    </conv:MapConverter>
    ///
    ///     <!-- Map from a brush to a string of the brush's color name, converting from a CheckBox.Foreground to CheckBox.Content.
    ///         the mapped keys are the Brush.ToString().
    ///         unmapped keys fallback to a dahes rectangle.
    ///    <conv:MapConverter x:Key="foregroundToTextConverter" NullValue="{x:Null}" >
    ///        <sys:String x:Key="#FF008000" >Green</sys:String>
    ///        <sys:String x:Key="#FFFF0000" >Red</sys:String>
    ///        <sys:String x:Key="#FFFFA500" >Orange</sys:String>
    ///
    ///        <conv:MapConverter.FallbackValue>
    ///            <Rectangle Width = "50" Height="2" StrokeThickness="2" Stroke="Blue" StrokeDashArray="1,2,1,2" Margin="1,7" />
    ///        </conv:MapConverter.FallbackValue>
    ///    </conv:MapConverter>
    /// </Window.Resources>
    /// 
    /// <CheckBox x:Name="MachineStateToThreeStateCheck" Grid.Row="0" Grid.Column="0"
    ///           IsThreeState="True"
    ///           IsChecked="{Binding MachineState, Converter={StaticResource machineStateToIsChecked}}"
    ///           Content="{Binding Foreground, RelativeSource={RelativeSource Self}, Mode=OneWay, Converter={StaticResource foregroundToTextConverter}}"
    ///           Foreground="{Binding MachineState, Mode=OneWay, Converter={StaticResource machineStateToForeground}}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="System.Windows.DependencyProperty.UnsetValue"/>
    public class MapConverter: IDictionary, IValueConverter
    {
        private IDictionary _dictionary = new Dictionary<object, object>();
        private object _nullValue;

        /// <summary>
        /// Instantiate a new MapConverter object
        /// </summary>
        public MapConverter() {}

        /// <summary>
        /// Value to return when input value is null.
        /// <see cref="System.Windows.DependencyProperty.UnsetValue"/> to cause dependency property to have its default value.
        /// <see cref="Binding.DoNothing"/> to cancel the conversion and not set a value on the target property
        /// <see cref="ConverterSpecialValue"/> to return the input value of the Convert method or the converter-parameter, or throw exception
        /// </summary>
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
        /// <para>Value to return when no matching value found in the dictionary for the converted key.</para>
        /// <para>By default, return the key (the value parameter passed to the Convert method)</para>
        /// </summary>
        public object FallbackValue { get; set; } = ConverterSpecialValue.UseInputValue;

        /// <summary>
        /// <para>Value to return when no matching value found in dictionary when converting-back.</para>
        /// <para>By default, it cancels the conversion (Return Binding.DoNothing)</para>
        /// </summary>
        public object ConvertBackFallbackValue { get; set; } = Binding.DoNothing;

        #region IDictionary

        public object this[object key]
        {
            get
            {
                return _dictionary[key];
            }

            set
            {
                _dictionary[key] = value;
            }
        }
        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }
        public bool IsFixedSize
        {
            get
            {
                return _dictionary.IsFixedSize;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return _dictionary.IsReadOnly;
            }
        }
        public bool IsSynchronized
        {
            get
            {
                return _dictionary.IsSynchronized;
            }
        }
        public ICollection Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }
        public object SyncRoot
        {
            get
            {
                return _dictionary.SyncRoot;
            }
        }
        public ICollection Values
        {
            get
            {
                return _dictionary.Values;
            }
        }
        public void Add(object key, object value)
        {
            _dictionary.Add(key, value);
        }
        public void Clear()
        {
            _dictionary.Clear();
        }
        public bool Contains(object key)
        {
            return _dictionary.Contains(key);
        }
        public void CopyTo(Array array, int index)
        {
            _dictionary.CopyTo(array, index);
        }
        public IDictionaryEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }
        public void Remove(object key)
        {
            _dictionary.Remove(key);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_dictionary as IEnumerable).GetEnumerator();
        }

        #endregion

        private bool IsNullable { get; set; }

        /// <summary>
        /// In case a value found in the Dictionary is IValueConverter, should call Convert on it as well?
        /// </summary>
        public bool ConvertAlsoFoundValueConverter { get; set; } = true;

        #region IValueConverter

        /// <summary>
        /// Converts an input value by looking it up as a key in dictionary and returning the mapped value for it.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value;
            if (ReferenceEquals(null, value))
            {
                return (IsNullable) ? NullValue : FallbackValue;
            }

            var foundValue = FallbackValue;
            if (this.Contains(key))
            {
                foundValue = this[key];
            }
            else if (this.Contains(key.ToString()))
            {
                foundValue = this[key.ToString()];
            }

            var converter = (foundValue as IValueConverter);
            if (ConvertAlsoFoundValueConverter && converter != null) foundValue = converter.Convert(value, targetType, parameter, culture);

            return ConverterSpecialValue.Apply(foundValue, value, parameter);
        }

        /// <summary>
        /// Converts back, by looking in the dictionary for matching value and returning the key of the matched value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = ConvertBackFallbackValue;
            if (IsNullable && Equals(NullValue, value))
            {
                result = null;
            }
            else
            {
                var it = GetEnumerator();
                while (it.MoveNext())
                {
                    if (Equals(value, it.Value))
                    {
                        result = it.Key;
                        break;
                    }
                }
            }

            result = ConverterSpecialValue.Apply(result, value, parameter);

            return result;
        }

        #endregion
    }
}
