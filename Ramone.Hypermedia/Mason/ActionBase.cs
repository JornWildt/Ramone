namespace Ramone.Hypermedia.Mason
{
  public abstract class ActionBase : ControlBase
  {
    public string HRef { get; set; }

    public string Method { get; set; }


    public ActionBase(string name, string href, string method)
      : base(name)
    {
      HRef = href;
      Method = method;
    }
  }
}
