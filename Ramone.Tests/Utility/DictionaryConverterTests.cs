using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.Utility;

namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class DictionaryConverterTests : TestHelper
  {
    [Test]
    public void CanConvertSimpleObjectsToDictionary()
    {
      // Arrange
      object o1 = new { A = 10, B = "Train", C = true };
      object o2 = new SimpleObject { X = 10, Y = "Train", Z = true };

      // Act
      Dictionary<string, string> d1 = DictionaryConverter.ConvertObjectPropertiesToDictionary(o1);
      Dictionary<string, string> d2 = DictionaryConverter.ConvertObjectPropertiesToDictionary(o2);

      // Assert
      Assert.IsNotNull(d1);
      Assert.IsNotNull(d2);
      Assert.AreEqual(3, d1.Count);
      Assert.AreEqual(4, d2.Count);

      Assert.AreEqual("10", d1["A"]);
      Assert.AreEqual("Train", d1["B"]);
      Assert.AreEqual("true", d1["C"]);
      Assert.AreEqual("10", d2["X"]);
      Assert.AreEqual("Train", d2["Y"]);
      Assert.AreEqual("true", d2["Z"]);
    }


    [Test]
    public void WhenConvertingNullItReturnsEmptyDictionary()
    {
      // Arrange

      // Act
      Dictionary<string, string> d = DictionaryConverter.ConvertObjectPropertiesToDictionary(null);

      // Assert
      Assert.IsNotNull(d);
      Assert.AreEqual(0, d.Count);
    }


    [Test]
    public void WhenConvertingDictionaryItReturnsIt()
    {
      // Arrange
      Dictionary<string, string> d1 = new Dictionary<string, string>();
      d1["W"] = "Window";

      // Act
      Dictionary<string, string> d2 = DictionaryConverter.ConvertObjectPropertiesToDictionary(d1);

      // Assert
      Assert.IsNotNull(d2);
      Assert.AreEqual(1, d2.Count);
      Assert.AreEqual("Window", d2["W"]);
    }


    public class SimpleObject
    {
      public int _x2;
      public int X { get; set; }

      public string _y2;
      public string Y { get; set; }

      public bool _z2;
      public bool Z { get; set; }
      
      
      public int Dummy { get; set; }
    }
  }
}
