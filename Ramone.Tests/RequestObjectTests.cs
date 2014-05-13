using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class RequestObjectTests : TestHelper
  {
    [Test]
    public void CanAccessSessionOnRequest()
    {
      Request req = Session.Bind("http://dr.dk");

      Assert.AreEqual(Session, req.Session);
    }
  }
}
