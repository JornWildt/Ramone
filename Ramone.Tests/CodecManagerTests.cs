using System;
using NUnit.Framework;
using Ramone.Implementation;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class CodecManagerTests : TestHelper
  {
    [Test]
    public void WhenAddingSameCodecAndMediaTypeTwiceItThrows()
    {
      // Arrange
      ICodecManager cm = new CodecManager();

      // Act
      cm.AddCodec<Cat>("x", new CatAsHtmlCodec());
      cm.AddCodec<Cat>("y", new CatAsHtmlCodec());

      // Assert
      AssertThrows<ArgumentException>(() => cm.AddCodec<Cat>("x", new CatAsHtmlCodec()));
    }
  }
}
