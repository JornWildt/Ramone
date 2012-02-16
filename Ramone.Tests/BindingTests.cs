using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Collections;
using System.Collections.Specialized;


namespace Ramone.Tests
{
  [TestFixture]
  public class BindingTests : TestHelper
  {
    // FIXME; include tests from RequestBuilderTests and SessionTests

    object ObjectParameters = new { a = 10, b = "John" };

    Hashtable HashtableParameters = new Hashtable();

    Dictionary<string, string> DictionaryParameters = new Dictionary<string, string>();

    NameValueCollection NameValueCollectionParameters = new NameValueCollection();

    // Templated inputs
    UriTemplate UriTemplate_Path     = new UriTemplate("users/{a}?b={b}");
    string      String_TemplatedPath = "users/{a}?b={b}";
    string      String_TemplatedUrl  = "http://home/users/{a}?b={b}";
    Uri         Uri_TemplatedUrl     = new Uri("http://home/users/{a}?b={b}");


    // Non-templated inputs
    //string StringPath = "/users";
    //string StringUrl = "http://home/users";
    //Uri UriUrl = new Uri("http://home/users");


    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();

      HashtableParameters["a"] = 10;
      HashtableParameters["b"] = "John";
      DictionaryParameters["a"] = "10";
      DictionaryParameters["b"] = "John";
      NameValueCollectionParameters["a"] = "10";
      NameValueCollectionParameters["b"] = "John";
    }


    // Repeat for parameter inputs
    //   None, object, hashtable, idict, namevalcoll
    // Repeat for return types
    //   Uri, Request


    [Test]
    public void CanBind_UriTemplate_Path()
    {
      // Act
      RamoneRequest req1 = Session.Bind(UriTemplate_Path, ObjectParameters);
      RamoneRequest req2 = Session.Bind(UriTemplate_Path, HashtableParameters);
      RamoneRequest req3 = Session.Bind(UriTemplate_Path, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(UriTemplate_Path, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual(BaseUrl + "users/10?b=John", req1.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", req2.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", req3.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", req4.Url.AbsoluteUri);
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
      Assert.AreEqual(BaseUrl + "users/10?b=John", url1.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", url2.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", url3.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", url4.AbsoluteUri);
    }


    [Test]
    public void CanBind_String_TemplatedPath()
    {
      // Act
      RamoneRequest req1 = Session.Bind(String_TemplatedPath, ObjectParameters);
      RamoneRequest req2 = Session.Bind(String_TemplatedPath, HashtableParameters);
      RamoneRequest req3 = Session.Bind(String_TemplatedPath, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(String_TemplatedPath, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual(BaseUrl + "users/10?b=John", req1.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", req2.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", req3.Url.AbsoluteUri);
      Assert.AreEqual(BaseUrl + "users/10?b=John", req4.Url.AbsoluteUri);
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
      RamoneRequest req1 = Session.Bind(String_TemplatedUrl, ObjectParameters);
      RamoneRequest req2 = Session.Bind(String_TemplatedUrl, HashtableParameters);
      RamoneRequest req3 = Session.Bind(String_TemplatedUrl, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(String_TemplatedUrl, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual("http://home/users/10?b=John", req1.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John", req2.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John", req3.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John", req4.Url.AbsoluteUri);
    }


    [Test]
    public void CanBind_Uri_TemplatedAbsoluteUrl()
    {
      // Does it make sense to bind absolute URL through session (service) which has its own BaseUrl?
      // - Yes, otherwise we cannot "follow" any arbitrary link in the response.

      // Act
      RamoneRequest req1 = Session.Bind(Uri_TemplatedUrl, ObjectParameters);
      RamoneRequest req2 = Session.Bind(Uri_TemplatedUrl, HashtableParameters);
      RamoneRequest req3 = Session.Bind(Uri_TemplatedUrl, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(Uri_TemplatedUrl, NameValueCollectionParameters);

      // Assert
      Assert.AreEqual("http://home/users/10?b=John", req1.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John", req2.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John", req3.Url.AbsoluteUri);
      Assert.AreEqual("http://home/users/10?b=John", req4.Url.AbsoluteUri);
    }


    [Test]
    public void CanAddParametersToStringPath()
    {
    }


    [Test]
    public void CanAddParametersToStringUrl()
    {
    }


    [Test]
    public void CanAddParametersToUri()
    {
    }
  }
}
