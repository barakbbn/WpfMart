using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using WpfMart.Converters;

namespace WpfMart.Markup
{
    /// <summary>
    /// Provide options on how to extract enum values
    /// </summary>
    [TypeConverter(typeof(EnumTypeConverter<EnumExtractionMode>))]
    public enum EnumExtractionMode
    {
        /// <summary>
        /// Extract the enum values - default.
        /// </summary>
        Value,
        /// <summary>
        /// Extract the enum values' string name - ToString()
        /// </summary>
        Name,
        /// <summary>
        /// Extract the enum values as int
        /// </summary>
        Number,
        /// <summary>
        /// Extract the DescriptionAttribute.Description on each enum value
        /// </summary>
        Description
    }

    /// <summary>
    /// Provides Enum's values for the provided Enum type.
    /// </summary>
    /// <remarks>
    /// <para>Simplify the ability to get enum values as ItemsSource.</para>
    /// <para>- Some values can be excluded from the list by specifying them using the <see cref="EnumValuesExtension.Exclude"/> property.</para>
    /// <para>- Can also extract the enum value names, int number of description of <see cref="DescriptionAttribute.Description"/></para>
    /// <para>  using the <see cref="EnumValuesExtension.Mode"/> property.</para>
    /// <para>- Can provide a value-converter to perform conversion of the results, using the <see cref="EnumValuesExtension.Converter"/> property.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// the following examples based on the following enum type
    /// <![CDATA[
    /// enum MachineStateEnum
    /// {
    ///     None,
    ///     [Description("Wax On")]
    ///     On,
    ///     [Description("Wax Off")]
    ///     Off,
    ///     [Description("Wax Burn")]
    ///     Faulted
    /// }    
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Simple usage as ItemsSource
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Exclude from the enum values the MachineStateEnum.None,Faulted values (can also be specified by their numeric value)
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Exclude=None,Faulted}" />
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Exclude=0,2}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Extract enum values as numbers.
    /// Extract enum values using <see cref="DescriptionAttribute.Description"/> attribute
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Mode=Description}" />
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Mode=Number}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <example>
    /// Use value-converter on the enum values
    /// <code>
    /// <![CDATA[
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Converter={myconv:EnumToStringResourceConverter}" />
    /// <ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Converter={myconv:EnumToReadableStringConverter}" />
    /// ]]>
    /// </code>
    /// </example>
    [MarkupExtensionReturnType(typeof(IEnumerable))]
    public class EnumValuesExtension : MarkupExtension
    {
        private TypeExtension _typeExtension;
        private IEnumerable _cachedResults;

        /// <summary>
        /// Initializes a new instance of the WpfMart.Markup.EnumValuesExtension class.
        /// </summary>
        public EnumValuesExtension()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the WpfMart.Markup.TypeExtension class,</para>
        /// <para>initializing the <see cref="WpfMart.Markup.EnumValuesExtension.EnumTypeName"/> value based on
        /// the provided enumTypeName string.</para>
        /// </summary>
        /// <param name="enumTypeName">
        /// <para>A string that identifies the type to make a reference to.</para>
        /// <para>This string uses the format prefix:className. prefix is the mapping prefix for a XAML namespace,</para>
        /// <para>and is only required to reference types that are not mapped to the default XAML namespace.</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Attempted to specify enumTypeName as null
        /// </exception>
        public EnumValuesExtension(string enumTypeName) 
        {
            if (enumTypeName == null) throw new ArgumentNullException("typeName");
            EnumTypeName = enumTypeName;
        }

        /// <summary>
        /// Initializes a new instance of the WpfMart.Markup.EnumValuesExtension class,
        /// declaring the type directly.
        /// </summary>
        /// <param name="enumType">The type to be represented by this WpfMart.Markup.EnumValuesExtension.</param>
        /// <exception cref="System.ArgumentNullException">
        /// type is null
        /// </exception>
        public EnumValuesExtension(Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException("type");
            if (!enumType.IsEnum) throw new ArgumentException("Type must be of Enum type", nameof(enumType));

            EnumType = enumType;
        }

        /// <summary>Gets or sets the enum type name to return values for.</summary>
        public string EnumTypeName { get; set; }

        /// <summary>Gets or sets the type of the enum to return values for.</summary>
        [ConstructorArgument("enumType")]
        public Type EnumType { get; set; }

        /// <summary>
        /// Comma-seperated list of enum values to exclude.
        /// Values can be either enum's string value or enum's numeric value (or mix).
        /// </summary>
        /// <example>
        /// <para>"None,On,Off"</para>
        /// <para>"0,1"</para>
        /// <para>"0,On,Off"</para>
        /// </example>
        public string Exclude { get; set; }

        /// <summary>
        /// Converter to apply on each enum value.
        /// Resulting in providing a collection of converted values.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Mode to determine which kind of values to return
        /// </summary>
        public EnumExtractionMode Mode { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_cachedResults != null) return _cachedResults;

            var type = ProvideEnumType(serviceProvider);

            var enumFieldsAndValues = GetEnumFieldsAndValues(type);

            IEnumerable results = null;

            switch (Mode)
            {
                case EnumExtractionMode.Name:
                    results = enumFieldsAndValues.Select(field => field.Name);
                    break;

                case EnumExtractionMode.Number:
                    results = enumFieldsAndValues.Select(field => (int)field.GetRawConstantValue());
                    break;

                case EnumExtractionMode.Description:
                    results = enumFieldsAndValues.Select(field =>
                        field.GetCustomAttributes<DescriptionAttribute>()
                        .Select(value => value.Description)
                        .DefaultIfEmpty(field.Name) //Fall back to enum name
                        .First());
                    break;

                default:
                    results = enumFieldsAndValues.Select(field => field.GetValue(null));
                    break;

            }

            results = ApplyConverter(serviceProvider, results);

            _cachedResults = results.OfType<object>().ToList();
            return _cachedResults;
        }

        private IEnumerable<FieldInfo> GetEnumFieldsAndValues(Type type)
        {
            IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (!string.IsNullOrWhiteSpace(Exclude))
            {
                var excludedEnums = Exclude?.Split(',').Select(x => Enum.Parse(type, x));
                var hash = new HashSet<object>(excludedEnums);
                fields = fields.Where(field => !hash.Contains(field.GetValue(null)));
            }

            return fields;
        }

        private IEnumerable ApplyConverter(IServiceProvider serviceProvider, IEnumerable results)
        {
            if (Converter != null)
            {
                //TODO: Rethink if getting target-type for converter, worth the effort
                var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
                var property = targetProvider?.TargetProperty;
                var targetType = (property as DependencyProperty)?.PropertyType ?? (property as PropertyInfo)?.PropertyType ?? typeof(object);

                results = results.OfType<object>().Select(value => Converter.Convert(value, targetType, null, CultureInfo.CurrentCulture));
            }

            return results;
        }

        private Type ProvideEnumType(IServiceProvider serviceProvider)
        {
            return (Type)TypeExtension.ProvideValue(serviceProvider);
        }

        private TypeExtension TypeExtension
        {
            get
            {
                if (_typeExtension == null)
                {
                    _typeExtension = (EnumTypeName != null)
                        ? new TypeExtension(EnumTypeName)
                        : new TypeExtension(EnumType);
                }

                return _typeExtension;
            }
        }
    }
}
