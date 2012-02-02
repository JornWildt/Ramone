using System;


namespace Ramone.Utility.ObjectSerialization.Formaters
{
  public class DelegateFormater<T> : FormaterBase<T>
  {
    protected Func<T, string> Formater { get; set; }


    public DelegateFormater(Func<T, string> formater)
    {
      Formater = formater;
    }


    protected override string Format(T src)
    {
      return Formater(src);
    }
  }
}
