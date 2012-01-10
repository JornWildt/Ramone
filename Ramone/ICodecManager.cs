using System;
using System.Collections.Generic;


namespace Ramone
{
  public interface ICodecManager
  {
    void AddCodec<TMediaType>(string mediaType, IMediaTypeCodec codec);

    /// <summary>
    /// Add codec for any media type
    /// </summary>
    /// <param name="codec"></param>
    void AddCodec<TMediaType>(IMediaTypeCodec codec);

    /// <summary>
    /// Add codec for any CLR type
    /// </summary>
    /// <param name="mediaType"></param>
    /// <param name="codec"></param>
    void AddCodec(string mediaType, IMediaTypeCodec codec);

    MediaTypeWriterRegistration GetWriter(Type t);
    MediaTypeWriterRegistration GetWriter(Type t, string mediaType);

    IEnumerable<MediaTypeReaderRegistration> GetReaders(Type t);
    MediaTypeReaderRegistration GetReader(Type t, string mediaType);
  }


  public class CodecRegistration<T>
  {
    public string MediaType { get; set; }
    public Type ClrType { get; set; }
    public T Codec { get; set; }

    public CodecRegistration(string mediaType, Type clrType, T codec)
    {
      MediaType = mediaType;
      ClrType = clrType;
      Codec = codec;
    }
  }


  public class MediaTypeReaderRegistration : CodecRegistration<IMediaTypeReader>
  {
    public MediaTypeReaderRegistration(string mediaType, Type clrType, IMediaTypeReader codec)
      : base(mediaType, clrType, codec)
    {
    }
  }


  public class MediaTypeWriterRegistration : CodecRegistration<IMediaTypeWriter>
  {
    public MediaTypeWriterRegistration(string mediaType, Type clrType, IMediaTypeWriter codec)
      : base(mediaType, clrType, codec)
    {
    }
  }
}
