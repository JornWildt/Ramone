using System;
using System.IO;
using System.Xml;
using Ramone;
using System.Text;
using Ramone.Utility;


namespace Ramone.MediaTypes
{
  /// <summary>
  /// Base for a type-checked codec which will only serialize objects of type TEntity
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  public abstract class XmlCodecBase<TEntity> 
    : IMediaTypeWriter, IMediaTypeReader where TEntity : class      
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      Encoding enc = Encoding.Default;

      MediaType m = MediaTypeParser.ParseMediaType(context.Response.ContentType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      using (var textReader = new StreamReader(context.HttpStream, enc))
      using (var reader = XmlReader.Create(textReader))
      {
        return ReadFrom(reader);
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(WriterContext context)
    {
      Encoding enc = Encoding.Default;

      MediaType m = MediaTypeParser.ParseMediaType(context.Request.ContentType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      using (var writer = new XmlTextWriter(context.HttpStream, enc))
      {
        WriteTo(context.Data as TEntity, writer);
      }
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion


    protected abstract TEntity ReadFrom(XmlReader reader);

    protected abstract void WriteTo(TEntity item, XmlWriter writer);
  }
}
