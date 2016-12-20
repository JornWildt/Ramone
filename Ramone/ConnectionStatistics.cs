using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;


namespace Ramone
{
  public static class ConnectionStatistics
  {
    public class ConnectionInfo
    {
      public Uri Url { get; private set; }
      public string Method { get; private set; }

      public ConnectionInfo(Uri url, string method)
      {
        Url = url;
        Method = method;
      }
    }


    private static Dictionary<Guid, ConnectionInfo> OpenConnections = new Dictionary<Guid, ConnectionInfo>();


    public static Guid RegisterConnection(HttpWebResponse response)
    {
      Guid id = Guid.NewGuid();
      lock (OpenConnections)
      {
        OpenConnections[id] = new ConnectionInfo(response.ResponseUri, response.Method);
      }
      return id;
    }


    public static void DiscardConnection(Guid id)
    {
      lock (OpenConnections)
      {
        OpenConnections.Remove(id);
      }
    }


    public static IList<ConnectionInfo> GetOpenConnections()
    {
      lock (OpenConnections)
      {
        return OpenConnections.Values.ToList();
      }
    }
  }
}
