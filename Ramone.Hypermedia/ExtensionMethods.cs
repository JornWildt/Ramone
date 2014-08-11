using Ramone;


namespace Ramone.Hypermedia
{
  public static class ExtensionMethods
  {
    public static Response Invoke(this Request req)
    {
      return req.Submit();
    }
    
    
    public static Response<T> Invoke<T>(this Request req) where T : class
    {
      return req.Submit<T>();
    }


    public static Response Follow(this IControl control, ISession session)
    {
      return control.Invoke(session);
    }


    public static Response<T> Follow<T>(this IControl control, ISession session) where T : class
    {
      return control.Invoke<T>(session);
    }
  }
}
