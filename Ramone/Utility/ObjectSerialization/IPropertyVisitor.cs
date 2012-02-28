using Ramone.IO;


namespace Ramone.Utility.ObjectSerialization
{
  public interface IPropertyVisitor
  {
    void Begin();
    void SimpleValue(string name, object value, string formatedValue);
    void File(IFile file, string name);
    void End();
  }
}
