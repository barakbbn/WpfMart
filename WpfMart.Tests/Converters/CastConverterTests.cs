using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfMart.Converters;

namespace WpfMart.Tests.Converters
{
    [TestClass]
    public class CastConverterTests
    {
        #region To enum

        [TestMethod]
        public void IntToEnum()
        {
            var expected = System.IO.SeekOrigin.Current;
            var intValue = (int)expected;
            var targetType = typeof(System.IO.SeekOrigin);
            var sut = new CastConverter(targetType);
            var actual = sut.Convert(intValue, targetType, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StringToEnum()
        {
            var expected = System.IO.SeekOrigin.Current;
            var stringValue = expected.ToString();
            var targetType = typeof(System.IO.SeekOrigin);
            var sut = new CastConverter(targetType);
            var actual = sut.Convert(stringValue, targetType, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StringNumberToEnum()
        {
            var expected = System.IO.SeekOrigin.Current;
            var stringValue = ((int)expected).ToString();
            var targetType = typeof(System.IO.SeekOrigin);
            var sut = new CastConverter(targetType);
            var actual = sut.Convert(stringValue, targetType, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DoubleToEnum_Exception()
        {
            var doubleValue = (double)System.IO.SeekOrigin.Current;
            var targetType = typeof(System.IO.SeekOrigin);
            var sut = new CastConverter(targetType);
            object actual = sut.Convert(doubleValue, targetType, null, CultureInfo.CurrentCulture);
        }


        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void InvalidStringToEnum_Exception()
        {
            var stringValue = "";
            var targetType = typeof(System.IO.SeekOrigin);
            var sut = new CastConverter(targetType);
            var actual = sut.Convert(stringValue, targetType, null, CultureInfo.CurrentCulture);
        }

        [TestMethod]
        public void OutOfRangeIntToEnum()
        {
            const int valueNotInEnum = 100;
            var expected = (System.IO.SeekOrigin)valueNotInEnum;
            var targetType = typeof(System.IO.SeekOrigin);
            var sut = new CastConverter(targetType);
            var actual = sut.Convert(valueNotInEnum, targetType, null, CultureInfo.CurrentCulture);

            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
}
