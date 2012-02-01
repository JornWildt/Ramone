namespace Ramone.Utility.ObjectSerialization
{
  public interface IPropertyVisitor
  {
    void Begin();
    void SimpleValue(string name, object value, string formatedValue);
    void End();
  }
}
