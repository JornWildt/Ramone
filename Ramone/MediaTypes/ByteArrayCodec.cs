using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ramone.MediaTypes
{
  public class ByteArrayCodec : IMediaTypeWriter, IMediaTypeReader
  {
    #region IMediaTypeWriter Members

    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;
      if (!(context.Data is byte[]))
        throw new ArgumentException(string.Format("Expected byte[] in ByteArrayCodec. Got {0}.", context.Data.GetType()));
      using (Stream input = new MemoryStream((byte[])context.Data))
        input.CopyTo(context.HttpStream);
    }

    #endregion


    #region IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      using (MemoryStream input = new MemoryStream())
      {
        context.HttpStream.CopyTo(input);
        return input.ToArray();
      }
    }

    #endregion

    #region IMediaTypeCodec Members

    public object CodecArgument { get; set; }

    #endregion
  }
}
