using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common;
using System;
using System.Threading.Tasks;

namespace Ramone.Tests
{
  [TestFixture]
  public class PostTests_Async : TestHelper
  {
    Dossier MyDossier = new Dossier
    {
      Title = "A new dossier"
    };

    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 });
    }


    [Test]
    public async Task WhenPostingEmptyDataAsyncTheRequestIsInFactAsync()
    {
      await VerifyIsAsync(
        async req => await req.Async().Post<SlowResource>(), 
        resp =>
        {
          Assert.That(resp.Body, Is.Not.Null);
          Assert.That(resp.Body.Time, Is.EqualTo(4));
        });
    }


    [Test]
    public async Task WhenPostingDataAsyncTheRequestIsInFactAsync()
    {
      await VerifyIsAsync(
        async req => await req.AsJson().Async().Post<SlowResource>(new SlowResource { Time = 10 }),
        resp =>
        {
          Assert.That(resp.Body, Is.Not.Null);
          Assert.That(resp.Body.Time, Is.EqualTo(10));
        });
    }
  }
}