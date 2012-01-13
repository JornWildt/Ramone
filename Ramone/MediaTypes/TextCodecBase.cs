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
      Encoding enc = Encoding.Default;

      MediaType m = MediaTypeParser.ParseMediaType(context.Response.ContentType);
      if (m.Parameters.ContainsKey("charset"))
        enc = Encoding.GetEncoding(m.Parameters["charset"]);

      using (var reader = new StreamReader(context.HttpStream, enc))
      {
        return ReadFrom(reader);
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(WriterContext context)
    {
      using (var writer = new StreamWriter(context.HttpStream))
      {
        WriteTo(context.Data as TEntity, writer);
      }
    }

    #endregion


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion

    protected abstract TEntity ReadFrom(TextReader reader);

    protected abstract void WriteTo(TEntity item, TextWriter writer);
  }
}
