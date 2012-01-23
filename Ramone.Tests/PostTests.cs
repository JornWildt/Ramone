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

    RamoneRequest DossiersReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossiersReq = Session.Request(DossiersUrl);
    }


    [Test]
    public void CanPostAndIgnoreReturnedBody()
    {
      // Act
      RamoneResponse response = DossiersReq.Post(MyDossier);

      // Assert
      Assert.IsNotNull(response);
    }


    [Test]
    public void CanPostAndGetResult()
    {
      // Act
      RamoneResponse<Dossier> response = DossiersReq.Post<Dossier>(MyDossier);
      Dossier newDossier = response.Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }


    [Test]
    public void CanPostAndGetResultWithAccept()
    {
      // Act
      Dossier newDossier = DossiersReq.Accept<Dossier>().Post(MyDossier).Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }


    [Test]
    public void CanPostAndGetResultWithAcceptMediaType()
    {
      // Act
      Dossier newDossier = DossiersReq.Accept<Dossier>(CMSConstants.CMSMediaType).Post(MyDossier).Body;

      // Assert
      Assert.IsNotNull(newDossier);
    }


#if false

    FIXME: Move to separate codec tests

    [Test]
    public void CanPostAtomItemToAtomFeed()
    {
      SyndicationItem item = new SyndicationItem("A new item", "New item content ...", null);

      var response = FeedEndPoint.Bind(new { feed = "Petes" })
                                 .Body(item)
                                 .Post<SyndicationItem>();

      Assert.AreEqual("A new item (in Petes)", response.Body.Title.Text);
      Assert.AreEqual("New item content ...", ((TextSyndicationContent)response.Body.Content).Text);
    }


    [Test]
    public void WhenPostResultsIn204NoContentItShouldReturnNullBody()
    {
      var response = ErrorEndpoint.Bind(new { code = 204, description = "No Content" })
                                  .ContentType("text/plain")
                                  .Body("hello")
                                  .Post<string>();

      Assert.IsNull(response.Body);
    }


    [Test]
    public void WhenPostResultsIn204NoContentAndNoReturnTypeIsSpecifiedItShouldReturnNullBody()
    {
      var response = ErrorEndpoint.Bind(new { code = 204, description = "No Content" })
                                  .ContentType("text/plain")
                                  .Body("hello")
                                  .Post();

      Assert.IsNull(response.Body);
    }
#endif
  }
}
