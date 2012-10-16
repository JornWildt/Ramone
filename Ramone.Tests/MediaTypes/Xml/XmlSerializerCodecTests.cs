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
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<UnregisteredClass> response = request.Accept("application/xml").ContentType("application/xml").Post<UnregisteredClass>(data))
      {
        Assert.AreEqual(data.Text, response.Body.Text);
      }
    }


    [Test]
    public void CanPostUnregisteredTypeWithShorthand()
    {
      UnregisteredClass data = new UnregisteredClass { Text = "Hello" };
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<UnregisteredClass> response = request.AsXml().AcceptXml().Post<UnregisteredClass>(data))
      {
        Assert.AreEqual(data.Text, response.Body.Text);
      }
    }


    [Test]
    public void CanPostRegisteredType()
    {
      RegisteredClass data = new RegisteredClass { Title = "The World" };
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<RegisteredClass> response = request.Post<RegisteredClass>(data))
      {
        Assert.AreEqual(data.Title, response.Body.Title);
      }
    }


    [Test]
    public void CanReadWriteDates()
    {
      RegisteredClass data = new RegisteredClass { Title = "The World", Date = DateTime.Now };
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<RegisteredClass> response = request.Post<RegisteredClass>(data))
      {
        Assert.AreEqual(data.Title, response.Body.Title);
        Assert.AreEqual(data.Date, response.Body.Date);
      }
    }


    [Test]
    public void CanLetServerEvolveByIgnoringUnknownProperties()
    {
      // Arrange
      Request dog1Request = Session.Bind(Dog1Template, new { name = "Fido" });
      Request dog2Request = Session.Bind(Dog2Template, new { name = "Hugo" });

      // Act
      using (var d1a = dog1Request.Get<Dog1>())
      using (var d1b = dog2Request.Get<Dog1>())
      using (var d2 = dog2Request.Get<Dog2>())
      {
        // Assert
        Assert.IsNotNull(d1a.Body);
        Assert.AreEqual("Fido", d1a.Body.Name);
        Assert.IsNotNull(d1b.Body);
        Assert.AreEqual("Hugo", d1b.Body.Name);
        Assert.IsNotNull(d2.Body);
        Assert.AreEqual("Hugo", d2.Body.Name);
        Assert.AreEqual(25, d2.Body.Weight);
      }
    }
  }
}
