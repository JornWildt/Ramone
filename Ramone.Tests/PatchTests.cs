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
    public void CanPatch_Untyped()
    {
      // Act
      using (Response response = DossierReq.AsFormUrlEncoded().Patch(new { title = "Duh" }))
      {
        // Assert
        Assert.AreEqual("Duh: ok", response.Decode<string>());
      }
    }


    [Test]
    public void CanPatch_Untyped_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().Async().Patch(new { title = "Duh" }, response =>
        {
          // Assert
          Assert.AreEqual("Duh: ok", response.Decode<string>());
          wh.Set();
        });
      });
    }


    [Test]
    public void CanPatchAndGetResult_Typed()
    {
      // Act
      using (Response<string> response = DossierReq.AsFormUrlEncoded().Patch<string>(new { title = "Duh" }))
      {
        // Assert
        Assert.AreEqual("Duh: ok", response.Body);
      }
    }


    [Test]
    public void CanPatchAndGetResult_Typed_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.AsFormUrlEncoded().Async().Patch<string>(new { title = "Duh" }, response =>
        {
          // Assert
          Assert.AreEqual("Duh: ok", response.Body);
          wh.Set();
        });
      });
    }


    [Test]
    public void CanPatchAndGetResultWithAccept()
    {
      // Act
      using (var title = DossierReq.AsFormUrlEncoded().Accept<string>().Patch(new { title = "Duh" }))
      {
        // Assert
        Assert.AreEqual("Duh: ok", title.Body);
      }
    }


    [Test]
    public void CanPatchAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var title = DossierReq.AsFormUrlEncoded().Accept<string>("text/plain").Patch(new { title = "Duh" }))
      {
        // Assert
        Assert.AreEqual("Duh: ok", title.Body);
      }
    }
  }
}
