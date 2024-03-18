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
      Assert.That(s, Is.EqualTo("£ rates"));
    }


    [Test]
    public void CanReadUtf8()
    {
      // Act
      string s = HeaderEncodingParser.ParseExtendedHeader("UTF-8''%c2%a3%20and%20%e2%82%ac%20rates");

      // Assert
      Assert.That(s, Is.EqualTo("£ and € rates"));
    }


    [Test]
    public void ItIgnoresUnknownEncodings()
    {
      // Act
      string s1 = HeaderEncodingParser.ParseExtendedHeader("''abc");
      string s2 = HeaderEncodingParser.ParseExtendedHeader("xx''def");

      // Assert
      Assert.That(s1, Is.EqualTo("abc"));
      Assert.That(s2, Is.EqualTo("def"));
    }


    [Test]
    public void ItIgnoresWrongNumberOfSingleQuotes()
    {
      // Act
      string s1 = HeaderEncodingParser.ParseExtendedHeader("utf-8'abc");
      string s2 = HeaderEncodingParser.ParseExtendedHeader("def");
      string s3 = HeaderEncodingParser.ParseExtendedHeader("utf-8''xyz'123");

      // Assert
      Assert.That(s1, Is.EqualTo("abc"));
      Assert.That(s2, Is.EqualTo("def"));
      Assert.That(s3, Is.EqualTo("xyz'123"));
    }
  }
}
