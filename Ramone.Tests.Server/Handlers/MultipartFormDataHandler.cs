using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenRasta.Web;
using System.IO;
using Ramone.Tests.Common;
using OpenRasta.IO;


namespace Ramone.Tests.Server.Handlers
{
  public class MultipartFormDataHandler
  {
    [HttpOperation(ForUriName = "SimpleData")]
    public object Post(MultipartData data)
    {
      return string.Format("{0}-{1}", data.Name, data.Age);
    }


    [HttpOperation(ForUriName = "FileData")]
    public object Post(MultipartDataFile data)
    {
      using (TextReader r = new StreamReader(data.DataFile.OpenStream()))
      {
        string content = r.ReadToEnd().Substring(0, 6); // Get substring in order to fetch "GIF89a" from binary GIF file
        return string.Format("{0}-{1}-{2}-{3}", data.DataFile.FileName, data.DataFile.ContentType, content, data.Age);
      }
    }


    // Cannot share this class - the IFile interface is different from OpenRasta and Ramone
    public class MultipartDataFile
    {
      public IFile DataFile { get; set; }
      public int Age { get; set; }
    }
  }
}