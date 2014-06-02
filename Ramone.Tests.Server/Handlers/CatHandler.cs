using Ramone.Tests.Common;
using OpenRasta.Web;
using System;
using Ramone.MediaTypes.Atom;
using System.Collections.Generic;


namespace Ramone.Tests.Server.Handlers
{
  public class CatHandler
  {
    public object Get()
    {
      Uri parentUrl = typeof(Cat).CreateUri(new { name = "Felix" });
      return new Cat
      {
        Name = "MIAUW",
        Parent = new AtomLink(parentUrl.AbsoluteUri, "up", MediaType.ApplicationJson, "Parent cat")
      };
    }


    public Cat Get(string name)
    {
      Uri parentUrl = typeof(Cat).CreateUri(new { name = "Felix" });
      return new Cat 
      { 
        Name = name,
        Parent = new AtomLink(parentUrl.AbsoluteUri, "up", MediaType.ApplicationJson, "Parent cat")
      };
    }


    public OperationResult Post(Cat c)
    {
      return new OperationResult.Created
      {
        ResponseResource = c,
        RedirectLocation = typeof(Cat).CreateUri(new { name = c.Name })
      };
    }
  }
}