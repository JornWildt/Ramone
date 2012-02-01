using System;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests.Utility
{
  [TestFixture]
  public class ObjectSerializerFormaterTests : TestHelper
  {
    IObjectSerializerFormaterManager MyObjectSerializerFormaterManager;

    
    protected override void SetUp()
    {
      base.SetUp();
      MyObjectSerializerFormaterManager = new ObjectSerializerFormaterManager();
    }


    [Test]
    public void CanSerializeWithStandardFormaters()
    {
      // Arrange
      RamoneConfiguration.RegisterStandardSerializationFormaters(Session.SerializerSettings.Formaters);
      var o = new
      {
        Url = new Uri("http://dr.dk"),
        Date = new DateTime(2012, 10, 12, 15, 14, 13)
      };

      // Act
      string result = Serialize(o);

      // Assert
      Assert.AreEqual("|Url=http://dr.dk/|Date=2012-10-12 15:14:13", result);
    }


    [Test]
    public void CanSerializeWithFormaters()
    {
      // Arrange
      MyObjectSerializerFormaterManager.AddFormater(typeof(Mail), new MailObjectSerializerFormater());
      var o = new
      {
        Mail = new Mail { Address = "jw@fjeldgruppen.dk" }
      };

      // Act
      ObjectSerializerSettings settings = new ObjectSerializerSettings
      {
        Formaters = MyObjectSerializerFormaterManager
      };

      string result = Serialize(o, settings);

      // Assert
      Assert.AreEqual("|Mail=jw@fjeldgruppen.dk", result);
    }


    public class Mail
    {
      public string Address { get; set; }
    }


    public class MailObjectSerializerFormater : IObjectSerializerFormater
    {
      #region IObjectSerializerFormater Members

      public string Format(object src)
      {
        return ((Mail)src).Address;
      }

      #endregion
    }
  }
}
