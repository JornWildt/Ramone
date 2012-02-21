using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;


namespace Ramone.Implementation
{
  public class CodecManager : ICodecManager
  {
    protected class CodecEntry
    {
      public MediaType MediaType { get; set; }
      public Type ClrType { get; set; }
      public Type CodecType { get; set; }

      public CodecEntry(MediaType mediaType, Type clrType, Type codecType)
      {
        MediaType = mediaType;
        ClrType = clrType;
        CodecType = codecType;
      }
    }


    protected IList<CodecEntry> RegisteredReaders = new List<CodecEntry>();

    protected IList<CodecEntry> RegisteredWriters = new List<CodecEntry>();


    #region ICodecManager Members


    public void AddCodec<TClrType, TCodec>(MediaType mediaType) where TCodec : IMediaTypeCodec
    {
      AddCodec<TClrType>(mediaType, typeof(TCodec));
    }


    public void AddCodec<TClrType>(MediaType mediaType, Type codecType)
    {
      AddCodec(typeof(TClrType), mediaType, codecType);
    }


    public void AddCodec(Type clrType, MediaType mediaType, Type codecType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(codecType, "codecType").IsNotNull();

      if (typeof(IMediaTypeReader).IsAssignableFrom(codecType))
        AddReader(clrType, mediaType, codecType);
      if (typeof(IMediaTypeWriter).IsAssignableFrom(codecType))
        AddWriter(clrType, mediaType, codecType);
    }

    
    public void AddCodec<TCodec>(MediaType mediaType) where TCodec : IMediaTypeCodec
    {
      AddCodec(null, mediaType, typeof(TCodec));
    }

    
    public void AddCodec(MediaType mediaType, Type codecType)
    {
      AddCodec(null, mediaType, codecType);
    }


    public IEnumerable<MediaTypeReaderRegistration> GetReaders(Type t)
    {
      Condition.Requires(t, "t").IsNotNull();

      return SelectReaders(t, MediaType.Wildcard).Select(e => new MediaTypeReaderRegistration(e.MediaType, e.ClrType, InstantiateReaderCodec(e.CodecType)));
    }


    public MediaTypeReaderRegistration GetReader(Type t, MediaType mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();

      CodecEntry entry = SelectReaders(t, mediaType).FirstOrDefault();
      if (entry == null)
        throw new ArgumentException(string.Format("Could not find a reader codec for '{0}' + {1}", mediaType, t));

      return new MediaTypeReaderRegistration(entry.MediaType, entry.ClrType, InstantiateReaderCodec(entry.CodecType));
    }


    public MediaTypeWriterRegistration GetWriter(Type t, MediaType mediaType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();

      CodecEntry entry = SelectWriters(t, mediaType).FirstOrDefault();
      if (entry == null)
        throw new ArgumentException(string.Format("Could not find a writer codec for '{0}' + {1}", mediaType, t));

      return new MediaTypeWriterRegistration(entry.MediaType, entry.ClrType, InstantiateWriterCodec(entry.CodecType));
    }

    #endregion


    #region Internals

    protected void AddReader(Type t, MediaType mediaType, Type readerType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(readerType, "readerType").IsNotNull();

      CodecEntry entry = SelectExactMatchingReaders(t, mediaType).FirstOrDefault();
      if (entry != null)
        throw new ArgumentException(string.Format("Could not add reader for media-type {0} since one already exists (got {1} for '{2}').", mediaType, entry.ClrType, entry.MediaType));
      RegisteredReaders.Add(new CodecEntry(mediaType, t, readerType));
    }


    protected virtual void AddWriter(Type t, MediaType mediaType, Type writerType)
    {
      Condition.Requires(mediaType, "mediaType").IsNotNull();
      Condition.Requires(writerType, "writerType").IsNotNull();

      CodecEntry entry = SelectExactMatchingWriters(t, mediaType).FirstOrDefault();
      if (entry != null)
        throw new ArgumentException(string.Format("Could not add writer of type {0} for media-type '{1}' since one already exists (got {2} for '{3}').", t, mediaType, entry.ClrType, entry.MediaType));
      RegisteredWriters.Add(new CodecEntry(mediaType, t, writerType));
    }


    protected IEnumerable<CodecEntry> SelectExactMatchingWriters(Type t, MediaType mediaType)
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
    protected IEnumerable<CodecEntry> SelectWriters(Type t, MediaType mediaType)
    {
      var exactMatch = SelectExactMatchingWriters(t, mediaType);

      var anyMediaTypeMatch = RegisteredWriters.Where(w => w.MediaType.Matches(mediaType) && w.ClrType != null && w.ClrType.IsAssignableFrom(t));

      var anyClrTypeMatch = RegisteredWriters.Where(w => w.MediaType == mediaType && w.ClrType == null);

      // Return Union since ordering is important (hopefully Union respects that ...)
      return exactMatch.Union(anyMediaTypeMatch).Union(anyClrTypeMatch);
    }


    protected IEnumerable<CodecEntry> SelectExactMatchingReaders(Type t, MediaType mediaType)
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
    protected IEnumerable<CodecEntry> SelectReaders(Type t, MediaType mediaType)
    {
      var exactMatch = SelectExactMatchingReaders(t, mediaType);

      var anyMediaTypeMatch = RegisteredReaders.Where(r => r.MediaType.Matches(mediaType) && r.ClrType != null && r.ClrType.IsAssignableFrom(t));

      var anyClrTypeMatch = RegisteredReaders.Where(r => r.MediaType == mediaType && r.ClrType == null);

      // Return Union since ordering is important (hopefully Union respects that ...)
      return exactMatch.Union(anyMediaTypeMatch).Union(anyClrTypeMatch);
    }


    protected IMediaTypeReader InstantiateReaderCodec(Type codecType)
    {
      return (IMediaTypeReader)Activator.CreateInstance(codecType);
    }


    protected IMediaTypeWriter InstantiateWriterCodec(Type codecType)
    {
      return (IMediaTypeWriter)Activator.CreateInstance(codecType);
    }

    #endregion
  }
}
