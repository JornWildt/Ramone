using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.MediaTypes.Hal
{
  [TestFixture]
  public class HalTests : TestHelper
  {
    [Test]
    public void CanGetHalResource()
    {
      // Arrange
      RamoneRequest halReq = Session.Bind(DossierTemplate, new { id = 5 });

      // Act
      HalDossier dossier = halReq.Get<HalDossier>().Body;

      // Assert
      Assert.IsNotNull(dossier);
      Assert.AreEqual(dossier.Id, 5);
    }


    [Test]
    public void HalFormatIsGood()
    {
      // Arrange
      RamoneRequest halReq = Session.Bind(DossierTemplate, new { id = 5 });

      // Act
      string halXml = halReq.Get<string>("application/hal+xml").Body;

      // Assert
      Assert.IsNotNullOrEmpty(halXml);
    }
  }
}
