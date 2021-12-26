using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMart.Converters
{
    /// <summary>
    /// Base Markup extension for value-converters that support converting from another converter
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ChainedConverterExtensionBase : MarkupExtension
    {
        /// <summary>
        /// Chained converter to use first when converting and last when converting-back.
        /// Can be used only as Markup extension.
        /// </summary>
        public IValueConverter FromConverter { get; set; }

        /// <summary>
        /// <see cref="System.Windows.Markup.MarkupExtension.ProvideValue(IServiceProvider)"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => Converter.FromConverter(FromConverter);

        /// <summary>
        /// The main converter the Markup extension provides
        /// </summary>
        protected abstract IValueConverter Converter { get; }
    }
}
