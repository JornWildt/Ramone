using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ramone
{
  public class MediaTypeContext
  {
    public Type DataType { get; protected set; }

    public Stream HttpStream { get; protected set; }
  }


  public interface IMediaTypeCodec
  {
  }
}
