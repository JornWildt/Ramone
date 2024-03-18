using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Ramone.Utility;
using Ramone.Utility.ObjectSerialization;
using System.Text;
using System.Collections;
using System.Collections.Specialized;


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
      Assert.That(data.MyInt, Is.EqualTo(10));
      Assert.That(data.MyString, Is.EqualTo("Abc"));
      Assert.That(data.MyDate, Is.EqualTo(new DateTime(2012,10,30,12,13,14)));
      Assert.That(data.MyFloat, Is.EqualTo(10.5f));
      Assert.That(data.MyDouble, Is.EqualTo(9.25d));
      Assert.That(data.MyDecimal, Is.EqualTo(8.75m));
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
      Assert.That(data.MyInt, Is.EqualTo(0));
      Assert.That(data.MyString, Is.EqualTo(""));
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
      Assert.That(data.MyInt, Is.EqualTo(0));
      Assert.That(data.MyString, Is.EqualTo(null));
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
      Assert.That(data.MyInt, Is.EqualTo(0));
      Assert.That(data.MyString, Is.EqualTo(null));
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
      Assert.That(data.MyInt, Is.EqualTo(2));
      Assert.That(data.MyString, Is.EqualTo("Xyz"));
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
      Assert.That(data.MyInt, Is.EqualTo(555));
      Assert.IsNotNull(data.Simple);
      Assert.That(data.Simple.MyString, Is.EqualTo("Abc"));
      Assert.That(data.Simple.MyInt, Is.EqualTo(10));
    }


    [Test]
    public void CanDeserializeDictionaryStringString()
    {
      // Arrange
      string s = "A=123&B=Qwerty";

      // Act
      Dictionary<string, string> data = Deserialize<Dictionary<string, string>>(s);

      // Assert
      Assert.That(data["A"], Is.EqualTo("123"));
      Assert.That(data["B"], Is.EqualTo("Qwerty"));
    }

    // TEST strong type with inner collections of the various types

    [Test]
    public void CanDeserializeHashtable()
    {
      // Arrange
      string s = "A=123&B=Qwerty";

      // Act
      Hashtable data = Deserialize<Hashtable>(s);

      // Assert
      Assert.That(data["A"], Is.EqualTo("123"));
      Assert.That(data["B"], Is.EqualTo("Qwerty"));
    }


    [Test]
    public void CanDeserializeNestedHashtable()
    {
      // Arrange
      string s = "A.x=123&B.y=Qwerty";

      // Act
      Hashtable data = Deserialize<Hashtable>(s);

      // Assert
      Assert.IsInstanceOf<Hashtable>(data["A"]);
      Assert.That(((Hashtable)data["A"])["x"], Is.EqualTo("123"));
      Assert.That(((Hashtable)data["B"])["y"], Is.EqualTo("Qwerty"));
    }


    [Test]
    public void WhenDeserializingNameValueCollectionItDoesNotHandleNesting()
      // => Use this to keep name formating in response
    {
      // Arrange
      string s = "A.x=123&B.y=Qwerty";

      // Act
      NameValueCollection data = Deserialize<NameValueCollection>(s);

      // Assert
      Assert.That(data["A.x"], Is.EqualTo("123"));
      Assert.That(data["B.y"], Is.EqualTo("Qwerty"));
    }


    [Test]
    public void CanDeserializeNestedTypedDataWithCollections()
    {
      // Arrange
      string s = @"
MyDictionaryStringNested.A.MyNameValueCollection.Z=30&
MyDictionaryStringNested.X.MyInt=10&
MyDictionaryStringNested.Y.MyInt=20&
MyDictionaryStringString.X=1&
MyDictionaryStringString.Y=2&
MyDictionaryStringString.X.Q=3&
MyNameValueCollection.K1.K2=5&
MyNameValueCollection.K1.K3=6&
MyNameValueCollection.K1=7&
MyNameValueCollection.X=8&
MyHashtable.A.X.1=1&
MyHashtable.A.Y.2=Q";

      // Act
      NestedDataWithDictionaries data = Deserialize<NestedDataWithDictionaries>(s);

      // Assert
      Assert.IsInstanceOf<Hashtable>(data.MyHashtable["A"]);
      Hashtable a = (Hashtable)data.MyHashtable["A"];
      Assert.That(((Hashtable)a["X"])["1"], Is.EqualTo("1"));
      Assert.That(((Hashtable)a["Y"])["2"], Is.EqualTo("Q"));
      
      Assert.IsInstanceOf<NameValueCollection>(data.MyNameValueCollection);
      Assert.That(data.MyNameValueCollection["K1.K2"], Is.EqualTo("5"));
      Assert.That(data.MyNameValueCollection["K1.K3"], Is.EqualTo("6"));
      Assert.That(data.MyNameValueCollection["K1"], Is.EqualTo("7"));
      Assert.That(data.MyNameValueCollection["X"], Is.EqualTo("8"));

      Assert.IsNotNull(data.MyDictionaryStringString);
      Assert.That(data.MyDictionaryStringString["X"], Is.EqualTo("1"));
      Assert.That(data.MyDictionaryStringString["Y"], Is.EqualTo("2"));
      Assert.That(data.MyDictionaryStringString["X.Q"], Is.EqualTo("3"));

      Assert.IsNotNull(data.MyDictionaryStringNested);
      Assert.IsNotNull(data.MyDictionaryStringNested["X"]);
      Assert.That(data.MyDictionaryStringNested["X"].MyInt, Is.EqualTo(10));
      Assert.That(data.MyDictionaryStringNested["Y"].MyInt, Is.EqualTo(20));
      Assert.That(data.MyDictionaryStringNested["A"].MyNameValueCollection["Z"], Is.EqualTo("30"));
    }


    [Test]
    public void CanHandleEmptyExpression()
    {
      // Arrange
      string s = "MyHashtable.A=1&";

      // Act
      NestedDataWithDictionaries data = Deserialize<NestedDataWithDictionaries>(s);

      // Assert
      Assert.That(data.MyHashtable["A"], Is.EqualTo("1"));
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
      Assert.That(data["MyString"], Is.EqualTo("Abc ÆØÅ^ü"));
    }


    protected T Deserialize<T>(string s, string charset = null)
      where T : class
    {
      FormUrlEncodingSerializer serializer = new FormUrlEncodingSerializer(typeof(T));
      s = s.Replace("\r", "");
      s = s.Replace("\n", "");

      using (TextReader reader = new StringReader(s))
      {
        Encoding enc = (charset != null ? Encoding.GetEncoding(charset) : null);
        T data = (T)serializer.Deserialize(reader, new ObjectSerializerSettings { Encoding = enc });
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


    public class NestedDataWithDictionaries
    {
      public int MyInt { get; set; }
      public Hashtable MyHashtable { get; set; }
      public NameValueCollection MyNameValueCollection { get; set; }
      public Dictionary<string, string> MyDictionaryStringString { get; set; }
      public Dictionary<string, NestedDataWithDictionaries> MyDictionaryStringNested { get; set; }
    }


    public class DictionaryData
    {
      public int MyInt { get; set; }
      public Dictionary<string, object> MyDict { get; set; }
      //public DateTime MyDate { get; set; }
    }
  }
}
