using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ramone.Tests.Common
{
  public class ComplexClassForSerializationTests
  {
    public class SubClass
    {
      public SubClass SubC { get; set; }
      public object[] Data { get; set; }
      public ComplexClassForSerializationTests SubComplex { get; set; }
    }

    public int X { get; set; }
    public string Y { get; set; }
    public int[] IntArray { get; set; }
    public SubClass SubC { get; set; }
    public Dictionary<string, string> Dict { get; set; }
    public DateTime Date { get; set; }
  }


  public class ComplexClassForOpenRastaSerializationTests
  {
    public class SubClass
    {
      public SubClass SubC { get; set; }
      public List<string> Data { get; set; }
    }

    public int X { get; set; }
    public string Y { get; set; }
    public List<int> IntArray { get; set; }
    public SubClass SubC { get; set; }
    public Dictionary<string, string> Dict { get; set; }
    public DateTime Date { get; set; }
    public double Dou { get; set; }
    public Guid GID { get; set; }
  }
}
