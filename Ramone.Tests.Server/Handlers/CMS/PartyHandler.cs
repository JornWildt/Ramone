using Ramone.Tests.Common.CMS;


namespace Ramone.Tests.Server.Handlers.CMS
{
  public class PartyHandler
  {
    public Party Get(long id)
    {
      return new Party
      {
        Id = id,
        FullName = string.Format("Bart-{0}", id),
        EMail = string.Format("bart-{0}@foo.bar", id)
      };
    }
  }
}