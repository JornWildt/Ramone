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

      Assert.That(pcoll1.Count, Is.EqualTo(2));
      Assert.That(pcoll1["x"], Is.EqualTo("1"));
      Assert.That(pcoll1["y"], Is.EqualTo("hello,world"));

      Assert.That(pcoll2.Count, Is.EqualTo(2));
      Assert.That(pcoll2["x"], Is.EqualTo("john,1"));
      Assert.That(pcoll2["y"], Is.EqualTo("wide,hello,world"));
    }


    [Test]
    public void CanAddQueryParametersToRequest()
    {
      // Arrange
      Request r = Session.Bind("http://dr.dk/xyz");

      // Act
      r.AddQueryParameters(new { x = 1 });

      // Assert
      Assert.That(r.Url.AbsoluteUri, Is.EqualTo("http://dr.dk/xyz?x=1"));
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

      Assert.That(pcoll1.Count, Is.EqualTo(2));
      Assert.That(pcoll1["x"], Is.EqualTo("1"));
      Assert.That(pcoll1["y"], Is.EqualTo("hello"));

      Assert.That(pcoll2.Count, Is.EqualTo(3));
      Assert.That(pcoll2["x"], Is.EqualTo("1"));
      Assert.That(pcoll2["y"], Is.EqualTo("hello"));
      Assert.That(pcoll2["q"], Is.EqualTo("world"));

      Assert.That(pcoll3.Count, Is.EqualTo(3));
      Assert.That(pcoll3["x"], Is.EqualTo("5,1"));
      Assert.That(pcoll3["y"], Is.EqualTo("hello"));
      Assert.That(pcoll3["q"], Is.EqualTo("web"));

      Assert.That(pcoll4.Count, Is.EqualTo(3));
      Assert.That(pcoll4["x"], Is.EqualTo("8,1"));
      Assert.That(pcoll4["y"], Is.EqualTo("hello"));
      Assert.That(pcoll4["q"], Is.EqualTo("world,wide"));
    }
  }
}
