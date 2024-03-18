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

      Assert.That(req.Session, Is.EqualTo(Session));
    }
  }
}
