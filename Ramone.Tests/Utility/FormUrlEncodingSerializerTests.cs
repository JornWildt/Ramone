using System.IO;
using NUnit.Framework;
using Ramone.Utility;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class FormUrlEncodingSerializerTests : TestHelper
  {
    [Test]
    public void CanSerializeSimpleClassWithEncoding(
      [Values("UTF-8|MyInt=10&MyString=Abc+%c3%86%c3%98%c3%85%5e%c3%bc",
              "Windows-1252|MyInt=10&MyString=Abc+%c6%d8%c5%5e%fc", 
              "iso-8859-1|MyInt=10&MyString=Abc+%c6%d8%c5%5e%fc")] string charsetData)
    {
      string[] elements = charsetData.Split('|');
      string charset = elements[0];
      string expected = elements[1];

      using (MemoryStream s = new MemoryStream())
      using (StreamWriter w = new StreamWriter(s, Encoding.GetEncoding(charset)))
      {
        SimpleData data = new SimpleData
        {
          MyInt = 10,
          MyString = "Abc ÆØÅ^ü"
        };
        new FormUrlEncodingSerializer(typeof(SimpleData)).Serialize(w, data);

        w.Flush();
        s.Seek(0, SeekOrigin.Begin);

        // Must always be written in plain old ASCII
        using (StreamReader r = new StreamReader(s, Encoding.ASCII))
        {
          string result = r.ReadToEnd();
          Assert.AreEqual(expected, result);
        }
      }
    }


    [Test]
    public void WhenSerializingDictionariesItIncludesContent()
    {
      // Arrange
      DictionaryData data = new DictionaryData
      {
        MyInt = 20,
        MyDict = new Dictionary<string, object>(),
        MyNameValues = new NameValueCollection(),
        MyHashtable = new Hashtable()
      };
      data.MyDict["A"] = "1234uio";
      data.MyNameValues["X"] = "xxx";
      data.MyHashtable["1"] = "aaa";

      // Act
      CheckSerialization("MyInt=20&MyDict%5bA%5d=1234uio&MyNameValues%5bX%5d=xxx&MyHashtable%5b1%5d=aaa", data);
    }


    [Test]
    public void WhenSerializingDictionaryOnlyItIncludesContentWithNoPrefix()
    {
      // Arrange
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary["A"] = "123";
      dictionary["B"] = "xyz";

      // Act
      CheckSerialization("A=123&B=xyz", dictionary);
    }


    [Test]
    public void WhenSerializingNameValueCollectionOnlyItIncludesContentWithNoPrefix()
    {
      // Arrange
      // Arrange
      NameValueCollection values = new NameValueCollection();
      values["A"] = "123";
      values["B"] = "xyz";

      // Act
      CheckSerialization("A=123&B=xyz", values);
    }


    [Test]
    public void WhenSerializingHashtableOnlyItIncludesContentWithNoPrefix()
    {
      // Arrange
      Hashtable hashtable = new Hashtable();
      hashtable["A"] = "123";
      hashtable["B"] = "xyz";

      // Act
      CheckSerialization("A=123&B=xyz", hashtable);
    }


    protected void CheckSerialization(string expected, object data)
    {
      using (MemoryStream s = new MemoryStream())
      using (StreamWriter w = new StreamWriter(s))
      {
        new FormUrlEncodingSerializer(data.GetType()).Serialize(w, data);

        w.Flush();
        s.Seek(0, SeekOrigin.Begin);

        // Must always be written in plain old ASCII
        using (StreamReader r = new StreamReader(s, Encoding.ASCII))
        {
          string result = r.ReadToEnd();
          Assert.AreEqual(expected, result);
        }
      }
    }

    
    public class SimpleData
    {
      public int MyInt { get; set; }
      public string MyString { get; set; }
      //public DateTime MyDate { get; set; }
    }


    public class DictionaryData
    {
      public int MyInt { get; set; }
      public Dictionary<string, object> MyDict { get; set; }
      public NameValueCollection MyNameValues { get; set; }
      public Hashtable MyHashtable { get; set; }
    }
  }
}
