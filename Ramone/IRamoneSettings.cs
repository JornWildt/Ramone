using System;


namespace Ramone
{
  public interface IRamoneSettings
  {
    ICodecManager CodecManager { get; }
    string UserAgent { get; set; }
    IRamoneService NewService(Uri baseUri);
  }
}
