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
    public void WhenSubmittingFormItIncludesDefaultValues_Keyed()
    {
      // Act
      IKeyValueForm form = GetForm();
      form.Value("Unused", "---");
      FormArgs result = form.Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("text", result.InputText);
      Assert.AreEqual("password", result.InputPassword);
      Assert.AreEqual("checkbox", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
    }


    [Test]
    public void WhenSubmittingFormItOverridesDefaultValues_Keyed()
    {
      // Act
      IKeyValueForm form = GetForm();
      form.Value("InputText", "abc");
      form.Value("InputPassword", "1234");
      form.Value("InputCheckbox", "not");
      FormArgs result = form.Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("abc", result.InputText);
      Assert.AreEqual("1234", result.InputPassword);
      Assert.AreEqual("not", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
    }


    [Test]
    public void WhenSubmittingFormItIncludesDefaultValues_Typed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("text", result.InputText);
      Assert.AreEqual("password", result.InputPassword);
      Assert.AreEqual("checkbox", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
    }


    [Test]
    public void WhenSubmittingFormItOverridesDefaultValues_Typed()
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "abc",
        InputPassword = "1234",
        InputCheckbox = "not"
      };

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("abc", result.InputText);
      Assert.AreEqual("1234", result.InputPassword);
      Assert.AreEqual("not", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
    }


    // Test with 
    //   action[empty|relative|absolut]
    //   encoding
    //   different submit buttons

    // WHat happens for forms with errors (repeated inputs for instance)

    [Test]
    public void WhenSubmittingWithoutNameItIncludesValuesFromFirstSubmitButton()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("Save", result.Save);
      Assert.IsEmpty(result.Cancel);
      Assert.IsEmpty(result.Help);
    }


    [Test]
    public void WhenSubmittingByNameItOnlyIncludesValuesFromTheSubmitButtonUsed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>("Cancel").Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.IsEmpty(result.Save);
      Assert.AreEqual("Cancel", result.Cancel);
      Assert.IsEmpty(result.Help);
    }


    [Test]
    public void WhenSubmittingByIdItOnlyIncludesValuesFromTheSubmitButtonUsed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>("#help-button").Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.IsEmpty(result.Save);
      Assert.IsEmpty(result.Cancel);
      Assert.AreEqual("Help", result.Help);
    }


    IKeyValueForm GetForm()
    {
      RamoneRequest formRequest = Session.Bind(FormTemplate);
      RamoneResponse<HtmlDocument> response = formRequest.Get<HtmlDocument>();
      IKeyValueForm form = response.Body.DocumentNode.SelectNodes(@"//form").First().Form(response);
      return form;
    }
  }
}
