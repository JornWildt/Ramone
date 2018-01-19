using System;

namespace Ramone.Implementation
{
  public class RequestInterceptorSet : InterceptorSet<IRequestInterceptor>, IRequestInterceptorSet
  {
    public RequestInterceptorSet()
      : base()
    {
    }


    public RequestInterceptorSet(RequestInterceptorSet src)
      : base(src)
    {
    }


    public override object Clone()
    {
      return new RequestInterceptorSet(this);
    }
  }
}
