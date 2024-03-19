using System;
using System.Globalization;
using System.Linq;
using System.Net.Cache;
using System.Text;
using NUnit.Framework;
using Ramone.Utility.ObjectSerialization;
using Ramone.Tests.Common.CMS;
using Ramone.MediaTypes.Xml;


namespace Ramone.Tests
{
  [TestFixture]
  public class SessionTests : TestHelper
  {
    IService MySettings = RamoneConfiguration.NewService(BaseUrl);


    [Test]
    public void WhenCreatingSessionItCopiesAllSimpleProperties()
    {
      // Arrange
      HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheOrNextCacheOnly);

      IService service = RamoneConfiguration.NewService(BaseUrl);
      service.UserAgent = "Dummy";
      service.DefaultEncoding = Encoding.ASCII;
      service.DefaultRequestMediaType = new MediaType("X/1");
      service.DefaultResponseMediaType = new MediaType("Y/1");
      service.CachePolicy = policy;

      // Act
      ISession session = service.NewSession();

      // Assert
      Assert.That(session.UserAgent, Is.EqualTo("Dummy"));
      Assert.That(session.DefaultEncoding, Is.EqualTo(Encoding.ASCII));
      Assert.That(session.DefaultRequestMediaType, Is.EqualTo(new MediaType("X/1")));
      Assert.That(session.DefaultResponseMediaType, Is.EqualTo(new MediaType("Y/1")));
      Assert.That(session.BaseUri, Is.EqualTo(BaseUrl));
      Assert.That(session.CachePolicy, Is.EqualTo(policy));
    }


    [Test]
    public void WhenCreatingSessionItClonesAuthorizationDispatcher()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      ISession session1 = service.NewSession();

      service.AuthorizationDispatcher.Add("dummy1", new DummyHandler1());
      
      ISession session2 = service.NewSession();
      session2.AuthorizationDispatcher.Add("dummy2", new DummyHandler2());

      ISession session3 = service.NewSession();

      // Assert
      Assert.That(session1.AuthorizationDispatcher.Get("dummy1"), Is.Null);
      Assert.That(session1.AuthorizationDispatcher.Get("dummy2"), Is.Null);
      
      Assert.That(session2.AuthorizationDispatcher.Get("dummy1"), Is.Not.Null);
      Assert.That(session2.AuthorizationDispatcher.Get("dummy2"), Is.Not.Null);
      
