using System.IO;
using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests.UtilitiesTests
{
  [TestFixture]
  public class FormUrlEncodingSerializerTests : TestHelper
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
        new FormUrlEncodingSerializer(typeof(SimpleData)).Serialize(w, data);

        string result = w.ToString();
        Assert.AreEqual("MyInt=10&MyString=Abc", result);
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
