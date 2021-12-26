using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// Converts boolean value to Visibility. true to Visible, false to Collapsed or Hidden
    /// </summary>
    /// <remarks>
    /// <para>Can use Hidden instead of Collapsed by setting the <see cref="BoolToVisibilityConverter.UseHidden"/> property to true.</para>
    /// <para/>
    /// <para>Converter can be negative, meaning comparing to false instead of true.</para>
    /// <para/>
    /// <para>Converter can reverse conversion and convert Visibility to boolean by setting the <see cref="BoolToVisibilityConverter.IsReversed"/> property to true.</para>
    /// <para/>
    /// <para>When used as markup-extension it's possible to chain another converter to convert from,</para>
    /// <para>using the <see cref="ChainedConverterExtensionBase.FromConverter"/>.</para>
    /// <para>When not used as markup-extension, use GroupConverter to chain several converters.</para>
    /// <para/>
    /// <para>There are predefined converters that can be used as markup-extensions:</para>
    /// <para>- <see cref="FalseToHiddenConverterExtension"/> - converts boolean false to <see cref="Visibility.Hidden"/>.</para>
    /// <para>- <see cref="TrueToCollapsedConverterExtension"/> - converts boolean true to <see cref="Visibility.Collapsed"/>.</para>
    /// <para>- <see cref="TrueToHiddenConverterExtension"/> - converts boolean true to <see cref="Visibility.Hidden"/>.</para>
    /// </remarks>
    /// <example>
    /// <para>When DevicesList property on view-model is either null or empty, show a text indicating that.</para>
    /// <para>Using chained converters, from NullOrEmptyConverter that returns true is collection is null or empty,</para>
    /// <para>through BoolToVisibilityConverter that convert the resulting boolean to Visibility.</para>
    /// <code>
    /// <![CDATA[
    /// <TextBlock Text="No devices" 
    ///     Visibility="{Binding DevicesList, Converter={conv:BoolToVisibilityConverter FromConverter={conv:NullOrEmptyConverter}}}"/>
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Using converter to convert true to Visibility.Collpased and false to Visibility.Visible using markup-extension
    /// <code>
    /// <![CDATA[
    /// <TextBlock Text="No restrictions" 
    ///     Visibility="{Binding IsRestricted, Converter={conv:TrueToCollapsedConverter}}"/>
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="Converters.NullOrEmptyConverter"/>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : ChainedConverterExtensionBase, IValueConverter
    {
        #region Predefined

        /// <summary>
        /// Predefined converter that convert boolean false value to <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Collapsed"/>
        /// </summary>
        public static BoolToVisibilityConverter HiddenWhenFalse => new BoolToVisibilityConverter { UseHidden = true };

        /// <summary>
        /// Predefined negative converter that convert boolean true value to <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Visible"/>
        /// </summary>
        public static BoolToVisibilityConverter HiddenWhenTrue => new BoolToVisibilityConverter { IsNegative = true, UseHidden = true };

        /// <summary>
        /// Predefined negative converter that convert boolean true value to <see cref="Visibility.Collapsed"/> instead of <see cref="Visibility.Visible"/>
        /// </summary>
        public static BoolToVisibilityConverter CollapedWhenTrue => new BoolToVisibilityConverter { IsNegative = true };

        #endregion

        private BoolConverter _converter = new BoolConverter { TrueValue = Boxed.Visible, FalseValue = Boxed.Hidden };

        /// <summary>
        /// C'tor
        /// </summary>
        public BoolToVisibilityConverter() {}

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="useHidden">Specify if <see cref="Visibility.Hidden"/> is used instead of <see cref="Visibility.Collapsed"/></param>
        public BoolToVisibilityConverter(bool useHidden)
        {
            UseHidden = useHidden;
        }

        /// <summary>
        /// When true, <see cref="Visibility.Hidden"/> is used instead of <see cref="Visibility.Collapsed"/>
        /// </summary>
        [ConstructorArgument("useHidden")]
        public bool UseHidden
        {
            get { return _converter.FalseValue == Boxed.Hidden; }
            set { _converter.FalseValue = value ? Boxed.Hidden : Boxed.Collapsed; }
        }

        /// <summary>
        /// Determines if to compare to false instead of true when converting and by that
        /// converting false to <see cref="Visibility.Visible"/>
        /// </summary>
        public bool IsNegative
        {
            get { return _converter.IsNegative; }
            set { _converter.IsNegative = value; }
        }

        /// <summary>
        /// Determine if to reverse the conversion. 
        /// Meaning, convert from Visibility to bool
        /// </summary>
        public bool IsReversed
        {
            get { return _converter.IsReversed; }
            set { _converter.IsReversed = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => _converter.Convert(value, targetType, parameter, culture);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => _converter.ConvertBack(value, targetType, parameter, culture);

        protected override IValueConverter Converter => this;
    }

    /// <summary>
    /// Converts boolean true value to <see cref="Visibility.Collapsed"/>
    /// and boolean false value to <see cref="Visibility.Visible"/>
    /// </summary>
    public class TrueToCollapsedConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly BoolToVisibilityConverter _converter = BoolToVisibilityConverter.CollapedWhenTrue;
        protected override IValueConverter Converter => _converter;
    }

    /// <summary>
    /// Converts boolean true value to <see cref="Visibility.Hidden"/>
    /// and boolean false value to <see cref="Visibility.Visible"/>
    /// </summary>
    public class TrueToHiddenConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly BoolToVisibilityConverter _converter = BoolToVisibilityConverter.HiddenWhenTrue;
        protected override IValueConverter Converter => _converter;
    }

    /// <summary>
    /// Converts boolean false value to <see cref="Visibility.Hidden"/>
    /// and boolean true value to <see cref="Visibility.Visible"/>
    /// </summary>
    public class FalseToHiddenConverterExtension : ChainedConverterExtensionBase
    {
        private static readonly BoolToVisibilityConverter _converter = BoolToVisibilityConverter.HiddenWhenFalse;
        protected override IValueConverter Converter => _converter;
    }
}
