using System.IO;
using OpenRasta.Codecs;
using OpenRasta.TypeSystem;
using OpenRasta.Web;
using Ramone.MediaTypes.JsonPatch;


namespace Ramone.Tests.Server.Codecs
{
  [MediaType("application/json-patch+json")]
  [MediaType("application/json-patch")]
  public class JsonPatchDocumentCodec : OpenRasta.Codecs.IMediaTypeReader
  {
    #region IMediaTypeReader Members

    public object ReadFrom(IHttpEntity request, IType destinationType, string destinationName)
    {
      using (var reader = new StreamReader(request.Stream))
      {
        return JsonPatchDocument.Read(reader);
      }
    }

    #endregion

    
    #region ICodec Members

    public object Configuration { get; set; }

    #endregion
  }
}