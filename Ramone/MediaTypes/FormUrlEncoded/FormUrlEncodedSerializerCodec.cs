using System;
using System.IO;
using Ramone.Utility;


namespace Ramone.MediaTypes.FormUrlEncoded
{
  public class FormUrlEncodedSerializerCodec<TEntity> : IMediaTypeWriter
    where TEntity : class
  {
    FormUrlEncodingSerializer Serializer = new FormUrlEncodingSerializer(typeof(TEntity));


    public void WriteTo(Stream s, Type t, object data)
    {
      if (data == null)
        return;

      TEntity entity = data as TEntity;
      if (entity == null)
        throw new InvalidOperationException(string.Format("Could not write {0} - expected it to be {1}.", data.GetType(), typeof(TEntity)));

      using (TextWriter w = new StreamWriter(s))
      {

        Serializer.Serialize(w, entity);
      }
    }


    #region IMediaTypeCodec

    public object CodecArgument { get; set; }

    #endregion
  }
}
