using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonFx.Json;
using System.IO;

namespace Ramone.MediaTypes.Json
{
  public class JsonSerializerCodec : TextCodecBase<object>
  {
    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      JsonReader jsr = new JsonReader();
      return jsr.Read(reader, context.DataType);
    }


    protected override void WriteTo(object item, TextWriter writer, WriterContext context)
    {
      if (item == null)
        throw new ArgumentNullException("item");

      JsonWriter jsw = new JsonWriter();
      jsw.Write(item, writer);
    }
  }
}


#if false

  public class JsonSerializerCodec : TextCodecBase<object>
  {
    protected Dictionary<Type, JsonReader> Readers { get; set; }

    protected Dictionary<Type, JsonWriter> Writers { get; set; }


    public JsonSerializerCodec()
    {
      Readers = new Dictionary<Type, JsonReader>();
      Writers = new Dictionary<Type, JsonWriter>();
    }


    protected override object ReadFrom(TextReader reader, ReaderContext context)
    {
      JsonReader jsr = GetReader(context.DataType);
      return jsr.Read(reader);
    }


    protected override void WriteTo(object item, TextWriter writer)
    {
      if (item == null)
        throw new ArgumentNullException("item");

      JsonWriter jsw = GetWriter(item.GetType());
      jsw.Write(item, writer);
    }


    protected JsonReader GetReader(Type t)
    {
      if (!Readers.ContainsKey(t))
      {
        Readers[t] = CreateReader(t);
      }
      return Readers[t];
    }


    protected JsonWriter GetWriter(Type t)
    {
      if (!Writers.ContainsKey(t))
      {
        Writers[t] = CreateWriter(t);
      }
      return Writers[t];
    }


    protected virtual JsonReader CreateReader(Type t)
    {
      return new JsonReader
    }


    protected virtual JsonWriter CreateWriter(Type t)
    {
      return new JsonWriter();
    }
  }


#endif