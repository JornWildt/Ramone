using System;
using System.Collections.Generic;


namespace Ramone
{
  public interface IRequestInterceptorSet : IEnumerable<KeyValuePair<string, IRequestInterceptor>>
  {
    void Add(IRequestInterceptor interceptor);
    void Add(string name, IRequestInterceptor interceptor);
    IRequestInterceptor Find(string name);
    void Remove(string name);
    void Remove(Type type);
    IRequestInterceptorSet Clone();
  }
}
