using NUnit.Framework;
using Ramone.Tests.Common;
using System;


namespace Ramone.Tests.MediaTypes.Json
{
  [TestFixture]
  public class JsonSerializerCodecTests : TestHelper
  {
    [Test]
    public void CanReadJson()
    {
      // Arrange
      Request req = Session.Bind(CatTemplate, new { name = "Ramstein" });

      // Act
      using (var cat = req.Accept("application/json").Get<Cat>())
      {
        // Assert
        Assert.IsNotNull(cat.Body);
        Assert.AreEqual("Ramstein", cat.Body.Name);
      }
    }


    [Test]
    public void CanPostJson()
    {
      // Arrange
      Cat cat = new Cat { Name = "Prince", DateOfBirth = DateTime.Now.ToUniversalTime() };
      cat.DateOfBirth = cat.DateOfBirth.AddTicks(-(cat.DateOfBirth.Ticks % TimeSpan.TicksPerSecond));

      Request request = Session.Bind(CatsTemplate);

      // Act
      using (var r = request.Accept("application/json").ContentType("application/json").Post<Cat>(cat))
      {
        Cat createdCat = r.Created();

        // Assert
        Assert.IsNotNull(createdCat);
        Assert.AreEqual("Prince", createdCat.Name);
        Assert.AreEqual(cat.DateOfBirth, createdCat.DateOfBirth);
      }
    }


    [Test]
    public void CanPostJsonUsingShorthand()
    {
      // Arrange
      Cat cat = new Cat { Name = "Prince" };
      Request request = Session.Bind(CatsTemplate);

      // Act
      using (var r = request.AsJson().AcceptJson().Post<Cat>(cat))
      {
        Cat createdCat = r.Created();

        // Assert
        Assert.IsNotNull(createdCat);
        Assert.AreEqual("Prince", createdCat.Name);
      }
    }


    [Test]
    public void CanPostUnregisteredType()
    {
      UnregisteredClass data = new UnregisteredClass { Text = "Hello" };
      Request request = Session.Bind(AnyEchoTemplate);

      using (Response<UnregisteredClass> response = request.Accept("application/json").ContentType("application/json").Post<UnregisteredClass>(data))
      {
        Assert.AreEqual(data.Text, response.Body.Text);
      }
    }


    [Test]
    public void CanPostUnregisteredTypeUsingShorthand()
    {
      UnregisteredClass data = new UnregisteredClass { Text = "Hello" };
      Request request = Session.Bind(AnyEchoTemplate);

      using (Response<UnregisteredClass> response = request.AsJson().AcceptJson().Post<UnregisteredClass>(data))
      {
        Assert.AreEqual(data.Text, response.Body.Text);
      }
    }


    [Test]
    public void CanReadJsonAsDynamic()
    {
      // Arrange
      Request req = Session.Bind(CatTemplate, new { name = "Ramstein" });

      // Act
      using (var r = req.Accept("application/json").Get())
      {
        dynamic cat = r.Body;

        // Assert
        Assert.IsNotNull(cat);
        Assert.AreEqual("Ramstein", cat.Name);
      }
    }


    [Test]
    public void CanWriteJsonFromAnonymous()
    {
      // Arrange
      dynamic cat = new { Name = "Prince" };
      Request request = Session.Bind(CatsTemplate);

      // Act
      using (var r = request.AsJson().AcceptJson().Post(cat))
      {
        dynamic createdCat = r.Body;

        // Assert
        Assert.IsNotNull(createdCat);
        Assert.AreEqual("Prince", createdCat.Name);
      }
    }
  }
}
