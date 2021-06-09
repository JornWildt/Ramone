using System;
using System.IO;
using System.Text;
using Ramone.Tests.Common;


namespace Ramone.Tests.Common.Codecs
{
  public class CatAsHtmlCodec : IMediaTypeReader, IMediaTypeWriter
  {
    #region IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      string text = null;
      using (StreamReader r = new StreamReader(context.HttpStream, Encoding.UTF8))
      {
        text = r.ReadToEnd();
      }
      return new Cat { Name = text };
    }

    #endregion

    #region IMediaTypeWriter Members

    public void WriteTo(WriterContext context)
    {
      Cat c = (Cat)context.Data;
      using (StreamWriter w = new StreamWriter(context.HttpStream, Encoding.UTF8))
      {
        // Absolute meaningless post data
        w.Write(string.Format("<html><body><p>{0}</p></body></html>", c.Name));
      }
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}
