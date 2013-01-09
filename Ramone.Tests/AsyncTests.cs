using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class AsyncTests : TestHelper
  {
    [Test]
    public void CanDoAsyncRequest()
    {
      // Arrange
      Request request = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      request.Async(HandleResult).Get();
    }


    private void HandleResult(Response r)
    {
    }
  }
}
