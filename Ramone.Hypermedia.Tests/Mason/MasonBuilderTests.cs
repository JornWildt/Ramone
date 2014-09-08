using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Ramone.Hypermedia.Mason;
using System;
using System.Collections.Generic;


namespace Ramone.Hypermedia.Tests.Mason
{
  [TestFixture]
  public class MasonBuilderTests : TestHelper
  {
    class X : Resource
    {
      public long X1 { get; set; }
      public long X2 { get; set; }
      public List<Y> YList { get; set; }
      public List<Y> YArray { get; set; }
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
    ""X2"": 70,
    ""YList"":
    [
      { ""Text"": ""Abc"" }
    ],
    ""YArray"":
    [
      { ""Text"": ""Xyz"" },
      { ""Text"": ""Qwe"" }
    ]
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
      Assert.IsNotNull(result.MyX.YList);
      Assert.AreEqual(1, result.MyX.YList.Count);
      Assert.AreEqual("Abc", result.MyX.YList[0].Text);
      Assert.AreEqual(2, result.MyX.YArray.Count);
      Assert.AreEqual("Xyz", result.MyX.YArray[0].Text);
      Assert.AreEqual("Qwe", result.MyX.YArray[1].Text);
      Assert.IsTrue(((dynamic)result.MyX).a);
    }


    [Test]
    public void CanBuildComplexDynamicDataStructures()
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
    ""X2"": 70,
    ""YList"":
    [
      { ""Text"": ""Abc"" }
    ]
  },
  ""b"":
  {
    ""b1"": ""abc""
  }
}";

      JObject json = JObject.Parse(js);

      // Act
      dynamic result = new Builder().Build(json, null);

      // Assert
      Assert.IsInstanceOf<MasonResource>(result);
      Assert.AreEqual(10L, result.a);
      Assert.AreEqual("Blah", result.Text);
      Assert.IsInstanceOf<MasonResource>(result.MyX);
      Assert.AreEqual(20, result.MyX.X1);
      Assert.AreEqual(70, result.MyX.X2);
      Assert.IsNotNull(result.MyX.YList);
      Assert.AreEqual(1, result.MyX.YList.Count);
      Assert.IsInstanceOf<MasonResource>(result.MyX.YList[0]);
      Assert.AreEqual("Abc", result.MyX.YList[0].Text);
      Assert.IsTrue(result.MyX.a);
    }
  }
}
