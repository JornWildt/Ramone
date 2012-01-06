using NUnit.Framework;
using Ramone.MediaTypes.Json;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.MediaTypes.Json
{
  [TestFixture]
  public class JsonDynamicCodecTests : TestHelper
  {
    [Test]
    public void CanReadJsonAsDynamic()
    {
      // Arrange
      RamoneRequest req = Session.Bind(CatTemplate, new { name = "Ramstein" });

      // Act
      dynamic cat = req.Accept("application/json").Get().Body;

      // Assert
      Assert.IsNotNull(cat);
      Assert.AreEqual("Ramstein", cat.Name);
    }


    [Test]
    public void CanWriteJsonFromAnonymous()
    {
      // Arrange
      dynamic cat = new { Name = "Prince" };
      RamoneRequest request = Session.Bind(CatsTemplate);

      // Act
      dynamic createdCat = request.DuplexType("application/json").Post(cat).Body;

      // Assert
      Assert.IsNotNull(createdCat);
      Assert.AreEqual("Prince", createdCat.Name);
    }
  }
}
