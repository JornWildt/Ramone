using Ramone.MediaTypes.JsonPatch;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers
{
  public class PatchHandler
  {
    [HttpOperation("PATCH")]
    public object Patch(JsonPatchDocument patch)
    {
      return patch.ToString();
    }
  }
}