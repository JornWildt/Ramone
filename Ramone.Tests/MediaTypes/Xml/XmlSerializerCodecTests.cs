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
        Assert.That(response.Body.Text, Is.EqualTo(data.Text));
      }
    }


    [Test]
    public void CanPostUnregisteredTypeWithShorthand()
    {
      UnregisteredClass data = new UnregisteredClass { Text = "Hello" };
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<UnregisteredClass> response = request.AsXml().AcceptXml().Post<UnregisteredClass>(data))
      {
        Assert.That(response.Body.Text, Is.EqualTo(data.Text));
      }
    }


    [Test]
    public void CanPostRegisteredType()
    {
      RegisteredClass data = new RegisteredClass { Title = "The World" };
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<RegisteredClass> response = request.Post<RegisteredClass>(data))
      {
        Assert.That(response.Body.Title, Is.EqualTo(data.Title));
      }
    }


    [Test]
    public void CanReadWriteDates()
    {
      RegisteredClass data = new RegisteredClass { Title = "The World", Date = DateTime.Now };
      Request request = Session.Bind(XmlEchoTemplate);

      using (Response<RegisteredClass> response = request.Post<RegisteredClass>(data))
      {
        Assert.That(response.Body.Title, Is.EqualTo(data.Title));
        Assert.That(response.Body.Date, Is.EqualTo(data.Date));
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
        Assert.That(d1a.Body, Is.Not.Null);
        Assert.That(d1a.Body.Name, Is.EqualTo("Fido"));
        Assert.That(d1b.Body, Is.Not.Null);
        Assert.That(d1b.Body.Name, Is.EqualTo("Hugo"));
        Assert.That(d2.Body, Is.Not.Null);
        Assert.That(d2.Body.Name, Is.EqualTo("Hugo"));
        Assert.That(d2.Body.Weight, Is.EqualTo(25));
      }
    }
  }
}
