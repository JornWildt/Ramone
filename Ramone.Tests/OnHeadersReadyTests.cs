using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using System.Net;
using Ramone.Tests.Common;


namespace Ramone.Tests
{
  [TestFixture]
  public class OnHeadersReadyTests : TestHelper
  {
    [Test]
    public void CanHandleHeadersReady()
    {
      // Arrange
      Request request = Session.Request(HeaderListUrl);

      HttpWebRequest httpRequest = null;
      using (request.OnHeadersReady(r => httpRequest = r).Get<HeaderList>()) { }
      Assert.IsNotNull(httpRequest);
    }


    [Test]
    public void CanHandleHeadersReady_Async()
    {
      // Arrange
      Request request = Session.Request(HeaderListUrl);

      HttpWebRequest httpRequest = null;
      TestAsync(wh =>
        {
          request.OnHeadersReady(r => httpRequest = r).Async().Get<HeaderList>(r =>
          {
            wh.Set();
          });
        });
      Assert.IsNotNull(httpRequest);
    }
  }
}
