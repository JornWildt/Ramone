﻿using System.Web;
using OpenRasta.Web;
using Ramone.MediaTypes.Atom;
using Ramone.Tests.Common.CMS;
using System;


namespace Ramone.Tests.Server.CMS.Handlers
{
  public class DossiersHandler
  {
    public ICommunicationContext Context { get; set; }


    [HttpOperation(ForUriName="Simple")]
    public Dossier Get(long id)
    {
      Party party = new PartyHandler().Get(19);

      return new Dossier
      {
        Id = id,
        Title = string.Format("Dossier no. {0}", id),
        Links = new AtomLinkList
        {
          new AtomLink(typeof(DossierDocumentList).CreateUri(new { id = id }), CMSConstants.DocumentsLinkRelType, CMSConstants.CMSMediaTypeId, "Documents"),
          //new AtomLink("documents", CMSConstants.DocumentsLinkRelType, CMSConstants.CMSMediaTypeId, "Documents"),
          new AtomLink(party.CreateUri(), CMSConstants.PartyLinkRelType, CMSConstants.CMSMediaTypeId, party.FullName)
        }
      };
    }


    [HttpOperation(ForUriName = "Verified")]
    public Dossier Get(string method, long id)
    {
      if (method != "GET")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was GET'.", method));
      return Get(id);
    }

    
    public OperationResult Post(Dossier dossier)
    {
      Dossier d = new Dossier
      {
        Id = 999,
        Title = dossier != null ? dossier.Title : null
      };

      if (dossier != null && dossier.Title == "Do not return body")
        d = null;

      return new OperationResult.Created
      {
        ResponseResource = d,
        RedirectLocation = typeof(Dossier).CreateUri("Simple", new { id = 999 })
      };
    }


    public OperationResult Post(string method, Dossier dossier)
    {
      if (method != "POST")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was POST'.", method));
      return Post(dossier);
    }


    public OperationResult Post(string method)
    {
      if (method != "POST")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was POST'.", method));
      return Post((Dossier)null);
    }
    
    
    public OperationResult Put(Dossier dossier)
    {
      return new OperationResult.Created
      {
        ResponseResource = dossier,
        RedirectLocation = dossier.CreateUri()
      };
    }


    public OperationResult Put(string method, Dossier dossier)
    {
      if (method != "PUT")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was PUT'.", method));
      return Put(dossier);
    }


    public OperationResult Put(string method)
    {
      if (method != "PUT")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was PUT'.", method));
      return Put((Dossier)null);
    }


    public object Patch(long id, string title = null)
    {
      return new Dossier
      {
        Id = id,
        Title = (title ?? "<null>") + ": ok"
      };
    }


    public object Patch(string method, long id, string title = null)
    {
      if (method != "PATCH")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was PATCH'.", method));
      return Patch(id, title);
    }


    public OperationResult Delete(long id)
    {
      return new OperationResult.OK("Deleted, yup!");
    }


    public OperationResult Delete(string method, long id)
    {
      if (method != "DELETE")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was DELETE'.", method));
      return Delete(id);
    }


    [HttpOperation(ForUriName = "Simple")]
    public object Head(long id)
    {
      HttpContext.Current.Response.Headers["X-ExtraHeader"] = "1";
      return null;
    }


    [HttpOperation(ForUriName = "Verified")]
    public object Head(string method, long id)
    {
      if (method != "HEAD")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was HEAD'.", method));
      return Head(id);
    }


    [HttpOperation(ForUriName = "Simple")]
    public object Options(long id)
    {
      HttpContext.Current.Response.Headers["X-ExtraHeader"] = "2";
      return "Yes";
   }


    [HttpOperation(ForUriName = "Verified")]
    public object Options(string method, long id)
    {
      if (method != "OPTIONS")
        throw new InvalidOperationException(string.Format("Unexpected method (should have been {0}, was OPTIONS'.", method));
      return Options(id);
    }
  }
}