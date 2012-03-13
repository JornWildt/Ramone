using System;


namespace Ramone.HyperMedia
{
  public interface IKeyValueForm
  {
    IKeyValueForm Value(string key, object value);
    IKeyValueForm Value(object value);
    RamoneRequest Bind(string button = null);
  }
}
