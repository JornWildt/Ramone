using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Ramone.Hypermedia.Mason;
using System;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonBuilderTests : TestHelper
  {
    class X : Resource
    {
      public long X1 { get; set; }
      public long X2 { get; set; }
    }

    class Y : Resource
    {
      public X MyX { get; set; }
      public string Text { get; set; }
    }


    [Test]
    public void CanBuildComplexDataStructures()
    {
      // Arrange
      string js = @"
{
  ""a"": 10,
  ""Text"": ""Blah"",
  ""MyX"":
  {
    ""X1"": 20,
    ""a"": true,
    ""X2"": 70
  },
  ""b"":
  {
    ""b1"": ""abc""
  }
}";

      JObject json = JObject.Parse(js);

      // Act
      Y result = (Y)new Builder().Build(json, typeof(Y));

      // Assert
      Assert.AreEqual(10L, ((dynamic)result).a);
      Assert.AreEqual("Blah", result.Text);
      Assert.IsInstanceOf<X>(result.MyX);
      Assert.AreEqual(20, result.MyX.X1);
      Assert.AreEqual(70, result.MyX.X2);
      Assert.IsTrue(((dynamic)result.MyX).a);
    }
  }
}
