using System;
using System.IO;
using JsonFx.Json;
using OpenRasta.Web;
using JsonFx.Serialization;


namespace Ramone.Tests.Server.Codecs
{
  public class JsonSerializerCodec<TEntity> : OpenRasta.Codecs.IMediaTypeWriter, OpenRasta.Codecs.IMediaTypeReader
    where TEntity : class
  {
    public object Configuration
    {
      get { return null; }
      set { }
    }


    public virtual void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
    {
      var item = entity as TEntity;
      if (item == null)
        throw new ArgumentException("Entity was not a " + typeof(TEntity).Name, "entity");

      using (var writer = new StreamWriter(response.Stream))
      {
        JsonWriter jsw = new JsonWriter();
        jsw.Write(item, writer);
      }
    }


    public virtual object ReadFrom(IHttpEntity request, OpenRasta.TypeSystem.IType destinationType, string destinationName)
    {
      using (var reader = new StreamReader(request.Stream))
      {
        JsonReader jsr = new JsonReader();
        return jsr.Read(reader, typeof(TEntity));
      }
    }
  }
}