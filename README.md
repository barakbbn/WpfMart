# WpfMart Help
Help on [WpfMart](https://www.nuget.org/packages/WpfMart/) nuget packge.

# WpfMart
A set of WPF helpers and utilities such as value converters, markup-extensions, behaviors, etc.

## Table of content
* [Markup extensions](#markup-extensions)
  * [Casting markup-extension](#casting-markup-extension)
  * [EnumValues markup-extension](#enumvalues-markup-extension)
* [Value Converters](#value-converters)
  * [GroupConverter](#groupconverter)
  * [BoolConverter](#boolconverter)
    * [NegativeBoolConverter](#negativeboolconverter)
  * [BoolToVisibilityConverter](#booltovisibilityconverter)
    * [TrueToCollapsedConverter](#truetocollapsedconverter)
    * [TrueToHiddenConverter](#truetohiddenconverter)
    * [FalseToHiddenConverter](#falsetohiddenconverter)
  * [CastConverter](#castconverter)
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
`WpfMart.Markup.IntExtension` `WpfMart.Markup.DoubleExtension` 
`WpfMart.Markup.LongExtension` `WpfMart.Markup.LongExtension` 
`WpfMart.Markup.DateTimeExtension` `WpfMart.Markup.TimeSpanExtension` 
`WpfMart.Markup.CharExtension` `WpfMart.Markup.BoolExtension`


Provides type conversion to the specified type, mainly from a string written in XAML. 
Used as shorthand for writing values of primitive type in XAML in places the expected type is `System.Object`.

> Please note that it's not necessary to do any casting when assigning to a Property of the desired type. 
> e.g. 
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
> `BoolExtension`, `IntExtension`, `DoubleExtension`, etc. 
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
<ContentControl Content="{z:Cast -1, sysio:SeekOrigin}" />

<ContentControl Content="{z:Cast en-us, globalization:CultureInfo}" />
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
`WpfMart.Converters.BoolConverter`

Converts a bool or nullable bool value to another value specified by properties TrueValue, FalseFalue and NullValue.

### Examples
```xml
<Control.Resources>
 <!-- Convert from bool to string value. -->
 <conv:BoolConverter x:Key="BoolToOnOffConverter" TrueValue="On" FalseValue="Off" />
                           
 <!-- Convert from bool to enum value. -->
 <conv:BoolConverter x:Key="IsTrueToMachineStateEnum" 
                     TrueValue="{x:Static local:MachineState.On}"
                     FalseValue="{x:Static local:MachineState.Off}" />
</Control.Resources>                     
```
### Nullable bool
In order to support nullable bool, either set NullValue property to a desired value to return when converting a null value 
or set IsNullable to true and by that, NullValue property will have its default value of null.  
(which is same as setting NullValue property to {x:Null}. )
> if none of the above properties are set, null is not converted and considered as false.

### Examples
```xml
<CheckBox IsThreeState="True" IsChecked="{Binding IsChecked, Converter={conv:BoolConverter IsNullable=True}}" />
<ComboBox SelectedItem="{Binding IsMachineOn, Converter={conv:BoolConverter 
            TrueValue={x:Static local:MachineState.On}
            FalseValue={x:Static local:MachineState.Off}
            NullValue={x:Static local:MachineState.Unknown}}}" />
```

##### IsNegative
Converter can be negative, meaning comparing to false instead of true, by setting IsNegative property to true.  
Although, for that purpose there is a predefined converter:
#### NegativeBoolConverter
`WpfMart.Converters.NegativeBoolConverterExtension`

### Examples
```xml
<CheckBox x:Name= IsThreeState="True" Content="{Binding IsKnownToBeClosed, Converter={conv:NegativeBoolConverter IsNullable=True}}" />
<!-- reusing existing BoolConverter, by this time as negative by setting ConverterParameter=True -->
<ContentControl Content="{Binding IsOff, Converter={StaticResource IsTrueToMachineStateEnum}, ConverterParameter=True}" />
```

##### Reverse conversion
There is ability to switch between Convert and ConvertBack, by setting IsReversed property to True.  
It will check if converted value equals to TrueValue property and return true otherwise false.  
If null is supported, it's also compared to NullValue property and returns null if equals.

### Examples
```cs
enum MachineState { None, On, Off }
```
```xml
<Control.Resources>
   <!-- reversed conversion. From Combobox.SelectedItem enum value to Checkbox.IsChecked (nullable) -->
   <conv:BoolConverter x:Key="EnumToIsCheckedConverter" 
                       IsReversed="True"
                       TrueValue="{x:Static local:MachineState.On}" 
                       FalseValue="{x:Static local:MachineState.Off}"
                       NullValue="{x:Static local:MachineState.None}" />
</Control.Resources>
<StackPanel>
    <ComboBox x:Name="combobox" ItemsSource="{z:EnumValues local:MachineState}" SelectedIndex="0" />
    <CheckBox IsThreeState="True" 
              IsChecked="{Binding SelectedItem, ElementName=combobox, Converter={StaticResource EnumToIsCheckedConverter}}"/>
</StackPanel>
```

---

## BoolToVisibilityConverter
`WpfMart.Converters.BoolToVisibilityConverter`  
The difference betwen this converter and built-in WPF converter `System.Windows.Data.BooleanToVisibilityConverter`  
Is that with this converter has additional abilities as follow:  

#### Hidden instead of Collapsed
it's possible to to use `Visibility.Hidden` instead of `Visibility.Collapsed`  
###### e.g.
```xml
<Border x:Name="ContentPlaceHolder" Visibility="{Binding IsMissings, Converter={conv:BoolToVisibilityConverter UseHidden=True}}" />
```
#### IsNegative 
Converter can be negative, meaning comparing to false instead of true, by setting IsNegative property to true.  
> Another way to look at it is, return `Visibility.Visible` when converting from `false`,  
> otherwise `Visibility.Collapsed` (or `Visibility.Hidden` if `UseHidden` property is true)  

###### e.g.
```xml
<TextBlock Visibility="{Binding HasLicense, Converter={conv:BoolToVisibilityConverter IsNegative=True}}">
    <Hyperlink NavigateUri="http://www.url.com/license" >Acquire license here...</Hyperlink>
</TextBlock>
```

##### Reverse conversion
There is ability to switch between Convert and ConvertBack, by setting IsReversed property to True.  
It will check if converted value equals to `Visibility.Visible` and then return true, otherwise false.  
(Unless `IsNegative` property is true, and therefore the conversion is opposite)  
###### e.g.
```xml
<StackPanel>
    <ContentControl x:Name="MovieDetailsViewPlaceHolder" />
    
    <TextBox Text="{Binding EditorComments}"
        IsEnabled="{Binding Content.Visibility, ElementName=MovieDetailsViewPlaceHolder,
            Converter={conv:BoolToVisibilityConverter IsRevered=True}}"/>
</StackPanel>
```
  
  
There are predefined converters of BoolToVisibilityConverter, that are configured to common needs, such as:  
##### TrueToCollapsedConverter
`WpfMart.Converters.TrueToCollapsedConverterExtension`  
Converts from true to `Visibility.Collapsed`, otherwise to `Visibility.Visible`  
```xml
<TextBlock Visibility="{Binding HasLicense, Converter={conv:TrueToCollapsedConverter}}">
    <Hyperlink NavigateUri="http://www.url.com/license" >Acquire license here...</Hyperlink>
</TextBlock>
```
##### TrueToHiddenConverter
`WpfMart.Converters.TrueToHiddenConverterExtension`  
Converts from true to `Visibility.Hidden`, otherwise to `Visibility.Visible`  
```xml
<Border x:Name="ContentPlaceHolder" Visibility="{Binding IsMissings, Converter={conv:TrueToHiddenConverter}}" />
```
##### FalseToHiddenConverter
`WpfMart.Converters.FalseToHiddenConverterxtension`  
Converts from false to `Visibility.Hidden`, otherwise to `Visibility.Visible`  

---

## CastConverter
Converts a value to another type  
> The difference between CastConverter and CastExtension is that CastConverter.ProvideValue returns IValueConverter,  
> while CastExtension performs the conversion and returns the value after conversion.

```xml
<ComboBox SelectedItem="{Binding MachineStateNumber, Converter={conv:CastConverter ToType={x:Type local:MachineState}}" />
```  
In order to be able to convert-back the `ConvertBackToType` property should be set.
```xml
<ComboBox SelectedItem="{Binding MachineStateNumber, 
            Converter={conv:CastConverter ToType={x:Type local:MachineState}, ConvertBackToType={x:Type sys:Int32}}" />
```  
##### Chaining converters
When used as markup-extension it's possible to chain another converter to convert from.  
using the `FromConverter` property as follow:  
```xml
<!-- Convert from nullable int that represent MachineState enum.
     If it's null, default it to 0 int value, and continue casting it to MachineState enum value -->
<TextBlock Text="{Binding NullableMachineStateId,
    Converter={conv:CastConverter {x:Type local:MachineState},
    FromConverter={conv:NullConverter TrueValue={z:Int 0}, FalseValue={conv:UseInputValue}}}" />
```
> the chaining ability apply most of the value-converters in the package.  
>   
> *This ability should not be abused. use it intelligently, in most of the cases a single converter can do the job.*  
> When not used with markup-extension syntax, wrap with [GroupConverter](#groupconverter) to chain several converters.

#### Example
```cs
enum MachineState { None, On, Off }
```
```xml
<Control.Resources>
    <conv:GroupConverter x:Key="NullSafeNumberToMachineStateEnum">
        <conv:NullConverter TrueValue="{z:Int 0}" FalseValue="{conv:UseInputValue}" />
        <conv:CastConverter ToType="{x:Type local:MachineState}" />
    </conv:GroupConverter>
</Control.Resources>

<TextBlock Text="{Binding NullableMachineStateId, Converter={StaticResource NullSafeNumberToMachineStateEnum}}" />
```

---


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
