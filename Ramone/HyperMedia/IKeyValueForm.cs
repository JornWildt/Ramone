namespace Ramone.HyperMedia
{
  public interface IKeyValueForm
  {
    IKeyValueForm Value(string key, object value);
    IKeyValueForm Value(object value);
    RamoneResponse Submit();
    RamoneResponse<T> Submit<T>() where T : class;
  }
}
