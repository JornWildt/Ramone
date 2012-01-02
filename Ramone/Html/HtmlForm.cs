using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Text;
using System.Web;
using System.IO;
using Ramone.Html;


namespace Ramone
{
  public class HtmlForm
  {
    public Uri Action { get; set; }

    public string Method { get; set; }

    public string EncType { get; set; }

    public Dictionary<string, string> Values { get; protected set; }

    public Dictionary<string, Stream> Files { get; protected set; }

    public ISession Session { get; protected set; }


    protected XmlNode OriginalXml { get; set; }


    public HtmlForm(Uri baseUri, XmlNode formXml, ISession session)
    {
      OriginalXml = formXml;
      Action = baseUri;
      Method = "POST";
      EncType = "application/x-www-form-urlencoded";
      Values = new Dictionary<string, string>();
      Files = new Dictionary<string, Stream>();
      Session = session;

      if (formXml.Attributes["action"] != null)
        Action = new Uri(baseUri, formXml.Attributes["action"].Value);
      if (formXml.Attributes["method"] != null)
        Method = formXml.Attributes["method"].Value;
      if (formXml.Attributes["enctype"] != null)
        EncType = formXml.Attributes["enctype"].Value;
    }


    public HtmlForm WithValue(string key, string value)
    {
      string name = GetInputNameFromKey(key);
      Values[name] = value;
      return this;
    }


    public HtmlForm WithFile(string key, Stream data)
    {
      string name = GetInputNameFromKey(key);
      Files[name] = data;
      return this;
    }


    public HttpWebResponse Submit(string submitKey, bool followRedirects = false)
    {
      string submitName = null;
      string selector;

      if (submitKey.StartsWith("#"))
      {
        selector = string.Format("//input[@type=\"submit\" and @id=\"{0}\"]", submitKey.Substring(1));
      }
      else
      {
        submitName = submitKey;
        selector = string.Format("//input[@type=\"submit\" and @name=\"{0}\"]", submitName);
      }

      XmlNode submitInput = OriginalXml.SelectSingleNode(selector);
      if (submitInput == null)
        throw new InvalidOperationException(string.Format("Could not locate input matching '{0}'", submitKey));

      if (submitKey.StartsWith("#") && submitInput.Attributes["name"] != null)
      {
        submitName = submitInput.Attributes["name"].Value;
      }

      string submitValue = submitInput.Attributes["value"] != null ? submitInput.Attributes["value"].Value : "";
      if (submitName != null)
        Values[submitName] = submitValue;

      HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Action);
      
      // Set headers and similar before writing to stream
      req.CookieContainer = Session.Cookies;
      req.Method = Method;
      req.ContentType = EncType;
      req.AllowAutoRedirect = followRedirects;

      if (EncType == "application/x-www-form-urlencoded")
        CreateFormUrlEncodedData(req);
      else if (EncType == "multipart/form-data")
        CreateMultiPartData(req);

      HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

      return resp;
    }


    private void CreateFormUrlEncodedData(HttpWebRequest req)
    {
      StringBuilder s = new StringBuilder();
      foreach (KeyValuePair<string, string> entry in Values)
      {
        s.AppendFormat("{0}{1}={2}", 
          s.Length > 0 ? "&" : "",
          HttpUtility.UrlEncode(entry.Key), 
          HttpUtility.UrlEncode(entry.Value));
      }

      Encoding enc = Encoding.UTF8; // FIXME: hardcoded
      byte[] data = enc.GetBytes(s.ToString());
      req.ContentLength = data.Length;
      req.GetRequestStream().Write(data, 0, data.Length);
    }


    private void CreateMultiPartData(HttpWebRequest req)
    {
      string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
      req.ContentType = "multipart/form-data; boundary=" + boundary;
      req.Method = "POST";
      req.KeepAlive = true;
      Encoding enc = Encoding.UTF8; // FIXME: hardcoded

      byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
      string formDataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

      Stream s = req.GetRequestStream();

      foreach (KeyValuePair<string, string> entry in Values)
      {
        string formItem = string.Format(formDataTemplate, entry.Key, entry.Value);
        byte[] formItemBytes = enc.GetBytes(formItem);
        s.Write(formItemBytes, 0, formItemBytes.Length);
      }

      s.Write(boundaryBytes, 0, boundaryBytes.Length);
      string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

      foreach (KeyValuePair<string, Stream> entry in Files)
      {
        string header = string.Format(headerTemplate, entry.Key, "fixme-filename.txt");
        byte[] headerBytes = enc.GetBytes(header);
        s.Write(headerBytes, 0, headerBytes.Length);
        CopyStream(entry.Value, s);
        s.Write(boundaryBytes, 0, boundaryBytes.Length);
      }
    }


    private void CopyStream(Stream input, Stream output)
    {
      byte[] buffer = new byte[8 * 1024];
      int len;
      while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
      {
        output.Write(buffer, 0, len);
      }
    }


    private string GetInputNameFromKey(string key)
    {
      if (key.StartsWith("#"))
      {
        string selector = string.Format("//*[@id=\"{0}\"]", key.Substring(1));
        XmlNode input = OriginalXml.SelectSingleNode(selector);
        if (input == null)
          throw new ArgumentException(string.Format("Could locate input element with ID '{0}'.", key), "key");
        XmlAttribute nameAttribute = input.Attributes["name"];
        if (nameAttribute == null)
          throw new ArgumentException(string.Format("Could locate 'name' attribute on input element with ID '{0}'.", key), "key");
        return nameAttribute.Value;
      }
      else
        return key;
    }
  }
}
