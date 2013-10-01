using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ramone;


namespace RamoneCoreTest
{
  class Program
  {
    static void Main(string[] args)
    {
      Request req = RamoneConfiguration.NewSession(new Uri("http://www.dr.dk")).Bind("/");
      using (var resp = req.Get())
      {
      }
    }
  }
}
