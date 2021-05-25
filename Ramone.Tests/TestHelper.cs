﻿using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests
{
  public class TestHelper
  {
    public static readonly Uri DefaultBaseUrl = new Uri("http://jorn-pc/ramone-testserver/");

    private static Uri _baseUrl;
    public static Uri BaseUrl
    {
      get
      {
        if (_baseUrl == null)
        {
          string url = Environment.GetEnvironmentVariable("RAMONETEST_BASEURL");
          if (url == null)
            url = ConfigurationManager.AppSettings["RAMONETEST_BASEURL"];
          _baseUrl = url != null ? new Uri(url) : DefaultBaseUrl;
        }
        return _baseUrl;
      }
    }


    protected Uri ResolveTestUrl(string path)
    {
      return new Uri(BaseUrl, path);
    }


    protected static readonly UriTemplate DossierTemplate = new UriTemplate(CMSConstants.DossierPath);

    protected static readonly UriTemplate VerifiedMethodDossierTemplate = new UriTemplate(CMSConstants.VerifiedMethodDossierPath);

    protected static readonly UriTemplate DossierDocumentsTemplate = new UriTemplate(CMSConstants.DossierDocumentsPath);

    protected static readonly UriTemplate DocumentTemplate = new UriTemplate(CMSConstants.DocumentPath);

    protected static readonly UriTemplate PartyTemplate = new UriTemplate(CMSConstants.PartyPath);
    
    protected static readonly Uri DossiersUrl = new Uri(BaseUrl, CMSConstants.DossiersPath);

    //protected static readonly Uri VerifiedMethodDossiersUrl = new Uri(BaseUrl, CMSConstants.VerifiedMethodDossiersPath);


    protected static readonly UriTemplate CatTemplate = new UriTemplate(Constants.CatPath);

    protected static readonly UriTemplate CatsTemplate = new UriTemplate(Constants.CatsPath);

    protected static readonly UriTemplate Dog1Template = new UriTemplate(Constants.DogPath + "?v=1");

    protected static readonly UriTemplate Dog2Template = new UriTemplate(Constants.DogPath + "?v=2");

    protected static readonly UriTemplate PersonTemplate = new UriTemplate(Constants.PersonPath);

    protected static readonly UriTemplate EncodingTemplate = new UriTemplate(Constants.EncodingPath);

    protected static readonly UriTemplate FileTemplate = new UriTemplate(Constants.FilePath);

    protected static readonly UriTemplate MultipartFormDataTemplate = new UriTemplate(Constants.MultipartFormDataPath);

    protected static readonly UriTemplate MultipartFormDataFileTemplate = new UriTemplate(Constants.MultipartFormDataFilePath);

    protected static readonly UriTemplate FormUrlEncodedTemplate = new UriTemplate(Constants.FormUrlEncodedPath);

    protected static readonly UriTemplate XmlEchoTemplate = new UriTemplate(Constants.XmlEchoPath);

    protected static readonly UriTemplate AnyEchoTemplate = new UriTemplate(Constants.AnyEchoPath);

    protected static readonly UriTemplate ComplexClassTemplate = new UriTemplate(Constants.ComplexClassPath);

    protected static readonly UriTemplate AtomFeedTemplate = new UriTemplate(Constants.AtomFeedPath);

    protected static readonly UriTemplate AtomItemTemplate = new UriTemplate(Constants.AtomItemPath);

    protected static readonly Uri HeaderListUrl = new Uri(BaseUrl, Constants.HeaderEchoPath);

    protected static readonly Uri BasicAuthUrl = new Uri(BaseUrl, Constants.BasicAuthPath);

    protected static readonly UriTemplate FormTemplate = new UriTemplate(Constants.FormPath);

    protected static readonly UriTemplate FileDownloadTemplate = new UriTemplate(Constants.FileDownloadPath);

    protected static readonly UriTemplate LinkHeaderTemplate = new UriTemplate(Constants.LinkHeaderPath);

    protected static readonly UriTemplate RedirectTemplate = new UriTemplate(Constants.RedirectPath);

    protected static readonly UriTemplate PatchTemplate = new UriTemplate(Constants.PatchPath);

    protected static readonly UriTemplate ApplicationErrorTemplate = new UriTemplate(Constants.ApplicationErrorPath);


    public static IService TestService { get; set; }

    protected ISession Session { get; set; }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [OneTimeSetUp]
    public void MasterTestFixtureSetUp()
    {
      TestFixtureSetUp();
    }


    /// <summary>
    /// Executed only once before all tests. Override in subclasses to do subclass
    /// set up. Remember to call base.TestFixtureSetUp().
    /// NOTE: The [TestFixtureSetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void TestFixtureSetUp()
    {
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [SetUp]
    public void MasterSetUp()
    {
      SetUp();
    }


    /// <summary>
    /// Executed before each test method is run. Override in subclasses to do subclass
    /// set up. Remember to call base.SetUp().
    /// NOTE: The [SetUp] attribute cannot be used in subclasses because it is already
    /// in use.
    /// </summary>
    protected virtual void SetUp()
    {
      RamoneConfiguration.Reset();
      // Create a new session for each test
      Session = TestService.NewSession();
    }


    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [TearDown]
    public void MasterTearDown()
    {
      TearDown();
    }

    /// <summary>
    /// Executed after each test method is run.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TearDown().
    /// NOTE: [TearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TearDown()
    {
    }

    /// <summary>
    /// Do not call. For use by NUnit only.
    /// </summary>
    [OneTimeTearDown]
    public void TestFixtureMasterTearDown()
    {
      TestFixtureTearDown();
    }

    /// <summary>
    /// Executed only once after all tests.  Override in subclasses to do subclass
    /// clean up. Remember to call base.TestFixtureTearDown().
    /// NOTE: [TestFixtureTearDown] attribute cannot be used in subclasses because it is
    /// already in use.
    /// </summary>
    protected virtual void TestFixtureTearDown()
    {
    }


    // source and inspiration: http://srtsolutions.com/blogs/chrismarinos/archive/2008/06/06/testing-for-exceptions-in-unit-test-frameworks.aspx
    public static void AssertThrows<Exception>(Action blockThatThrowsException) where Exception : System.Exception
    {
      try
      {
        blockThatThrowsException();
      }
      catch (System.Exception ex)
      {
        if (ex.GetType() == typeof(Exception))
        {
          Console.WriteLine("Got expected exception with message: {0}", ex.Message);
          return;
        }

        Console.WriteLine(ex.ToString());
        Assert.Fail(String.Format("Expected {0}, got {1} saying: {2}", typeof(Exception), ex.GetType(), ex.Message));
      }

      Assert.Fail(String.Format("Expected {0}, but no exception was thrown", typeof(Exception)));
    }


    public static async Task AssertThrowsAsync<Exception>(Func<Task> blockThatThrowsException) 
      where Exception : System.Exception
    {
      try
      {
        await blockThatThrowsException();
      }
      catch (System.Exception ex)
      {
        if (ex.GetType() == typeof(Exception))
        {
          Console.WriteLine("Got expected exception with message: {0}", ex.Message);
          return;
        }

        Console.WriteLine(ex.ToString());
        Assert.Fail(String.Format("Expected {0}, got {1} saying: {2}", typeof(Exception), ex.GetType(), ex.Message));
      }

      Assert.Fail(String.Format("Expected {0}, but no exception was thrown", typeof(Exception)));
    }


    public static void AssertThrows<ExT>(Action blockThatThrowsException,
                                         Func<ExT, bool> exceptionVerifier) where ExT : System.Exception
    {
      // Safe to do .Wait() here as we know it is a *sync* operation
      AssertThrows<ExT>(null, blockThatThrowsException, exceptionVerifier).Wait();
    }


    public static async Task AssertThrows<ExT>(Func<Task> blockThatThrowsException,
                                               Func<ExT, bool> exceptionVerifier) where ExT : System.Exception
    {
      await AssertThrows<ExT>(blockThatThrowsException, null, exceptionVerifier);
    }


    private static async Task AssertThrows<ExT>(Func<Task> asyncBlockThatThrowsException,
                                                Action blockThatThrowsException,
                                                Func<ExT, bool> exceptionVerifier) where ExT : System.Exception
    {
      try
      {
        if (asyncBlockThatThrowsException != null)
          await asyncBlockThatThrowsException();
        else
          blockThatThrowsException();
      }
      catch (System.Exception ex)
      {
        if (ex.GetType() != typeof(ExT))
        {
          Console.WriteLine(ex.ToString());
          Assert.Fail(String.Format("Expected {0}, got {1} saying: {2}", typeof(ExT), ex.GetType(), ex.Message));
        }

        if (!exceptionVerifier((ExT)ex))
        {
          Console.WriteLine(ex.ToString());
          Assert.Fail(string.Format("Exception {0} failed verification. Got message: {1}", typeof(ExT), ex.Message));
        }

        Console.WriteLine("Got expected exception with message: {0}", ex.Message);
        return;
      }

      Assert.Fail(String.Format("Expected {0}, but no exception was thrown", typeof(ExT)));
    }


    public static void AssertThrowsWebException(Action blockThatThrowsException, HttpStatusCode expectedStatusCode)
    {
      AssertThrows<WebException>(
        blockThatThrowsException,
        ex => ((HttpWebResponse)ex.Response).StatusCode == expectedStatusCode);
    }


    protected string Serialize(object data, ObjectSerializerSettings settings = null)
    {
      ObjectSerializer serializer = new ObjectSerializer(data.GetType());
      ObjectToStringPropertyVisitor visitor = new ObjectToStringPropertyVisitor();
      serializer.Serialize(data, visitor, settings);
      return visitor.Result;
    }


    protected async Task VerifyIsAsync<T>(Func<Request,Task<Response<T>>> asyncBlock, Action<Response<T>> verifier)
      where T : class
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);

      // The (initiation of) the operation itself takes zero time - but is expected to work async in the background
      DateTime t1 = DateTime.Now;
      Task<Response<T>> task1 = asyncBlock(request);
      TimeSpan getTime = DateTime.Now - t1;

      // Wait for the request to finish on the server - the Delay(4) simulates work done in the mean time
      await Task.Delay(TimeSpan.FromSeconds(4));

      // Now await response - this should not take any time at this point as the request should have finished already
      DateTime t2 = DateTime.Now;
      using (var response = await task1)
      {
        TimeSpan responseTime = DateTime.Now - t2;
        TimeSpan totalTime = DateTime.Now - t1;

        Assert.Less(getTime.TotalMilliseconds, 1000);
        Assert.Less(responseTime.TotalMilliseconds, 1000);
        Assert.GreaterOrEqual(totalTime.TotalMilliseconds, 4000);

        if (verifier != null)
          verifier(response);
      }
    }


    protected void TestAsyncEvent(Action<AutoResetEvent> asyncBlock)
    {
      AutoResetEvent handle = new AutoResetEvent(false);

      asyncBlock(handle);

      // Wait for request to complete
      bool signalReceived = handle.WaitOne(TimeSpan.FromSeconds(10));

      Assert.IsTrue(signalReceived, "Timeout in async handler");
    }
  }
}
