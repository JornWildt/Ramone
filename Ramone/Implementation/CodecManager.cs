using System;
using System.Linq;
using System.Collections.Generic;


namespace Ramone.Implementation
{
  public class CodecManager : ICodecManager
  {
    protected IList<MediaTypeReaderRegistration> RegisteredReaders = new List<MediaTypeReaderRegistration>();

    protected IList<MediaTypeWriterRegistration> RegisteredWriters = new List<MediaTypeWriterRegistration>();


    #region ICodecManager Members

    public void AddCodec<TMediaType>(string mediaType, IMediaTypeCodec codec)
    {
      if (codec is IMediaTypeReader)
        AddReader<TMediaType>(mediaType, (IMediaTypeReader)codec);
      if (codec is IMediaTypeWriter)
        AddWriter<TMediaType>(mediaType, (IMediaTypeWriter)codec);
    }


    public IEnumerable<MediaTypeReaderRegistration> GetReaders(Type t)
    {
      return GetReaders(t, null);
    }


    public MediaTypeReaderRegistration GetReader(Type t, string mediaType)
    {
      MediaTypeReaderRegistration r = GetSingleReaderOrNull(t, GetReaders(t, mediaType));
      if (r == null)
        throw new ArgumentException(string.Format("Could not find a reader codec for '{0}' + {1}", mediaType, t));

      return r;
    }


    public MediaTypeWriterRegistration GetWriter(Type t)
    {
      return GetWriter(t, null);
    }


    public MediaTypeWriterRegistration GetWriter(Type t, string mediaType)
    {
      MediaTypeWriterRegistration w = GetSingleWriterOrNull(t, GetWriters(t, mediaType));
      if (w == null)
        throw new ArgumentException(string.Format("Could not find a writer codec for '{0}' + {1}", mediaType, t));

      return w;
    }

    #endregion


    protected virtual void AddReader<TMediaType>(string mediaType, IMediaTypeReader reader)
    {
      MediaTypeReaderRegistration r = GetSingleReaderOrNull(typeof(TMediaType), GetReaders(typeof(TMediaType), mediaType));
      if (r != null)
        throw new ArgumentException(string.Format("Could not add reader for media-type '{0}' since one already exists", mediaType));
      RegisteredReaders.Add(new MediaTypeReaderRegistration(mediaType, typeof(TMediaType), reader));
    }


    protected virtual void AddWriter<TMediaType>(string mediaType, IMediaTypeWriter writer)
    {
      MediaTypeWriterRegistration w = GetSingleWriterOrNull(typeof(TMediaType), GetWriters(typeof(TMediaType), mediaType));
      if (w != null)
        throw new ArgumentException(string.Format("Could not add writer of type {'0'} for media-type '{1}' since one already exists"));
      RegisteredWriters.Add(new MediaTypeWriterRegistration(mediaType, typeof(TMediaType), writer));
    }


    protected IEnumerable<MediaTypeReaderRegistration> GetReaders(Type t, string mediaType)
    {
      return from entry in RegisteredReaders
             where (entry.MediaType == mediaType || mediaType == null) && entry.ClrType == t
             select entry;
    }


    protected virtual MediaTypeReaderRegistration GetSingleReaderOrNull(Type t, IEnumerable<MediaTypeReaderRegistration> readers)
    {
      IList<MediaTypeReaderRegistration> readersList = readers.ToList();
      if (readersList.Count == 0)
        return null;
      if (readersList.Count > 1)
        throw new ArgumentException(string.Format("Got multiple reader codecs for type '{0}'. Try specifying a media type.", t));
      return readersList[0];
    }


    protected IEnumerable<MediaTypeWriterRegistration> GetWriters(Type t, string mediaType)
    {
      return from entry in RegisteredWriters
             where (entry.MediaType == mediaType || mediaType == null) && entry.ClrType == t
             select entry;
    }


    protected virtual MediaTypeWriterRegistration GetSingleWriterOrNull(Type t, IEnumerable<MediaTypeWriterRegistration> writers)
    {
      IList<MediaTypeWriterRegistration> writersList = writers.ToList();
      if (writersList.Count == 0)
        return null;
      if (writersList.Count > 1)
        throw new ArgumentException(string.Format("Got multiple writer codecs for type '{0}'. Try specifying a media type.", t));
      return writersList[0];
    }
  }
}
