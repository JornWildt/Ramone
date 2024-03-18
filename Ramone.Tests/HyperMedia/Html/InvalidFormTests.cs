using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Html;
using Ramone.Tests.Common;
using System.Collections.Generic;
using System;


namespace Ramone.Tests.HyperMedia.Html
{
  [TestFixture]
  public class InvalidFormTest : TestHelper
  {
    [Test]
    public void WhenNoNameIsSetForAnInputItIgnoresIt()
    {
      // Act
      Form form = GetFormNode(missingNamesForm).Form(Session, new Uri("http://dr.dk"), "utf-8");

      // Assert
      Assert.That(form.Values.Count, Is.EqualTo(0));
    }


    [Test]
    public void WhenFormHasMultipleInputsWithSameNameItSelectsOne()
    {
      // Act
      Form form = GetFormNode(multipleNamesForm).Form(Session, new Uri("http://dr.dk"), "utf-8");

      // Assert
      Assert.That(form.Values.Count, Is.EqualTo(3));
    }


    protected HtmlNode GetFormNode(string html)
    {
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(html);

      // Act
      return doc.DocumentNode.SelectNodes(@"//form").First();
    }


    const string missingNamesForm = @"
<html>
 <body>
   <form>
    <input value=""1""/>
    <textarea>xxx</textarea>
    <select>
     <option></option>
     <option value=""1""></option>
     <option value=""2"" selected=""selected""></option>
    </select>
   </form>
 </body>
</html>
";


    const string multipleNamesForm = @"
<html>
 <body>
   <form>
    <input name=""A"" value=""1""/>
    <input name=""A"" value=""2""/>
    <textarea name=""B"">xxx</textarea>
    <textarea name=""B"">xxx</textarea>
    <select name=""C"">
     <option value=""1"" selected=""selected""></option>
    </select>
    <select name=""C"">
     <option value=""1"" selected=""selected""></option>
    </select>
   </form>
 </body>
</html>
";
  }
}
