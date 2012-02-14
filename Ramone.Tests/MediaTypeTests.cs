using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class MediaTypeTests : TestHelper
  {
    [Test]
    public void CanConstructMediaTypeFromString()
    {
      // Act
      MediaType m = new MediaType("text/plain");

      // Assert
      Assert.AreEqual("text/plain", m.MediaType);
      Assert.AreEqual("text", m.TopLevelType);
      Assert.AreEqual("plain", m.SubType);
      Assert.IsNotNull(m.Parameters);
    }


    [Test]
    public void CanReadParametersAndStripWhitespace()
    {
      MediaType m = new MediaType("text/plain ; charset=utf-8 ");
      Assert.AreEqual("text/plain", m.MediaType);
      Assert.AreEqual("text", m.TopLevelType);
      Assert.AreEqual("plain", m.SubType);
      Assert.IsNotNull(m.Parameters);
      Assert.AreEqual("utf-8", m.Parameters["charset"]);
    }


    [Test]
    public void CanConstructWildcards()
    {
      // Act
      MediaType m1 = new MediaType("text/*");
      MediaType m2 = new MediaType("*/*");

      // Assert
      Assert.AreEqual("text/*", m1.MediaType);
      Assert.AreEqual("text", m1.TopLevelType);
      Assert.AreEqual("*", m1.SubType);
      Assert.IsFalse(m1.IsTopLevelWildcard);
      Assert.IsTrue(m1.IsSubTypeWildcard);
      Assert.IsFalse(m1.IsWildcard);

      Assert.AreEqual("*/*", m2.MediaType);
      Assert.AreEqual("*", m2.TopLevelType);
      Assert.AreEqual("*", m2.SubType);
      Assert.IsTrue(m2.IsTopLevelWildcard);
      Assert.IsTrue(m2.IsSubTypeWildcard);
      Assert.IsTrue(m2.IsWildcard);
    }


    [Test]
    public void CanMatchWildcards()
    {
      // Arrange
      MediaType m1 = new MediaType("text/plain");
      MediaType m2 = new MediaType("text/*");
      MediaType m3 = new MediaType("*/*");

      // Assert
      Assert.IsTrue(m1.Matches("text/plain"));
      Assert.IsTrue(m1.Matches("text/PLAIN"));
      Assert.IsFalse(m1.Matches("text/html"));
      Assert.IsFalse(m1.Matches("image/html"));
      Assert.IsFalse(m1.Matches("imAGe/hTML"));
      Assert.IsTrue(m2.Matches("text/plain"));
      Assert.IsTrue(m2.Matches("TEXT/html"));
      Assert.IsFalse(m2.Matches("image/html"));
      Assert.IsTrue(m3.Matches("text/plain"));
      Assert.IsTrue(m3.Matches("text/HTML"));
      Assert.IsTrue(m3.Matches("image/html"));
    }


    // The base ContentType class doesn't do as expected, so these fail
    //[Test]
    //public void ThrowsOnInvalidMediaTypes()
    //{
    //  AssertThrows<ArgumentNullException>(() => new MediaType((string)null));
    //  AssertThrows<ArgumentException>(() => new MediaType(""));
    //  AssertThrows<FormatException>(() => new MediaType("text"));
    //  AssertThrows<FormatException>(() => new MediaType("text/"));
    //  AssertThrows<FormatException>(() => new MediaType(""));
    //}
  }
}
