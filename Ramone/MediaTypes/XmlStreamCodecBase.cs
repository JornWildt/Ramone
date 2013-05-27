using System;
using System.IO;
using System.Xml;
using Ramone;
using System.Text;
using Ramone.Utility;
using Ramone.MediaTypes.Xml;


namespace Ramone.MediaTypes
{
  /// <summary>
  /// Base for a type-checked codec which will only serialize objects of type TEntity
  /// </summary>
  public abstract class XmlStreamCodecBase : IMediaTypeWriter, IMediaTypeReader
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Response.ContentType, context.Session.DefaultEncoding);

      XmlReaderSettings settings = XmlConfiguration.XmlReaderSettings;
      if (settings == null)
      {
        settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Ignore;
      }

      using (var textReader = new StreamReader(context.HttpStream, enc))
      using (var reader = XmlReader.Create(textReader, settings))
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
