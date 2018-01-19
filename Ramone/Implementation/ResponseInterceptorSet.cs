namespace Ramone.Implementation
{
  public class ResponseInterceptorSet : InterceptorSet<IResponseInterceptor>, IResponseInterceptorSet
  {
    public ResponseInterceptorSet()
      : base()
    {
    }


    public ResponseInterceptorSet(ResponseInterceptorSet src)
      : base(src)
    {
    }


    public override object Clone()
    {
      return new ResponseInterceptorSet(this);
    }
  }
}
