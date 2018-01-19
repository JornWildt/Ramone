using System;
using System.Collections.Generic;

namespace Ramone
{
  public interface IInterceptorSet<T> : IEnumerable<KeyValuePair<string, T>>
  {
    void Add(T interceptor);
    void Add(string name, T interceptor);
    T Find(string name);
    void Remove(string name);
    void Remove(Type type);
    object Clone();
  }
}
