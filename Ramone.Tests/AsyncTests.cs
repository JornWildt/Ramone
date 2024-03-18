using NUnit.Framework;
using System;
using Ramone.Tests.Common;
using System.Threading.Tasks;
using System.Net;

namespace Ramone.Tests
{
  // Not many tests here - they are dispersed among the normal tests

  [TestFixture]
  public class AsyncTests : TestHelper
  {
    [Test]
    // A simple test to verify that we got something right (mostly while modeling the API)
    public async Task CanDoAsyncRequest()
    {
      // Arrange
      Request request = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      using (var response = await request.Async().Get())
      {
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      }
    }


    [Test]
    public async Task ExceptionsAreHandledTransparently()
    {
      // Arrange
      Request request = Session.Bind("/unknown-url");

      // Act + Assert

      await AssertThrows<WebException>(
        async () => await request.Async().Get(),
        ex => ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound);
    }
  }
}
