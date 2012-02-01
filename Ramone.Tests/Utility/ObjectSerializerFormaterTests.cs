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
      RamoneConfiguration.RegisterStandardSerializationFormaters(MyObjectSerializerFormaterManager);
      var o = new
      {
        Bool = false,
        Date = new DateTime(2012, 10, 12, 15, 14, 13),
        Url = new Uri("http://dr.dk")
      };

      // Act
      ObjectSerializerSettings settings = new ObjectSerializerSettings
      {
        Formaters = MyObjectSerializerFormaterManager
      };

      string result = Serialize(o, settings);

      // Assert
      Assert.AreEqual("|Bool=False|Date=2012-10-12T15:14:13|Url=http://dr.dk/", result);
    }


    [Test]
    public void CanSerializeWithFormaters()
    {
      // Arrange
      MyObjectSerializerFormaterManager.AddFormater(typeof(Mail), new MailObjectSerializerFormater());
      MyObjectSerializerFormaterManager.AddFormater(typeof(bool), new BoolObjectSerializerFormater());
      var o = new
      {
        Mail = new Mail { Address = "jw@fjeldgruppen.dk" },
        Bool = true
      };

      // Act
      ObjectSerializerSettings settings = new ObjectSerializerSettings
      {
        Formaters = MyObjectSerializerFormaterManager
      };

      string result = Serialize(o, settings);

      // Assert
      Assert.AreEqual("|Mail=jw@fjeldgruppen.dk|Bool=1", result);
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


    public class BoolObjectSerializerFormater : IObjectSerializerFormater
    {
      #region IObjectSerializerFormater Members

      public string Format(object src)
      {
        return ((bool)src) ? "1" : "0";
      }

      #endregion
    }
  }
}
