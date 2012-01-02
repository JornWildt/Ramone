using OpenRasta.Codecs;
using OpenRasta.Web;
using System;
using System.IO;


namespace Ramone.Tests.Server.Codecs
{
  public class TextCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
    {
      string text = entity as string;
      if (text == null)
        throw new ArgumentException("Entity was not a string", "entity");

      using (var writer = new StreamWriter(response.Stream))
      {
        writer.Write(text);
      }
    }

    #endregion


    #region ICodec Members

    public object Configuration
    {
      get;
      set;
    }

    #endregion
  }
}
