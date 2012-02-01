namespace Ramone.Utility.ObjectSerialization
{
  public class ObjectSerializerSettings
  {
    public string ArrayFormat { get; set; }
    
    public string DictionaryFormat { get; set; }
    
    public string PropertyFormat { get; set; }
    
    public IObjectSerializerFormaterManager Formaters { get; set; }


    public ObjectSerializerSettings()
    {
      ArrayFormat = "{0}[{1}]";
      DictionaryFormat = "{0}[{1}]";
      PropertyFormat = "{0}.{1}";
      Formaters = new ObjectSerializerFormaterManager();
    }


    public ObjectSerializerSettings(ObjectSerializerSettings src)
    {
      ArrayFormat = src.ArrayFormat;
      DictionaryFormat = src.DictionaryFormat;
      PropertyFormat = src.PropertyFormat;
      Formaters = src.Formaters.Clone();
    }
  }
}
