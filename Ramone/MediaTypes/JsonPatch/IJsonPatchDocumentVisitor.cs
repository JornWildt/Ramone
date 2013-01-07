namespace Ramone.MediaTypes.JsonPatch
{
  public interface IJsonPatchDocumentVisitor
  {
    bool Add(string path, object value);
    bool Remove(string path);
    bool Replace(string path, object value);
    bool Move(string from, string path);
    bool Copy(string from, string path);
    bool Test(string path, object value);
    void Complete();
  }
}
