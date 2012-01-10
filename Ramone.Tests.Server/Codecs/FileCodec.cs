using OpenRasta.Web;
using System.IO;
using OpenRasta.IO;


namespace Ramone.Tests.Server.Codecs
{
  public class FileCodec : OpenRasta.Codecs.IMediaTypeWriter
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
    {
      IFile file = (IFile)entity;
    }

    #endregion

    #region ICodec Members

    public object Configuration { get; set; }

    #endregion
  }
}