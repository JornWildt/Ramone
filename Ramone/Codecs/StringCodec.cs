#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ramone.Codecs
{
  public class StringCodec : IMediaTypeWriter, IMediaTypeReader
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(Stream s, Type t)
    {
      using (var reader = new StreamReader(s))
      {
        return reader.ReadToEnd();
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(Stream s, Type t, object data)
    {
      using (var writer = XmlWriter.Create(s))
      {
        WriteTo(data as TEntity, writer);
      }
    }

    #endregion
  }
}
#endif
