using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class AdditionalMethodsTests : TestHelper
  {
    [Test]
    public void CanGetDossier()
    {
      // Arrange
      RamoneRequest dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      //RamoneResponse response = dossierReq.Head();

      // Assert
      //Assert.IsNotNull(response);
    }
  }
}
