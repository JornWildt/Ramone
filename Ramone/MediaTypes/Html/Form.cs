using System;
using System.Collections;
using System.Linq;
using CuttingEdge.Conditions;
using HtmlAgilityPack;
using Ramone.HyperMedia;


namespace Ramone.MediaTypes.Html
{
  public class Form : IKeyValueForm
  {
    public Uri Action { get; set; }

    public string Method { get; set; }

    public MediaType EncodingType { get; set; }

    public string AcceptCharset { get; set; }

    public Uri BaseUrl { get; set; }


    protected Hashtable Values { get; set; }

    protected object AlternateValues { get; set; }

    protected IRamoneSession Session { get; set; }


    #region IKeyValueForm Members

    public IKeyValueForm Value(string key, object value)
    {
      Condition.Requires(key, "key").IsNotNullOrEmpty();
      Condition.Requires(value, "value").IsNotNull();

      Values.Add(key, value);

      return this;
    }


    public IKeyValueForm Value(object value)
    {
      Condition.Requires(value, "value").IsNotNull();

      AlternateValues = value;

      return this;
    }


    public RamoneResponse Submit()
    {
      RamoneResponse response = Session.Bind(Action, Values)
                                       .ContentType(EncodingType)
                                       .Execute(Method, AlternateValues ?? Values);
      return response;
    }


    public RamoneResponse<T> Submit<T>() where T : class
    {
      RamoneResponse<T> response = Session.Bind(Action)
                                          .ContentType(EncodingType)
                                          .Execute<T>(Method, AlternateValues ?? Values);
      return response;
    }

    #endregion


    public Form(HtmlNode formNode, IRamoneSession session, Uri baseUrl)
    {
      Condition.Requires(formNode, "formNode").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(baseUrl, "baseUrl").IsNotNull();

      if (!formNode.Name.Equals("form", StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException(string.Format("Cannot create HTML form from '{0}' node.", formNode.Name));

      // FIXME: needs response to get default URI if it is not supplied
      Action = new Uri(formNode.GetAttributeValue("action", null));
      Method = formNode.GetAttributeValue("method", "get");
      Session = session;
      BaseUrl = baseUrl;

      string enctype = formNode.GetAttributeValue("enctype", null);
      EncodingType = (enctype != null ? new MediaType(enctype) : MediaType.ApplicationFormUrlEncoded);

      // FIXME: needs response in order to get default charset
      AcceptCharset = formNode.GetAttributeValue("accept-charset", "UNKNOWN");

      Values = new Hashtable();

      ParseInputs(formNode);
    }


    protected void ParseInputs(HtmlNode formNode)
    {
      foreach (HtmlNode inputNode in formNode.SelectNodes(".//input") ?? Enumerable.Empty<HtmlNode>())
      {
        string type = inputNode.GetAttributeValue("type", "text");
        string value = inputNode.GetAttributeValue("value", null);
        string name = inputNode.GetAttributeValue("name", null);

        if (name != null)
        {
          if (type == "submit")
          {
          }
          else// if (type == "hidden")
          {
            // Set default values
            if (value != null)
              Values.Add(name, value);
          }
        }
      }
    }
  }
}
