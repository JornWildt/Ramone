namespace Ramone.MediaTypes.Atom
{
  public static class AtomInitializer
  {
    public static void Initialize()
    {
      RamoneConfiguration.AddCodecRegistrator(new AtomCodecRegistrator());
    }
  }
}
