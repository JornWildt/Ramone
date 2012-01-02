using System;
using System.Linq;
using NUnit.Framework;
using Ramone.Implementation;


namespace Ramone.Tests
{
  [TestFixture]
  public class SessionTests : TestHelper
  {
    ISettings MySettings = RamoneConfiguration.NewSettings();


    [Test]
    public void WhenCreatingSessionItCopiesAllSimpleProperties()
    {
      // Arrange
      ISettings settings = RamoneConfiguration.NewSettings();
      settings.UserAgent = "Dummy";

      // Act
      IService service = settings.NewService(new Uri("http://dr.dk"));
      ISession session = service.NewSession();

      // Assert
      Assert.AreEqual("Dummy", service.UserAgent);
      Assert.AreEqual("Dummy", session.UserAgent);
    }


    [Test]
    public void WhenCreatingSessionItClonesAuthorizationDispatcher()
    {
      // Arrange
      IService service = MySettings.NewService(new Uri("http://dr.dk"));

      // Act
      ISession session1 = service.NewSession();

      service.AuthorizationDispatcher.Add("dummy1", new DummyHandler1());
      
      ISession session2 = service.NewSession();
      session2.AuthorizationDispatcher.Add("dummy2", new DummyHandler2());

      ISession session3 = service.NewSession();

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
      IService service = MySettings.NewService(new Uri("http://dr.dk"));

      // Act
      ISession session1 = service.NewSession();

      service.RequestInterceptors.Add(new DummyInterceptor1());

      ISession session2 = service.NewSession();
      session2.RequestInterceptors.Add(new DummyInterceptor2());

      ISession session3 = service.NewSession();

      // Assert
      Assert.AreEqual(0, session1.RequestInterceptors.Count());

      Assert.AreEqual(2, session2.RequestInterceptors.Count());
      
      Assert.AreEqual(1, session3.RequestInterceptors.Count());
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
  }
}
