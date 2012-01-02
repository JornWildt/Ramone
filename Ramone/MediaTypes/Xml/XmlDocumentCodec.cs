using System;
using System.IO;
using System.Xml;


namespace Ramone.MediaTypes.Xml
{
  public class XmlDocumentCodec : IMediaTypeReader
  {
    #region IMediaTypeReader Members

    public object ReadFrom(Stream s, Type t)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(s);
      return doc;
    }

    #endregion
  }
}
