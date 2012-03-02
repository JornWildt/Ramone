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
    public void WhenSubmittingFormItIncludesDefaultValues()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("text", result.InputText);
    }


    // Test with 
    //   action[empty|relative|absolut]
    //   encoding
    //   different submit buttons

    // WHat happens for forms with errors (repeated inputs for instance)

    [Test]
    public void CanSubmitFormWithAllSortsOfSpecialities()
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "My title"
      };

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(args.InputText, result.InputText);
      Assert.AreEqual("Createx", result.Create);
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
