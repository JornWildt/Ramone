using System;


namespace Ramone
{
  public interface ISettings
  {
    ICodecManager CodecManager { get; }
    string UserAgent { get; set; }
    IService NewService(Uri baseUri);
  }
}
