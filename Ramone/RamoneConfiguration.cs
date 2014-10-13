using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using HtmlAgilityPack;
using Ramone.Implementation;
using Ramone.MediaTypes;
using Ramone.MediaTypes.Atom;
using Ramone.MediaTypes.FormUrlEncoded;
using Ramone.MediaTypes.Html;
using Ramone.MediaTypes.Json;
using Ramone.MediaTypes.MultipartFormData;
using Ramone.MediaTypes.OpenSearch;
using Ramone.MediaTypes.Xml;
using Ramone.Utility.ObjectSerialization;
using Ramone.Utility.ObjectSerialization.Formaters;
using Ramone.MediaTypes.JsonPatch;


namespace Ramone
{
  public static class RamoneConfiguration
  {
    public static bool UseStandardCodecs { get; set; }

    public static string UserAgent { get; set; }

    public static Encoding DefaultEncoding { get; set; }

    public static ObjectSerializerSettings SerializerSettings { get; set; }


    static RamoneConfiguration()
    {
      Reset();
      HtmlNode.ElementsFlags.Remove("form");
      HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);
    }


    public static void Reset()
    {
      UseStandardCodecs = true;
      UserAgent = "Ramone/1.0";
      DefaultEncoding = Encoding.UTF8;
      SerializerSettings = new ObjectSerializerSettings();
    }


    public static IService NewService()
    {
      return NewService(null);
    }


    public static IService NewService(Uri baseUrl)
    {
      IService service = new RamoneService(baseUrl)
      {
        UserAgent = UserAgent,
        DefaultEncoding = DefaultEncoding,
        SerializerSettings = new ObjectSerializerSettings(SerializerSettings)
      };
      if (UseStandardCodecs)
        RamoneConfiguration.RegisterStandardCodecs(service.CodecManager);
      return service;
    }


    public static ISession NewSession()
    {
      return NewSession(null);
    }


    /// <summary>
    /// Create a new session with implicit service.
    /// </summary>
    /// <remarks>This will create a new session just as if it was created normally from a service. The only difference is
    /// that a new implicit service is create automatically on each call to NewSession(). This method is primarily for "getting
    /// started" scenarios. Any serious usage of Ramone should create and configure a specific service.</remarks>
    /// <param name="baseUrl">Base URL for implicit service.</param>
    /// <returns>A new session.</returns>
    public static ISession NewSession(Uri baseUrl)
    {
      IService service = NewService(baseUrl);
      return service.NewSession();
    }


    public static void RegisterStandardCodecs(ICodecManager cm)
    {
      // XML
      cm.AddCodec<XmlSerializerCodec>(MediaType.ApplicationXml);
      cm.AddCodec<XmlDocument, XmlSerializerCodec>(MediaType.ApplicationXml);
      cm.AddCodec<XmlDocument, XmlSerializerCodec>(MediaType.TextXml);
      cm.AddCodec<XmlDocument, XmlSerializerCodec>(MediaType.TextHtml);
      cm.AddCodec<XmlDocument, XmlSerializerCodec>(MediaType.ApplicationXHtml);

      // HTML + XHTML
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.TextHtml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.TextXml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.ApplicationXHtml);
      cm.AddCodec<HtmlDocument, HtmlDocumentCodec>(MediaType.ApplicationXml);

      // Atom
      cm.AddCodec<SyndicationFeed, AtomFeedCodec>(MediaType.ApplicationAtom);
      cm.AddCodec<SyndicationItem, AtomItemCodec>(MediaType.ApplicationAtom);

      // JSON
      cm.AddCodec<JsonSerializerCodec>(MediaType.ApplicationJson);

      // Multipart/form-data
      cm.AddCodec<MultipartFormDataSerializerCodec>(MediaType.MultipartFormData);

      // Form url encoded
      cm.AddCodec<FormUrlEncodedSerializerCodec>(MediaType.ApplicationFormUrlEncoded);

      // Strings
      cm.AddCodec<String, StringCodec>(new MediaType("*/*"));

      // Streams
      cm.AddCodec<Stream, StreamCodec>(new MediaType("*/*"));

      // Byte arrays
      cm.AddCodec<byte[], ByteArrayCodec>(new MediaType("*/*"));

      // Open search
      cm.AddXml<OpenSearchDescription>(new MediaType("application/opensearchdescription+xml"));

      // Json patch
      cm.AddCodec<JsonPatchDocument, JsonPatchDocumentCodec>(new MediaType("application/json-patch+json"));
    }


    public static void RegisterStandardSerializationFormaters(IObjectSerializerFormaterManager formatManager)
    {
      formatManager.AddFormater(typeof(Uri), new UriFormater());
    }
  }
}
