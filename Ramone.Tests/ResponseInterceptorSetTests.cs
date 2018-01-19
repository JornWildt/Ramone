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
      Assert.IsNotNull(i);
    }


    [Test]
    public void WhenNotFindingInterceptorItReturnsNull()
    {
      // Act
      IResponseInterceptor i = ResponseInterceptorSet.Find("Y");

      // Assert
      Assert.IsNull(i);
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
      Assert.IsNotNull(x);
      Assert.IsNotNull(y);
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
