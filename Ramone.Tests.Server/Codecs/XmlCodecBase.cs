using System;
using System.Xml;
using OpenRasta.Codecs;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Codecs
{
  /// <summary>
  /// Base for a type-checked codec which will only serialize objects of type TEntity
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  public abstract class XmlCodecBase<TEntity> : OpenRasta.Codecs.IMediaTypeWriter, OpenRasta.Codecs.IMediaTypeReader
    where TEntity : class
  {
    public object Configuration
    {
      get { return null; }
      set { }
    }


    public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
    {
      var item = entity as TEntity;
      if (item == null)
        throw new ArgumentException("Entity was not a " + typeof(TEntity).Name, "entity");  
    
      using(var writer = XmlWriter.Create(response.Stream))
      {
        WriteTo(item, writer);
      }
    }


    public object ReadFrom(IHttpEntity request, OpenRasta.TypeSystem.IType destinationType, string destinationName)
    {
     using (var reader = XmlReader.Create(request.Stream))
     {
      return ReadFrom(reader);
     }
    }

    
    protected abstract void WriteTo(TEntity item, XmlWriter writer);

    protected abstract TEntity ReadFrom(XmlReader reader);
  }
}