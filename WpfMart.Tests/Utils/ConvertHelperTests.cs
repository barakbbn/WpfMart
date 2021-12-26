using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfMart.Tests.Utils
{
    using Pair = KeyValuePair<object, object>;
    [TestClass]
    public class ConvertHelperTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var typeAndValues = new Dictionary<Type, Pair[]>
            {
                {
                    typeof(CultureInfo), new []
                    {
                        new Pair("en-us", CultureInfo.GetCultureInfo("en-us") )
                    }
                },
                {
                    typeof(System.IO.SeekOrigin), new []
                    {
                        new Pair("-1", (System.IO.SeekOrigin)(-1)),
                        new Pair(-1, (System.IO.SeekOrigin)(-1))
                    }
                },
            };

            foreach (var pair in typeAndValues)
            {
                foreach (var inout in pair.Value)
                {
                    var actual = WpfMart.Utils.ConvertHelper.ChangeType(inout.Key, pair.Key);
                    Assert.AreEqual(inout.Value, actual);
                }
            }
        }
    }

    
}
