using System;
using System.IO;
using System.Text;
using Ramone.Tests.Common;


namespace Ramone.Tests.Codecs
{
  public class CatAsTextCodec : IMediaTypeReader, IMediaTypeWriter
  {
    #region IMediaTypeReader Members

    public object ReadFrom(Stream s, Type t)
    {
      string text = null;
      using (StreamReader r = new StreamReader(s, Encoding.UTF8))
      {
        text = r.ReadToEnd();
      }
      return new Cat { Name = text };
    }

    #endregion


    #region IMediaTypeWriter Members

    public void WriteTo(Stream s, Type t, object data)
    {
      Cat c = (Cat)data;
      using (StreamWriter w = new StreamWriter(s, Encoding.UTF8))
      {
        w.Write(c.Name);
      }
    }

    #endregion
  }
}
