using System;
using System.IO;


namespace Ramone
{
  public interface IMediaTypeReader : IMediaTypeCodec
  {
    object ReadFrom(Stream s, Type t);
  }
}
