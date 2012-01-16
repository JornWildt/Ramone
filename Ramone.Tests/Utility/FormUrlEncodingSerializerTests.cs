using System.IO;
using NUnit.Framework;
using Ramone.Utility;
using System.Text;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class FormUrlEncodingSerializerTests : TestHelper
  {
    [Test]
    public void CanSerializeSimpleClass(
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


    public class SimpleData
    {
      public int MyInt { get; set; }
      public string MyString { get; set; }
      //public DateTime MyDate { get; set; }
    }
  }
}
