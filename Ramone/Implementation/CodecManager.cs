using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;


namespace Ramone.Implementation
{
  public class CodecManager : ICodecManager
  {
    protected IList<MediaTypeReaderRegistration> RegisteredReaders = new List<MediaTypeReaderRegistration>();

    protected IList<MediaTypeWriterRegistration> RegisteredWriters = new List<MediaTypeWriterRegistration>();


    #region ICodecManager Members

    public void AddCodec<TMediaType>(MediaType mediaType, IMediaTypeCodec codec)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(codec, "codec").IsNotNull();

      if (codec is IMediaTypeReader)
        AddReader(typeof(TMediaType), mediaType, (IMediaTypeReader)codec);
      if (codec is IMediaTypeWriter)
        AddWriter(typeof(TMediaType), mediaType, (IMediaTypeWriter)codec);
    }


    public void AddCodec(MediaType mediaType, IMediaTypeCodec codec)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(codec, "codec").IsNotNull();

      if (codec is IMediaTypeReader)
        AddReader(null, mediaType, (IMediaTypeReader)codec);
      if (codec is IMediaTypeWriter)
        AddWriter(null, mediaType, (IMediaTypeWriter)codec);
    }


    public IEnumerable<MediaTypeReaderRegistration> GetReaders(Type t)
    {
      Condition.Requires(t, "t").IsNotNull();

      return SelectReaders(t, MediaType.Wildcard);
    }


    public MediaTypeReaderRegistration GetReader(Type t, MediaType mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();

      MediaTypeReaderRegistration reader = SelectReaders(t, mediaType).FirstOrDefault();
      if (reader == null)
        throw new ArgumentException(string.Format("Could not find a reader codec for '{0}' + {1}", mediaType, t));

      return reader;
    }


    public MediaTypeWriterRegistration GetWriter(Type t, MediaType mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();

      MediaTypeWriterRegistration writer = SelectWriters(t, mediaType).FirstOrDefault();
      if (writer == null)
        throw new ArgumentException(string.Format("Could not find a writer codec for '{0}' + {1}", mediaType, t));

      return writer;
    }

    #endregion


    protected virtual void AddReader(Type t, MediaType mediaType, IMediaTypeReader reader)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(reader, "reader").IsNotNull();

      MediaTypeReaderRegistration r = SelectExactMatchingReaders(t, mediaType).FirstOrDefault();
      if (r != null)
        throw new ArgumentException(string.Format("Could not add reader for media-type {0} since one already exists (got {1} for '{2}').", mediaType, r.ClrType, r.MediaType));
      RegisteredReaders.Add(new MediaTypeReaderRegistration(mediaType, t, reader));
    }


    protected virtual void AddWriter(Type t, MediaType mediaType, IMediaTypeWriter writer)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(writer, "writer").IsNotNull();

      MediaTypeWriterRegistration w = SelectExactMatchingWriters(t, mediaType).FirstOrDefault();
      if (w != null)
        throw new ArgumentException(string.Format("Could not add writer of type {0} for media-type '{1}' since one already exists (got {2} for '{3}').", t, mediaType, w.ClrType, w.MediaType));
      RegisteredWriters.Add(new MediaTypeWriterRegistration(mediaType, t, writer));
    }


    protected IEnumerable<MediaTypeWriterRegistration> SelectExactMatchingWriters(Type t, MediaType mediaType)
    {
      var exactMatch = RegisteredWriters.Where(r => r.MediaType == mediaType && r.ClrType == t);

      return exactMatch;
    }


    /// <summary>
    /// Find all writers matching a CLR type and a media type.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="mediaType"></param>
    /// <returns>Returns sorted sequence of matching writers (most relevant writers first)</returns>
    protected IEnumerable<MediaTypeWriterRegistration> SelectWriters(Type t, MediaType mediaType)
    {
      var exactMatch = SelectExactMatchingWriters(t, mediaType);

      var anyMediaTypeMatch = RegisteredWriters.Where(w => w.MediaType.Matches(mediaType) && w.ClrType != null && w.ClrType.IsAssignableFrom(t));

      var anyClrTypeMatch = RegisteredWriters.Where(w => w.MediaType == mediaType && w.ClrType == null);

      // Return Union since ordering is important (hopefully Union respects that ...)
      return exactMatch.Union(anyMediaTypeMatch).Union(anyClrTypeMatch);
    }


    protected IEnumerable<MediaTypeReaderRegistration> SelectExactMatchingReaders(Type t, MediaType mediaType)
    {
      var exactMatch = RegisteredReaders.Where(r => r.MediaType == mediaType && r.ClrType == t);

      return exactMatch;
    }



    /// <summary>
    /// Find all readers matching a CLR type and a media type.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="mediaType"></param>
    /// <returns>Returns sorted sequence of matching readers (most relevant readers first)</returns>
    protected IEnumerable<MediaTypeReaderRegistration> SelectReaders(Type t, MediaType mediaType)
    {
      var exactMatch = SelectExactMatchingReaders(t, mediaType);

      var anyMediaTypeMatch = RegisteredReaders.Where(r => r.MediaType.Matches(mediaType) && r.ClrType != null && r.ClrType.IsAssignableFrom(t));

      var anyClrTypeMatch = RegisteredReaders.Where(r => r.MediaType == mediaType && r.ClrType == null);

      // Return Union since ordering is important (hopefully Union respects that ...)
      return exactMatch.Union(anyMediaTypeMatch).Union(anyClrTypeMatch);
    }
  }
}
