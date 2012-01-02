using System;
using System.Collections;
using System.Collections.Generic;


namespace Ramone.Implementation
{
  public class RequestInterceptorSet : IRequestInterceptorSet
  {
    Dictionary<string, IRequestInterceptor> Interceptors;


    public RequestInterceptorSet()
    {
      Interceptors = new Dictionary<string, IRequestInterceptor>();
    }


    public RequestInterceptorSet(RequestInterceptorSet src)
    {
      Interceptors = new Dictionary<string, IRequestInterceptor>(src.Interceptors);
    }


    public void Add(IRequestInterceptor interceptor)
    {
      Add(interceptor.GetType().ToString(), interceptor);
    }


    public void Add(string name, IRequestInterceptor interceptor)
    {
      Interceptors.Add(name, interceptor);
    }


    public void Remove(Type type)
    {
      Interceptors.Remove(type.ToString());
    }


    public void Remove(string name)
    {
      Interceptors.Remove(name);
    }


    public IRequestInterceptorSet Clone()
    {
      return new RequestInterceptorSet(this);
    }


    public IEnumerator<IRequestInterceptor> GetEnumerator()
    {
      return Interceptors.Values.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
      return Interceptors.Values.GetEnumerator();
    }
  }
}
