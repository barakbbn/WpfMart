using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WpfMart.Utils;

namespace WpfMart.Converters
{
    /// <summary>
    /// Converts a value to another type
    /// </summary>
    /// <remarks>
    /// <para>The difference between CastConverter and CastExtension is that CastConverter.ProvideValue returns IValueConverter, </para>
    /// <para>while CastExtension performs the conversion and returns the value after conversion.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <TextBlock Text={Binding MachineStateId, Converter={conv:CastConverter ToType="{z:Nullable local:MachineState}}" />
    /// ]]>
    /// </code>
    /// </example>
    public class CastConverter : ChainedConverterExtensionBase, IValueConverter
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public CastConverter()
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="toType">Type to convert to</param>
        public CastConverter(Type toType)
        {
            ToType = toType;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="toType">Type to convert to</param>
        /// <param name="convertBackToType">Type to convert-back to</param>
        public CastConverter(Type toType, Type convertBackToType)
        {
            ToType = toType;
            ConvertBackToType = convertBackToType;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="toTypeName">Type-name to convert to</param>
        public CastConverter(string toTypeName) : this(Type.GetType(toTypeName, true))
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="toTypeName">Type-name to convert to</param>
        /// <param name="convertBackToTypeName">Type-name to convert-back to</param>
        public CastConverter(string toTypeName, string convertBackToTypeName)
            : this(Type.GetType(toTypeName, true), Type.GetType(convertBackToTypeName, true))
        {
        }

        /// <summary>
        /// The tpe to convert to
        /// </summary>
        [ConstructorArgument("toType")]
        public Type ToType { get; set; }

        /// <summary>
        /// The type to convert to when converting-back
        /// </summary>
        [ConstructorArgument("convertBackToType")]
        public Type ConvertBackToType { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertHelper.ChangeType(value, ToType ?? targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertHelper.ChangeType(value, ConvertBackToType ?? targetType);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        protected override IValueConverter Converter => this;
    }
}
