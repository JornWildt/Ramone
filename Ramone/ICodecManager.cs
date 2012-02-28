using System;
using System.Collections.Generic;


namespace Ramone
{
  public interface ICodecManager
  {
    void AddCodec<TClrType, TCodec>(MediaType mediaType) where TCodec : IMediaTypeCodec;
    void AddCodec<TClrType>(MediaType mediaType, Type codecType);
    void AddCodec(Type clrType, MediaType mediaType, Type codecType);

    /// <summary>
    /// Add codec for any CLR type
    /// </summary>
    /// <param name="mediaType"></param>
    /// <param name="codec"></param>
    void AddCodec<TCodec>(MediaType mediaType) where TCodec : IMediaTypeCodec;
    void AddCodec(MediaType mediaType, Type codecType);


    MediaTypeWriterRegistration GetWriter(Type t, MediaType mediaType);

    IEnumerable<MediaTypeReaderRegistration> GetReaders(Type t);
    MediaTypeReaderRegistration GetReader(Type t, MediaType mediaType);
  }


  public class CodecRegistration<T>
  {
    public MediaType MediaType { get; set; }
    public Type ClrType { get; set; }
    public T Codec { get; set; }

    public CodecRegistration(MediaType mediaType, Type clrType, T codec)
    {
      MediaType = mediaType;
      ClrType = clrType;
      Codec = codec;
    }
  }


  public class MediaTypeReaderRegistration : CodecRegistration<IMediaTypeReader>
  {
    public MediaTypeReaderRegistration(MediaType mediaType, Type clrType, IMediaTypeReader codec)
      : base(mediaType, clrType, codec)
    {
    }
  }


  public class MediaTypeWriterRegistration : CodecRegistration<IMediaTypeWriter>
  {
    public MediaTypeWriterRegistration(MediaType mediaType, Type clrType, IMediaTypeWriter codec)
      : base(mediaType, clrType, codec)
    {
    }
  }
}
