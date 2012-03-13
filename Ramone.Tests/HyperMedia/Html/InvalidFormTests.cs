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
      // Arrange
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(invalidForm1);

      // Act
      Form form = doc.DocumentNode.SelectNodes(@"//form").First().Form(Session, new Uri("http://dr.dk"), "utf-8");

      // Assert
      Assert.AreEqual(0, form.Values.Count);
    }


    const string invalidForm1 = @"
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
  }
}
