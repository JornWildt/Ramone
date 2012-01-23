using System;
using OpenRasta.Codecs;
using OpenRasta.Web;
using System.Web;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("*/*")]
  public class AnyEchoCodec : OpenRasta.Codecs.IMediaTypeWriter, OpenRasta.Codecs.IMediaTypeReader
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      response.ContentType = new MediaType(HttpContext.Current.Request.ContentType);
      Ramone.Tests.Server.Configuration.AnyEcho e = (Ramone.Tests.Server.Configuration.AnyEcho)entity;
      e.S.CopyTo(response.Stream);
    }

    #endregion

    #region ICodec Members

    public object Configuration
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    #endregion

    #region IMediaTypeReader Members

    public object ReadFrom(OpenRasta.Web.IHttpEntity request, OpenRasta.TypeSystem.IType destinationType, string destinationName)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}