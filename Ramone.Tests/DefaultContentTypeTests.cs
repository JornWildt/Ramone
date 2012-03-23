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
      dynamic response = request.Post(cat);

      // Assert
      Assert.IsNotNull(response);
      Assert.AreEqual("Prince", response.Body.Name);
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
      var response = request.Post<string>(cat);

      // Assert
      Assert.IsNotNull(response);
      Assert.AreEqual("Prince", response.Body);
    }
  }
}
