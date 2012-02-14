using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class MediaTypeTests : TestHelper
  {
    public void CanConstructMediaTypeFromString()
    {
      // Act
      MediaType t1 = new MediaType("x/y");

      // Assert
      Assert.AreEqual("x", t1.TopLevelType);
      Assert.AreEqual("y", t1.SubType);
    }
  }
}
