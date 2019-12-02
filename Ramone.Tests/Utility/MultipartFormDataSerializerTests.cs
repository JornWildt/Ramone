using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Ramone.IO;
using Ramone.Utility;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests.Utility
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
Content-Type: text/plain; charset=utf-8

10
--xyzq
Content-Disposition: form-data; name=""MyString""
Content-Type: text/plain; charset=utf-8

Abc
--xyzq--";

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
Content-Type: text/plain; charset=utf-8

Abc ÆØÅ
--xyzq
Content-Disposition: form-data; name=""MyFile""; filename=""data1.txt""

Æüî´`'
--xyzq--";

        s.Seek(0, SeekOrigin.Begin);
        using (StreamReader r = new StreamReader(s))
        {
          string result = r.ReadToEnd();
          Assert.AreEqual(expected, result);
        }
      }
    }


    [Test]
    public void CanSerializeFileWithInternationalCharactersInFilenameWhenSettingsAllowIt()
    {
      ObjectSerializerSettings settings = new ObjectSerializerSettings();
      settings.EnableNonAsciiCharactersInMultipartFilenames = true;

      using (MemoryStream s = new MemoryStream())
      {
        FileData data = new FileData
        {
          MyFile = new FileWithSpecialName("..\\..\\data1.txt", "Bøllefrø.txt")
        };
        new MultipartFormDataSerializer(typeof(FileData)).Serialize(s, data, Encoding.UTF8, "xyzq", settings);

        string expected = @"
--xyzq
Content-Disposition: form-data; name=""MyFile""; filename=""Bxllefrx.txt""; filename*=UTF-8''B%c3%b8llefr%c3%b8.txt

Æüî´`'
--xyzq--";

        s.Seek(0, SeekOrigin.Begin);
        using (StreamReader r = new StreamReader(s))
        {
          string result = r.ReadToEnd();
          Assert.AreEqual(expected, result);
        }
      }
    }


    [Test]
    public void CannotSerializeFileWithInternationalCharactersInFilenameWhenSettingsDisallowIt()
    {
      ObjectSerializerSettings settings = new ObjectSerializerSettings();
      settings.EnableNonAsciiCharactersInMultipartFilenames = false;

      using (MemoryStream s = new MemoryStream())
      {
        FileData data = new FileData
        {
          MyFile = new FileWithSpecialName("..\\..\\data1.txt", "Bøllefrø.txt")
        };
        new MultipartFormDataSerializer(typeof(FileData)).Serialize(s, data, Encoding.UTF8, "xyzq", settings);

        string expected = @"
--xyzq
Content-Disposition: form-data; name=""MyFile""; filename=""Bxllefrx.txt""

Æüî´`'
--xyzq--";

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
Content-Type: text/plain; charset=utf-8

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
