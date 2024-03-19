using NUnit.Framework;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class DefaultContentTypeTests : TestHelper
  {
    [Test]
    public void CanConfigurDefaultContentTypesPart1()
    {
      // Arrange
      var cat = new { Name = "Prince" };
      Request request = Session.Bind(CatsTemplate);

      // Act
      AssertThrows<ArgumentException>(() => request.Post(cat));
      Session.DefaultRequestMediaType = MediaType.ApplicationJson;
      Session.DefaultResponseMediaType = MediaType.ApplicationJson;
      using (dynamic response = request.Post(cat))
      {
        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Body.Name, Is.EqualTo("Prince"));
      }
    }


    [Test]
    public void CanConfigurDefaultContentTypesPart2()
    {
      // Arrange
      var cat = new { Name = "Prince" };
      Request request = Session.Bind(CatsTemplate);

      // Act
      AssertThrows<ArgumentException>(() => request.Post(cat));
      Session.DefaultRequestMediaType = MediaType.MultipartFormData;
      Session.DefaultResponseMediaType = MediaType.TextPlain;
      using (var response = request.Post<string>(cat))
      {
        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Body, Is.EqualTo("Prince"));
      }
    }
  }
}
