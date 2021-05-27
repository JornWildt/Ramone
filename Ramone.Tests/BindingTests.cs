using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Atom;
using Template = Tavis.UriTemplates.UriTemplate;

namespace Ramone.Tests
{
  [TestFixture]
  public class BindingTests : TestHelper
  {
    object ObjectParameters = new { a = 10, b = "John", c = 33 };

    Hashtable HashtableParameters = new Hashtable();

    Dictionary<string, string> DictionaryParameters = new Dictionary<string, string>();

    NameValueCollection NameValueCollectionParameters = new NameValueCollection();

    // Templated inputs
    Template UriTemplate_Path     = new Template("users/{a}{?b,c}");
    string   String_TemplatedPath = "users/{a}{?b,c}";
    string   String_TemplatedUrl  = "http://home/users/{a}{?b,c}";
    Uri      Uri_TemplatedUrl     = new Uri("http://home/users/{a}{?b,c}");


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();

      HashtableParameters["a"] = 10;
      HashtableParameters["b"] = "John";
      HashtableParameters["c"] = 33;
      DictionaryParameters["a"] = "10";
      DictionaryParameters["b"] = "John";
      DictionaryParameters["c"] = "33";
      NameValueCollectionParameters["a"] = "10";
      NameValueCollectionParameters["b"] = "John";
      NameValueCollectionParameters["c"] = "33";
    }


    // Repeat for parameter inputs
    //   None, object, hashtable, idict, namevalcoll
    // Repeat for return types
    //   Uri, Request


    [Test]
    public void CanBind_UriTemplate_Path()
    {
      // Act
      Request req1 = Session.Bind(UriTemplate_Path, ObjectParameters);
      Request req2 = Session.Bind(UriTemplate_Path, HashtableParameters);
      Request req3 = Session.Bind(UriTemplate_Path, DictionaryParameters);
      Request req4 = Session.Bind(UriTemplate_Path, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req1.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req2.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req3.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req4.Url.AbsoluteUri);
    }


    [Test]
    public void CanBindAsUri_UriTemplate_Path()
    {
      // Act
      Uri url1 = Session.BindUri(UriTemplate_Path, ObjectParameters);
      Uri url2 = Session.BindUri(UriTemplate_Path, HashtableParameters);
      Uri url3 = Session.BindUri(UriTemplate_Path, DictionaryParameters);
      Uri url4 = Session.BindUri(UriTemplate_Path, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url1.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url2.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url3.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url4.AbsoluteUri);
    }


