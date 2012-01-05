using System.Xml;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System;


namespace Ramone.Tests.MediaTypes.Xml
{
  [TestFixture]
  public class JsonSerializerCodecTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      //settings.CodecManager.AddCodec<HalResource>("application/hal+xml", new AtomItemCodec());

    }
    [Test]
    public void CanReadJson()
    {
      // Arrange
      RamoneRequest req = Session.Bind(DossierTemplate, new { id = 5 });

      // Act
      Dossier dossier = req.Accept("application/json").Get<Dossier>().Body;

      // Assert
      Assert.IsNotNull(dossier);
      Assert.AreEqual(5, dossier.Id);
    }


    [Test]
    public void CanWriteJson()
    {
      // Arrange

      RamoneRequest request = Session.Request(DossiersUrl);

      // Act
      //RamoneResponse<Dossier> response = request.Post<Dossier>(dossierDoc);

      // Assert
      //Dossier createdDossier = response.Body;

      //Assert.IsNotNull(createdDossier);
      //Assert.AreEqual("My dossier", createdDossier.Title);
    }
  }
}
