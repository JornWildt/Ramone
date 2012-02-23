namespace Ramone.HyperMedia
{
  public interface IKeyValueForm
  {
    void Value(string key, string value);
    RamoneResponse Submit(RamoneResponse response);
    RamoneResponse<T> Submit<T>(RamoneResponse response) where T : class;
  }
}
