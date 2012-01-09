using System;
using System.IO;
using System.Web;
using System.Net;


namespace Ramone
{
  public class ReaderContext : MediaTypeContext
  {
    public HttpWebResponse Response { get; protected set; }

    public ReaderContext(Stream s, Type t, HttpWebResponse response)
    {
      HttpStream = s;
      DataType = t;
      Response = response;
    }
  }


  public interface IMediaTypeReader : IMediaTypeCodec
  {
    object ReadFrom(ReaderContext context);
  }
}