      Assert.That(session3.AuthorizationDispatcher.Get("dummy1"), Is.Not.Null);
      Assert.That(session3.AuthorizationDispatcher.Get("dummy2"), Is.Null);
    }


    [Test]
    public void WhenCreatingSessionItClonesRedirectSettings()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      ISession session1 = service.NewSession();

      service.SetAllowedRedirects(300, 11);

      ISession session2 = service.NewSession();
      session2.SetAllowedRedirects(301, 7);

      ISession session3 = service.NewSession();

      // Assert
      Assert.That(service.GetAllowedRedirects(300), Is.EqualTo(11));
      Assert.That(session1.GetAllowedRedirects(300), Is.EqualTo(0));
      Assert.That(session2.GetAllowedRedirects(300), Is.EqualTo(11));
      Assert.That(session2.GetAllowedRedirects(301), Is.EqualTo(7));
    }


    [Test]
    public void WhenCreatingSessionItClonesInterceptors()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      ISession session1 = service.NewSession();

      service.RequestInterceptors.Add(new DummyInterceptor1());

      ISession session2 = service.NewSession();
      session2.RequestInterceptors.Add(new DummyInterceptor2());

      ISession session3 = service.NewSession();

      // Assert
      Assert.That(session1.RequestInterceptors.Count(), Is.EqualTo(0));

      Assert.That(session2.RequestInterceptors.Count(), Is.EqualTo(2));

      Assert.That(session3.RequestInterceptors.Count(), Is.EqualTo(1));
    }


    [Test]
    public void WhenCreatingSessionItClonesSerializerSettings()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);
      service.SerializerSettings.ArrayFormat = "A";
      service.SerializerSettings.DictionaryFormat = "B";
      service.SerializerSettings.PropertyFormat = "C";
      service.SerializerSettings.DateTimeFormat = "O";
      service.SerializerSettings.Formaters.AddFormater(typeof(SomeClass1), new SomeClass1Formater());
      service.SerializerSettings.Culture = CultureInfo.GetCultureInfo("da-DK");
      service.SerializerSettings.Encoding = Encoding.ASCII;
      service.SerializerSettings.EnableNonAsciiCharactersInMultipartFilenames = true;

      // Act 1
      ISession session = service.NewSession();

      // Assert
      Assert.That(session.SerializerSettings.ArrayFormat, Is.EqualTo("A"));
      Assert.That(session.SerializerSettings.DictionaryFormat, Is.EqualTo("B"));
      Assert.That(session.SerializerSettings.PropertyFormat, Is.EqualTo("C"));
      Assert.That(session.SerializerSettings.DateTimeFormat, Is.EqualTo("O"));
      Assert.That(session.SerializerSettings.Culture.Name, Is.EqualTo("da-DK"));
      Assert.That(session.SerializerSettings.Encoding, Is.EqualTo(Encoding.ASCII));
      Assert.That(session.SerializerSettings.EnableNonAsciiCharactersInMultipartFilenames, Is.True);
      
      // Act 2
      session.SerializerSettings.ArrayFormat = "A2";
      session.SerializerSettings.DictionaryFormat = "B2";
      session.SerializerSettings.PropertyFormat = "C2";
      session.SerializerSettings.DateTimeFormat = "R";
      session.SerializerSettings.Formaters.AddFormater(typeof(SomeClass2), new SomeClass2Formater());
      session.SerializerSettings.Culture = CultureInfo.GetCultureInfo("pt-BR");
      session.SerializerSettings.Encoding = Encoding.Unicode;
      session.SerializerSettings.EnableNonAsciiCharactersInMultipartFilenames = false;

      // Assert
      Assert.That(session.SerializerSettings.ArrayFormat, Is.EqualTo("A2"));
      Assert.That(session.SerializerSettings.DictionaryFormat, Is.EqualTo("B2"));
      Assert.That(session.SerializerSettings.PropertyFormat, Is.EqualTo("C2"));
      Assert.That(session.SerializerSettings.DateTimeFormat, Is.EqualTo("R"));
      Assert.That(session.SerializerSettings.Culture.Name, Is.EqualTo("pt-BR"));
      Assert.That(session.SerializerSettings.Encoding, Is.EqualTo(Encoding.Unicode));
      Assert.That(session.SerializerSettings.EnableNonAsciiCharactersInMultipartFilenames, Is.False);
      Assert.That(service.SerializerSettings.ArrayFormat, Is.EqualTo("A"));
      Assert.That(service.SerializerSettings.DictionaryFormat, Is.EqualTo("B"));
      Assert.That(service.SerializerSettings.PropertyFormat, Is.EqualTo("C"));
      Assert.That(service.SerializerSettings.DateTimeFormat, Is.EqualTo("O"));
      Assert.That(service.SerializerSettings.Culture.Name, Is.EqualTo("da-DK"));
      Assert.That(service.SerializerSettings.Encoding, Is.EqualTo(Encoding.ASCII));
      Assert.That(service.SerializerSettings.EnableNonAsciiCharactersInMultipartFilenames, Is.True);
      Assert.That(session.SerializerSettings.Formaters.GetFormater(typeof(SomeClass1)), Is.Not.Null);
      Assert.That(session.SerializerSettings.Formaters.GetFormater(typeof(SomeClass2)), Is.Not.Null);
      Assert.That(service.SerializerSettings.Formaters.GetFormater(typeof(SomeClass1)), Is.Not.Null);
      Assert.That(service.SerializerSettings.Formaters.GetFormater(typeof(SomeClass2)), Is.Null);
    }


    [Test]
    public void CanSkipUseOfService()
    {
      // Act
      ISession session = RamoneConfiguration.NewSession(BaseUrl);

      // Assert
      Assert.That(session.Service, Is.Not.Null);
      Assert.That(session.Service.BaseUri, Is.Not.Null);
      Assert.That(BaseUrl.AbsoluteUri, Is.EqualTo(session.Service.BaseUri.AbsoluteUri));
    }


    [Test]
    public void CanSetAndGetSessionItems()
    {
      // Arrange
      ISession session = RamoneConfiguration.NewSession(BaseUrl);

      // Act
      session.Items["X"] = 1234;
      int x = (int)session.Items["X"];

      // Assert
      Assert.That(x, Is.EqualTo(1234));
    }


    [Test]
    public void CanCreateSessionWithoutBaseUrlAndMakeAbsoluteRequests()
    {
      // Act
      ISession session = RamoneConfiguration.NewSession();

      session.Service.CodecManager.AddCodec<Dossier, XmlSerializerCodec>(CMSConstants.CMSMediaType);
      Request req = session.Bind(new Uri(BaseUrl, CMSConstants.DossierPath.Replace("{id}", "0")));
      using (var resp = req.Get<Dossier>())
      {
        // Assert
        Assert.That(resp.Body, Is.Not.Null);
        Assert.That(resp.Body.Id, Is.EqualTo(0));
      }
    }


    [Test]
    public void WhenNotUsingBaseUrlItThrowsInvalidOperationException()
    {
      // Act
      ISession session = RamoneConfiguration.NewSession();

      AssertThrows<InvalidOperationException>(
        () => session.Bind(DossierTemplate, new { id = 2 }),
        ex => ex.Message.Contains("base URL"));
    }


    class DummyHandler1 : IAuthorizationHandler
    {
      public bool HandleAuthorizationRequest(AuthorizationContext context)
      {
        return true;
      }
    }


    class DummyHandler2 : IAuthorizationHandler
    {
      public bool HandleAuthorizationRequest(AuthorizationContext context)
      {
        return true;
      }
    }


    class DummyInterceptor1 : IRequestInterceptor
    {
      public void HeadersReady(RequestContext context)
      {
      }

      public void DataSent(RequestContext context)
      {
      }
    }


    class DummyInterceptor2 : IRequestInterceptor
    {
      public void HeadersReady(RequestContext context)
      {
      }

      public void DataSent(RequestContext context)
      {
      }
    }


    class SomeClass1
    {
    }


    public class SomeClass1Formater : IObjectSerializerFormater
    {
      #region IObjectSerializerFormater Members

      public string Format(object src)
      {
        throw new NotImplementedException();
      }

      #endregion
    }


    class SomeClass2
    {
    }


    public class SomeClass2Formater : IObjectSerializerFormater
    {
      #region IObjectSerializerFormater Members

      public string Format(object src)
      {
        throw new NotImplementedException();
      }

      #endregion
    }
  }
}
