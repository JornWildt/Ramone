using System;
using System.IO;


namespace Ramone.MediaTypes.Json
{
  public class JsonSerializerCodec : IMediaTypeReader
  {
    #region IMediaTypeReader Members

    public object ReadFrom(Stream s, Type t)
    {
      return null;
    }

    #endregion
  }
}
