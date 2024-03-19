using System;
using NUnit.Framework;
using Ramone.Implementation;
using System.Collections.Generic;


namespace Ramone.Tests
{
  [TestFixture]
  public class RequestInterceptorSetTests : TestHelper
  {
    IRequestInterceptorSet RequestInterceptorSet { get; set; }

    protected override void SetUp()
    {
      base.SetUp();
      RequestInterceptorSet = new RequestInterceptorSet();
    }


    [Test]
    public void CanAddAndGetInterceptor()
    {
      // Act
      RequestInterceptorSet.Add("X", new SomeInterceptor());
      IRequestInterceptor i = RequestInterceptorSet.Find("X");

      // Assert
      Assert.That(i, Is.Not.Null);
    }


    [Test]
    public void WhenNotFindingInterceptorItReturnsNull()
    {
      // Act
      IRequestInterceptor i = RequestInterceptorSet.Find("Y");

      // Assert
      Assert.That(i, Is.Null);
    }


    [Test]
    public void CanEnumerateInterceptors()
    {
      // Arrange
      RequestInterceptorSet.Add("X", new SomeInterceptor());
      RequestInterceptorSet.Add("Y", new SomeInterceptor());

      IRequestInterceptor x = null;
      IRequestInterceptor y = null;

      // Act
      foreach (KeyValuePair<string,IRequestInterceptor> i in RequestInterceptorSet)
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


    class SomeInterceptor : IRequestInterceptor
    {
      #region IRequestInterceptor Members

      public void HeadersReady(RequestContext context)
      {
        throw new NotImplementedException();
      }

      public void DataSent(RequestContext context)
      {
      }

      #endregion
    }

  }
}
