using System.IO;
using System.Text;
using Ramone.Utility;


namespace Ramone.MediaTypes
{
  /// <summary>
  /// Base for a type-checked codec which will only serialize objects of type TEntity
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  public abstract class TextCodecBase<TEntity>
    : IMediaTypeWriter, IMediaTypeReader where TEntity : class
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(ReaderContext context)
    {
      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Response.ContentType);

      using (var reader = new StreamReader(context.HttpStream, enc))
      {
        return ReadFrom(reader, context);
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(WriterContext context)
    {
      Encoding enc = MediaTypeParser.GetEncodingFromCharset(context.Request.ContentType);

      using (var writer = new StreamWriter(context.HttpStream, enc))
      {
        WriteTo(context.Data as TEntity, writer, context);
      }
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion

    protected abstract TEntity ReadFrom(TextReader reader, ReaderContext context);

    protected abstract void WriteTo(TEntity item, TextWriter writer, WriterContext context);
  }
}
