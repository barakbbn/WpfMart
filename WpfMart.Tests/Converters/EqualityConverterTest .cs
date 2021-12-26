using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using WpfMart.Converters;
using System.Globalization;
using System.Linq;

namespace WpfMart.Tests
{
    [TestClass]
    public class EqualityConverterTest
    {
        [TestMethod]
        public void Convert_IsEquals_ReturnTrueValueProperty()
        {
            object obj = new object();
            var trueValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in trueValues
                                 from falseValue in trueValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(testData.compareTo, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : sut.TrueValue, result);
                Assert.AreEqual(testData.isNegative ? testData.falseValue : testData.trueValue, result);
            }
        }

        [TestMethod]
        public void Convert_CompareToConverterParameter_IsEquals_ReturnTrueValueProperty()
        {
            object obj = new object();
            var trueValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in trueValues
                                 from falseValue in trueValues.Reverse()
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    CompareTo = ConverterSpecialValue.UseConverterParameter,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(testData.converterParameter, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : sut.TrueValue, result);
                Assert.AreEqual(testData.isNegative ? testData.falseValue : testData.trueValue, result);
            }
        }

        [TestMethod]
        public void Convert_NotEquals_ReturnFalseValueProperty()
        {
            object obj = new object();
            var falseValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in falseValues
                                 from trueValue in falseValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(new object(), typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : sut.FalseValue, result);
                Assert.AreEqual(testData.isNegative ? testData.trueValue : testData.falseValue, result);
            }
        }

        [TestMethod]
        public void Convert_CompareToConverterParameter_NotEquals_ReturnFalseValueProperty()
        {
            object obj = new object();
            var falseValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in falseValues
                                 from trueValue in falseValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new {trueValue, falseValue, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    CompareTo = ConverterSpecialValue.UseConverterParameter,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(new object(), typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : sut.FalseValue, result);
                Assert.AreEqual(testData.isNegative ? testData.trueValue : testData.falseValue, result);
            }
        }

        [TestMethod]
        public void Convert_IsNullable_InputValueIsNull_ReturnNullValueProperty()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in values
                                 from trueValue in values
                                 from nullValue in values
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, nullValue, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    NullValue = testData.nullValue,
                    CompareTo = ConverterSpecialValue.UseConverterParameter,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(null, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(sut.NullValue, result);
                Assert.AreEqual(testData.nullValue, result);
            }
        }
   
        [TestMethod]
        public void Convert_IsNullable_InputValueIsNotNull_IsEquals_ReturnTrueValueProperty()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValuesExceptNull = new object[] { true, false, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in values
                                 from trueValue in values
                                 from nullValue in values
                                 from compareTo in compareToValuesExceptNull
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, nullValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    NullValue = testData.nullValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(testData.compareTo, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : sut.TrueValue, result);
                Assert.AreEqual(testData.isNegative ? testData.falseValue : testData.trueValue, result);
            }
        }

        [TestMethod]
        public void Convert_IsNullable_InputValueIsNotNull_NotEquals_ReturnFalseValueProperty()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValuesExceptNull = new object[] { true, false, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in values
                                 from trueValue in values
                                 from nullValue in values
                                 from compareTo in compareToValuesExceptNull
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, nullValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    NullValue = testData.nullValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(new object(), typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : sut.FalseValue, result);
                Assert.AreEqual(testData.isNegative ? testData.trueValue : testData.falseValue, result);
            }
        }

        [TestMethod]
        public void Convert_TrueValueIsInputValue_IsEquals_ReturnInputValue()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in values
                                 from trueValue in values
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = ConverterSpecialValue.UseInputValue,
                    FalseValue = testData.falseValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var inputValue = testData.compareTo;
                var result = sut.Convert(inputValue, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : inputValue, result);
            }
        }

        [TestMethod]
        public void Convert_TrueValueIsConverterParameter_IsEquals_ReturnConverterParameter()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in values
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = ConverterSpecialValue.UseConverterParameter,
                    FalseValue = testData.falseValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var inputValue = testData.compareTo;
                var result = sut.Convert(inputValue, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : testData.converterParameter, result);
            }
        }

        [TestMethod]
        public void Convert_FalseValueIsInputValue_NotEquals_ReturnInputValue()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in values
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = ConverterSpecialValue.UseInputValue,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var inputValue = new object();
                var result = sut.Convert(inputValue, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : inputValue, result);
            }
        }

        [TestMethod]
        public void Convert_FalseValueIsConverterParameter_NotEquals_ReturnConverterParameter()
        {
            object obj = new object();
            var values = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in values
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = ConverterSpecialValue.UseConverterParameter,
                    CompareTo = testData.compareTo,
                    IsNegative = testData.isNegative
                };
                var inputValue = new object();
                var result = sut.Convert(inputValue, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : testData.converterParameter, result);
            }
        }
    }
}
