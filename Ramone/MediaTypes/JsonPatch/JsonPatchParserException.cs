using System;


namespace Ramone.MediaTypes.JsonPatch
{
  public class JsonPatchParserException : Exception
  {
    public JsonPatchParserException(string msg)
      : base(msg)
    {
    }


    public JsonPatchParserException(string msg, Exception innerException)
      : base(msg, innerException)
    {
    }
  }
}
