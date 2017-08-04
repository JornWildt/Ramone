using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class GenericMethodTests_Async : TestHelper
  {
    [Test]
    public async Task CanExecuteGetWithGenericResult()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var dossier1 = await dossierReq.Async().Execute<Dossier>("GET"))
      using (var dossier2 = await dossierReq.Accept<Dossier>().Async().Execute("GET"))
      {
        // Make sure method is actually taken from parameter
        AssertThrows<WebException>(() => dossierReq.Execute<Dossier>("UNKNOWN"));

        // Assert
        Assert.AreEqual(8, dossier1.Body.Id);
        Assert.AreEqual(8, dossier2.Body.Id);
      }
    }
  }
}
