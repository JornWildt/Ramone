using System;
using System.Xml;
using OpenRasta.Codecs;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/xml")]
  public class XmlEchoCodec : OpenRasta.Codecs.IMediaTypeWriter, OpenRasta.Codecs.IMediaTypeReader
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, OpenRasta.Web.IHttpEntity response, string[] codecParameters)
    {
      Ramone.Tests.Server.Configuration.XmlEcho e = (Ramone.Tests.Server.Configuration.XmlEcho)entity;
      using (XmlWriter w = XmlWriter.Create(response.Stream))
        e.Doc.WriteTo(w);
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