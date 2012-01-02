using System;
using System.Collections.Generic;


namespace Ramone
{
  public interface IRequestInterceptorSet : IEnumerable<IRequestInterceptor>
  {
    void Add(IRequestInterceptor interceptor);
    void Add(string name, IRequestInterceptor interceptor);
    void Remove(string name);
    void Remove(Type type);
    IRequestInterceptorSet Clone();
  }
}
