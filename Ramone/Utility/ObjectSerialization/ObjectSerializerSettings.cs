namespace Ramone.Utility.ObjectSerialization
{
  public class ObjectSerializerSettings
  {
    public string ArrayFormat { get; set; }
    
    public string DictionaryFormat { get; set; }
    
    public string PropertyFormat { get; set; }

    public string DateTimeFormat { get; set; }
    
    public IObjectSerializerFormaterManager Formaters { get; set; }


    public ObjectSerializerSettings()
    {
      ArrayFormat = "{0}[{1}]";
      DictionaryFormat = "{0}[{1}]";
      PropertyFormat = "{0}.{1}";
      DateTimeFormat = "s";
      Formaters = new ObjectSerializerFormaterManager();
    }


    public ObjectSerializerSettings(ObjectSerializerSettings src)
    {
      ArrayFormat = src.ArrayFormat;
      DictionaryFormat = src.DictionaryFormat;
      PropertyFormat = src.PropertyFormat;
      DateTimeFormat = src.DateTimeFormat;
      Formaters = src.Formaters.Clone();
    }
  }
}
