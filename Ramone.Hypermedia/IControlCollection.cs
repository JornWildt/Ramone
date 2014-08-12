namespace Ramone.Hypermedia
{
  public interface IControlCollection
  {
    bool Exists(string name);
    IControl this[string name] { get; set; }
    void Add(IControl control);
  }
}
