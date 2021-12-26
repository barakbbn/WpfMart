using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WpfMart.Markup
{
    /// <summary>
    /// Provides nullable type for the specified type or type-name.
    /// </summary>
    /// <remarks>
    /// Extends <see cref="System.Windows.Markup.TypeExtension"/> to provide Nullable Type
    /// </remarks>
    public class NullableExtension: TypeExtension
    {
        private static Type _nullableType = typeof(Nullable<>);

        /// <summary>
        /// Initializes a new instance of the WpfMart.Markup.NullableExtension class.
        /// </summary>
        public NullableExtension(): base() { }

        /// <summary>
        /// <para>Initializes a new instance of the WpfMart.Markup.NullableExtension class,
        /// initializing the <see cref="TypeExtension.TypeName"/> value based on
        /// the provided typeName string.</para>
        /// </summary>
        /// <param name="typeName">
        /// <para>A string that identifies the type to make a reference to.</para>
        /// <para>This string uses the format prefix:className. prefix is the mapping prefix for a XAML namespace,</para>
        /// <para>and is only required to reference types that are not mapped to the default XAML namespace.</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Attempted to specify typeName as null
        /// </exception>
        public NullableExtension(string typeName): base(typeName) {}

        /// <summary>
        /// Initializes a new instance of the WpfMart.Markup.NullableExtension class,
        /// declaring the type directly.
        /// </summary>
        /// <param name="type">The type to be represented by this WpfMart.Markup.NullableExtension.</param>
        /// <exception cref="System.ArgumentNullException">
        /// type is null
        /// </exception>
        public NullableExtension(Type type): base(type) {}

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var baseType = (Type)base.ProvideValue(serviceProvider);
            return _nullableType.MakeGenericType(baseType);
        }
    }
}
