namespace Ramone.HyperMedia
{
  public interface IKeyValueForm
  {
    IKeyValueForm Value(string key, object value);
    IKeyValueForm Value(object value);
    RamoneResponse Submit(string button = null);
    RamoneResponse<T> Submit<T>(string button = null) where T : class;
  }
}
