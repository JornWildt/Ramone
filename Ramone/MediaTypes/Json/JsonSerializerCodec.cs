using System;
using System.IO;
using JsonFx.Json;


namespace Ramone.MediaTypes.Json
{
  public class JsonSerializerCodec<TEntity> : TextCodecBase<TEntity>
    where TEntity : class
  {
    protected JsonReader Reader { get; set; }

    protected JsonWriter Writer { get; set; }


    public JsonSerializerCodec()
    {
      Reader = CreateReader();
      Writer = CreateWriter();
    }


    protected override TEntity ReadFrom(TextReader reader)
    {
      return Reader.Read<TEntity>(reader);
    }


    protected override void WriteTo(TEntity item, TextWriter writer)
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
