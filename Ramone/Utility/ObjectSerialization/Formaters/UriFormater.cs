using System;


namespace Ramone.Utility.ObjectSerialization.Formaters
{
  public class UriFormater : FormaterBase<Uri>
  {
    protected override string Format(Uri src)
    {
      return src.AbsoluteUri;
    }
  }
}
