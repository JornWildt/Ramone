using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class PatchTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(DossierTemplate, new { id = 15 });
    }


    [Test]
    public void CanPatchUntyped()
    {
      // Act
      Response response = DossierReq.AsFormUrlEncoded().Patch(new { title = "Duh" });

      // Assert
      Assert.AreEqual("Duh: ok", response.Decode<string>());
    }


    [Test]
    public void CanPatchAndGetResult()
    {
      // Act
      Response<string> response = DossierReq.AsFormUrlEncoded().Patch<string>(new { title = "Duh" });

      // Assert
      Assert.AreEqual("Duh: ok", response.Body);
    }


    [Test]
    public void CanPatchAndGetResultWithAccept()
    {
      // Act
      string title = DossierReq.AsFormUrlEncoded().Accept<string>().Patch(new { title = "Duh" }).Body;

      // Assert
      Assert.AreEqual("Duh: ok", title);
    }


    [Test]
    public void CanPatchAndGetResultWithAcceptMediaType()
    {
      // Act
      string title = DossierReq.AsFormUrlEncoded().Accept<string>("text/plain").Patch(new { title = "Duh" }).Body;

      // Assert
      Assert.AreEqual("Duh: ok", title);
    }


    [Test]
    public void CanPatchEmptyBody_Typed()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      Response<string> response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Patch<string>();

      // Assert
      Assert.AreEqual(null, response.Body);
    }


    [Test]
    public void CanPatchEmptyBody_Untyped()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      Response response = request.Accept("text/plain").ContentType("application/x-www-url-formencoded").Patch();

      // Assert
      Assert.AreEqual(null, response.Body);
    }
  }
}
