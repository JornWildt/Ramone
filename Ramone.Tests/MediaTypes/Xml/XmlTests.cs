using NUnit.Framework;
using Ramone.MediaTypes.Xml;


namespace Ramone.Tests.MediaTypes.Xml
{
  [TestFixture]
  public class XmlTests : TestHelper
  {
    public class DataContractDossier
    {
      public long Id { get; set; }

      public string Title { get; set; }
    }


    protected override void SetUp()
    {
      base.SetUp();
      Session.Service.Settings.CodecManager.AddCodec<DataContractDossier>("application/xml", new XmlSerializerCodec<DataContractDossier>());
    }


    [Test]
    public void CanReadUsingXmlDataContractSerializerCodec()
    {
      // Arrange      
      RamoneRequest req = Session.Bind(DossierTemplate, new { id = 5 });

      // Act
      DataContractDossier d = req.Get<DataContractDossier>().Body;

      // Assert
      Assert.IsNotNull(d);
      Assert.AreEqual(d.Id, 5);
    }
  }
}
