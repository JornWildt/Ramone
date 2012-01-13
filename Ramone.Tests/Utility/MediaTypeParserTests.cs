using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class MediaTypeParserTests : TestHelper
  {
    [Test]
    public void CanParseSimpleMediaType()
    {
      MediaType m = MediaTypeParser.ParseMediaType("text/plain");
      Assert.IsNotNull(m);
      Assert.AreEqual("text/plain", m.FullType);
      Assert.AreEqual("text", m.Type);
      Assert.AreEqual("plain", m.SubType);
      Assert.IsNotNull(m.Parameters);
    }


    [Test]
    public void CanHandleMissingSubType()
    {
      MediaType m = MediaTypeParser.ParseMediaType("text");
      Assert.IsNotNull(m);
      Assert.AreEqual("text", m.FullType);
      Assert.AreEqual("text", m.Type);
      Assert.AreEqual("", m.SubType);
    }

    
    [Test]
    public void CanHandleEmptyType()
    {
      MediaType m = MediaTypeParser.ParseMediaType("");
      Assert.IsNotNull(m);
      Assert.AreEqual("", m.FullType);
      Assert.AreEqual("", m.Type);
      Assert.AreEqual("", m.SubType);
    }


    [Test]
    public void CanHandleNullType()
    {
      MediaType m = MediaTypeParser.ParseMediaType(null);
      Assert.IsNull(m);
    }


    [Test]
    public void CanReadParametersAndStripWhitespace()
    {
      MediaType m = MediaTypeParser.ParseMediaType("text/plain ; charset=utf-8 ");
      Assert.IsNotNull(m);
      Assert.AreEqual("text/plain", m.FullType);
      Assert.AreEqual("text", m.Type);
      Assert.AreEqual("plain", m.SubType);
      Assert.IsNotNull(m.Parameters);
      Assert.AreEqual("utf-8", m.Parameters["charset"]);
    }
  }
}
