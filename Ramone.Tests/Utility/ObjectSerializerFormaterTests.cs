using System;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests.Utility
{
  public class UriObjectSerializerFormater : IObjectSerializerFormater
  {
    #region IObjectSerializerFormater Members

    public string Format(object src)
    {
      return ((Uri)src).AbsoluteUri;
    }

    #endregion
  }


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
    public void CanSerializeWithFormaters()
    {
      // Arrange
      MyObjectSerializerFormaterManager.AddFormater(typeof(Uri), new UriObjectSerializerFormater());
      var o = new
      {
        Url = new Uri("http://dr.dk")
      };

      // Act
      ObjectSerializerSettings settings = new ObjectSerializerSettings
      {
        Formaters = MyObjectSerializerFormaterManager
      };

      string result = Serialize(o, settings);

      // Assert
      Assert.AreEqual("|Url=http://dr.dk/", result);
    }


    protected string Serialize(object data, ObjectSerializerSettings settings = null)
    {
      ObjectSerializer serializer = new ObjectSerializer(data.GetType());
      ObjectToStringPropertyVisitor visitor = new ObjectToStringPropertyVisitor();
      serializer.Serialize(data, visitor, settings);
      return visitor.Result;
    }
  }
}
