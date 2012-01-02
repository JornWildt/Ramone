using System;
using System.IO;

namespace Ramone
{
  public interface IMediaTypeWriter : IMediaTypeCodec
  {
    void WriteTo(Stream s, Type t, object data);
  }
}
