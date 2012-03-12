using System.Collections.Generic;


namespace Ramone.Tests.Common
{
  public class FormArgs
  {
    public string InputText { get; set; }
    public string InputPassword { get; set; }
    public string InputCheckbox { get; set; }
    public string InputHidden { get; set; }
    public string TextArea { get; set; }
    public string Select { get; set; }
    public string Radio1 { get; set; }
    public string Radio2 { get; set; }
    public List<string> MultiSelect { get; set; }
    public string MultiSelectValue { get; set; }
    public string Save { get; set; }
    public string Cancel { get; set; }
    public string Help { get; set; }
    public string EncType { get; set; }
    public string Charset { get; set; }
  }
}
