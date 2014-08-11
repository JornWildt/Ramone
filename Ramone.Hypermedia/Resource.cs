using System.Collections.Generic;
using System.Dynamic;


namespace Ramone.Hypermedia
{
  public class Resource : DynamicObject, IResource
  {
    public Dictionary<string, object> Properties { get; set; }

    public IControlCollection Controls { get; protected set; }


    public Resource()
    {
      Properties = new Dictionary<string, object>();
      Controls = new ControlCollection();
    }

    

    #region Dynamic stuff

    public override IEnumerable<string> GetDynamicMemberNames()
    {
      return Properties.Keys;
    }


    public virtual void RegisterPropertyValue(string name, object value)
    {
      Properties[name] = value;
    }


    // If you try to get a value of a property  
    // not defined in the class, this method is called. 
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      string name = binder.Name;

      object value;
      if (Properties.TryGetValue(name, out value))
        result = value;
      else
        result = null;
      return true;
    }

    #endregion
  }
}
