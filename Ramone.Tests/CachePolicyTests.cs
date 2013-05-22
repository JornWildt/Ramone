using System;
using System.Net.Cache;
using NUnit.Framework;
using Ramone.Tests.Common;
using System.Net;


namespace Ramone.Tests
{
  [TestFixture]
  public class CachePolicyTests : TestHelper
  {
    HttpRequestCachePolicy Policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheOrNextCacheOnly);


    [Test]
    public void CanAssignCachePolicyToSession()
    {
      // Act
      Session.CachePolicy = Policy;
      Request request = Session.Request(HeaderListUrl);

      // Assert
      RequestCachePolicy policy = null;
      using (request.OnHeadersReady(r => policy = r.CachePolicy).Get<HeaderList>())
      {
        Assert.AreEqual(Policy, policy);
      }
    }


    [Test]
    public void CanAssignCachePolicyToSession_Async()
    {
      // Act
      Session.CachePolicy = Policy;
      Request request = Session.Request(HeaderListUrl);

      // Assert
      RequestCachePolicy policy = null;
      TestAsync(wh =>
        {
          request.OnHeadersReady(r => policy = r.CachePolicy).Async().Get<HeaderList>(r =>
            {
              wh.Set();
            });
        });
      Assert.AreEqual(Policy, policy);
    }


    [Test]
    public void CanAssignCachePolicyToService()
    {
      // Act
      IService service = RamoneConfiguration.NewService(TestHelper.BaseUrl);
      service.CachePolicy = Policy;
      ISession session = service.NewSession();

      Request request = session.Request(HeaderListUrl);

      // Assert
      RequestCachePolicy policy = null;
      using (request.OnHeadersReady(r => policy = r.CachePolicy).AcceptXml().Get<HeaderList>())
      {
        Assert.AreEqual(Policy, policy);
      }
    }


#if false
    // For some internal experiments

    [Test]
    public void TestCache()
    {
      HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
      Session.CachePolicy = policy;

      Request req = new Request(Session, new Uri("http://www.dr.dk/drfront/images//2013/05/21/c=0,17,1024,505;w=300;42676.jpg"));

      using (var resp = req.Accept("image/jpeg").Get<byte[]>())
      {
        resp.SaveToFile("c:\\tmp\\scrap.jpg");
        Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
      }

      using (var resp = req.Accept("image/jpeg").Get<byte[]>())
      {
        resp.SaveToFile("c:\\tmp\\scrap.jpg");
        Assert.AreEqual(HttpStatusCode.NotModified, resp.StatusCode);
      }
    }
#endif
  }
}
