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
      Assert.That(d1.Count, Is.EqualTo(3));
      Assert.That(d2.Count, Is.EqualTo(4));

      Assert.That(d1["A"], Is.EqualTo("10"));
      Assert.That(d1["B"], Is.EqualTo("Train"));
      Assert.That(d1["C"], Is.EqualTo("true"));
      Assert.That(d2["X"], Is.EqualTo("10"));
      Assert.That(d2["Y"], Is.EqualTo("Train"));
      Assert.That(d2["Z"], Is.EqualTo("true"));
    }


    [Test]
    public void WhenConvertingNullItReturnsEmptyDictionary()
    {
      // Arrange

      // Act
      Dictionary<string, string> d = DictionaryConverter.ConvertObjectPropertiesToDictionary(null);

      // Assert
      Assert.IsNotNull(d);
      Assert.That(d.Count, Is.EqualTo(0));
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
      Assert.That(d2.Count, Is.EqualTo(1));
      Assert.That(d2["W"], Is.EqualTo("Window"));
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
