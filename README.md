# WpfMart Help
Help on [WpfMart](https://www.nuget.org/packages/WpfMart/) nuget packge.

# WpfMart
A set of WPF helpers and utilities such as
value converters, markup-extensions, behaviors, etc.

## Table of content
* [Markup extensions](#markup-extensions)
  * [Casting markup-extension](#casting-markup-extension)
  * [EnumValues markup-extension](#enumvalues-markup-extension)
* [Value Converters](#value-converters)
  * [GroupConverter](#groupconverter)
  * ~~[BoolConverter](#boolconverter)~~
    * ~~[NegativeBoolConverter](#negativeboolconverter)~~
  * ~~[BoolToVisibilityConverter](#booltovisibilityconverter)~~
    * ~~[TrueToCollapsedConverter](#truetocollapsedconverter)~~
    * ~~[TrueToHiddenConverter](#truetohiddenconverter)~~
    * ~~[FalseToHiddenConverter](#falsetohiddenconverter)~~
  * ~~[CastConverter](#castconverter)~~
  * ~~[EqualityConverter](#equalityconverter)~~
  * ~~[InRangeConverter](#inrangeconverter)~~
    * ~~[NumInRangeConverter](#numinrangeconverter)~~
    * ~~[DateInRangeConverter](#dateinrangeconverter)~~
  * ~~[NullConverter](#nullconverter)~~
    * ~~[IsNotNullConverter](#isnotnullconverter)~~
    * ~~[NullToInvisibleConverter](#nulltoinvisibleconverter)~~
  * ~~[NullOrEmptyConverter](#nulloremptyconverter)~~
    * ~~[IsNotNullOrEmptyConverter](#isnotnulloremptyconverter)~~
    * ~~[NullOrEmptyToInvisibleConverter](#nulloremptytoinvisibleconverter)~~
* ~~[Multi Value Converters](#multi-value-converters)~~
  * ~~[InRangeMultiConverter](#inrangemulticonverter)~~
  * ~~[EqualityMultiConverter](#equalitymulticonverter)~~
    
---  

## **Markup-extensions**
## Casting markup-extension 
`WpfMart.Markup.CastExtension` 
`WpfMart.Markup.BoolExtension`
`WpfMart.Markup.CharExtension` `WpfMart.Markup.IntExtension` `WpfMart.Markup.LongExtension`
`WpfMart.Markup.DoubleExtension` `WpfMart.Markup.DecimalExtension`
`WpfMart.Markup.DateTimeExtension` `WpfMart.Markup.TimeSpanExtension`

Provides type conversion to the specified type, mainly from a string written in XAML.
Used as shorthand for writing values of primitive type in XAML in places the expected type is `System.Object`.

> Please note that it's not necessary to do any casting when assigning to a Property of the desired type.
> ie.
> ```xml
<!-- no need casting since property type is int (not object) and WPF will convert it for us -->
<ComboBox SelectedIndex="1" />
<!-- wrongly using the casting markup-extension, it will work, but don't. -->
<ComboBox SelectedIndex="{z:Int 1}" />
<!-- Assign to a property of type object a typed value -->
<ComboBox Tag="{z:Int 1}" />
<!-- wrongly beliving that assigned value is int, while actually it's a string -->
<ComboBox Tag="1" />
```

The `CastExtension` is used by specifying its target type either thru constructor or its ToType property.
> Whenever possible use the more speccific cast markups such as
> `BoolExtension`, `IntExtension`, `DoubleExtension`, etc,
> instead of the general purpose `CastExtension`

### examples: 
```xml
<!-- old school -->
<Control><Control.Tag><sys:Int32>-1</sys:Int32></Control.Tag></Control>
<!-- lazy and cool -->
<Control Tag="{z:Int -1}" />

<!-- old school -->
<ContentControl><ContentControl.Content><sys:Double>0.5</sys:Double></ContentControl.Content></ContentControl>
<!-- lazy and cool -->
<ContentControl Content="{z:Double 0.5}" />

<!-- Intentionally use unsupported enum value, to signal special handling -->
<!-- old school -->
<ContentControl><ContentControl.Content><sysio:SeekOrigin>-1</sysio:SeekOrigin></ContentControl.Content></ContentControl>
<!-- lazy and cool -->
<ContentControl Content="{z:Cast -1, {x:Type sysio:SeekOrigin}}" />

<ContentControl Content="{z:Cast en-us, {x:Type globalization:CultureInfo}}" />
```

---


## EnumValues markup-extension
`WpfMart.Markup.EnumValuesExtension`

Provides Enum's values for the provided Enum type.

Simplify the ability to get enum values as ItemsSource.
- Some values can be excluded from the list by specifying them using the Exclude property 
  as comma delimited names or numbers.
  `<ComboBox ItemsSource="{z:EnumValues globalization:GregorianCalendarTypes, Exclude=11,22}" />`
- Can also extract the enum value names as string, int number of description of DescriptionAttribute,
  by setting the Mode property.
  `<ComboBox ItemsSource="{z:EnumValues globalization:GregorianCalendarTypes, Mode=Name}" />`
- Can provide a value-converter to perform conversion of the results, using the Converter property

### examples: 
```cs
enum MachineStateEnum
{
    None,
    [Description("Wax On")]
    On,
    [Description("Wax Off")]
    Off,
    [Description("Wax Burn")]
    Faulted
}
```
```xml
<!-- Simple usage as ItemsSource -->
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum}" />

<!-- Exclude from the enum values the MachineStateEnum.None,Faulted values (can also be specified by their numeric value) -->
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Exclude=None,Faulted}" />
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Exclude=0,2}" />

<!-- Extract enum values as numbers. -->
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Mode=Number}" />
<!-- Extract enum values using [Description] attribute -->
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Mode=Description}" />

<!-- Use value-converter on the enum values -->
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Converter={myconv:EnumToStringResourceConverter}" />
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Converter={myconv:EnumToReadableStringConverter}" />
<ComboBox ItemsSource="{z:EnumValues local:MachineStateEnum, Mode=Name, Converter={myconv:ToUpperCaseConverter}" />
```

---


## **Value Converters**
## GroupConverter
`WpfMart.Converters.GroupConverter`

Groups several value-converters together.

Performs Convert from top to bottom converter and ConvertBack from bottom to top converter.
If one of the converter's return value is either Binding.DoNothing or DependencyProperty.UnsetValue, the conversion stop.
When converting back, if one of the converter implement interface ICantConvertBack, it's skipped from conversion

### Examples
```xml
<!-- Convert from nullable int property. 
     First converter, converts null to zero, otherwise keep the same value.
     Second converter, converts the result of previous converter (int), by casting to specified Enum type
-->
<Control.Resources>
    <conv:GroupConverter x:Key="NullSafeNumberToMyEnum">
        <conv:NullConverter TrueValue="{z:Int 0}" FalseValue="{conv:UseInputValue}" />
        <conv:CastConverter ToType="local:MyEnum" />
    </conv:GroupConverter>
</Control.Resources>
<ContentControl Content="{Binding NullableInt, Converter={StaticResouces NullSafeNumberToMyEnum}" />
```
---


## BoolConverter
##### NegativeBoolConverter
-- not yet documented --

## BoolToVisibilityConverter
##### TrueToCollapsedConverter
##### TrueToHiddenConverter
##### FalseToHiddenConverter
-- not yet documented --


## CastConverter
-- not yet documented --

## EqualityConverter
-- not yet documented --

## InRangeConverter
##### NumInRangeConverter
##### DateInRangeConverter
-- not yet documented --

## MapConverter
-- not yet documented --

## NullConverter
##### IsNotNullConverter
##### NullToInvisibleConverter
-- not yet documented --

## NullOrEmptyConverter
##### IsNotNullOrEmptyConverter
##### NullOrEmptyToInvisibleConverter
-- not yet documented --

---


## **Multi Value Converters**
## InRangeMultiConverter
-- not published documented --

## EqualityMultiConverter
-- future release --
