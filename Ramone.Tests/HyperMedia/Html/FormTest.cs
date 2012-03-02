using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.HyperMedia.Html;
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
    public void WhenSubmittingFormItOnlySubmitsDataFromTheActivatedSubmitButton()
    {
      IKeyValueForm form = GetForm();
      form.Value("Title", "My title");

      FormArgs args = new FormArgs
      {
        Title = "My title"
      };

      Session.Bind(FormTemplate).Post(args);

      //form.Submit().Body;
    }


    IKeyValueForm GetForm()
    {
      RamoneRequest formRequest = Session.Bind(FormTemplate);

      // GET blog
      RamoneResponse<HtmlDocument> response = formRequest.Get<HtmlDocument>();

      IKeyValueForm form = response.Body.DocumentNode.SelectNodes(@"//form").First().Form(response);

      return form;
    }
  }
}
