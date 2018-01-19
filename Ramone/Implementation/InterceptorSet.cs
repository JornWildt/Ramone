using System;
using System.Collections;
using System.Collections.Generic;

namespace Ramone.Implementation
{
  public abstract class InterceptorSet<T> : IInterceptorSet<T>
  {
    Dictionary<string, T> Interceptors;


    public InterceptorSet()
    {
      Interceptors = new Dictionary<string, T>();
    }


    public InterceptorSet(InterceptorSet<T> src)
    {
      Interceptors = new Dictionary<string, T>(src.Interceptors);
    }


    public void Add(T interceptor)
    {
      Add(interceptor.GetType().ToString(), interceptor);
    }


    public void Add(string name, T interceptor)
    {
      Interceptors.Add(name, interceptor);
    }


    public T Find(string name)
    {
      T i;
      if (Interceptors.TryGetValue(name, out i))
        return i;
      return default(T);
    }


    public void Remove(Type type)
    {
      Interceptors.Remove(type.ToString());
    }


    public void Remove(string name)
    {
      Interceptors.Remove(name);
    }


    public abstract object Clone();


    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
      return Interceptors.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
      return Interceptors.GetEnumerator();
    }
  }
}
