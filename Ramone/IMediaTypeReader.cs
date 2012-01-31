using System;
using System.IO;
using System.Net;


namespace Ramone
{
  public class ReaderContext : MediaTypeContext
  {
    public Type DataType { get; protected set; }

    public HttpWebResponse Response { get; protected set; }

    public ReaderContext(Stream s, Type t, HttpWebResponse response, IRamoneSession session)
    {
      HttpStream = s;
      DataType = t;
      Response = response;
      Session = session;
    }
  }


  public interface IMediaTypeReader : IMediaTypeCodec
  {
    object ReadFrom(ReaderContext context);
  }
}
