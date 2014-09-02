using System;
using System.Net;
namespace Ramone.Hypermedia.Tests.Mason
{
  public class TestHelper : Ramone.Hypermedia.Tests.TestHelper
  {
    public const string IssueTrackerIndexUrl = "http://jorn-pc/mason-demo/resource-common";


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


    protected Resource CreateProject(string code, string title, string description)
    {
      Resource common = GetCommonResource();
      var args = new { Code = code, Title = title, Description = description };
      using (var resp = common.Controls[MasonTestConstants.Rels.ProjectCreate].Invoke<Resource>(Session, args))
      {
        return resp.Created();
      }
    }


    private Resource _sharedProject;

    protected Resource GetSharedProject()
    {
      if (_sharedProject == null)
      {
        try
        {
          _sharedProject = GetProject("SHA");
        }
        catch (WebException)
        {
        }
        if (_sharedProject == null)
          _sharedProject = CreateProject("SHA", "Shared", "Shared project");
      }
      return _sharedProject;
    }


    protected Resource GetProject(string code)
    {
      Resource common = GetCommonResource();
      using (var rps = common.Controls[MasonTestConstants.Rels.Projects].Follow<Resource>(Session))
      {
        dynamic projects = rps.Body;
        foreach (var project in projects.Projects)
        {
          if (project.Code == code)
          {
            using (var rp = ((IControl)project.Controls["self"]).Follow<Resource>(Session))
            {
              return (Resource)rp.Body;
            }
          }
        }
      }
      return null;
    }
  }
}
