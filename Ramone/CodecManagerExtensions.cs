using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.MediaTypes.Json;
using Ramone.MediaTypes.MultipartFormData;
using Ramone.MediaTypes.Xml;


namespace Ramone
{
  public static class CodecManagerExtensions
  {
    public static void AddFormUrlEncoded<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T, FormUrlEncodedSerializerCodec>(MediaType.ApplicationFormUrlEncoded);
    }


    public static void AddFormUrlEncoded<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T, FormUrlEncodedSerializerCodec>(mediaType);
    }


    public static void AddMultipartFormData<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T, MultipartFormDataSerializerCodec>(MediaType.MultipartFormData);
    }


    public static void AddMultipartFormData<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T, MultipartFormDataSerializerCodec>(mediaType);
    }


    public static void AddXml<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T, XmlSerializerCodec>(MediaType.ApplicationXml);
    }


    public static void AddXml<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T, XmlSerializerCodec>(mediaType);
    }


    public static void AddJson<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T, JsonSerializerCodec>(MediaType.ApplicationJson);
    }


    public static void AddJson<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T, JsonSerializerCodec>(mediaType);
    }
  }
}
