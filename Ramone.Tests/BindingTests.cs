using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;


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
    UriTemplate UriTemplate_Path     = new UriTemplate("users/{a}?b={b}");
    string      String_TemplatedPath = "users/{a}?b={b}";
    string      String_TemplatedUrl  = "http://home/users/{a}?b={b}";
    Uri         Uri_TemplatedUrl     = new Uri("http://home/users/{a}?b={b}");


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
      RamoneRequest req1 = Session.Bind(UriTemplate_Path, ObjectParameters);
      RamoneRequest req2 = Session.Bind(UriTemplate_Path, HashtableParameters);
      RamoneRequest req3 = Session.Bind(UriTemplate_Path, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(UriTemplate_Path, NameValueCollectionParameters);

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
      RamoneRequest req1 = Session.Bind(String_TemplatedPath, ObjectParameters);
      RamoneRequest req2 = Session.Bind(String_TemplatedPath, HashtableParameters);
      RamoneRequest req3 = Session.Bind(String_TemplatedPath, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(String_TemplatedPath, NameValueCollectionParameters);

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
      RamoneRequest req1 = Session.Bind(String_TemplatedUrl, ObjectParameters);
      RamoneRequest req2 = Session.Bind(String_TemplatedUrl, HashtableParameters);
      RamoneRequest req3 = Session.Bind(String_TemplatedUrl, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(String_TemplatedUrl, NameValueCollectionParameters);

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
      RamoneRequest req1 = Session.Bind(Uri_TemplatedUrl, ObjectParameters);
      RamoneRequest req2 = Session.Bind(Uri_TemplatedUrl, HashtableParameters);
      RamoneRequest req3 = Session.Bind(Uri_TemplatedUrl, DictionaryParameters);
      RamoneRequest req4 = Session.Bind(Uri_TemplatedUrl, NameValueCollectionParameters);

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
  }
}
