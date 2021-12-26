using System;
using System.Globalization;
using WpfMart.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfMart.Tests.Converters
{
    [TestClass]
    public class InRangeMultiConverterTests
    {
        [TestMethod]
        public void ConvertInRange_CustomTrueValue_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "1", "", "2"},
                new object[]{ "b", "a" ,"c"},
                new object[]{0, -1 ,1},
                new object[]{1, 0, 2},
                new object[]{2, 1, 3},
                new object[]{1U, 0U, 2U},
                new object[]{2U, 1U, 3U},
                new object[]{-0.99, -1.0, -0.1},
                new object[]{0.01, 0.0, 0.1},
                new object[]{1.01, 1.0, 1.02},
                new object[]{-0.99M, -1.0M, -0.1M},
                new object[]{0.01M, 0.0M, 0.1M},
                new object[]{1.01M, 1.0M, 1.1M},
                new object[]{new DateTime().AddMilliseconds(1), new DateTime(), new DateTime().AddMilliseconds(2)},
                new object[]{DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{new TimeSpan(0,0,0,0,1), new TimeSpan(), new TimeSpan(0,0,0,0,2)},
                new object[]{TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.002")},
                new object[]{System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End},
                new object[]{new Version(1,0,15,0), new Version(0,0,800,130), new Version(2,0,0,0)},
                new object[]{new Version(1,1,0,0), new Version(1,0,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = "ConvertInRange_CustomTrueValue_TrueValue";
            var sut = new InRangeMultiConverter
            {
                TrueValue = expected,
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNotInRange_CustomFalseValue_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "1", "2"},
                new object[]{ "a", "b", "c"},
                new object[]{-1, 0, 1},
                new object[]{0, 1, 2},
                new object[]{1, 2, 3},
                new object[]{0U, 1U, 2U},
                new object[]{1U, 2U, 3U},
                new object[]{-1.0, -0.99, -0.1},
                new object[]{0.0, 0.01, 0.1},
                new object[]{1.0, 1.01, 1.02},
                new object[]{-1.0M, -0.99M, -0.1M},
                new object[]{0.0M, 0.01M, 0.1M},
                new object[]{1.0M, 1.01M, 1.1M},
                new object[]{new DateTime(), new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{new Version(0,0,800,130), new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{new Version(1,0,0,0), new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = new IndexOutOfRangeException();
            var sut = new InRangeMultiConverter
            {
                FalseValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertInRange_IsNegative_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "1", "", "2"},
                new object[]{ "b", "a" ,"c"},
                new object[]{0, -1 ,1},
                new object[]{1, 0, 2},
                new object[]{2, 1, 3},
                new object[]{1U, 0U, 2U},
                new object[]{2U, 1U, 3U},
                new object[]{-0.99, -1.0, -0.1},
                new object[]{0.01, 0.0, 0.1},
                new object[]{1.01, 1.0, 1.02},
                new object[]{-0.99M, -1.0M, -0.1M},
                new object[]{0.01M, 0.0M, 0.1M},
                new object[]{1.01M, 1.0M, 1.1M},
                new object[]{new DateTime().AddMilliseconds(1), new DateTime(), new DateTime().AddMilliseconds(2)},
                new object[]{DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{new TimeSpan(0,0,0,0,1), new TimeSpan(), new TimeSpan(0,0,0,0,2)},
                new object[]{TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.002")},
                new object[]{System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.End},
                new object[]{new Version(1,0,15,0), new Version(0,0,800,130), new Version(2,0,0,0)},
                new object[]{new Version(1,1,0,0), new Version(1,0,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = new ArgumentOutOfRangeException();
            var sut = new InRangeMultiConverter
            {
                IsNegative = true,
                FalseValue = expected,
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNotInRange_IsNegative_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "1", "2"},
                new object[]{ "a", "b", "c"},
                new object[]{-1, 0, 1},
                new object[]{0, 1, 2},
                new object[]{1, 2, 3},
                new object[]{0U, 1U, 2U},
                new object[]{1U, 2U, 3U},
                new object[]{-1.0, -0.99, -0.1},
                new object[]{0.0, 0.01, 0.1},
                new object[]{1.0, 1.01, 1.02},
                new object[]{-1.0M, -0.99M, -0.1M},
                new object[]{0.0M, 0.01M, 0.1M},
                new object[]{1.0M, 1.01M, 1.1M},
                new object[]{new DateTime(), new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{new Version(0,0,800,130), new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{new Version(1,0,0,0), new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = "ConvertNotInRange_IsNegative_TrueValue";
            var sut = new InRangeMultiConverter
            {
                IsNegative = true,
                TrueValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNull_NullableNotSupported_AndCustomFalseValue_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ null, "1", "2"},
                new object[]{ null, "a", "c"},
                new object[]{ null, 0, 1},
                new object[]{ null, 1, 2},
                new object[]{ null, 2, 3},
                new object[]{ null, 1U, 2U},
                new object[]{ null, 2U, 3U},
                new object[]{ null, -0.99, -0.1},
                new object[]{ null, 0.01, 0.1},
                new object[]{ null, 1.01, 1.02},
                new object[]{ null, -0.99M, -0.1M},
                new object[]{ null, 0.01M, 0.1M},
                new object[]{ null, 1.01M, 1.1M},
                new object[]{ null, new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{ null, DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{ null, new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{ null, TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{ null, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{ null, new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{ null, new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = new IndexOutOfRangeException();
            var sut = new InRangeMultiConverter
            {
                FalseValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNull_NullableNotSupported_AndIsNegative_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ null, "1", "2"},
                new object[]{ null, "a", "c"},
                new object[]{ null, 0, 1},
                new object[]{ null, 1, 2},
                new object[]{ null, 2, 3},
                new object[]{ null, 1U, 2U},
                new object[]{ null, 2U, 3U},
                new object[]{ null, -0.99, -0.1},
                new object[]{ null, 0.01, 0.1},
                new object[]{ null, 1.01, 1.02},
                new object[]{ null, -0.99M, -0.1M},
                new object[]{ null, 0.01M, 0.1M},
                new object[]{ null, 1.01M, 1.1M},
                new object[]{ null, new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{ null, DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{ null, new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{ null, TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{ null, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{ null, new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{ null, new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = "ConvertNull_NullableNotSupported_AndIsNegative_TrueValue";
            var sut = new InRangeMultiConverter(true)
            {
                TrueValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNull_NullableSupported_Null()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ null, "1", "2"},
                new object[]{ null, "a", "c"},
                new object[]{ null, 0, 1},
                new object[]{ null, 1, 2},
                new object[]{ null, 2, 3},
                new object[]{ null, 1U, 2U},
                new object[]{ null, 2U, 3U},
                new object[]{ null, -0.99, -0.1},
                new object[]{ null, 0.01, 0.1},
                new object[]{ null, 1.01, 1.02},
                new object[]{ null, -0.99M, -0.1M},
                new object[]{ null, 0.01M, 0.1M},
                new object[]{ null, 1.01M, 1.1M},
                new object[]{ null, new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{ null, DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{ null, new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{ null, TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{ null, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{ null, new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{ null, new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            object expected = null;
            var sut = new InRangeMultiConverter
            {
                FalseValue = "False",
                TrueValue = "True",
                IsNullable = true
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNull_NullableSupported_AndCustomNullValue_NullValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ null, "1", "2"},
                new object[]{ null, "a", "c"},
                new object[]{ null, 0, 1},
                new object[]{ null, 1, 2},
                new object[]{ null, 2, 3},
                new object[]{ null, 1U, 2U},
                new object[]{ null, 2U, 3U},
                new object[]{ null, -0.99, -0.1},
                new object[]{ null, 0.01, 0.1},
                new object[]{ null, 1.01, 1.02},
                new object[]{ null, -0.99M, -0.1M},
                new object[]{ null, 0.01M, 0.1M},
                new object[]{ null, 1.01M, 1.1M},
                new object[]{ null, new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{ null, DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{ null, new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{ null, TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{ null, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{ null, new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{ null, new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            object expected = System.IO.SeekOrigin.Current;
            var sut = new InRangeMultiConverter
            {
                FalseValue = "False",
                TrueValue = "True",
                NullValue = expected
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ConvertNull_NullableSupported_AndIsNegative_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ null, "1", "2"},
                new object[]{ null, "a", "c"},
                new object[]{ null, 0, 1},
                new object[]{ null, 1, 2},
                new object[]{ null, 2, 3},
                new object[]{ null, 1U, 2U},
                new object[]{ null, 2U, 3U},
                new object[]{ null, -0.99, -0.1},
                new object[]{ null, 0.01, 0.1},
                new object[]{ null, 1.01, 1.02},
                new object[]{ null, -0.99M, -0.1M},
                new object[]{ null, 0.01M, 0.1M},
                new object[]{ null, 1.01M, 1.1M},
                new object[]{ null, new DateTime().AddMilliseconds(1), new DateTime().AddMilliseconds(2)},
                new object[]{ null, DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01 00:00:00.002")},
                new object[]{ null, new TimeSpan(0,0,0,0,1), new TimeSpan(0,0,0,0,2)},
                new object[]{ null, TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01.002")},
                new object[]{ null, System.IO.SeekOrigin.Current, System.IO.SeekOrigin.End},
                new object[]{ null, new Version(1,0,15,0), new Version(2,0,0,0)},
                new object[]{ null, new Version(1,1,0,0), new Version(1, 1, 0, 1) }
            };

            var expected = "ConvertNull_NullableNotSupported_AndIsNegative_TrueValue";
            var sut = new InRangeMultiConverter(true)
            {
                TrueValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }


        [TestMethod]
        public void Convert_LowerExclusive_ValueIsGreatherThanLower_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "1", ""},
                new object[]{ "b", "a" },
                new object[]{0, -1 },
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{1U, 0U},
                new object[]{2U, 1U},
                new object[]{-0.99, -1.0},
                new object[]{0.01, 0.0},
                new object[]{1.01, 1.0},
                new object[]{-0.99M, -1.0M},
                new object[]{0.01M, 0.0M},
                new object[]{1.01M, 1.0M},
                new object[]{new DateTime().AddMilliseconds(1), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                new object[]{new TimeSpan(0,0,0,0,1), new TimeSpan()},
                new object[]{TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                new object[]{System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(0,0,800,130)},
                new object[]{new Version(1,1,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                LowerInclusive = false
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LowerInclusive_ValueIsGreatherThanLower_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "1", ""},
                new object[]{ "b", "a" },
                new object[]{0, -1 },
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{1U, 0U},
                new object[]{2U, 1U},
                new object[]{-0.99, -1.0},
                new object[]{0.01, 0.0},
                new object[]{1.01, 1.0},
                new object[]{-0.99M, -1.0M},
                new object[]{0.01M, 0.0M},
                new object[]{1.01M, 1.0M},
                new object[]{new DateTime().AddMilliseconds(1), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                new object[]{new TimeSpan(0,0,0,0,1), new TimeSpan()},
                new object[]{TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                new object[]{System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(0,0,800,130)},
                new object[]{new Version(1,1,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LowerInclusive_ValueIsEqualsLower_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "" },
                new object[]{ "a", "a" },
                new object[]{-1, -1 },
                new object[]{0, 0},
                new object[]{1, 1},
                new object[]{0U, 0U},
                new object[]{1U, 1U},
                new object[]{-1.0, -1.0},
                new object[]{0.0, 0.0},
                new object[]{1.0, 1.0},
                new object[]{-1.0M, -1.0M},
                new object[]{0.0M, 0.0M},
                new object[]{1.0M, 1.0M},
                new object[]{new DateTime(), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_UpperExlusive_ValueIsLessThanUpper_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "1" },
                new object[]{ "a", "b" },
                new object[]{-1, 0 },
                new object[]{0, 1},
                new object[]{1, 2},
                new object[]{0U, 1U},
                new object[]{1U, 2U},
                new object[]{-1.0, -0.99},
                new object[]{0.0, 0.01},
                new object[]{1.0, 1.01},
                new object[]{-1.0M, -0.99M},
                new object[]{0.0M, 0.01M},
                new object[]{1.0M, 1.01M},
                new object[]{new DateTime(), new DateTime().AddMilliseconds(1)},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                new object[]{new Version(0,0,800,130), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,1,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                UpperInclusive = false
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_UpperInclusive_ValueIsLessThanUpper_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "1" },
                new object[]{ "a", "b" },
                new object[]{-1, 0 },
                new object[]{0, 1},
                new object[]{1, 2},
                new object[]{0U, 1U},
                new object[]{1U, 2U},
                new object[]{-1.0, -0.99},
                new object[]{0.0, 0.01},
                new object[]{1.0, 1.01},
                new object[]{-1.0M, -0.99M},
                new object[]{0.0M, 0.01M},
                new object[]{1.0M, 1.01M},
                new object[]{new DateTime(), new DateTime().AddMilliseconds(1)},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                new object[]{new Version(0,0,800,130), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,1,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                UpperInclusive = true
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_UpperInclusive_ValueEqualsUpper_TrueValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "" },
                new object[]{ "a", "a" },
                new object[]{-1, -1 },
                new object[]{0, 0},
                new object[]{1, 1},
                new object[]{0U, 0U},
                new object[]{1U, 1U},
                new object[]{-1.0, -1.0},
                new object[]{0.0, 0.0},
                new object[]{1.0, 1.0},
                new object[]{-1.0M, -1.0M},
                new object[]{0.0M, 0.0M},
                new object[]{1.0M, 1.0M},
                new object[]{new DateTime(), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                UpperInclusive = true
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }


        [TestMethod]
        public void Convert_LowerExclusive_ValueEqualsLower_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "" },
                new object[]{ "a", "a" },
                new object[]{-1, -1 },
                new object[]{0, 0},
                new object[]{1, 1},
                new object[]{0U, 0U},
                new object[]{1U, 1U},
                new object[]{-1.0, -1.0},
                new object[]{0.0, 0.0},
                new object[]{1.0, 1.0},
                new object[]{-1.0M, -1.0M},
                new object[]{0.0M, 0.0M},
                new object[]{1.0M, 1.0M},
                new object[]{new DateTime(), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                LowerInclusive = false
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LowerExclusive_ValueLessThanLower_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "1" },
                new object[]{ "a", "b" },
                new object[]{-1, 0 },
                new object[]{0, 1},
                new object[]{1, 2},
                new object[]{0U, 1U},
                new object[]{1U, 2U},
                new object[]{-1.0, -0.99},
                new object[]{0.0, 0.01},
                new object[]{1.0, 1.01},
                new object[]{-1.0M, -0.99M},
                new object[]{0.0M, 0.01M},
                new object[]{1.0M, 1.01M},
                new object[]{new DateTime(), new DateTime().AddMilliseconds(1)},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                new object[]{new Version(0,0,800,130), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,1,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                LowerInclusive = false
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LowerInclusive_ValueLessThanLower_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "1" },
                new object[]{ "a", "b" },
                new object[]{-1, 0 },
                new object[]{0, 1},
                new object[]{1, 2},
                new object[]{0U, 1U},
                new object[]{1U, 2U},
                new object[]{-1.0, -0.99},
                new object[]{0.0, 0.01},
                new object[]{1.0, 1.01},
                new object[]{-1.0M, -0.99M},
                new object[]{0.0M, 0.01M},
                new object[]{1.0M, 1.01M},
                new object[]{new DateTime(), new DateTime().AddMilliseconds(1)},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                new object[]{new Version(0,0,800,130), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,1,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                LowerInclusive = true
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }


        [TestMethod]
        public void Convert_UpperExclusive_ValueEqualsUpper_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "", "" },
                new object[]{ "a", "a" },
                new object[]{-1, -1 },
                new object[]{0, 0},
                new object[]{1, 1},
                new object[]{0U, 0U},
                new object[]{1U, 1U},
                new object[]{-1.0, -1.0},
                new object[]{0.0, 0.0},
                new object[]{1.0, 1.0},
                new object[]{-1.0M, -1.0M},
                new object[]{0.0M, 0.0M},
                new object[]{1.0M, 1.0M},
                new object[]{new DateTime(), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                new object[]{new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                new object[]{TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                new object[]{System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(1,0,15,0)},
                new object[]{new Version(1,0,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                UpperInclusive = false
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_UpperExclusive_ValueGreaterThanUpper_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "1", ""},
                new object[]{ "b", "a" },
                new object[]{0, -1 },
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{1U, 0U},
                new object[]{2U, 1U},
                new object[]{-0.99, -1.0},
                new object[]{0.01, 0.0},
                new object[]{1.01, 1.0},
                new object[]{-0.99M, -1.0M},
                new object[]{0.01M, 0.0M},
                new object[]{1.01M, 1.0M},
                new object[]{new DateTime().AddMilliseconds(1), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                new object[]{new TimeSpan(0,0,0,0,1), new TimeSpan()},
                new object[]{TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                new object[]{System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(0,0,800,130)},
                new object[]{new Version(1,1,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                UpperInclusive = false
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_UpperInclusive_ValueGreaterThanUpper_FalseValue()
        {
            var valueAndLimit = new object[][]
            {
                new object[]{ "1", ""},
                new object[]{ "b", "a" },
                new object[]{0, -1 },
                new object[]{1, 0},
                new object[]{2, 1},
                new object[]{1U, 0U},
                new object[]{2U, 1U},
                new object[]{-0.99, -1.0},
                new object[]{0.01, 0.0},
                new object[]{1.01, 1.0},
                new object[]{-0.99M, -1.0M},
                new object[]{0.01M, 0.0M},
                new object[]{1.01M, 1.0M},
                new object[]{new DateTime().AddMilliseconds(1), new DateTime()},
                new object[]{DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                new object[]{new TimeSpan(0,0,0,0,1), new TimeSpan()},
                new object[]{TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                new object[]{System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                new object[]{new Version(1,0,15,0), new Version(0,0,800,130)},
                new object[]{new Version(1,1,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeMultiConverter
            {
                TrueValue = true,
                FalseValue = false,
                UpperInclusive = true
            };

            foreach (var values in valueAndLimit)
            {
                var actual = sut.Convert(values, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

    }
}

