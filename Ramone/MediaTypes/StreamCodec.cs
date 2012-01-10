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

    public void WriteTo(Stream s, Type t, object data)
    {
    }

    #endregion
  }
}
