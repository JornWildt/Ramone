using System;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.Html;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.Utility.ObjectSerialization;
using Template = Tavis.UriTemplates.UriTemplate;


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


    protected static readonly Template DossierTemplate = new Template(CMSConstants.DossierPath);

    protected static readonly Template VerifiedMethodDossierTemplate = new Template(CMSConstants.VerifiedMethodDossierPath);

    protected static readonly Template DossierDocumentsTemplate = new Template(CMSConstants.DossierDocumentsPath);

    protected static readonly Template DocumentTemplate = new Template(CMSConstants.DocumentPath);

    protected static readonly Template PartyTemplate = new Template(CMSConstants.PartyPath);
    
    protected static readonly Uri DossiersUrl = new Uri(BaseUrl, CMSConstants.DossiersPath);

    //protected static readonly Uri VerifiedMethodDossiersUrl = new Uri(BaseUrl, CMSConstants.VerifiedMethodDossiersPath);


    protected static readonly Template CatTemplate = new Template(Constants.CatPath);

    protected static readonly Template CatsTemplate = new Template(Constants.CatsPath);

    protected static readonly Template Dog1Template = new Template(Constants.DogPath + "?v=1");

    protected static readonly Template Dog2Template = new Template(Constants.DogPath + "?v=2");

    protected static readonly Template PersonTemplate = new Template(Constants.PersonPath);

    protected static readonly Template EncodingTemplate = new Template(Constants.EncodingPath);

    protected static readonly Template FileTemplate = new Template(Constants.FilePath);

    protected static readonly Template MultipartFormDataTemplate = new Template(Constants.MultipartFormDataPath);

    protected static readonly Template MultipartFormDataFileTemplate = new Template(Constants.MultipartFormDataFilePath);

    protected static readonly Template FormUrlEncodedTemplate = new Template(Constants.FormUrlEncodedPath);

    protected static readonly Template XmlEchoTemplate = new Template(Constants.XmlEchoPath);

    protected static readonly Template AnyEchoTemplate = new Template(Constants.AnyEchoPath);

    protected static readonly Template ComplexClassTemplate = new Template(Constants.ComplexClassPath);

    protected static readonly Template AtomFeedTemplate = new Template(Constants.AtomFeedPath);

    protected static readonly Template AtomItemTemplate = new Template(Constants.AtomItemPath);

    protected static readonly Uri HeaderListUrl = new Uri(BaseUrl, Constants.HeaderEchoPath);

    protected static readonly Uri BasicAuthUrl = new Uri(BaseUrl, Constants.BasicAuthPath);

    protected static readonly Template FormTemplate = new Template(Constants.FormPath);

    protected static readonly Template FileDownloadTemplate = new Template(Constants.FileDownloadPath);

    protected static readonly Template LinkHeaderTemplate = new Template(Constants.LinkHeaderPath);

    protected static readonly Template RedirectTemplate = new Template(Constants.RedirectPath);

    protected static readonly Template PatchTemplate = new Template(Constants.PatchPath);

    protected static readonly Template ApplicationErrorTemplate = new Template(Constants.ApplicationErrorPath);


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
      AtomInitializer.Initialize();
      HtmlInitializer.Initialize();

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
