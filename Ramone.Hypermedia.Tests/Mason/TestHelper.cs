namespace Ramone.Hypermedia.Tests.Mason
{
  public class TestHelper : Ramone.Hypermedia.Tests.TestHelper
  {
    protected const string IssueTrackerIndexUrl = "http://jorn-pc/mason-demo/resource-common";


    private Resource _commonResource;
    protected Resource GetCommonResource()
    {
      if (_commonResource == null)
      {
        Request req = Session.Request(IssueTrackerIndexUrl);
        using (var resp = req.Get<Resource>())
        {
          _commonResource = resp.Body;
        }
      }
      return _commonResource;
    }
  }
}
