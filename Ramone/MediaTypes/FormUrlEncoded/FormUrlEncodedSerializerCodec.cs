using System;
using System.IO;
using Ramone.Utility;


namespace Ramone.MediaTypes.FormUrlEncoded
{
  public class FormUrlEncodedSerializerCodec<TEntity> : IMediaTypeWriter
    where TEntity : class
  {
    FormUrlEncodingSerializer Serializer = new FormUrlEncodingSerializer(typeof(TEntity));


    public void WriteTo(WriterContext context)
    {
      if (context.Data == null)
        return;

      TEntity entity = context.Data as TEntity;
      if (entity == null)
        throw new InvalidOperationException(string.Format("Could not write {0} - expected it to be {1}.", context.Data.GetType(), typeof(TEntity)));

      using (TextWriter w = new StreamWriter(context.HttpStream))
      {

        Serializer.Serialize(w, entity);
      }
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}
