namespace Ramone.HyperMedia
{
  public interface IKeyValueForm
  {
    void Value(string key, object value);
    void Value(object value);
    RamoneResponse Submit();
    RamoneResponse<T> Submit<T>() where T : class;
  }
}
