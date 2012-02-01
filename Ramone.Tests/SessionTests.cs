using System;
using System.Linq;
using NUnit.Framework;
using Ramone.Implementation;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests
{
  [TestFixture]
  public class SessionTests : TestHelper
  {
    IRamoneService MySettings = RamoneConfiguration.NewService(BaseUrl);


    [Test]
    public void WhenCreatingSessionItCopiesAllSimpleProperties()
    {
      // Arrange
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);
      service.UserAgent = "Dummy";

      // Act
      IRamoneSession session = service.NewSession();

      // Assert
      Assert.AreEqual("Dummy", session.UserAgent);
      Assert.AreEqual(BaseUrl, session.BaseUri);
    }


    [Test]
    public void WhenCreatingSessionItClonesAuthorizationDispatcher()
    {
      // Arrange
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      IRamoneSession session1 = service.NewSession();

      service.AuthorizationDispatcher.Add("dummy1", new DummyHandler1());
      
      IRamoneSession session2 = service.NewSession();
      session2.AuthorizationDispatcher.Add("dummy2", new DummyHandler2());

      IRamoneSession session3 = service.NewSession();

      // Assert
      Assert.IsNull(session1.AuthorizationDispatcher.Get("dummy1"));
      Assert.IsNull(session1.AuthorizationDispatcher.Get("dummy2"));
      
      Assert.IsNotNull(session2.AuthorizationDispatcher.Get("dummy1"));
      Assert.IsNotNull(session2.AuthorizationDispatcher.Get("dummy2"));
      
      Assert.IsNotNull(session3.AuthorizationDispatcher.Get("dummy1"));
      Assert.IsNull(session3.AuthorizationDispatcher.Get("dummy2"));
    }


    [Test]
    public void WhenCreatingSessionItClonesInterceptors()
    {
      // Arrange
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      IRamoneSession session1 = service.NewSession();

      service.RequestInterceptors.Add(new DummyInterceptor1());

      IRamoneSession session2 = service.NewSession();
      session2.RequestInterceptors.Add(new DummyInterceptor2());

      IRamoneSession session3 = service.NewSession();

      // Assert
      Assert.AreEqual(0, session1.RequestInterceptors.Count());

      Assert.AreEqual(2, session2.RequestInterceptors.Count());
      
      Assert.AreEqual(1, session3.RequestInterceptors.Count());
    }


    [Test]
    public void WhenCreatingSessionItClonesSerializerSettings()
    {
      // Arrange
      IRamoneService service = RamoneConfiguration.NewService(BaseUrl);
      service.SerializerSettings.ArrayFormat = "A";
      service.SerializerSettings.DictionaryFormat = "B";
      service.SerializerSettings.PropertyFormat = "C";
      service.SerializerSettings.DateTimeFormat = "O";
      service.SerializerSettings.Formaters.AddFormater(typeof(SomeClass1), new SomeClass1Formater());

      // Act 1
      IRamoneSession session = service.NewSession();

      // Assert
      Assert.AreEqual("A", session.SerializerSettings.ArrayFormat);
      Assert.AreEqual("B", session.SerializerSettings.DictionaryFormat);
      Assert.AreEqual("C", session.SerializerSettings.PropertyFormat);
      Assert.AreEqual("O", session.SerializerSettings.DateTimeFormat);
      
      // Act 2
      session.SerializerSettings.ArrayFormat = "A2";
      session.SerializerSettings.DictionaryFormat = "B2";
      session.SerializerSettings.PropertyFormat = "C2";
      session.SerializerSettings.DateTimeFormat = "R";
      session.SerializerSettings.Formaters.AddFormater(typeof(SomeClass2), new SomeClass2Formater());

      // Assert
      Assert.AreEqual("A2", session.SerializerSettings.ArrayFormat);
      Assert.AreEqual("B2", session.SerializerSettings.DictionaryFormat);
      Assert.AreEqual("C2", session.SerializerSettings.PropertyFormat);
      Assert.AreEqual("R", session.SerializerSettings.DateTimeFormat);
      Assert.AreEqual("A", service.SerializerSettings.ArrayFormat);
      Assert.AreEqual("B", service.SerializerSettings.DictionaryFormat);
      Assert.AreEqual("C", service.SerializerSettings.PropertyFormat);
      Assert.AreEqual("O", service.SerializerSettings.DateTimeFormat);
      Assert.IsNotNull(session.SerializerSettings.Formaters.GetFormater(typeof(SomeClass1)));
      Assert.IsNotNull(session.SerializerSettings.Formaters.GetFormater(typeof(SomeClass2)));
      Assert.IsNotNull(service.SerializerSettings.Formaters.GetFormater(typeof(SomeClass1)));
      Assert.IsNull(service.SerializerSettings.Formaters.GetFormater(typeof(SomeClass2)));
    }


    [Test]
    public void CanBindRelativeUrlAsStringWithoutParameters()
    {
      // Act
      RamoneRequest request = Session.Bind("xx/yy");

      // Assert
      Assert.AreEqual(new Uri(BaseUrl, "xx/yy"), request.Url);
    }


    [Test]
    public void CanBindRelativeUrlAsStringWithParameters()
    {
      // Act
      RamoneRequest request = Session.Bind("xx/{yy}", new { yy = 123 });

      // Assert
      Assert.AreEqual(new Uri(BaseUrl, "xx/123"), request.Url);
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
      public void Intercept(System.Net.HttpWebRequest request)
      {
      }
    }


    class DummyInterceptor2 : IRequestInterceptor
    {
      public void Intercept(System.Net.HttpWebRequest request)
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
