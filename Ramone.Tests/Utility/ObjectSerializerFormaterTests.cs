using System;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;
using Ramone.Utility.ObjectSerialization.Formaters;


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
      Assert.That(result, Is.EqualTo("|Bool=false|Date=2012-10-12T15:14:13|Url=http://dr.dk/"));
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
      Assert.That(result, Is.EqualTo("|Mail=jw@fjeldgruppen.dk|Bool=1"));
    }


    [Test]
    public void CanAddFormatersInVariousWays()
    {
      // Act
      MyObjectSerializerFormaterManager.AddFormater<Mail>(new MailObjectSerializerFormater());
      MyObjectSerializerFormaterManager.AddFormater<bool>(b => b ? "yes" : "no");

      // Act
      IObjectSerializerFormater f1 = MyObjectSerializerFormaterManager.GetFormater(typeof(Mail));
      IObjectSerializerFormater f2 = MyObjectSerializerFormaterManager.GetFormater(typeof(bool));

      // Assert
      Assert.IsNotNull(f1);
      Assert.IsNotNull(f2);
      Assert.That(f1.GetType(), Is.EqualTo(typeof(MailObjectSerializerFormater)));
      Assert.That(f2.GetType(), Is.EqualTo(typeof(DelegateFormater<bool>)));
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
