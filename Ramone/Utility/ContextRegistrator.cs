using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Ramone.Utility
{
  public static class ContextRegistrator
  {
    public static void RegisterContext(ISession session, Uri baseUrl, object item)
    {
      if (item == null)
        return;

      if (item is IContextContainer)
      {
        Type t = item.GetType();
        PropertyInfo[] properties = t.GetProperties();
        foreach (PropertyInfo pi in properties)
        {
          try
          {
            object pValue = pi.GetValue(item, null);
            RegisterContext(session, baseUrl, pValue);
          }
          catch (Exception)
          {
            // Ignore
          }
        }
      }

      if (item is IHaveContext)
      {
        ((IHaveContext)item).RegisterContext(session, baseUrl);
      }

      if (item is IEnumerable)
      {
        foreach (object sub in (IEnumerable)item)
        {
          RegisterContext(session, baseUrl, sub);
        }
      }
    }
  }
}
