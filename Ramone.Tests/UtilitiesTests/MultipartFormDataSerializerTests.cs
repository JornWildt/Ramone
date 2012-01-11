using System.IO;
using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests.UtilitiesTests
{
  [TestFixture]
  public class MultipartFormDataSerializerTests : TestHelper
  {
    [Test]
    public void CanSerializeSimpleClass()
    {
      using (StringWriter w = new StringWriter())
      {
        SimpleData data = new SimpleData
        {
          MyInt = 10,
          MyString = "Abc"
        };
        new MultipartFormDataSerializer(typeof(SimpleData)).Serialize(w, data, "xyzq");

        string expected = @"
--xyzq
Content-Disposition: form-data; name=""MyInt""

10
--xyzq
Content-Disposition: form-data; name=""MyString""

Abc";

        string result = w.ToString();
        Assert.AreEqual(expected, result);
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
