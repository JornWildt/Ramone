namespace Ramone.MediaTypes.JsonPatch
{
  public interface IJsonPatchDocumentVisitor
  {
    void Add(string path, object value);
    void Remove(string path);
    void Replace(string path, object value);
    void Move(string from, string path);
    void Copy(string from, string path);
    void Test(string path, object value);
    void Complete();
  }
}
