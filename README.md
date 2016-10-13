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
  * *[Converter special values](#converter-special-values)*
  * [CastConverter](#castconverter)
  * [EqualityConverter](#equalityconverter)
  * ~~[InRangeConverter](#inrangeconverter)~~
    * ~~[NumInRangeConverter](#numinrangeconverter)~~
    * ~~[DateInRangeConverter](#dateinrangeconverter)~~
  * [MapConverter](#mapconverter)
  * [NullConverter](#nullconverter)
    * [IsNotNullConverter](#isnotnullconverter)
    * [NullToInvisibleConverter](#nulltoinvisibleconverter)
  * ~~[NullOrEmptyConverter](#nulloremptyconverter)~~
    * ~~[IsNotNullOrEmptyConverter](#isnotnulloremptyconverter)~~
    * ~~[NullOrEmptyToInvisibleConverter](#nulloremptytoinvisibleconverter)~~
* ~~[Multi Value Converters](#multi-value-converters)~~
  * ~~[InRangeMultiConverter](#inrangemulticonverter)~~
  * ~~[EqualityMultiConverter](#equalitymulticonverter)~~
    
--------------------------------------------------------------------------------

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

--------------------------------------------------------------------------------


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

--------------------------------------------------------------------------------


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

--------------------------------------------------------------------------------


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
> Consider using the Binding's TargetNullValue instead.  
### Examples
```xml
<CheckBox IsThreeState="True" IsChecked="{Binding IsChecked, Converter={conv:BoolConverter IsNullable=True}}" />
<ComboBox SelectedItem="{Binding IsMachineOn, Converter={conv:BoolConverter 
            TrueValue={x:Static local:MachineState.On}
            FalseValue={x:Static local:MachineState.Off}
            NullValue={x:Static local:MachineState.None}}}" />
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

--------------------------------------------------------------------------------

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

##### IsReversed
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

--------------------------------------------------------------------------------

## Converter special values
In some of the converters, where it has a property that can be set to any object,  
i.e. `BoolConverter.TrueValue`, `EqualityComparer.CompareTo`  
It's possible to provide special values from `WpfMart.Converters.ConverterSpecialValue`  
That will affect the desired behavior of the converter.  
##### e.g
```xml
<conv:BoolConverter TrueValue="{x:Static conv:ConverterSpecialValue.UseConverterParameter}" />
```
For each of the special value there is also a corresponding markup-extension that is simpler to use than the `{x:Static}` syntax.  
The special values are:  
* UseInputValue - `{conv:UseInputValue}`  
  Indicates that the converter should use the 'value' parameter passed to the Convert method.  
* UseConverterParameter - `{conv:UseConverterParameter}`  
  Indicates that the converter should use the ConverterParameter value used in `Binding.ConverterParameter` to the Convert method.
* ThrowException - `{conv:ThrowException}`  
  Indicates that the converter should throw `System.InvalidOperationException`.  
  Used only as rare option to cause exception in case developer requires that certain conditions won't go unnoticed.  
  Such as to trigger the `System.Windows.Data.Binding.ValidatesOnExceptions` property.  

### Examples
```xml
<Control.Resources>
  <conv:EqualityConverter x:Key="CompareToParameterReturnBackground" 
                          CompareTo="{conv:UseConverterParameter}" 
                          TrueValue="Green", FalseValue="Red" />
</Control.Resources>
<TextBlock Text="On" 
  Background="{Binding MachineState, 
    Converter={StaticResource CompareToParameterReturnBackground}, 
    ConverterParameter={x:Static local:MachineState.On}}}" />
<TextBlock Text="Off" 
  Background="{Binding MachineState, 
    Converter={StaticResource CompareToParameterReturnBackground}, 
    ConverterParameter={x:Static local:MachineState.Off}}}" />   
```
```xml
<!-- Cause validation error on the TextBox if using null as text.
    (Prefer doing it in view-model, or using validation rules) -->
<TextBox Text="{Binding Name, ValidatesOnExceptions=True,
         Converter={conv:NullConverter TrueValue={conv:ThrowException}, FalseValue={conv:UseInputValue}}}" />
```  


> Please note that the built-in WPF special values such as:  
> `DependencyProperty.UnsetValue`, `Binding.DoNothing` are still valid.  
> but now they got markup-extension to make it simpler to use from XAML:  
> `{conv:UnsetValue}`, `{conv:DoNothing}`

```xml
<!-- based on the example above. change background only when equals -->
<conv:EqualityConverter x:Key="CompareToParameterReturnBackground" 
                          CompareTo="{conv:UseConverterParameter}" 
                          TrueValue="Green", FalseValue="{Conv:UnsetValue}" />
```

--------------------------------------------------------------------------------


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

--------------------------------------------------------------------------------


## EqualityConverter
`WpfMart.Converters.EqualityConverter`  
Checks if converted value equals to the value of `CompareTo` property  ,
if equals, return value of `TrueValue` property, otherwise `FalseValue` property.  

#### Examples
```xml
<CheckBox IsChecked="{Binding SeekOrigin, CompareTo={x:Static sysio:SeekOrigin.Current}}" />
```

```cs
enum MachineState { None, On, Off }
```
```xml
<ComboBox SelectedItem="{Binding MachineState, Converter={conv:EqualityConverter
            CompareTo={x:Static local:MachineState.None}
            TrueValue={x:Null}
            FalseValue={conv:UseInputValue}}}">
    <x:Static Member="local:MachineState.On" />
    <x:Static Member="local:MachineState.Off" />
</ComboBox>

```
### IsNegative
Converter can be negative, meaning checking if the value is not equals to the `CompareTo` property,  
by setting `IsNegative` property to true.  
```xml
<!-- If no items or only one item in combo-box, make it disabled -->
<ComboBox SelectedIndex="0"
    IsEnabled="{Binding Items.Count, RelativeSource={RelativeSource Self}, 
        Converter={conv:EqualityConverter IsNegative=True, CompareTo={z:Int 0}}}"   />
```
### null value
In order to specially handle conversion from a null value,   
either set `NullValue` property to a desired value to return when converting a null value,  
or set IsNullable to true and by that, NullValue property will have its default value of null.  
(which is same as setting NullValue property to {x:Null}. )
> if none of the above properties are set, null is not converted and compared to `CompareTo` property.  
> Consider using the Binding's TargetNullValue instead.  
### Chaining converters
When used as markup-extension it's possible to chain another converter to convert from.  
using the `FromConverter` property as follow:  
```xml
<!-- compare to string "YES" and ensure any string is first converted to upper-case -->
<TextBlock Text="{Binding UserInput,
  Converter={conv:EqualityConverter CompareTo='YES', TrueValue='Confirmed', FalseValue='---', NullValue='invalid'
  FromConverter={notmycon:ToUpperCaseConverter}}} />
```

## InRangeConverter
##### NumInRangeConverter
##### DateInRangeConverter
-- not yet documented --

--------------------------------------------------------------------------------


## MapConverter
`WpfMart.Converters.MapConverter`  
Converts a key to a mapped value.

Defines a dictionary of key-value pairs for which a converted value is considered a key in the dictionary  
and the matching value for the key is returned as result.  
The dictionary is defined in the same manner as a `ResourceDictionary` is defined in XAML.

##### e.g.
```xml
<Window.Resources>
  <!-- Map System.IO.SeekOrigin enum values to icons -->
  <conv:MapConverter x:Key="SeekOriginToIcon">
    <BitmapImage x:Key="{x:Static sysio:SeekOrigin.Begin}" UriSource="../Assets/SeekBegin.png" />
    <BitmapImage x:Key="{x:Static sysio:SeekOrigin.Current}" UriSource="../Assets/SeekCurrent.png" />
    <BitmapImage x:Key="{x:Static sysio:SeekOrigin.End}" UriSource="../Assets/SeekEnd.png" />
  </conv:MapConverter>
</Window.Resources>
<Button Command="{Binding SeekToCommand}" >
  <Image Source="{Binding SelectedSeekOrigin, Converter={StaticResource SeekOriginToIcon}}" />
</Button>
```

In case the key not found in the dictionary, there will be an attempt to located by its `ToString()` value.  
This in order to make it more flexible to specify some keys in XAML.    
(Illustration on the above example)  
```xml
  <conv:MapConverter x:Key="SeekOriginToIcon">
    <BitmapImage x:Key="Begin" UriSource="../Assets/SeekBegin.png" />
    <BitmapImage x:Key="Current" UriSource="../Assets/SeekCurrent.png" />
    <BitmapImage x:Key="End" UriSource="../Assets/SeekEnd.png" />
  </conv:MapConverter>
```
### Fallback value
If the key couldn't be found in the dictionary, a fallback-value is used,  
by setting the desired value in the `FallbackValue` property.  
by default it will return the key itself, which is actually the input value to be converted.  
##### e.g.
```xml
<Window.Resources>
  <!-- Map System.Globalization.GregorianCalendarTypes enum values to texts -->
  <conv:MapConverter x:Key="GregorianCalendarTypesToText" FallbackValue="-----">
    <sys:String x:Key="{x:Static glob:GregorianCalendarTypes.Localized}" >Local</sys:String>
    <sys:String x:Key="{x:Static glob:GregorianCalendarTypes.USEnglish}" >English</sys:String>
    <sys:String x:Key="{x:Static glob:GregorianCalendarTypes.Arabic}"    >Arabic</sys:String>
  </conv:MapConverter>
</Window.Resources>
<TextBlock Text="{Binding SelectedCalendarType, Converter={StaticResource GregorianCalendarTypesToText}}" />
```

### null value
In order to map a null to a value, it's not possible to set it as a key in a dictionary.  
~~`<conv:MapConverter><sys:String x:Key={x:Null}>No value</sys:String></conv:MapConverter>`~~  
For this purpose use the `NullValue` property to set the value to return when converting from null.  
> Consider using the Binding's TargetNullValue instead.  
#### Examlpes
```cs
enum MachineState { None, On, Off, Faulted, Maintenance }
```
```xml
<Window.Resources>
  <SolidColorBrush x:Key="GrayBrush" Color="Gray" />
  <conv:MapConverter x:Key="MachineStateToBackground" NullValue="{StaticResource GrayBrush}" FallbackValue="{conv:UnsetValue}" >
    <SolidColorBrush x:Key="{x:Static local:MachineState.On}" Color="Green" />
    <SolidColorBrush x:Key="{x:Static local:MachineState.Off}" Color="Red" />
    <SolidColorBrush x:Key="{x:Static local:MachineState.Faulted}" Color="Orange" />
  </conv:MapConverter>
</Window.Resources>
```
### Converting back
The MapConverter also performs convert-back.  
To specify a fallback value in case convert-back cannot find vaue in the dictionary,  
use the `ConvertBackFallbackValue` property. by default it cancel the conversion (`Binding.DoNothing`).  
> in case more than one key mapped to the same value,  
> The resulting key from convert-back can be any of them.


### Mapping to another converter
If a mapped value in the dictionary is a value-converter, by default it will be used in order to produce the final value.  
*to disalbe this ability, set `ConvertAlsoFoundValueConverter` property to false.*  
*Also convert-back cannot handle it.*
```xml
<!-- Map familier angles to named-angle, for the rest of them, convert them from double to integer -->
<conv:MapConverter x:Key="AngleToNameOrRoundedAngle" FallbackValue="{conv:CastConverter ToType={x:Type sys:Int32}}" >
  <sys:String x:Key="{z:Double 0.0}"  >Zero</sys:String>
  <sys:String x:Key="{z:Double 90.0}" >Ninty</sys:String>
  <sys:String x:Key="{z:Double 180.0}">One hundred eighty</sys:String>
</conv:MapConverter>
<TextBlock Text="{Binding Angle, Converter={StaticResource AngleToNameOrRoundedAngle}}" />
```

### Recap Examples
```cs
enum MachineState { None, On, Off, Faulted, Maintenance }
```
```xml
<Window.Resources>
    <!-- Map two of the enum values to boolean true and false. have the rest of the enum values fallback to null .
         When converting back, unmapped value is fallback to MachineState.None on the view-model's property -->
   <conv:MapConverter x:Key="MachineStateToIsChecked" FallbackValue="{x:Null}" 
                      ConvertBackFallbackValue="{x:Static local:MachineState.None}" >
        <sys:Boolean x:Key="{x:Static local:MachineState.On}" >True</sys:Boolean>
        <sys:Boolean x:Key="{x:Static local:MachineState.Off}" >False</sys:Boolean>
    </conv:MapConverter>
    
     <!-- Map enum values to brushes used by CheckBox.Foreground property.
          unmapped enum values are fallback to the Foreground default value by using DependencyProperty.UnsetValue. -->
    <conv:MapConverter x:Key="MachineStateToForeground" FallbackValue="{conv:UnsetValue}">
        <SolidColorBrush x:Key="{x:Static local:MachineState.On}" Color="Green" />
        <SolidColorBrush x:Key="{x:Static local:MachineState.Off}" Color="Red" />
        <SolidColorBrush x:Key="{x:Static local:MachineState.Faulted}" Color="Orange" />
    </conv:MapConverter>

     <!-- Map from a brush to a string of the brush's color name, converting from a CheckBox.Foreground to CheckBox.Content.
         the mapped keys are the Brush.ToString().
         unmapped keys fallback to a dahes rectangle. -->
    <conv:MapConverter x:Key="ForegroundToTextConverter" NullValue="{x:Null}" >
        <sys:String x:Key="#FF008000" >Green</sys:String>
        <sys:String x:Key="#FFFF0000" >Red</sys:String>
        <sys:String x:Key="#FFFFA500" >Orange</sys:String>

        <conv:MapConverter.FallbackValue>
            <Rectangle Width = "50" Height="2" StrokeThickness="2" Stroke="Blue" StrokeDashArray="1,2,1,2" Margin="1,7" />
        </conv:MapConverter.FallbackValue>
    </conv:MapConverter>
 </Window.Resources>
 
 <CheckBox x:Name="MachineStateToThreeStateCheck" IsThreeState="True"
   IsChecked="{Binding MachineState, Converter={StaticResource MachineStateToIsChecked}}"
   Content="{Binding Foreground, RelativeSource={RelativeSource Self}, Mode=OneWay, 
                     Converter={StaticResource ForegroundToTextConverter}}"
   Foreground="{Binding MachineState, Mode=OneWay, Converter={StaticResource MachineStateToForeground}}" />
```

--------------------------------------------------------------------------------


## NullConverter
`WpfMart.Converters.NullConverter`  
Converts from a null value to another value specified by properties `TrueValue` and `FalseFalue`.

NullConverter is usually not useful, as other converters can achieve same abilities, but it's most common usages are:  
* return true/false in case input value is null or not - this is the default behavior.

  ```xml
  <CheckBox Content="Use default" IsChecked="{Binding SelectedItem, Converter={conv:NullConverter}}" /> 
  ```
* convert null value to something else but in case the input value is not null, keep it as is without converting it. 
   
   > Note: prefer using the Binding's TargetNullValue instead, unless requiring more advanced usages.
   ```xml
   <!-- example where NullConverter should be avoided -->
   <ComboBox SelectedItem="{Binding SelectedTraceLevel, TargetNullValue={x:Static diag:TraceLevel.Off}}" />
   ```
   ```xml
   <Window.Resources>
     <!-- Instead of setting same TargetNullValue on all bindings, reuse it thru NullConverter-->
     <conv:NullConverter x:Key="NullToDefaultColor" TrueValue="{conv:UnsetValue}" FalseValue="{conv:UseInputValue}" />
   </Window.Resources>
   <!-- --This could be achieved also by style -->
   <RadioButton Background="{Binding ColorA, Converter={StaticResource NullToDefaultColor}}" />
   <RadioButton Background="{Binding ColorB, Converter={StaticResource NullToDefaultColor}}" />
   <RadioButton Background="{Binding ColorC, Converter={StaticResource NullToDefaultColor}}" />
   ```
* return true in case input value is not null, false otherwise. (negative to option 1 above).  
   This can be achieved by setting the `IsNegative` property to true. 
   There is also a predefined `IsNotNullConverterExtension"` converter used to perform negative conversion.

> Note: If it's required to check if value is *null or empty string/collection*, use `NullOrEmptyConverter` instead.  

##### IsNotNullConverter
`WpfMart.Converters.IsNotNullConverterExtension`  
Pre configured converter, to convert a null to `false` and non-null value to `true`.  
##### NullToInvisibleConverter
`WpfMart.Converters.NullToInvisibleConverterExtension`  
Pre configured converter, to convert a null to `Visiblity.Collapsed`, otherwise to` Visiblity.Visible`.


--------------------------------------------------------------------------------


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

