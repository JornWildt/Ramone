using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Ramone.Utility;
using Ramone.Utility.ObjectSerialization;
using System.Text;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class FormUrlEncodingDeserializerTests : TestHelper
  {
    [Test]
    public void CanDeserializeSimpleTypes()
    {
      // Arrange
      string s = "MyInt=10&MyString=Abc&MyDate=2012-10-30T12:13:14&MyFloat=10.5&MyDouble=9.25&MyDecimal=8.75";

      // Act
      SimpleData data = Deserialize<SimpleData>(s);

      // Assert
      Assert.IsNotNull(data);
      Assert.AreEqual(10, data.MyInt);
      Assert.AreEqual("Abc", data.MyString);
      Assert.AreEqual(new DateTime(2012,10,30,12,13,14), data.MyDate);
      Assert.AreEqual(10.5f, data.MyFloat);
      Assert.AreEqual(9.25d, data.MyDouble);
      Assert.AreEqual(8.75m, data.MyDecimal);
    }


    [Test]
    public void CanDeserializeEmptyValues()
    {
      // Arrange
      string s = "MyInt=&MyString=";

      // Act
      SimpleData data = Deserialize<SimpleData>(s);

      // Assert
      Assert.IsNotNull(data);
      Assert.AreEqual(0, data.MyInt);
      Assert.AreEqual("", data.MyString);
    }


    [Test]
    public void CanDeserializeEmptyInput()
    {
      // Arrange
      string s = "";

      // Act
      SimpleData data = Deserialize<SimpleData>(s);

      // Assert
      Assert.IsNotNull(data);
      Assert.AreEqual(0, data.MyInt);
      Assert.AreEqual(null, data.MyString);
    }


    [Test]
    public void CanDeserializeMissingAssignments()
    {
      // Arrange
      string s = "MyInt&MyString";

      // Act
      SimpleData data = Deserialize<SimpleData>(s);

      // Assert
      Assert.IsNotNull(data);
      Assert.AreEqual(0, data.MyInt);
      Assert.AreEqual(null, data.MyString);
    }


    [Test]
    public void WhenDeserializingItIgnoresExtraValues()
    {
      // Arrange
      string s = "Z=1&MyInt=2&MyString=Xyz&Y=2";

      // Act
      SimpleData data = Deserialize<SimpleData>(s);

      // Assert
      Assert.IsNotNull(data);
      Assert.AreEqual(2, data.MyInt);
      Assert.AreEqual("Xyz", data.MyString);
    }


    [Test]
    public void CanDeserializeNestedTypes()
    {
      // Arrange
      string s = "MyInt=555&Simple.MyInt=10&Simple.MyString=Abc";

      // Act
      NestedData data = Deserialize<NestedData>(s);

      // Assert
      Assert.IsNotNull(data);
      Assert.AreEqual(555, data.MyInt);
      Assert.IsNotNull(data.Simple);
      Assert.AreEqual("Abc", data.Simple.MyString);
      Assert.AreEqual(10, data.Simple.MyInt);
    }


    [Test]
    public void CanDeserializeDictionaryStringString()
    {
      // Arrange
      string s = "A=123&B=Qwerty";

      // Act
      Dictionary<string, string> data = Deserialize<Dictionary<string, string>>(s);

      // Assert
      Assert.AreEqual("123", data["A"]);
      Assert.AreEqual("Qwerty", data["B"]);
    }


    [Test]
    public void CanDeserializeInternationalCharacters(
      [Values("UTF-8|MyInt=10&MyString=Abc+%c3%86%c3%98%c3%85%5e%c3%bc",
              "Windows-1252|MyInt=10&MyString=Abc+%c6%d8%c5%5e%fc",
              "iso-8859-1|MyInt=10&MyString=Abc+%c6%d8%c5%5e%fc")] string charsetData)
    {
      // Arrange
      string[] elements = charsetData.Split('|');
      string charset = elements[0];
      string input = elements[1];

      // Act
      Dictionary<string, string> data = Deserialize<Dictionary<string, string>>(input, charset);

      // Assert
      Assert.AreEqual("Abc ÆØÅ^ü", data["MyString"]);
    }


    protected T Deserialize<T>(string s, string charset = null)
      where T : class
    {
      FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(typeof(T));

      using (TextReader reader = new StringReader(s))
      {
        Encoding enc = (charset != null ? Encoding.GetEncoding(charset) : null);
        T data = (T)serializer.Deserialize(reader, new ObjectSerializerSettings { Charset = enc });
        return data;
      }
    }


    public class SimpleData
    {
      public int MyInt { get; set; }
      public string MyString { get; set; }
      public DateTime MyDate { get; set; }
      public float MyFloat { get; set; }
      public float MyDouble { get; set; }
      public float MyDecimal { get; set; }
    }


    public class NestedData
    {
      public int MyInt { get; set; }
      public SimpleData Simple { get; set; }
    }


    public class DictionaryData
    {
      public int MyInt { get; set; }
      public Dictionary<string, object> MyDict { get; set; }
      //public DateTime MyDate { get; set; }
    }
  }
}
