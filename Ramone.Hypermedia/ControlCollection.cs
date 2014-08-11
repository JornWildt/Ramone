using System.Collections.Generic;


namespace Ramone.Hypermedia
{
  public class ControlCollection : IControlCollection
  {
    private Dictionary<string, IList<IControl>> Controls = new Dictionary<string, IList<IControl>>();


    public void Add(IControl control)
    {
      if (control == null)
        return;

      if (!Controls.ContainsKey(control.Name))
        Controls[control.Name] = new List<IControl>();
      Controls[control.Name].Add(control);
    }


    #region IControlCollection Members

    public IControl this[string key]
    {
      get
      {
        IList<IControl> c;
        if (Controls.TryGetValue(key, out c))
          return c[0];
        return null;
      }
      set
      {
        IList<IControl> c;
        if (Controls.TryGetValue(key, out c))
          c.Add(value);
        else
          Controls[key] = new List<IControl> { value };
      }
    }

    #endregion
  }
}
