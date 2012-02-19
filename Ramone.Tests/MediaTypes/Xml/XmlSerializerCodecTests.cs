using System;
using NUnit.Framework;
using Ramone.Tests.Common;


namespace Ramone.Tests.MediaTypes.Xml
{
  [TestFixture]
  public class XmlSerializerCodecTests : TestHelper
  {
    [Test]
    public void CanPostUnregisteredType()
    {
      UnregisteredClass data = new UnregisteredClass { Text = "Hello" };
      RamoneRequest request = Session.Bind(XmlEchoTemplate);

      RamoneResponse<UnregisteredClass> response = request.Accept("application/xml").ContentType("application/xml").Post<UnregisteredClass>(data);

      Assert.AreEqual(data.Text, response.Body.Text);
    }


    [Test]
    public void CanPostUnregisteredTypeWithShorthand()
    {
      UnregisteredClass data = new UnregisteredClass { Text = "Hello" };
      RamoneRequest request = Session.Bind(XmlEchoTemplate);

      RamoneResponse<UnregisteredClass> response = request.AsXml().AcceptXml().Post<UnregisteredClass>(data);

      Assert.AreEqual(data.Text, response.Body.Text);
    }


    [Test]
    public void CanPostRegisteredType()
    {
      RegisteredClass data = new RegisteredClass { Title = "The World" };
      RamoneRequest request = Session.Bind(XmlEchoTemplate);

      RamoneResponse<RegisteredClass> response = request.Post<RegisteredClass>(data);

      Assert.AreEqual(data.Title, response.Body.Title);
    }


    [Test]
    public void CanReadWriteDates()
    {
      RegisteredClass data = new RegisteredClass { Title = "The World", Date = DateTime.Now };
      RamoneRequest request = Session.Bind(XmlEchoTemplate);

      RamoneResponse<RegisteredClass> response = request.Post<RegisteredClass>(data);

      Assert.AreEqual(data.Title, response.Body.Title);
      Assert.AreEqual(data.Date, response.Body.Date);
    }


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
