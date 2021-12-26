using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfMart.Converters;

namespace WpfMart.Tests.Converters
{
    [TestClass]
    public class EqualityMultiConverterTests
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
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };
                object[] values = new []{ testData.compareTo, testData.compareTo };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

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
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };
                object[] values = new[] { testData.converterParameter, ConverterSpecialValue.UseConverterParameter };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

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
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };
                object[] values = new[] { new object(), testData.compareTo };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

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
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };
                object[] values = new[] { new object(), ConverterSpecialValue.UseConverterParameter };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : sut.FalseValue, result);
                Assert.AreEqual(testData.isNegative ? testData.trueValue : testData.falseValue, result);
            }
        }

        [TestMethod]
        public void Convert_TrueValueIsInputValue_IsEquals_ReturnInputValue()
        {
            object obj = new object();
            var allValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in allValues
                                 from trueValue in allValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = ConverterSpecialValue.UseInputValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };

                var inputValue = testData.compareTo;
                object[] values = new[] { inputValue, testData.compareTo };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : inputValue, result);
            }
        }

        [TestMethod]
        public void Convert_TrueValueIsConverterParameter_IsEquals_ReturnConverterParameter()
        {
            object obj = new object();
            var allValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from falseValue in allValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = ConverterSpecialValue.UseConverterParameter,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };
                var inputValue = testData.compareTo;
                object[] values = new[] { inputValue, testData.compareTo };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : testData.converterParameter, result);
            }
        }

        [TestMethod]
        public void Convert_FalseValueIsInputValue_NotEquals_ReturnInputValue()
        {
            object obj = new object();
            var allValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in allValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = ConverterSpecialValue.UseInputValue,
                    IsNegative = testData.isNegative
                };
                var inputValue = new object();
                object[] values = new[] { inputValue, testData.compareTo };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : inputValue, result);
            }
        }

        [TestMethod]
        public void Convert_FalseValueIsConverterParameter_NotEquals_ReturnConverterParameter()
        {
            object obj = new object();
            var allValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue};
            var compareToValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in allValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new EqualityMultiConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = ConverterSpecialValue.UseConverterParameter,
                    IsNegative = testData.isNegative
                };
                var inputValue = new object();
                object[] values = new[] { inputValue, testData.compareTo };
                var result = sut.Convert(values, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : testData.converterParameter, result);
            }
        }
    }    
}
