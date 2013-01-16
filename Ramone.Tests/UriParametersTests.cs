using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone;
using System.Collections.Specialized;
using System.Web;


namespace Ramone.Tests
{
  [TestFixture]
  public class UriParametersTests : TestHelper
  {
    [Test]
    public void CanAddQueryParametersFromClassProperties()
    {
      // Arrange
      object p = new
      {
        x = 1,
        y = "hello"
      };

      // Act + Assert
      CanAddQueryParametersFromAny(p);
    }


    [Test]
    public void CanAddQueryParametersFromNameValueCollection()
    {
      // Arrange
      NameValueCollection p = new NameValueCollection();
      p["x"] = "1";
      p["y"] = "hello";

      // Act + Assert
      CanAddQueryParametersFromAny(p);
    }


    [Test]
    public void CanAddQueryParametersFromDictionary()
    {
      // Arrange
      IDictionary<string, string> p = new Dictionary<string, string>();
      p["x"] = "1";
      p["y"] = "hello";

      // Act + Assert
      CanAddQueryParametersFromAny(p);
    }


    [Test]
    public void CanAddQueryParametersFromNameValueCollectionWithRepeatedKeys()
    {
      // Arrange
      Uri url1 = new Uri("http://dr.dk");
      Uri url2 = new Uri("http://dr.dk?x=john&y=wide");

      NameValueCollection p = new NameValueCollection();
      p["x"] = "1";
      p.Add("y", "hello");
      p.Add("y", "world");

      // Act
      url1 = url1.AddQueryParameters(p);
      url2 = url2.AddQueryParameters(p);

      // Assert
      NameValueCollection pcoll1 = HttpUtility.ParseQueryString(url1.Query);
      NameValueCollection pcoll2 = HttpUtility.ParseQueryString(url2.Query);

      Assert.AreEqual(2, pcoll1.Count);
      Assert.AreEqual("1", pcoll1["x"]);
      Assert.AreEqual("hello,world", pcoll1["y"]);

      Assert.AreEqual(2, pcoll2.Count);
      Assert.AreEqual("john,1", pcoll2["x"]);
      Assert.AreEqual("wide,hello,world", pcoll2["y"]);
    }


    private void CanAddQueryParametersFromAny(object p)
    {
      // Arrange
      Uri url1 = new Uri("http://dr.dk");
      Uri url2 = new Uri("http://dr.dk?q=world");
      Uri url3 = new Uri("http://dr.dk?x=5&q=web");
      Uri url4 = new Uri("http://dr.dk?q=world&q=wide&x=8");

      // Act
      url1 = url1.AddQueryParameters(p);
      url2 = url2.AddQueryParameters(p);
      url3 = url3.AddQueryParameters(p);
      url4 = url4.AddQueryParameters(p);

      // Assert
      NameValueCollection pcoll1 = HttpUtility.ParseQueryString(url1.Query);
      NameValueCollection pcoll2 = HttpUtility.ParseQueryString(url2.Query);
      NameValueCollection pcoll3 = HttpUtility.ParseQueryString(url3.Query);
      NameValueCollection pcoll4 = HttpUtility.ParseQueryString(url4.Query);

      Assert.AreEqual(2, pcoll1.Count);
      Assert.AreEqual("1", pcoll1["x"]);
      Assert.AreEqual("hello", pcoll1["y"]);
      
      Assert.AreEqual(3, pcoll2.Count);
      Assert.AreEqual("1", pcoll2["x"]);
      Assert.AreEqual("hello", pcoll2["y"]);
      Assert.AreEqual("world", pcoll2["q"]);
      
      Assert.AreEqual(3, pcoll3.Count);
      Assert.AreEqual("5,1", pcoll3["x"]);
      Assert.AreEqual("hello", pcoll3["y"]);
      Assert.AreEqual("web", pcoll3["q"]);
      
      Assert.AreEqual(3, pcoll4.Count);
      Assert.AreEqual("8,1", pcoll4["x"]);
      Assert.AreEqual("hello", pcoll4["y"]);
      Assert.AreEqual("world,wide", pcoll4["q"]);
    }
  }
}
