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
      codecManager.AddCodec<T>(MediaType.ApplicationFormUrlEncoded, new FormUrlEncodedSerializerCodec());
    }


    public static void AddFormUrlEncoded<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new FormUrlEncodedSerializerCodec());
    }


    public static void AddMultipartFormData<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>(MediaType.MultipartFormData, new MultipartFormDataSerializerCodec());
    }


    public static void AddMultipartFormData<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new MultipartFormDataSerializerCodec());
    }


    public static void AddXml<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>(MediaType.ApplicationXml, new XmlSerializerCodec());
    }


    public static void AddXml<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new XmlSerializerCodec());
    }


    public static void AddJson<T>(this ICodecManager codecManager)
    {
      codecManager.AddCodec<T>(MediaType.ApplicationJson, new JsonSerializerCodec());
    }


    public static void AddJson<T>(this ICodecManager codecManager, MediaType mediaType)
    {
      codecManager.AddCodec<T>(mediaType, new JsonSerializerCodec());
    }
  }
}
