using System.IO;
using NUnit.Framework;
using Ramone.Utility;
using Ramone.IO;
using System.Text;
using System;


namespace Ramone.Tests.Utilities
{
  [TestFixture]
  public class MultipartFormDataSerializerTests : TestHelper
  {
    [Test]
    public void CanSerializeSimpleClass()
    {
      using (MemoryStream s = new MemoryStream())
      {
        SimpleData data = new SimpleData
        {
          MyInt = 10,
          MyString = "Abc"
        };
        new MultipartFormDataSerializer(typeof(SimpleData)).Serialize(s, data, Encoding.UTF8, "xyzq");

        string expected = @"
--xyzq
Content-Disposition: form-data; name=""MyInt""

10
--xyzq
Content-Disposition: form-data; name=""MyString""

Abc";

        s.Seek(0, SeekOrigin.Begin);
        using (StreamReader r = new StreamReader(s))
        {
          string result = r.ReadToEnd();
          Assert.AreEqual(expected, result);
        }
      }
    }


    [Test]
    public void CanSerializeFile()
    {
      using (MemoryStream s = new MemoryStream())
      {
        FileData data = new FileData
        {
          MyString = "Abc ÆØÅ",
          MyFile = new Ramone.IO.File("..\\..\\data1.txt")
        };
        new MultipartFormDataSerializer(typeof(FileData)).Serialize(s, data, Encoding.UTF8, "xyzq");

        string expected = @"
--xyzq
Content-Disposition: form-data; name=""MyString""

Abc ÆØÅ
--xyzq
Content-Disposition: form-data; name=""MyFile""; filename=""data1.txt""

XxxÆØÅ";

        s.Seek(0, SeekOrigin.Begin);
        using (StreamReader r = new StreamReader(s))
        {
          string result = r.ReadToEnd();
          Assert.AreEqual(expected, result);
        }
      }
    }


    [Test]
    public void CanSerializeBinaryFileWithContentType()
    {
      using (MemoryStream s = new MemoryStream())
      {
        FileData data = new FileData
        {
          MyString = "Abc ÆØÅ",
          MyFile = new Ramone.IO.File("..\\..\\data1.gif", "image/gif")
        };
        new MultipartFormDataSerializer(typeof(FileData)).Serialize(s, data, Encoding.UTF8, "xyzq");

        string expected = @"
--xyzq
Content-Disposition: form-data; name=""MyString""

Abc ÆØÅ
--xyzq
Content-Disposition: form-data; name=""MyFile""; filename=""data1.gif""
Content-Type: image/gif

";

        s.Seek(0, SeekOrigin.Begin);
        using (StreamReader r = new StreamReader(s))
        {
          string result = r.ReadToEnd();
          if (!result.StartsWith(expected))
            Console.Write(string.Format("Expected: \n{0}\n\nGot:\n{1}", expected, result));
          Assert.IsTrue(result.StartsWith(expected), "Serialized result must begin with expected value (see console output)");
        }
      }
    }


    public class SimpleData
    {
      public int MyInt { get; set; }
      public string MyString { get; set; }
      //public DateTime MyDate { get; set; }
    }


    public class FileData
    {
      public string MyString { get; set; }
      public IFile MyFile { get; set; }
    }
  }
}