    [Test]
    public void CanBind_String_TemplatedPath()
    {
      // Act
      Request req1 = Session.Bind(String_TemplatedPath, ObjectParameters);
      Request req2 = Session.Bind(String_TemplatedPath, HashtableParameters);
      Request req3 = Session.Bind(String_TemplatedPath, DictionaryParameters);
      Request req4 = Session.Bind(String_TemplatedPath, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req1.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req2.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req3.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", req4.Url.AbsoluteUri);
    }


    [Test]
    public void CanBindAsUri_String_TemplatedPath()
    {
      // Act
      Uri url1 = Session.BindUri(String_TemplatedPath, ObjectParameters);
      Uri url2 = Session.BindUri(String_TemplatedPath, HashtableParameters);
      Uri url3 = Session.BindUri(String_TemplatedPath, DictionaryParameters);
      Uri url4 = Session.BindUri(String_TemplatedPath, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url1.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url2.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url3.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John&c=33", url4.AbsoluteUri);
    }

    
    // FIXME: test also with escaped URI values
    // FIXME: test also including fragments and semicolon
    // FIXME: date formating and encoding?


    [Test]
    public void CanBind_String_TemplatedAbsoluteUrl()
    {
      // Does it make sense to bind absolute URL through session (service) which has its own BaseUrl?
      // - Yes, otherwise we cannot "follow" any arbitrary link in the response.

      // Act
      Request req1 = Session.Bind(String_TemplatedUrl, ObjectParameters);
      Request req2 = Session.Bind(String_TemplatedUrl, HashtableParameters);
      Request req3 = Session.Bind(String_TemplatedUrl, DictionaryParameters);
      Request req4 = Session.Bind(String_TemplatedUrl, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual("http://home/users/10?b=John&c=33", req1.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", req2.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", req3.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", req4.Url.AbsoluteUri);
    }


    [Test]
    public void CanBindAsUri_String_TemplatedAbsoluteUrl()
    {
      // Does it make sense to bind absolute URL through session (service) which has its own BaseUrl?
      // - Yes, otherwise we cannot "follow" any arbitrary link in the response.

      // Act
      Uri url1 = Session.BindUri(String_TemplatedUrl, ObjectParameters);
      Uri url2 = Session.BindUri(String_TemplatedUrl, HashtableParameters);
      Uri url3 = Session.BindUri(String_TemplatedUrl, DictionaryParameters);
      Uri url4 = Session.BindUri(String_TemplatedUrl, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual("http://home/users/10?b=John&c=33", url1.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", url2.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", url3.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", url4.AbsoluteUri);
    }


    [Test]
    public void CanBind_Uri_TemplatedAbsoluteUrl()
    {
      // Does it make sense to bind absolute URL through session (service) which has its own BaseUrl?
      // - Yes, otherwise we cannot "follow" any arbitrary link in the response.

      // Act
      Request req1 = Session.Bind(Uri_TemplatedUrl, ObjectParameters);
      Request req2 = Session.Bind(Uri_TemplatedUrl, HashtableParameters);
      Request req3 = Session.Bind(Uri_TemplatedUrl, DictionaryParameters);
      Request req4 = Session.Bind(Uri_TemplatedUrl, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual("http://home/users/10?b=John&c=33", req1.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", req2.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", req3.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", req4.Url.AbsoluteUri);
    }


    [Test]
    public void CanBindAsUri_Uri_TemplatedAbsoluteUrl()
    {
      // Does it make sense to bind absolute URL through session (service) which has its own BaseUrl?
      // - Yes, otherwise we cannot "follow" any arbitrary link in the response.

      // Act
      Uri url1 = Session.BindUri(Uri_TemplatedUrl, ObjectParameters);
      Uri url2 = Session.BindUri(Uri_TemplatedUrl, HashtableParameters);
      Uri url3 = Session.BindUri(Uri_TemplatedUrl, DictionaryParameters);
      Uri url4 = Session.BindUri(Uri_TemplatedUrl, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual("http://home/users/10?b=John&c=33", url1.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", url2.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", url3.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John&c=33", url4.AbsoluteUri);
    }


    [Test]
    public void CanBind_UriTemplate_NonTemplated()
    {
      // Act
      Request req = Session.Bind(new Template("/cats"));

      // Assert
      Assert.AreEqual(BaseUrl + "cats", req.Url.AbsoluteUri);
    }


    [Test]
    public void CanBind_String_NonTemplatedPath()
    {
      // Act
      Request req = Session.Bind("/cats");

      // Assert
      Assert.AreEqual(BaseUrl + "cats", req.Url.AbsoluteUri);
    }


    [Test]
    public void CanBind_String_NonTemplatedAbsoluteUrl()
    {
      // Act
      Request req = Session.Bind("http://home/cats");

      // Assert
      Assert.AreEqual("http://home/cats", req.Url.AbsoluteUri);
    }


    [Test]
    public void CanBind_Uri_NonTemplated()
    {
      // Act
      Request req = Session.Bind(new Uri("http://dr.dk"));

      // Assert
      Assert.AreEqual("http://dr.dk/", req.Url.AbsoluteUri);
    }


    [Test]
    public void CanBind_UriTemplate_WithEncoding()
    {
      // Act
      Request req = Session.Bind(new Template("set?name={name}"), new { name = "Jørn" });
      Uri url = req.Url;

      // Assert
      Assert.AreEqual(BaseUrl + "set?name=J%C3%B8rn", req.Url.AbsoluteUri);
    }


    [Test]
    public void CanBindLink()
    {
      // Arrange
      ILink link = new AtomLink("http://dr.dk", "alt", MediaType.ApplicationXHtml, "Here");

      // Act
      Request req = Session.Bind(link);

      // Assert
      Assert.IsNotNull(req);
      Assert.AreEqual(new Uri("http://dr.dk"), req.Url);
    }


    [Test]
    public void CanBindLinkAsTemplate()
    {
      // Arrange
      ILink link = new AtomLink("http://host/api/items/{id}", "template", MediaType.ApplicationXHtml, "Templated link");

      // Act
      Request req = link.Bind(Session, new { id = 5 });

      // Assert
      Assert.IsNotNull(req);
      Assert.IsNotNull(req.Session);
      Assert.AreEqual(new Uri("http://host/api/items/5"), req.Url);
    }


    [Test]
    public void CanSelectAndBindLinkAsTemplate()
    {
      // Arrange
      AtomLink link1 = new AtomLink("http://host/api/items/{id}", "template", MediaType.ApplicationXHtml, "Templated link");
      AtomLink link2 = new AtomLink("http://host/api/other/stuff/1234", "other", MediaType.ApplicationXHtml, "Simple link");
      
      AtomLinkList links = new AtomLinkList();
      links.Add(link1);
      links.Add(link2);

      // Act
      Request req = links.Select("template").Bind(Session, new { id = 20 });

      // Assert
      Assert.IsNotNull(req);
      Assert.IsNotNull(req.Session);
      Assert.AreEqual(new Uri("http://host/api/items/20"), req.Url);
    }


    [Test]
    public void CanBindSessionLinkAsTemplate()
    {
      // Arrange
      AtomLink link = new AtomLink("http://host/api/items/{id}", "template", MediaType.ApplicationXHtml, "Templated link");
      link.Session = Session;

      // Act
      Request req = link.Bind(new { id = 5 });

      // Assert
      Assert.IsNotNull(req);
      Assert.IsNotNull(req.Session);
      Assert.AreEqual(new Uri("http://host/api/items/5"), req.Url);
    }


    /*
    This us simply not possible with the UriTemplate classes as the JSON will get unescaped at some
    point and then interpreted as template variables.

    [Test]
    public void CanBindWithJSONInUrl()
    {
      // Arrange (URL with JSON {"a":10}
      string url = "http://example.com/?filter=%7b%22a%22%3a10%7d";

      // Act
      Request r = Session.Bind(url);

      // Assert
      Assert.IsNotNull(r);
      Assert.AreEqual(url, r.Url.AbsoluteUri);
    }


    [Test]
    public void CanBindWithJSONAndTemplateVariableInUrl()
    {
      // Arrange (URL with JSON {"a":10}
      string url = "http://example.com?filter=%7B%22a%22%3A10%7D&x={x}";

      // Act
      Request r = Session.Bind(url, new { x = 10 });

      // Assert
      Assert.IsNotNull(r);
      Assert.AreEqual("http://example.com?filter=%7B%22a%22%3A10%7D&x=10", r.Url);
    }
    */

    [Test]
    public void CanBindJSONIntoTemplateVariable1()
    {
      // Arrange
      string url = "http://example.com{?filter}";

      // Act
      Request r = Session.Bind(url, new { filter = "{\"a\":10}" });

      // Assert
      Assert.IsNotNull(r);
      Assert.AreEqual("http://example.com/?filter={\"a\"%3A10}", r.Url.ToString());
    }


    [Test]
    public void CanBindJSONIntoTemplateVariable2()
    {
      // Arrange
      string url = "http://example.com?filter={filter}";

      // Act
      Request r = Session.Bind(url, new { filter = "{\"a\":10}" });

      // Assert
      Assert.IsNotNull(r);
      Assert.AreEqual("http://example.com/?filter={\"a\"%3A10}", r.Url.ToString());
    }


    [Test]
    public void DoNotAllowUsingSessionObjectAsParameterWhenBindingUrl()
    {
      // Arrange
      Uri url = new Uri("http://example.org");

      // Act + Assert
      AssertThrows<ArgumentException>(() => url.Bind(Session),
        ex => ex.Message.Contains("probably should have written"));

      // The point here is that the right way to bind with session is Session.Bind(url)!
    }
  }
}
