using System;
using System.Net;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;


namespace Ramone.Tests
{
  [TestFixture]
  public class ExceptionHandlingTests : TestHelper
  {
    [Test]
    public void WhenCatchingAuthorizationExceptionItAllowsToDeserializeContent()
    {
      try
      {
        using (Session.Request(BasicAuthUrl).Get<string>()) { }
        Assert.Fail("Missing exception.");
      }
      catch (WebException ex)
      {
        HtmlDocument error = ((HttpWebResponse)ex.Response).AsRamoneResponse<HtmlDocument>(Session).Body;
        Assert.IsNotNull(error);
      }
    }


    [Test]
    public void WhenCatchingAuthorizationExceptionItAllowsToDeserializeContent_Async()
    {
      TestAsync(wh =>
        {
          Session.Request(BasicAuthUrl).Async()
                 .OnError(response =>
                  {
                    HtmlDocument error = response.Decode<HtmlDocument>();
                    Assert.IsNotNull(error);
                    wh.Set();
                  })
                 .Get<string>(response => { });
        });
    }


    [Test]
    public void CanHandleClientSideExceptionsWhenDisposing()
    {
      Request request = Session.Bind(DossierTemplate, new { id = 8 });
      AssertThrows<NonStandardException>(() =>
        {
          using (var r = request.Get())
          {
            throw new NonStandardException();
          }
        });
    }


    [Test]
    public void CanHandleDomainNameExceptionsWhenDisposing()
    {
      Request request = Session.Bind("http://unknown-host.name");
      AssertThrows<WebException>(() =>
        {
          using (var r = request.Get()) { }
        });
      AssertThrows<WebException>(() =>
      {
        using (var r = request.Get<Dossier>()) { }
      });
    }


    [Test]
    public void CanDisposeBadlyConstructedResponse()
    {
      using (var r = new Response(null, null, 0, null)) { }
    }

    public class NonStandardException : Exception
    {
    }
  }
}
