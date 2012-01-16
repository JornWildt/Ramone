using NUnit.Framework;
using Ramone.MediaTypes.Json;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.MediaTypes.Json
{
  [TestFixture]
  public class JsonSerializerCodecTests : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      TestService.CodecManager.AddCodec<Cat>("application/json", new JsonSerializerCodec<Cat>());
    }


    [Test]
    public void CanReadJson()
    {
      // Arrange
      RamoneRequest req = Session.Bind(CatTemplate, new { name = "Ramstein" });

      // Act
      Cat cat = req.Accept("application/json").Get<Cat>().Body;

      // Assert
      Assert.IsNotNull(cat);
      Assert.AreEqual("Ramstein", cat.Name);
    }


    [Test]
    public void CanWriteJson()
    {
      // Arrange
      Cat cat = new Cat { Name = "Prince" };
      RamoneRequest request = Session.Bind(CatsTemplate);

      // Act
      Cat createdCat = request.ContentType("application/json").Post<Cat>(cat).Created();

      // Assert
      Assert.IsNotNull(createdCat);
      Assert.AreEqual("Prince", createdCat.Name);
    }


    [Test]
    public void CanWriteJsonUsingShorthand()
    {
      // Arrange
      Cat cat = new Cat { Name = "Prince" };
      RamoneRequest request = Session.Bind(CatsTemplate);

      // Act
      Cat createdCat = request.AsJson().Post<Cat>(cat).Created();

      // Assert
      Assert.IsNotNull(createdCat);
      Assert.AreEqual("Prince", createdCat.Name);
    }
  }
}
