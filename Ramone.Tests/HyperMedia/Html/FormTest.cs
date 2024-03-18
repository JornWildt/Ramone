using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Html;
using Ramone.Tests.Common;


namespace Ramone.Tests.HyperMedia.Html
{
  [TestFixture]
  public class FormTest : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      TestService.CodecManager.AddFormUrlEncoded<FormArgs>();
    }


    [Test]
    public void WhenSubmittingFormItIncludesDefaultValues_Keyed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Act
      IKeyValueForm form = GetForm(encType: encType);
      form.Value("Unused", "---");
      using (var r = form.Bind().Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.InputText, Is.EqualTo("text"));
        Assert.That(result.InputPassword, Is.EqualTo("password"));
        Assert.That(result.InputCheckbox, Is.EqualTo("checkbox"));
        Assert.That(result.InputHidden, Is.EqualTo("hidden"));
        Assert.That(result.TextArea, Is.EqualTo("textarea"));
        Assert.That(result.Select, Is.EqualTo("2"));
        Assert.That(result.Radio1, Is.EqualTo("1b"));
        Assert.IsNull(result.Radio2);
        //Assert.AreEqual("B,C", result.MultiSelectValue);
        Assert.That(result.EncType, Is.EqualTo(encType));
      }
    }


    [Test]
    public void WhenSubmittingFormItOverridesDefaultValues_Keyed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Act
      IKeyValueForm form = GetForm(encType: encType);
      form.Value("InputText", "abc");
      form.Value("InputPassword", "1234");
      form.Value("InputCheckbox", "not");
      form.Value("TextArea", "qwe");
      form.Value("Select", "1");
      form.Value("Radio1", "1a");
      form.Value("Radio2", "2b");
      using (var r = form.Bind().Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.InputText, Is.EqualTo("abc"));
        Assert.That(result.InputPassword, Is.EqualTo("1234"));
        Assert.That(result.InputCheckbox, Is.EqualTo("not"));
        Assert.That(result.InputHidden, Is.EqualTo("hidden"));
        Assert.That(result.TextArea, Is.EqualTo("qwe"));
        Assert.That(result.Select, Is.EqualTo("1"));
        Assert.That(result.Radio1, Is.EqualTo("1a"));
        Assert.That(result.Radio2, Is.EqualTo("2b"));
        //Assert.AreEqual("A,D", result.MultiSelectValue);
        Assert.That(result.EncType, Is.EqualTo(encType));
      }
    }


    [Test]
    public void WhenSubmittingFormItIncludesDefaultValues_Typed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Arrange
      FormArgs args = new FormArgs();

      //Session.SerializerSettings.

      // Act
      IKeyValueForm form = GetForm(encType: encType);
      using (var r = form.Value(args).Bind().Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.InputText, Is.EqualTo("text"));
        Assert.That(result.InputPassword, Is.EqualTo("password"));
        Assert.That(result.InputCheckbox, Is.EqualTo("checkbox"));
        Assert.That(result.InputHidden, Is.EqualTo("hidden"));
        Assert.That(result.TextArea, Is.EqualTo("textarea"));
        Assert.That(result.Select, Is.EqualTo("2"));
        Assert.That(result.Radio1, Is.EqualTo("1b"));
        Assert.IsNull(result.Radio2);
        //Assert.AreEqual("B,C", result.MultiSelectValue);
        Assert.That(result.EncType, Is.EqualTo(encType));
      }
    }


    [Test]
    public void WhenSubmittingFormItOverridesDefaultValues_Typed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "abc",
        InputPassword = "1234",
        InputCheckbox = "not",
        TextArea = "qwe",
        Select = "3",
        Radio1 = "1a",
        Radio2 = "2b"
        //MultiSelect = new List<string> { "A", "D" }
      };

      // Act
      IKeyValueForm form = GetForm(encType: encType);
      using (var r = form.Value(args).Bind().Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.InputText, Is.EqualTo("abc"));
        Assert.That(result.InputPassword, Is.EqualTo("1234"));
        Assert.That(result.InputCheckbox, Is.EqualTo("not"));
        Assert.That(result.InputHidden, Is.EqualTo("hidden"));
        Assert.That(result.TextArea, Is.EqualTo("qwe"));
        Assert.That(result.Select, Is.EqualTo("3"));
        Assert.That(result.Radio1, Is.EqualTo("1a"));
        Assert.That(result.Radio2, Is.EqualTo("2b"));
        //Assert.AreEqual("A,D", result.MultiSelectValue);
        Assert.That(result.EncType, Is.EqualTo(encType));
      }
    }


    [Test]
    public void WhenSubmittingWithoutNameItIncludesValuesFromFirstSubmitButton()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      using (var r = form.Value(args).Bind().Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Save, Is.EqualTo("Save"));
        Assert.IsNull(result.Cancel);
        Assert.IsNull(result.Help);
      }
    }


    [Test]
    public void WhenSubmittingByNameItOnlyIncludesValuesFromTheSubmitButtonUsed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      using (var r = form.Value(args).Bind("Cancel").Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.Save);
        Assert.That(result.Cancel, Is.EqualTo("Cancel"));
        Assert.IsNull(result.Help);
      }
    }


    [Test]
    public void WhenSubmittingByIdItOnlyIncludesValuesFromTheSubmitButtonUsed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      using (var r = form.Value(args).Bind("#help-button").Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.Save);
        Assert.IsNull(result.Cancel);
        Assert.That(result.Help, Is.EqualTo("Help"));
      }
    }


    [Test]
    public void CanSubmitToRelativeActionUrl()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm("relative");
      using (var r = form.Value(args).Bind("Cancel").Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Cancel, Is.EqualTo("Cancel"));
      }
    }


    [Test]
    public void WhenNoActionUrlIsSetItSubmitsToCurrentUrl()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm("empty");
      using (var r = form.Value(args).Bind("Cancel").Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Cancel, Is.EqualTo("Cancel"));
      }
    }


    [Test]
    public void ItEitherUsesAcceptCharsetFromFormOrDefaultCharsetWhenSubmitting_Typed(
      [Values("iso-8859-1", "utf-8", "unused")] string charset)
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "ÆØÅüì"
      };

      // Act
      IKeyValueForm form = GetForm(charset: charset);
      Request submitRequest = form.Value(args).Bind();
      using (var r = submitRequest.Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        if (charset == "unused")
          charset = Session.DefaultEncoding.WebName;
        Assert.That(submitRequest.CodecParameter("Charset"), Is.EqualTo(charset));
        Assert.IsNotNull(result);
        Assert.That(result.InputText, Is.EqualTo("ÆØÅüì"));
      }
    }


    [Test]
    public void CanSubmitWithGetMethod()
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "This is a GET",
        Select = "Select XXX"
      };

      // Act
      IKeyValueForm form = GetForm("relative", encType: "urlencoded", method: "GET");
      using (var r = form.Value(args).Bind("Cancel").Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Method, Is.EqualTo("GET"));
        Assert.That(result.InputText, Is.EqualTo(args.InputText));
        Assert.That(result.Select, Is.EqualTo(args.Select));
      }
    }


    [Test]
    public void CanSubmitWithPostMethod()
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "This is a POST",
        Select = "Select XXX"
      };


      // Act
      IKeyValueForm form = GetForm("relative", method: "POST");
      using (var r = form.Value(args).Bind("Cancel").Submit<FormArgs>())
      {
        FormArgs result = r.Body;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Method, Is.EqualTo("POST"));
        Assert.That(result.InputText, Is.EqualTo(args.InputText));
        Assert.That(result.Select, Is.EqualTo(args.Select));
      }
    }


    IKeyValueForm GetForm(string actionUrlMode = "absolute", string encType = "multipart", string charset = "iso-8859-1", string method = "POST")
    {
      // Pass charset to form creator such that it can insert "accept-charset" in the form.
      Request formRequest = Session.Bind(FormTemplate, new { actionUrlMode = actionUrlMode, encType = encType, charset = charset, method = method, InputText = "", Select = "" });
      using (Response<HtmlDocument> response = formRequest.Get<HtmlDocument>())
      {
        IKeyValueForm form = response.Body.DocumentNode.SelectNodes(@"//form").First().Form(response);
        return form;
      }
    }
  }
}
