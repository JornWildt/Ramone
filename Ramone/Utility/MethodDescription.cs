using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ramone.Utility
{
  public class MethodDescription
  {
    public string Method { get; protected set; }
    public bool BodyAllowed { get; protected set; }

    private MethodDescription(string method, bool bodyAllowed)
    {
      Method = method;
      BodyAllowed = bodyAllowed;
    }

    
    protected static Dictionary<string, MethodDescription> Methods = new Dictionary<string, MethodDescription>();

    
    static MethodDescription()
    {
      RegisterMethod("GET", false);
      RegisterMethod("HEAD", false);
      RegisterMethod("DELETE", false);
      RegisterMethod("OPTIONS", false);
      RegisterMethod("TRACE", false);
      RegisterMethod("POST", true);
      RegisterMethod("PUT", true);
      RegisterMethod("PATCH", true);
    }


    public static void RegisterMethod(string method, bool bodyAllowed)
    {
      method = method.ToUpper();
      Methods[method] = new MethodDescription(method, bodyAllowed);
    }


    public static MethodDescription GetMethod(string method)
    {
      method = method.ToUpper();
      if (!Methods.ContainsKey(method))
        throw new InvalidOperationException(string.Format("Trying to get meta information about method '{0}' but cannot find it: unknown method.", method));
      return Methods[method];
    }
  }
}
