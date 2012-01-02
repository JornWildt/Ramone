using System.Linq;
using System.Net;
using NUnit.Framework;
using Ramone.Implementation;


namespace Ramone.Tests
{
  [TestFixture]
  public class InterceptorTests : TestHelper
  {
    [SetUp]
    public void Setup()
    {
      RastaContext.ContextProvider.Context.RequestInterceptors.AddRequestInterceptor(new CountInterceptor());
    }


    [TearDown]
    public void Teardown()
    {
      RastaContext.ContextProvider.Context.RequestInterceptors.RemoveRequestInterceptor(typeof(CountInterceptor));
    }

    
    [Test]
    public void CanAddGetRemoveInterceptors()
    {
      int cou = RastaContext.ContextProvider.Context.RequestInterceptors.Interceptors.Count();

      RastaContext.ContextProvider.Context.RequestInterceptors.AddRequestInterceptor("x", new CountInterceptor());
      Assert.AreEqual(cou + 1, RastaContext.ContextProvider.Context.RequestInterceptors.Interceptors.Count());

      RastaContext.ContextProvider.Context.RequestInterceptors.RemoveRequestInterceptor("x");
      Assert.AreEqual(cou, RastaContext.ContextProvider.Context.RequestInterceptors.Interceptors.Count());
    }


    [Test]
    public void WhenExecutingRequestItCallsInterceptors()
    {
      int cou = CountInterceptor.Count;
      string result = TextEndPoint.Get<string>().Body;
      Assert.IsNotNull(result);
      Assert.AreEqual(cou + 1, CountInterceptor.Count);
    }


    public class CountInterceptor : IRequestInterceptor
    {
      public static int Count = 0;

      #region IRequestInterceptor Members

      public void Intercept(HttpWebRequest request)
      {
        ++Count;
      }

      #endregion
    }
  }
}
