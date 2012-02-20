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
  public abstract class XmlStreamCodecBase : IMediaTypeWriter, IMediaTypeReader
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Response.ContentType, context.Session.DefaultEncoding);

      using (var textReader = new StreamReader(context.HttpStream, enc))
      using (var reader = XmlReader.Create(textReader))
      {
        return ReadFrom(reader, context);
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(WriterContext context)
    {
      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Request.ContentType, context.Session.DefaultEncoding);

      using (var writer = new XmlTextWriter(context.HttpStream, enc))
      {
        WriteTo(context.Data, writer, context);
      }
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion


    protected abstract object ReadFrom(XmlReader reader, ReaderContext context);

    protected abstract void WriteTo(object item, XmlWriter writer, WriterContext context);
  }
}
