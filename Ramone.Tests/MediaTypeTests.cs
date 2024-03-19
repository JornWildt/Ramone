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
      Assert.That((string)m, Is.EqualTo("text/plain"));
      Assert.That(m.TopLevelType, Is.EqualTo("text"));
      Assert.That(m.SubType, Is.EqualTo("plain"));
    }


    [Test]
    public void ItOnlyReadsMediaTypeAndThrowsAwayOtherParameters()
    {
      // Act
      MediaType m = new MediaType("text/plain; charset=utf-8");

      // Assert
      Assert.That((string)m, Is.EqualTo("text/plain"));
      Assert.That(m.TopLevelType, Is.EqualTo("text"));
      Assert.That(m.SubType, Is.EqualTo("plain"));
    }


    [Test]
    public void CanConstructWildcards()
    {
      // Act
      MediaType m1 = new MediaType("text/*");
      MediaType m2 = new MediaType("*/*");

      // Assert
      Assert.That((string)m1, Is.EqualTo("text/*"));
      Assert.That(m1.TopLevelType, Is.EqualTo("text"));
      Assert.That(m1.SubType, Is.EqualTo("*"));
      Assert.That(m1.IsTopLevelWildcard, Is.False);
      Assert.That(m1.IsSubTypeWildcard, Is.True);
      Assert.That(m1.IsWildcard, Is.False);

      Assert.That((string)m2, Is.EqualTo("*/*"));
      Assert.That(m2.TopLevelType, Is.EqualTo("*"));
      Assert.That(m2.SubType, Is.EqualTo("*"));
      Assert.That(m2.IsTopLevelWildcard, Is.True);
      Assert.That(m2.IsSubTypeWildcard, Is.True);
      Assert.That(m2.IsWildcard, Is.True);
    }


    [Test]
    public void CanMatchWildcards()
    {
      // Arrange
      MediaType m1 = new MediaType("text/plain");
      MediaType m2 = new MediaType("text/*");
      MediaType m3 = new MediaType("*/*");

      // Assert
      Assert.That(m1.Matches("text/plain"), Is.True);
      Assert.That(m1.Matches("text/PLAIN"), Is.True);
      Assert.That(m1.Matches("text/html"), Is.False);
      Assert.That(m1.Matches("image/html"), Is.False);
      Assert.That(m1.Matches("imAGe/hTML"), Is.False);
      Assert.That(m2.Matches("text/plain"), Is.True);
      Assert.That(m2.Matches("TEXT/html"), Is.True);
      Assert.That(m2.Matches("image/html"), Is.False);
      Assert.That(m3.Matches("text/plain"), Is.True);
      Assert.That(m3.Matches("text/HTML"), Is.True);
      Assert.That(m3.Matches("image/html"), Is.True);
    }


    [Test]
    public void ThrowsOnInvalidMediaTypes()
    {
      AssertThrows<ArgumentNullException>(() => new MediaType((string)null));
      AssertThrows<FormatException>(() => new MediaType(""));
      AssertThrows<FormatException>(() => new MediaType("text"));
      AssertThrows<FormatException>(() => new MediaType("text/"));
      AssertThrows<FormatException>(() => new MediaType("text/"));
      AssertThrows<FormatException>(() => new MediaType("text/xxx/qqq"));
      AssertThrows<FormatException>(() => new MediaType("  ; charset=utf-8"));
    }


    [Test]
    public void ItComparesCaseInsensitive()
    {
      // Arrange
      MediaType m1 = new MediaType("x/y");
      MediaType m2 = new MediaType("X/Y");

      // Assert
      Assert.That(m1 == m2, Is.True);
    }


    [Test]
    public void CanAssignMediaTypeFromString()
    {
      // Act
      MediaType m = "app/x";

      // Assert
      Assert.That((string)m, Is.EqualTo("app/x"));
    }


    [Test]
    public void CanExplicitlyCastMediaTypeToString()
    {
      // Ararnge
      MediaType mt = "app/x";

      // Act
      string m = (string)mt;

      // Assert
      Assert.That(m, Is.EqualTo("app/x"));
    }


    [Test]
    public void CanExplicitlyCastNullMediaTypeToString()
    {
      // Ararnge
      MediaType mt = null;

      // Act
      string m = (string)mt;

      // Assert
      Assert.That(m, Is.Null);
    }


    [Test]
    public void WhenCreatingMediaTypeFromNullValueItReturnsValue()
    {
      // Act
      MediaType m = MediaType.Create(null);

      // Assert
      Assert.That(m, Is.Null);
    }


    [Test]
    public void CanCompareEmptyStringWithMediaType()
    {
      string ct = "";
      Assert.That(ct == MediaType.ApplicationXml, Is.False);
      Assert.That(MediaType.ApplicationXml == ct, Is.False);
      Assert.That(ct != MediaType.ApplicationXml, Is.True);
      Assert.That(MediaType.ApplicationXml != ct, Is.True);
    }
  }
}
