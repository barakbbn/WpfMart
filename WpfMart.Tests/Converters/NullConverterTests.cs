using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfMart.Converters;

namespace WpfMart.Tests.Converters
{
    [TestClass]
    public class NullConverterTests
    {
        [TestMethod]
        public void Convert_IsNull_TrueValue()
        {
            object obj = new object();
            var trueValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var compareToValues = new object[] { null };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in trueValues
                                 from falseValue in trueValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new WpfMart.Converters.EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative
                };
                var result = sut.Convert(testData.compareTo, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.FalseValue : sut.TrueValue, result);
                Assert.AreEqual(testData.isNegative ? testData.falseValue : testData.trueValue, result);
            }
        }

        [TestMethod]
        public void Convert_IsNotNull_FalseValue()
        {
            object obj = new object();
            var trueValues = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var compareToValues = new object[] { true, false, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var converterParameters = new object[] { true, false, null, Visibility.Visible, Visibility.Collapsed, "True", "", obj, Guid.NewGuid(), 0, 0.0, 0m, -1, -1.0, -1m, 1, 1.0, 1m, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End, DateTime.MaxValue, new DateTime(), SystemColors.ActiveBorderColor, SystemColors.ActiveBorderBrush, DependencyProperty.UnsetValue };
            var testParameters = from trueValue in trueValues
                                 from falseValue in trueValues
                                 from compareTo in compareToValues
                                 from converterParameter in converterParameters
                                 from isNegative in new[] { false, true }
                                 select new { trueValue, falseValue, compareTo, converterParameter, isNegative };

            foreach (var testData in testParameters)
            {
                var sut = new WpfMart.Converters.EqualityConverter()
                {
                    TrueValue = testData.trueValue,
                    FalseValue = testData.falseValue,
                    IsNegative = testData.isNegative,
                };
                var result = sut.Convert(testData.compareTo, typeof(object), testData.converterParameter, CultureInfo.CurrentCulture);

                Assert.AreEqual(testData.isNegative ? sut.TrueValue : sut.FalseValue, result);
                Assert.AreEqual(testData.isNegative ? testData.trueValue : testData.falseValue, result);
            }
        }
    }
}
