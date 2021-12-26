using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Data;
using WpfMart.Converters;
using System.Globalization;
using System.Linq;

namespace WpfMart.Tests
{
    [TestClass]
    public class BooleanConverterTest
    {
        [TestMethod]
        public void Convert_InputIsTrue_ReturnValueOfTrueValueProperty()
        {
            var testData = new object[] { true, false, Visibility.Visible, Visibility.Collapsed, null, "True" };
            var testParameters = from trueValue in testData
                    from falseValue in testData
                    from nullValue in testData
                    select new { trueValue, falseValue, nullValue };

            foreach (var parameter in testParameters)
            {
                Convert_InputIsTrue_ReturnValueOfTrueValueProperty(parameter.trueValue, parameter.falseValue, parameter.nullValue);
            }
        }

        private void Convert_InputIsTrue_ReturnValueOfTrueValueProperty(
            object trueValue,                                                                    
            object falseValue,                                                                   
            object nullValue)
        {
            var sut = new BoolConverter()
            {
                TrueValue = trueValue,
                FalseValue = falseValue,
                NullValue = nullValue
            };

            const object nonBoolConverterParameter = null;
            CultureInfo anyCulture = CultureInfo.CurrentCulture;

            var actualResult = sut.Convert(true, typeof(Visibility), nonBoolConverterParameter, anyCulture);

            Assert.AreEqual(sut.TrueValue, actualResult);
        }
    }
}
