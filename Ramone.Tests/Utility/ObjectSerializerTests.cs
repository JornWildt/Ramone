using NUnit.Framework;
using Ramone.Utility;
using System.Collections.Generic;


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


    protected string Serialize(object data)
    {
      ObjectSerializer serializer = new ObjectSerializer(data.GetType());
      ObjectVisitor visitor = new ObjectVisitor();
      serializer.Serialize(data, visitor);
      return visitor.Result;
    }


    class ObjectVisitor : IPropertyVisitor
    {
      public string Result;


      #region IPropertyVisitor Members

      public void Begin()
      {
        Result = "";
      }

      public void SimpleValue(string name, object value)
      {
        Result += string.Format("|{0}={1}", name, value);
      }

      public void End()
      {
      }

      #endregion
    }

  }
}
