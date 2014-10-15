using System;
using System.IO;
using Newtonsoft.Json;
using OpenRasta.Codecs;
using OpenRasta.Web;
using System.Dynamic;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/json")]
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
      using (JsonWriter jsw = new JsonTextWriter(writer))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(jsw, item);
      }
    }


    public virtual object ReadFrom(IHttpEntity request, OpenRasta.TypeSystem.IType destinationType, string destinationName)
    {
      using (var reader = new StreamReader(request.Stream))
      using (JsonReader jsr = new JsonTextReader(reader))
      {
        JsonSerializer serializer = new JsonSerializer();

        // Avoid JSON.NET wrapping result in JToken wrapper - return ExpandoObject for "object"
        Type t = (destinationType.StaticType == typeof(object) ? typeof(ExpandoObject) : destinationType.StaticType);

        return serializer.Deserialize(jsr, t);
      }
    }
  }
}