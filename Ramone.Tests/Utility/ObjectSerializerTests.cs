using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class ObjectSerializerTests : TestHelper
  {
    [Test]
    public void CanSerializeSimpleClass()
    {
      // Arrange
      object o = new 
      { 
        A = 10, 
        B = "Train", 
        C = true 
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A=10|B=Train|C=True", result);
    }


    [Test]
    public void CanSerializeGuid()
    {
      // Arrange
      Guid g = Guid.NewGuid();
      object o = new
      {
        A = g
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A="+g.ToString(), result);
    }


    [Test]
    public void CanSerializeClassWithDictionary()
    {
      // Arrange
      var o = new
      {
        A = 10,
        B = new Dictionary<string, object>()
      };
      o.B["X"] = 100;
      o.B["K"] = true;

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A=10|B[X]=100|B[K]=True", result);
    }


    [Test]
    public void CanSerializeClassWithNestedDictionaries()
    {
      // Arrange
      var o = new
      {
        A = 10,
        B = new Dictionary<string, object>()
      };
      o.B["X"] = 100;
      Dictionary<string,string> d = new Dictionary<string,string>();
      o.B["D"] = d;
      d["a"] = "Ok";
      d["b"] = "Fail";

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A=10|B[X]=100|B[D][a]=Ok|B[D][b]=Fail", result);
    }


    [Test]
    public void CanSerializeClassWithArray()
    {
      // Arrange
      var o = new
      {
        A = 10,
        B = new int[] { 10, 20 }
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A=10|B[0]=10|B[1]=20", result);
    }


    [Test]
    public void CanSerializeClassWithNestedArrays()
    {
      // Arrange
      var o = new
      {
        A = 10,
        B = new object[]
        {
          "James",
          new string[] { "Jamie", "Jolee" }
        }
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A=10|B[0]=James|B[1][0]=Jamie|B[1][1]=Jolee", result);
    }


    [Test]
    public void CanSerializeClassWithNestedClass()
    {
      // Arrange
      var o = new
      {
        B = new { x = 2, y = "abc" },
        A = 10
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|B.x=2|B.y=abc|A=10", result);
    }


    [Test]
    public void CanSerializeClassWithNullValues()
    {
      // Arrange
      var o = new
      {
        B = new { x = (int?)null, y = "abc" },
        A = (string)null
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|B.x=|B.y=abc|A=", result);
    }


    [Test]
    public void CanSerializeWithDifferentSeparatorFormaters()
    {
      // Arrange
      var o = new
      {
        B = new string[] { "abc", "xyz" },
        A = new Hashtable()
      };
      ((Hashtable)o.A)["p"] = 17;
      ((Hashtable)o.A)["q"] = "abc";

      ObjectSerializerSettings settings = new ObjectSerializerSettings
      {
        ArrayFormat = "{0}:{1}",
        DictionaryFormat = "{0}#{1}",
        PropertyFormat = "{0}+{1}"
      };

      // Act
      string result = Serialize(o, settings);

      // Assert
      Assert.AreEqual("|B:0=abc|B:1=xyz|A#p=17|A#q=abc", result);
    }


    [Test]
    public void CanSerializeComplexMix()
    {
      // Arrange
      var o = new
      {
        B = new 
        { 
          x = 2, 
          y = "abc" 
        },
        A = new object[]
        {
          "xyz",
          new Dictionary<string, object>(),
          new { p = 2, q = "uuu" }
        }
      };
      ((Dictionary<string, object>)o.A[1])["I"] = new
      {
        b = 9,
        n = 7
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|B.x=2|B.y=abc|A[0]=xyz|A[1][I].b=9|A[1][I].n=7|A[2].p=2|A[2].q=uuu", result);
    }


    [Test]
    public void CanSerializeComplexClass()
    {
      // Arrange
      ComplexClassForSerializationTests o = new ComplexClassForSerializationTests
      {
        X = 15,
        Y = "Abc",
        IntArray = new int[] { 1, 2 },
        SubC = new ComplexClassForSerializationTests.SubClass
        {
          SubC = new ComplexClassForSerializationTests.SubClass
          {
            Data = new object[]
            {
              new Hashtable()
            }
          },
          Data = new object[]
          {
            5, "Hello"
          }
        },
        Dict = new Dictionary<string,string>(),
        Date = new DateTime(2012, 5, 30, 19, 20 ,21)
      };
      ((Hashtable)o.SubC.SubC.Data[0])["w"] = 99;
      o.Dict["123"] = "abc";

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|X=15|Y=Abc|IntArray[0]=1|IntArray[1]=2|SubC.SubC.SubC=|SubC.SubC.Data[0][w]=99|SubC.SubC.SubComplex=|SubC.Data[0]=5|SubC.Data[1]=Hello|SubC.SubComplex=|Dict[123]=abc|Date=2012-05-30T19:20:21", result);
    }


    [Test]
    public void WhenSerializingSingleDictionaryItAddsNoSeparators()
    {
      // Arrange
      var o = new Dictionary<string,string>();
      o["A"] = "0";
      o["B"] = "X";

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|A=0|B=X", result);
    }


    [Test]
    public void WhenSerializingItUsesDefaultLocale()
    {
      // Arrange
      ClassWithLocaleDependentValues o = new ClassWithLocaleDependentValues
      {
        Dec = 10.5M,
        Flo = 20.12F,
        Dou = 13.23
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|Dec=10.5|Flo=20.12|Dou=13.23", result);
    }


    [Test]
    public void WhenSerializingItUsesSuppliedLocale()
    {
      // Arrange
      ClassWithLocaleDependentValues o = new ClassWithLocaleDependentValues
      {
        Dec = 10.5M,
        Flo = 20.12F,
        Dou = 13.23
      };

      // Act
      ObjectSerializerSettings settings = new ObjectSerializerSettings
      {
        Culture = CultureInfo.GetCultureInfo("da-DK")
      };

      string result = Serialize(o, settings);

      // Assert
      Assert.AreEqual("|Dec=10,5|Flo=20,12|Dou=13,23", result);
    }


    class ClassWithLocaleDependentValues
    {
      public decimal Dec { get; set; }
      public float Flo { get; set; }
      public double Dou { get; set; }
    }
  }
}
