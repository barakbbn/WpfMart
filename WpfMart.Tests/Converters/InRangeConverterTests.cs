using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfMart.Converters;

namespace WpfMart.Tests.Converters
{
    [TestClass]
    public class InRangeConverterTests
    {
        #region InRangeConverter - any object

        [TestMethod]
        public void ComparableClassHierarcy()
        {
            var sut = new InRangeConverter
            {
                From= new RegularEmplopyee { WorkStartDate=DateTime.Parse("2016-01-01")},
                To=new VP { WorkStartDate = DateTime.Parse("2016-01-01") }
            };
            var value = new Manager { WorkStartDate = DateTime.Parse("2016-01-01") };

            var actual = sut.Convert(value, typeof(object), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(true, actual);
        }

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
            var sut = new InRangeConverter
            {
                TrueValue = expected,
            };

            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter
            {
                FalseValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter
            {
                IsNegative = true,
                FalseValue = expected,
            };

            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter
            {
                IsNegative = true,
                TrueValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter
            {
                FalseValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter(true)
            {
                TrueValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter
            {
                FalseValue = "False",
                TrueValue = "True",
                IsNullable = true
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter
            {
                FalseValue = "False",
                TrueValue = "True",
                NullValue = expected
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
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
            var sut = new InRangeConverter(true)
            {
                TrueValue = expected,
            };
            foreach (var values in valueAndLimit)
            {
                sut.From = values[1];
                sut.To = values[2];
                var actual = sut.Convert(values[0], typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Convert_GreaterThan_ValueIsGreatherThan_TrueValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "1" },
                { "a", "b" },
                {-1, 0 }, {0, 1}, {1, 2},
                {0U, 1U}, {1U, 2U},
                {-1.0, -0.99}, {0.0, 0.01}, {1.0, 1.01},
                {-1.0M, -0.99M}, {0.0M, 0.01M}, {1.0M, 1.01M},
                {new DateTime(), new DateTime().AddMilliseconds(1)},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                {new Version(0,0,800,130), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,1,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.After = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_GreaterThanEquals_ValueIsGreatherThan_TrueValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "1" },
                { "a", "b" },
                {-1, 0 }, {0, 1}, {1, 2},
                {0U, 1U}, {1U, 2U},
                {-1.0, -0.99}, {0.0, 0.01}, {1.0, 1.01},
                {-1.0M, -0.99M}, {0.0M, 0.01M}, {1.0M, 1.01M},
                {new DateTime(), new DateTime().AddMilliseconds(1)},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                {new Version(0,0,800,130), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,1,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.From = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_GreaterThanEquals_ValueIsEquals_TrueValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "" },
                { "a", "a" },
                {-1, -1 }, {0, 0}, {1, 1},
                {0U, 0U}, {1U, 1U},
                {-1.0, -1.0}, {0.0, 0.0}, {1.0, 1.0},
                {-1.0M, -1.0M}, {0.0M, 0.0M}, {1.0M, 1.0M},
                {new DateTime(), new DateTime()},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.From = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }


        [TestMethod]
        public void Convert_LessThan_ValueIsLessThan_TrueValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "1", ""},
                { "b", "a" },
                {0, -1 }, {1, 0}, {2, 1},
                {1U, 0U}, {2U, 1U},
                {-0.99, -1.0}, {0.01, 0.0}, {1.01, 1.0},
                {-0.99M, -1.0M}, {0.01M, 0.0M}, {1.01M, 1.0M},
                {new DateTime().AddMilliseconds(1), new DateTime()},
                {DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                {new TimeSpan(0,0,0,0,1), new TimeSpan()},
                {TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                {System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(0,0,800,130)},
                {new Version(1,1,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.Before = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LessThanEquals_ValueIsLessThan_TrueValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "1", ""},
                { "b", "a" },
                {0, -1 }, {1, 0}, {2, 1},
                {1U, 0U}, {2U, 1U},
                {-0.99, -1.0}, {0.01, 0.0}, {1.01, 1.0},
                {-0.99M, -1.0M}, {0.01M, 0.0M}, {1.01M, 1.0M},
                {new DateTime().AddMilliseconds(1), new DateTime()},
                {DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                {new TimeSpan(0,0,0,0,1), new TimeSpan()},
                {TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                {System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(0,0,800,130)},
                {new Version(1,1,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.To = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LessThanEquals_ValueIsEquals_TrueValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "" },
                { "a", "a" },
                {-1, -1 }, {0, 0}, {1, 1},
                {0U, 0U}, {1U, 1U},
                {-1.0, -1.0}, {0.0, 0.0}, {1.0, 1.0},
                {-1.0M, -1.0M}, {0.0M, 0.0M}, {1.0M, 1.0M},
                {new DateTime(), new DateTime()},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.To = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }


        [TestMethod]
        public void Convert_GreaterThan_ValueIsEquals_FalseValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "" },
                { "a", "a" },
                {-1, -1 }, {0, 0}, {1, 1},
                {0U, 0U}, {1U, 1U},
                {-1.0, -1.0}, {0.0, 0.0}, {1.0, 1.0},
                {-1.0M, -1.0M}, {0.0M, 0.0M}, {1.0M, 1.0M},
                {new DateTime(), new DateTime()},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.After = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_GreaterThan_ValueIsLessThan_FalseValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "1", ""},
                { "b", "a" },
                {0, -1 }, {1, 0}, {2, 1},
                {1U, 0U}, {2U, 1U},
                {-0.99, -1.0}, {0.01, 0.0}, {1.01, 1.0},
                {-0.99M, -1.0M}, {0.01M, 0.0M}, {1.01M, 1.0M},
                {new DateTime().AddMilliseconds(1), new DateTime()},
                {DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                {new TimeSpan(0,0,0,0,1), new TimeSpan()},
                {TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                {System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(0,0,800,130)},
                {new Version(1,1,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.After = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_GreaterThanEquals_ValueIsLessThan_FalseValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "1", ""},
                { "b", "a" },
                {0, -1 }, {1, 0}, {2, 1},
                {1U, 0U}, {2U, 1U},
                {-0.99, -1.0}, {0.01, 0.0}, {1.01, 1.0},
                {-0.99M, -1.0M}, {0.01M, 0.0M}, {1.01M, 1.0M},
                {new DateTime().AddMilliseconds(1), new DateTime()},
                {DateTime.Parse("2000-01-01 00:00:00.001"), DateTime.Parse("2000-01-01")},
                {new TimeSpan(0,0,0,0,1), new TimeSpan()},
                {TimeSpan.Parse("00:00:01.001"), TimeSpan.Parse("00:00:01")},
                {System.IO.SeekOrigin.Current, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(0,0,800,130)},
                {new Version(1,1,0,0), new Version(1,0,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.From = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }


        [TestMethod]
        public void Convert_LessThan_ValueIsGreaterThan_FalseValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "1" },
                { "a", "b" },
                {-1, 0 }, {0, 1}, {1, 2},
                {0U, 1U}, {1U, 2U},
                {-1.0, -0.99}, {0.0, 0.01}, {1.0, 1.01},
                {-1.0M, -0.99M}, {0.0M, 0.01M}, {1.0M, 1.01M},
                {new DateTime(), new DateTime().AddMilliseconds(1)},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                {new Version(0,0,800,130), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,1,0,0)}
            };

            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.Before = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LessThan_ValueIsEquals_FalseValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "" },
                { "a", "a" },
                {-1, -1 }, {0, 0}, {1, 1},
                {0U, 0U}, {1U, 1U},
                {-1.0, -1.0}, {0.0, 0.0}, {1.0, 1.0},
                {-1.0M, -1.0M}, {0.0M, 0.0M}, {1.0M, 1.0M},
                {new DateTime(), new DateTime()},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.000")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,0)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.000")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Begin},
                {new Version(1,0,15,0), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,0,0,0)}
            };

            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.Before = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        [TestMethod]
        public void Convert_LessThanEquals_ValueIsGreaterThan_FalseValue()
        {
            var valueAndLimit = new Dictionary<object, object>
            {
                { "", "1" },
                { "a", "b" },
                {-1, 0 }, {0, 1}, {1, 2},
                {0U, 1U}, {1U, 2U},
                {-1.0, -0.99}, {0.0, 0.01}, {1.0, 1.01},
                {-1.0M, -0.99M}, {0.0M, 0.01M}, {1.0M, 1.01M},
                {new DateTime(), new DateTime().AddMilliseconds(1)},
                {DateTime.Parse("2000-01-01"), DateTime.Parse("2000-01-01 00:00:00.001")},
                {new TimeSpan(), new TimeSpan(0,0,0,0,1)},
                {TimeSpan.Parse("00:00:01"), TimeSpan.Parse("00:00:01.001")},
                {System.IO.SeekOrigin.Begin, System.IO.SeekOrigin.Current},
                {new Version(0,0,800,130), new Version(1,0,15,0)},
                {new Version(1,0,0,0), new Version(1,1,0,0)}
            };


            var sut = new InRangeConverter
            {
                TrueValue = true,
                FalseValue = false,
            };

            foreach (var pair in valueAndLimit)
            {
                sut.To = pair.Key;
                var actual = sut.Convert(pair.Value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }


        #endregion

        [TestMethod]
        public void ConvertNumber_InRange_TrueValue()
        {
            var sut = new NumInRangeConverter
            {
                After = 0,
                TrueValue = true,
                FalseValue = false,
                IsNullable = false
            };

            var values = new object[] { 0.1, 0.1m, 2, 2L};
            foreach (var value in values)
            {
                var actual = sut.Convert(value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.TrueValue, actual);
            }
        }

        [TestMethod]
        public void ConvertNumber_NotInRange_TrueValue()
        {
            var sut = new NumInRangeConverter
            {
                From = -1,
                Before = 0,
                TrueValue = true,
                FalseValue = false,
                IsNullable = false
            };

            var values = new object[] { 0.0d, 0.0f, -0.0m, 0, 0L, 0U, 0UL, "0.0", System.IO.SeekOrigin.Begin, null, false, '\x00'};
            foreach (var value in values)
            {
                var actual = sut.Convert(value, typeof(object), null, CultureInfo.CurrentCulture);
                Assert.AreEqual(sut.FalseValue, actual);
            }
        }

        #region helper types

        private interface IEmployee
        {
            DateTime WorkStartDate { get; set; }
            string Name { get; set; }
            int AchievmentsCount { get; set; }
        }
        private abstract class EmployeeBase: IEmployee, IComparable, IComparable<IEmployee>
        {
            private static List<Type> _employeeTypeRanking = new List<Type>
            {
                typeof(RegularEmplopyee),
                typeof(Manager),
                typeof(VP),
                typeof(Cto),
            };

            public DateTime WorkStartDate { get; set; }
            public string Name { get; set; }
            public int AchievmentsCount { get; set; }

            public virtual int CompareTo(object other)
            {
                if (ReferenceEquals(other, null)) return 1;
                return CompareTo(other as IEmployee);
            }

            public virtual int CompareTo(IEmployee other)
            {
                if (ReferenceEquals(other, null)) return 1;
                var thisRank = _employeeTypeRanking.IndexOf(GetType());
                var otherRank = _employeeTypeRanking.IndexOf(other.GetType());
                var compare = thisRank.CompareTo(otherRank);
                if (compare != 0) return compare;

                var acheivmentsCompare = this.AchievmentsCount.CompareTo(other.AchievmentsCount);
                if (acheivmentsCompare != 0) return acheivmentsCompare;

                var workExperienceCompare = other.WorkStartDate.CompareTo(WorkStartDate);
                return workExperienceCompare;
            }
        }

        private class RegularEmplopyee : EmployeeBase
        {
        }

        private class Manager : EmployeeBase
        {
            public List<IEmployee> Employees { get; set; }
        }
        private class VP : Manager
        {
        }
        private class Cto : Manager
        {
        }

        #endregion
    }



}
