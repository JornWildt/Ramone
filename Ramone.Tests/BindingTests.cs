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
    // - merge "a" into path.
    // - merge "b" into query parameter
    // - add unreferenced "c" automatically as extra query parameter
    Template UriTemplate_Path     = new Template("users/{a}{?b}");
    string   String_TemplatedPath = "users/{a}{?b}";
    string   String_TemplatedUrl  = "http://home/users/{a}{?b}";
    Uri      Uri_TemplatedUrl     = new Uri("http://home/users/{a}{?b}");


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
      Assert.That(req1.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(req2.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(req3.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(req4.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
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
      Assert.That(url1.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(url2.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(url3.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(url4.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
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
      Assert.That(req1.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(req2.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(req3.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(req4.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
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
      Assert.That(url1.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(url2.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(url3.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
      Assert.That(url4.AbsoluteUri, Is.EqualTo(BaseUrl + "users/10?b=John&c=33"));
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
      Assert.That(req1.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(req2.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(req3.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(req4.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
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
      Assert.That(url1.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(url2.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(url3.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(url4.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
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
      Assert.That(req1.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(req2.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(req3.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(req4.Url.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
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
      Assert.That(url1.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(url2.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(url3.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
      Assert.That(url4.AbsoluteUri, Is.EqualTo("http://home/users/10?b=John&c=33"));
    }


    [Test]
    public void CanBind_UriTemplate_NonTemplated()
    {
      // Act
      Request req = Session.Bind(new Template("/cats"));

      // Assert
      Assert.That(req.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "cats"));
    }


    [Test]
    public void CanBind_String_NonTemplatedPath()
    {
      // Act
      Request req = Session.Bind("/cats");

      // Assert
      Assert.That(req.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "cats"));
    }


    [Test]
    public void CanBind_String_NonTemplatedAbsoluteUrl()
    {
      // Act
      Request req = Session.Bind("http://home/cats");

      // Assert
      Assert.That(req.Url.AbsoluteUri, Is.EqualTo("http://home/cats"));
    }


    [Test]
    public void CanBind_Uri_NonTemplated()
    {
      // Act
      Request req = Session.Bind(new Uri("http://dr.dk"));

      // Assert
      Assert.That(req.Url.AbsoluteUri, Is.EqualTo("http://dr.dk/"));
    }


    [Test]
    public void CanBind_UriTemplate_WithEncoding()
    {
      // Act
      Request req = Session.Bind(new Template("set?name={name}"), new { name = "Jørn" });
      Uri url = req.Url;

      // Assert
      Assert.That(req.Url.AbsoluteUri, Is.EqualTo(BaseUrl + "set?name=J%C3%B8rn"));
    }


    [Test]
    public void CanBindLink()
    {
      // Arrange
      ILink link = new AtomLink("http://dr.dk", "alt", MediaType.ApplicationXHtml, "Here");

      // Act
      Request req = Session.Bind(link);

      // Assert
      Assert.That(req, Is.Not.Null);
      Assert.That(req.Url, Is.EqualTo(new Uri("http://dr.dk")));
    }


    [Test]
    public void CanBindLinkAsTemplate()
    {
      // Arrange
      ILink link = new AtomLink("http://host/api/items/{id}", "template", MediaType.ApplicationXHtml, "Templated link");

      // Act
      Request req = link.Bind(Session, new { id = 5 });

      // Assert
      Assert.That(req, Is.Not.Null);
      Assert.That(req.Session, Is.Not.Null);
      Assert.That(req.Url, Is.EqualTo(new Uri("http://host/api/items/5")));
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
      Assert.That(req, Is.Not.Null);
      Assert.That(req.Session, Is.Not.Null);
      Assert.That(req.Url, Is.EqualTo(new Uri("http://host/api/items/20")));
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
      Assert.That(req, Is.Not.Null);
      Assert.That(req.Session, Is.Not.Null);
      Assert.That(req.Url, Is.EqualTo(new Uri("http://host/api/items/5")));
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
      Assert.That(r, Is.Not.Null);
      Assert.That(r.Url.ToString(), Is.EqualTo("http://example.com/?filter={\"a\"%3A10}"));
    }


    [Test]
    public void CanBindJSONIntoTemplateVariable2()
    {
      // Arrange
      string url = "http://example.com?filter={filter}";

      // Act
      Request r = Session.Bind(url, new { filter = "{\"a\":10}" });

      // Assert
      Assert.That(r, Is.Not.Null);
      Assert.That(r.Url.ToString(), Is.EqualTo("http://example.com/?filter={\"a\"%3A10}"));
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
