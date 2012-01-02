using System.Collections.Generic;


namespace Ramone.Tests.Common.CMS
{
  public class CaseFile
  {
    public long Id { get; set; }
    public string Title { get; set; }
    public IList<Dossier> Dossiers { get; set; }

    public CaseFile()
    {
      Dossiers = new List<Dossier>();
    }
  }
}
