using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone;

namespace Ramone.Tests
{
  [TestFixture]
  public class ApplicationErrorTests : TestHelper
  {
    [Test]
    public void CanDecodeErrorStream()
    {
      try
      {
        using (var response = Session.Bind(ApplicationErrorTemplate, new { code = 10 }).Get())
        {
        }
      }
      catch (WebException ex)
      {
        using (var response = Session.Decode<ApplicationError>(ex))
        {
          Assert.That(response, Is.Not.Null);
          Assert.That(response.Body, Is.Not.Null);
          Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
          Assert.That(response.Body, Is.InstanceOf<ApplicationError>());
          Assert.That(response.Body.Code, Is.EqualTo(10));
          Assert.That(response.Body.Message, Is.EqualTo("Error X"));
        }
      }
    }


    [Test]
    public void WhenDecodingMissingErrorStreamItReturnsNull()
    {
      try
      {
        using (var response = Session.Bind("http://absolutely.unknown.webserver.ksrz.ouch").Get())
        {
        }
      }
      catch (WebException ex)
      {
        using (var response = Session.Decode<ApplicationError>(ex))
        {
          Assert.That(response, Is.Null);
        }
      }
    }


    [Test]
    public void ItSignalsMissingSessionWhenDecodingResponse()
    {
      // Example: for some odd reasons a web response is created outside of Ramone and we now try to decode it,
      // but without a session at hand.

      WebRequest request = WebRequest.Create(BindingExtensions.BindTemplate(BaseUrl, DossierTemplate, new { id = 8 }));
      using (WebResponse response = request.GetResponse())
      {
        Response ramoneResponse = new Response((HttpWebResponse)response, null, 0);
        AssertThrows<ArgumentNullException>(
          () => ramoneResponse.Decode<ApplicationError>(),
          ex => ex.Message.Contains("session"));
      }
    }
  }
}
