using System.IO;
using System.Text;
using OpenRasta.Codecs;
using OpenRasta.Web;
using Ramone.Tests.Common;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("text/plain")]
  public class CatAsTextCodec : OpenRasta.Codecs.IMediaTypeWriter, OpenRasta.Codecs.IMediaTypeReader
  {
    #region IMediaTypeWriter Members

    public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
    {
      Cat c = (Cat)entity;

      using (var writer = new StreamWriter(response.Stream))
      {
        writer.Write(c.Name);
      }
    }

    #endregion


    #region IMediaTypeReader Members

    public object ReadFrom(IHttpEntity request, OpenRasta.TypeSystem.IType destinationType, string destinationName)
    {
      string text = null;
      using (StreamReader r = new StreamReader(request.Stream, Encoding.UTF8))
      {
        text = r.ReadToEnd();
      }
      return new Cat { Name = text };
    }

    #endregion

    public object Configuration { get; set; }
  }
}