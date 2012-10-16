using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class PostTests : TestHelper
  {
    Dossier MyDossier = new Dossier
    {
      Title = "A new dossier"
    };

    Request DossiersReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossiersReq = Session.Request(DossiersUrl);
    }


    [Test]
    public void CanPostAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossiersReq.Post(MyDossier))
      {
        // Assert
        Assert.IsNotNull(response);
      }
    }


    [Test]
    public void CanPostAndGetResult()
    {
      // Act
      using (Response<Dossier> response = DossiersReq.Post<Dossier>(MyDossier))
      {
        Dossier newDossier = response.Body;

        // Assert
        Assert.IsNotNull(newDossier);
      }
    }


    [Test]
    public void CanPostAndGetResultWithAccept()
    {
      // Act
      using (var newDossier = DossiersReq.Accept<Dossier>().Post(MyDossier))
      {
        // Assert
        Assert.IsNotNull(newDossier.Body);
      }
    }


    [Test]
    public void CanPostAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var newDossier = DossiersReq.Accept<Dossier>(CMSConstants.CMSMediaType).Post(MyDossier))
      {
        // Assert
        Assert.IsNotNull(newDossier.Body);
      }
    }


    [Test]
    public void CanPostEmptyBody_Typed()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain").ContentType("application/octet-stream").Post<string>())
      {
        // Assert
        Assert.AreEqual(null, response.Body);
      }
    }


    [Test]
    public void CanPostEmptyBodyWhenNoDefaultMediaTypeIsSpecified_Typed()
    {
      // Arrange
      Session.DefaultRequestMediaType = null;
      Session.DefaultResponseMediaType = null;
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Post<string>())
      {
        // Assert
        Assert.AreEqual(null, response.Body);
      }
    }


    [Test]
    public void CanPostEmptyBody_Untyped()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Accept("text/plain").ContentType("application/octet-stream").Post())
      {
        // Assert
        Assert.AreEqual(null, response.Body);
      }
    }


    [Test]
    public void CanPostEmptyBodyWhenNoDefaultMediaTypeIsSpecified_Untyped()
    {
      // Arrange
      Session.DefaultRequestMediaType = null;
      Session.DefaultResponseMediaType = null;
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Post())
      {
        // Assert
        Assert.AreEqual(null, response.Body);
      }
    }
  }
}
