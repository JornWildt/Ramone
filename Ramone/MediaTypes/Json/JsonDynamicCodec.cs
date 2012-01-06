using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonFx.Json;
using System.IO;

namespace Ramone.MediaTypes.Json
{
  public class JsonDynamicCodec : TextCodecBase<object>
  {
    protected JsonReader Reader { get; set; }

    protected JsonWriter Writer { get; set; }


    public JsonDynamicCodec()
    {
      Reader = CreateReader();
      Writer = CreateWriter();
    }


    protected override object ReadFrom(TextReader reader)
    {
      return Reader.Read(reader);
    }


    protected override void WriteTo(object item, TextWriter writer)
    {
      Writer.Write(item, writer);
    }


    protected virtual JsonReader CreateReader()
    {
      return new JsonReader();
    }


    protected virtual JsonWriter CreateWriter()
    {
      return new JsonWriter();
    }
  }
}
