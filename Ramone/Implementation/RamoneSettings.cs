using System;


namespace Ramone.Implementation
{
  public class RamoneSettings : ISettings
  {
    #region IRamoneSettings

    public ICodecManager CodecManager { get; protected set; }

    public string UserAgent { get; set; }


    public IService NewService(Uri baseUri)
    {
      return new RamoneService(this, baseUri);
    }

    #endregion


    internal RamoneSettings()
    {
      CodecManager = new CodecManager();
      UserAgent = "Ramone/1.0";
    }
  }
}
