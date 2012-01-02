using NUnit.Framework;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class XmlSerializerCodecTests : TestHelper
  {
    [Test]
    public void CanLetServerEvolveByIgnoringUnknownProperties()
    {
      // Arrange
      RamoneRequest dog1Request = Session.Bind(Dog1Template, new { name = "Fido" });
      RamoneRequest dog2Request = Session.Bind(Dog2Template, new { name = "Hugo" });

      // Act
      Dog1 d1a = dog1Request.Get<Dog1>().Body;
      Dog1 d1b = dog2Request.Get<Dog1>().Body;
      Dog2 d2 = dog2Request.Get<Dog2>().Body;

      // Assert
      Assert.IsNotNull(d1a);
      Assert.AreEqual("Fido", d1a.Name);
      Assert.IsNotNull(d1b);
      Assert.AreEqual("Hugo", d1b.Name);
      Assert.IsNotNull(d2);
      Assert.AreEqual("Hugo", d2.Name);
      Assert.AreEqual(25, d2.Weight);
    }
  }
}
