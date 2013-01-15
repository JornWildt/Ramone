using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class ServiceTests : TestHelper
  {
    [Test]
    public void CanSetAndGetServiceItems()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);

      // Act
      service.Items["X"] = 1234;
      int x = (int)service.Items["X"];

      // Assert
      Assert.AreEqual(1234, x);
    }


    [Test]
    public void WhenCreatingSessionItInheritsServiceItems()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);
      service.Items["X"] = 1234;

      // Act
      ISession session = service.NewSession();
      int x = (int)session.Items["X"];

      // Assert
      Assert.AreEqual(1234, x);
    }


    [Test]
    public void WhenChangingItemsInSessionItDoesNotChangeItemsInService()
    {
      // Arrange
      IService service = RamoneConfiguration.NewService(BaseUrl);
      service.Items["X"] = 1234;
      ISession session = service.NewSession();

      // Act
      session.Items["Y"] = "Hello";
      session.Items.Remove("X");

      // Assert
      Assert.AreEqual(1234, (int)service.Items["X"]);
      Assert.IsFalse(service.Items.ContainsKey("Y"));
    }
  }
}
