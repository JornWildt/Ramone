using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ramone.MediaTypes
{
  public class StreamCodec : IMediaTypeWriter, IMediaTypeReader
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      return context.HttpStream;
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;
      if (!(context.Data is Stream))
        throw new ArgumentException(string.Format("Expected Stream in StreamCodec. Got {0}.", context.Data.GetType()));
      Stream input = context.Data as Stream;
      input.CopyTo(context.HttpStream);
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}
