﻿using System;
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
        Assert.That(error, Is.Not.Null);
      }
    }


    [Test]
    public void WhenCatchingAuthorizationExceptionItAllowsToDeserializeContent_AsyncEvent()
    {
      TestAsyncEvent(wh =>
        {
          Session.Request(BasicAuthUrl).AsyncEvent()
                 .OnError(error =>
                  {
                    HtmlDocument html = error.Response.Decode<HtmlDocument>();
                    Assert.That(html, Is.Not.Null);
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
