using System;


namespace Ramone.Utility.ObjectSerialization.Formaters
{
  public abstract class FormaterBase<T> : IObjectSerializerFormater
  {
    #region IObjectSerializerFormater Members

    public string Format(object src)
    {
      if (src == null)
        return null;

      if (!(src is T))
        throw new ArgumentException(string.Format("Cannot format {0} in {1} - expected {2}.",
                                    src.GetType().ToString(),
                                    GetType(),
                                    typeof(T)));

      return Format((T)src);
    }

    #endregion


    protected abstract string Format(T src);
  }
}
