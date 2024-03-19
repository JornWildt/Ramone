using System;
using NUnit.Framework;
using Ramone.Implementation;
using System.Collections.Generic;


namespace Ramone.Tests
{
  [TestFixture]
  public class ResponseInterceptorSetTests : TestHelper
  {
    IResponseInterceptorSet ResponseInterceptorSet { get; set; }

    protected override void SetUp()
    {
      base.SetUp();
      ResponseInterceptorSet = new ResponseInterceptorSet();
    }


    [Test]
    public void CanAddAndGetInterceptor()
    {
      // Act
      ResponseInterceptorSet.Add("X", new SomeInterceptor());
      IResponseInterceptor i = ResponseInterceptorSet.Find("X");

      // Assert
      Assert.That(i, Is.Not.Null);
    }


    [Test]
    public void WhenNotFindingInterceptorItReturnsNull()
    {
      // Act
      IResponseInterceptor i = ResponseInterceptorSet.Find("Y");

      // Assert
      Assert.That(i, Is.Null);
    }


    [Test]
    public void CanEnumerateInterceptors()
    {
      // Arrange
      ResponseInterceptorSet.Add("X", new SomeInterceptor());
      ResponseInterceptorSet.Add("Y", new SomeInterceptor());

      IResponseInterceptor x = null;
      IResponseInterceptor y = null;

      // Act
      foreach (KeyValuePair<string,IResponseInterceptor> i in ResponseInterceptorSet)
      {
        if (i.Key == "X")
          x = i.Value;
        else if (i.Key == "Y")
          y = i.Value;
      }

      // Assert
      Assert.That(x, Is.Not.Null);
      Assert.That(y, Is.Not.Null);
    }


    class SomeInterceptor : IResponseInterceptor
    {
      #region IResponseInterceptor Members

      public void ResponseReady(ResponseContext response)
      {
        throw new NotImplementedException();
      }

      #endregion
    }

  }
}
