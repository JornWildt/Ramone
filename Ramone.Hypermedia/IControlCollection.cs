namespace Ramone.Hypermedia
{
  public interface IControlCollection
  {
    IControl this[string name] { get; set; }
    void Add(IControl control);
  }
}
