using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using WpfMart.Utils;

namespace WpfMart.Markup
{
    /// <summary>
    /// Base class for markup-extension that converts a value to another type
    /// </summary>
    [ContentProperty("Value")]
    public abstract class CastBase : MarkupExtension
    {
        /// <summary>
        /// C'tor
        /// </summary>
        protected CastBase() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        protected CastBase(object value)
        {
            Value = value;
        }

        /// <summary>
        /// The value to convert
        /// </summary>
        [ConstructorArgument("value")]
        public object Value { get; set; }

        /// <summary>
        /// Return the Type to convert to by derived classes
        /// </summary>
        /// <returns></returns>
        protected abstract Type GetTargetType(IServiceProvider serviceProvider);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetType = GetTargetType(serviceProvider);
            var result = ConvertHelper.ChangeType(Value, targetType);
            if (ReferenceEquals(result, null)) return result;

            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if(provideValueTarget != null)
            {
                var propertyType = 
                    (provideValueTarget.TargetProperty as DependencyProperty)?.PropertyType ?? 
                    (provideValueTarget.TargetProperty as PropertyInfo)?.PropertyType;

                if (propertyType != null && !propertyType.IsAssignableFrom(result.GetType()))
                {
                    result = ConvertHelper.ChangeType(result, propertyType);
                }

            }
            return result;
        }
    }

    /// <summary>
    /// Base class for markup-extension that converts a value to the specified generic type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CastBase<T>: CastBase
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public CastBase() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public CastBase(object value) : base(value) { }

        /// <summary>
        /// Return the Type to convert to, which is the generic type argument
        /// </summary>
        /// <returns></returns>
        protected override Type GetTargetType(IServiceProvider serviceProvider) => typeof(T);
    }

    /// <summary>
    /// Markup-extension to convert a value to specified type
    /// </summary>
    /// <remarks>
    /// <para>Provide ability to convert a string written in XAML to the desired type.</para>
    /// <para>Can convert to nullable type.</para>
    /// <para>Prefer using the other predefined markup-extensions for specific types conversion, such as:</para>
    /// <para><see cref="BoolExtension"/>, <see cref="IntExtension"/>, <see cref="DoubleExtension"/>, <see cref="CharExtension"/></para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <Control.Resources>
    ///   <conv:NullConverter x:Key="NullToZero" TrueValue="{z:Cast 0.3, Type={x:Type sys:Decimal}}" />
    /// </Control.Resources>
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Using int of zero the traditional way and with IntExtension
    /// <code>
    /// <![CDATA[
    /// <Control.Resources>
    ///   <conv:NullConverter x:Key="NullToZeroTraditional">
    ///     <conv:NullConverter.TrueValue><sys:Int32>0</sys:Int32>
    ///     </conv:NullConverter.TrueValue>
    ///   </conv:NullConverter>
    ///   
    ///   <conv:NullConverter x:Key="NullToZeroSimple" TrueValue="{z:Int 0}" />
    /// </Control.Resources>
    /// ]]>
    /// </code>
    /// </example>
    [MarkupExtensionReturnType(typeof(object))]
    public class CastExtension : CastBase
    {
        private TypeExtension _typeExtension;

        /// <summary>
        /// C'tor
        /// </summary>
        public CastExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public CastExtension(object value) : base(value)
        {
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        /// <param name="typeName">Type-name to convert to</param>
        public CastExtension(object value, string typeName) : base(value)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        /// <param name="type">Type to convert to</param>
        public CastExtension(object value, Type type) : base(value)
        {
            Type = type;
        }

        ///<summary>
        ///The type to convert to
        ///</summary>
        /// <remarks>
        /// can use generics like: System.Nullable`1[[System.Int32]]
        /// </remarks>
        [ConstructorArgument("type")]
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the type name to convert to
        /// </summary>
        [ConstructorArgument("type")]
        public string TypeName { get; set; }

        /// <summary>
        /// Return the Type to convert to, which is the type provided by the <see cref="CastExtension.Type"/> property
        /// </summary>
        /// <returns></returns>
        protected override Type GetTargetType(IServiceProvider serviceProvider)
        {
            if (_typeExtension == null)
            {
                _typeExtension = (TypeName != null)
                    ? new TypeExtension(TypeName)
                    : new TypeExtension(Type);
            }

            return (Type)_typeExtension.ProvideValue(serviceProvider);
        }
    }

    /// <summary>
    /// Markup-extension to convert a value to bool
    /// </summary>
    [MarkupExtensionReturnType(typeof(bool?))]
    public class BoolExtension : CastBase<bool>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public BoolExtension() { }
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public BoolExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to int
    /// </summary>
    [MarkupExtensionReturnType(typeof(int?))]
    public class IntExtension : CastBase<int>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public IntExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public IntExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to long
    /// </summary>
    [MarkupExtensionReturnType(typeof(long?))]
    public class LongExtension : CastBase<long>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public LongExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public LongExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to double
    /// </summary>
    [MarkupExtensionReturnType(typeof(double?))]
    public class DoubleExtension : CastBase<double>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public DoubleExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public DoubleExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to decimal
    /// </summary>
    [MarkupExtensionReturnType(typeof(decimal?))]
    public class DecimalExtension : CastBase<decimal>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public DecimalExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public DecimalExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to char
    /// </summary>
    [MarkupExtensionReturnType(typeof(char?))]
    public class CharExtension : CastBase<char>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public CharExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public CharExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to DateTime
    /// </summary>
    [MarkupExtensionReturnType(typeof(DateTime?))]
    public class DateTimeExtension : CastBase<DateTime>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public DateTimeExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public DateTimeExtension(object value) : base(value) { }
    }

    /// <summary>
    /// Markup-extension to convert a value to DateTime
    /// </summary>
    [MarkupExtensionReturnType(typeof(TimeSpan?))]
    public class TimeSpanExtension : CastBase<TimeSpan>
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public TimeSpanExtension() { }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="value">the value to convert its type</param>
        public TimeSpanExtension(object value) : base(value) { }
    }
}
