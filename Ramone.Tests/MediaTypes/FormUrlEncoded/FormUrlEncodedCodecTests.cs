using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;
using System.Collections.Specialized;
using System.Text;


namespace Ramone.Tests.MediaTypes.FormUrlEncoded
{
  [TestFixture]
  public class FormUrlEncodedCodecTests : TestHelper
  {
    [Test]
    public void CanPostUnregisteredFormUrlEncoded()
    {
      // Arrange
      var data = new { Name = "Pete", Age = 10, Active = "false" }; // Matches "MultipartData" class
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").ContentType("application/x-www-form-urlencoded").Post<string>(data))
      {
        // Assert
        Assert.AreEqual("application/x-www-form-urlencoded", response.Headers["x-contenttype"]);
        Assert.AreEqual("Pete-10-False", response.Body);
      }
    }


    [Test]
    public void CanPostUnregisteredFormUrlEncodedUsingShorthand()
    {
      // Arrange
      var data = new { Name = "Pete", Age = 10, Active = true }; // Matches "MultipartData" class
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").AsFormUrlEncoded().Post<string>(data))
      {
        // Assert
        Assert.AreEqual("application/x-www-form-urlencoded", response.Headers["x-contenttype"]);
        Assert.AreEqual("Pete-10-True", response.Body);
      }
    }

    
    class RegisteredData // Matches "MultipartData" class
    {
      public string Name { get; set; }
      public int Age { get; set; }
      public bool Active { get; set; }
    }


    [Test]
    public void CanPostRegisteredFormUrlEncoded()
    {
      // Arrange
      Session.Service.CodecManager.AddCodec<RegisteredData, FormUrlEncodedSerializerCodec>(MediaType.ApplicationFormUrlEncoded);
      RegisteredData data = new RegisteredData { Name = "Pete", Age = 10, Active = false };
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain").Post<string>(data))
      {
        // Assert
        Assert.AreEqual("application/x-www-form-urlencoded", response.Headers["x-contenttype"]);
        Assert.AreEqual("Pete-10-False", response.Body);
      }
    }


    [Test]
    public void CanPostFormUrlEncodedWithEncoding()
    {
      // Arrange
      Session.SerializerSettings.Encoding = Encoding.GetEncoding("iso-8859-1");
      var data = new { Name = "ÆØÅüî", Age = 10 }; // Matches "MultipartData" class
      Request formdataReq = Session.Bind(MultipartFormDataTemplate);

      // Act
      using (Response<string> response = formdataReq.Accept("text/plain")
                                                    .AsFormUrlEncoded()
                                                    .Post<string>(data))
      {
        // Assert
        Assert.AreNotEqual("ÆØÅüî-10", response.Body, "What a hack: OpenRasta always assume UTF-8, so if body is not identical to the expected it must mean that it was actually send in non-UTF-8!");
      }
    }


    [Test]
    public void CanPostComplexClass()
    {
      // Arrange
      Guid g = Guid.NewGuid();
      ComplexClassForOpenRastaSerializationTests o = new ComplexClassForOpenRastaSerializationTests
      {
        X = 15,
        Y = "Abc",
        IntArray = new List<int> { 1, 2 },
        SubC = new ComplexClassForOpenRastaSerializationTests.SubClass
        {
          SubC = new ComplexClassForOpenRastaSerializationTests.SubClass
          {
            Data = new List<string> { "Benny" }
          },
          Data = new List<string> { "Brian" }
        },
        Dict = new Dictionary<string,string>(),
        Date = new DateTime(2012, 10, 30, 12, 13, 14),
        Dou = 15.234,
        GID = g
      };
      o.Dict["abc"] = "123";
      o.Dict["qwe"] = "xyz";

      Session.SerializerSettings = new ObjectSerializerSettings
      {
        ArrayFormat = "{0}:{1}",
        DictionaryFormat = "{0}:{1}",
        PropertyFormat = "{0}.{1}"
      };

      Request request = Session.Bind(ComplexClassTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain")
                                                .AsFormUrlEncoded()
                                                .Post<string>(o))
      {
        // Assert
        Console.WriteLine(response.Body);
        Assert.AreEqual("|X=15|Y=Abc|IntArray[0]=1|IntArray[1]=2|SubC.SubC.Data[0]=Benny|SubC.Data[0]=Brian|Dict[abc]=123|Dict[qwe]=xyz|Date=2012-10-30T12:13:14|Dou=15.234|GID=" + g.ToString(), response.Body);
      }
    }


    [Test]
    public void CanReadTyped()
    {
      // Arrange
      Request request = Session.Bind(FormUrlEncodedTemplate, new { mode = "x" });

      // Act
      using (Response<FormUrlEncodedData> response = request.Accept("application/x-www-form-urlencoded")
                                                            .Get<FormUrlEncodedData>())
      {
        FormUrlEncodedData data = response.Body;

        // Assert
        Assert.AreEqual("Abc", data.Title);
        Assert.AreEqual(15, data.Age);
      }
    }


    [Test]
    public void CanReadNameValueCollection()
    {
      // Arrange
      Request request = Session.Bind(FormUrlEncodedTemplate, new { mode = "x" });

      // Act
      using (Response<NameValueCollection> response = request.AcceptFormUrlEncoded().Get<NameValueCollection>())
      {
        NameValueCollection data = response.Body;

        // Assert
        Assert.AreEqual("Abc", data["Title"]);
        Assert.AreEqual("15", data["Age"]);
        Assert.AreEqual("Grete", data["SubData.Name"]);
      }
    }


    [Test]
    public void CanReadTypedInternationalCharacters(
      [Values("UTF-8", "iso-8859-1")] string charset)
    {
      // Arrange
      Request request = Session.Bind(FormUrlEncodedTemplate, new { mode = "intl" });

      Session.SerializerSettings.Encoding = Encoding.GetEncoding(charset);

      // Act
      using (Response<FormUrlEncodedData> response = request.Accept("application/x-www-form-urlencoded")
                                                            .AcceptCharset(charset)
                                                            .Get<FormUrlEncodedData>())
      {
        FormUrlEncodedData data = response.Body;

        // Assert
        Assert.AreEqual("ÆØÅ", data.Title);
        Assert.AreEqual("Güntør", data.SubData.Name);
      }
    }
  }
}
