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
    public ICommunicationContext CommunicationContext { get; set; }


    [HttpOperation(ForUriName = "SimpleData")]
    public object Post(MultipartData data)
    {
      CommunicationContext.Response.Headers["X-contenttype"] = CommunicationContext.Request.Headers["Content-Type"];
      return new EncodingData
      {
        Data = string.Format("{0}-{1}", data.Name, data.Age)
      };
    }


    [HttpOperation(ForUriName = "FileData")]
    public object Post(MultipartDataFile data)
    {
      HttpContext.Current.Response.Headers["X-contenttype"] = HttpContext.Current.Request.Headers["Content-Type"];
      byte[] rawData = new byte[50];
      int length = data.DataFile.OpenStream().Read(rawData, 0, 10);

      string base64Data = Convert.ToBase64String(rawData, 0, length);
      return string.Format("{0}-{1}-{2}-{3}", data.DataFile.FileName, data.DataFile.ContentType, base64Data, data.Age);
    }


    // Cannot share this class - the IFile interface is different from OpenRasta and Ramone
    public class MultipartDataFile
    {
      public IFile DataFile { get; set; }
      public int Age { get; set; }
    }
  }
}