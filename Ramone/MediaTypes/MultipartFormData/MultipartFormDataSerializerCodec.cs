using System;
using System.IO;
using Ramone.Utility;
using System.Text;


namespace Ramone.MediaTypes.MultipartFormData
{
  public class MultipartFormDataSerializerCodec<TEntity> : IMediaTypeWriter
    where TEntity : class
  {
    MultipartFormDataSerializer Serializer = new MultipartFormDataSerializer(typeof(TEntity));


    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;

      TEntity entity = context.Data as TEntity;
      if (entity == null)
        throw new InvalidOperationException(string.Format("Could not write {0} - expected it to be {1}.", context.Data.GetType(), typeof(TEntity)));

      // FIXME: parameterize somewhere
      Encoding enc = Encoding.UTF8;

      Serializer.Serialize(context.HttpStream, entity, enc, CodecArgument as string);
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}
