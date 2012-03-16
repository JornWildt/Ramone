using NUnit.Framework;
using Ramone.Utility;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class HeaderEncodingParserTests : TestHelper
  {
    [Test]
    public void CanReadIso8859()
    {
      // Act
      string s = HeaderEncodingParser.ParseExtendedHeader("iso-8859-1'en'%A3%20rates");

      // Assert
      Assert.AreEqual("£ rates", s);
    }


    [Test]
    public void CanReadUtf8()
    {
      // Act
      string s = HeaderEncodingParser.ParseExtendedHeader("UTF-8''%c2%a3%20and%20%e2%82%ac%20rates");

      // Assert
      Assert.AreEqual("£ and € rates", s);
    }


    [Test]
    public void ItIgnoresUnknownEncodings()
    {
      // Act
      string s1 = HeaderEncodingParser.ParseExtendedHeader("''abc");
      string s2 = HeaderEncodingParser.ParseExtendedHeader("xx''def");

      // Assert
      Assert.AreEqual("abc", s1);
      Assert.AreEqual("def", s2);
    }


    [Test]
    public void ItIgnoresWrongNumberOfSingleQuotes()
    {
      // Act
      string s1 = HeaderEncodingParser.ParseExtendedHeader("utf-8'abc");
      string s2 = HeaderEncodingParser.ParseExtendedHeader("def");
      string s3 = HeaderEncodingParser.ParseExtendedHeader("utf-8''xyz'123");

      // Assert
      Assert.AreEqual("abc", s1);
      Assert.AreEqual("def", s2);
      Assert.AreEqual("xyz'123", s3);
    }
  }
}
