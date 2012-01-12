using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Ramone.Tests.Codecs
{
  public class TextCodec : IMediaTypeWriter, IMediaTypeReader
  {
    #region IMediaTypeWriter Members

    public void WriteTo(WriterContext context)
    {
      string text = context.Data as string;
      context.HttpStream.Write(Encoding.UTF8.GetBytes(text), 0, Encoding.UTF8.GetByteCount(text));
    }

    #endregion


    #region IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      string text = null;
      using (StreamReader r = new StreamReader(context.HttpStream, Encoding.UTF8))
      {
        text = r.ReadToEnd();
      }
      return text;
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}
