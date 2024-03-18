using System;
using NUnit.Framework;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class ServiceTests : TestHelper
  {
    [Test]
    public void CanSetAndGetServiceItems()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      service.Items["X"] = 1234;
      int x = (int)service.Items["X"];

      // Assert
      Assert.That(x, Is.EqualTo(1234));
    }


    [Test]
    public void WhenCreatingSessionItInheritsServiceItems()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);
      service.Items["X"] = 1234;

      // Act
      ISession session = service.NewSession();
      int x = (int)session.Items["X"];

      // Assert
      Assert.That(x, Is.EqualTo(1234));
    }


    [Test]
    public void WhenChangingItemsInSessionItDoesNotChangeItemsInService()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);
      service.Items["X"] = 1234;
      ISession session = service.NewSession();

      // Act
      session.Items["Y"] = "Hello";
      session.Items.Remove("X");

      // Assert
      Assert.That((int)service.Items["X"], Is.EqualTo(1234));
      Assert.IsFalse(service.Items.ContainsKey("Y"));
    }


    [Test]
    public void CanCreateServiceWithoutBaseUrlAndMakeAbsoluteRequests()
    {
      // Act
      IService service = RamoneConfiguration.NewService();

      service.CodecManager.AddCodec<Dossier, XmlSerializerCodec>(CMSConstants.CMSMediaType);
      ISession session = service.NewSession();
      Request req = session.Bind(new Uri(BaseUrl, CMSConstants.DossierPath.Replace("{id}", "0")));
      using (var resp = req.Get<Dossier>())
      {
        // Assert
        Assert.IsNotNull(resp.Body);
        Assert.That(resp.Body.Id, Is.EqualTo(0));
      }
    }


    [Test]
    public void WhenNotUsingBaseUrlItThrowsInvalidOperationException()
    {
      // Act
      IService service = RamoneConfiguration.NewService();
      ISession session = service.NewSession();

      AssertThrows<InvalidOperationException>(
        () => session.Bind(DossierTemplate, new { id = 2 }),
        ex => ex.Message.Contains("base URL"));
    }
  }
}
