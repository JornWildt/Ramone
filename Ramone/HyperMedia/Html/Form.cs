using System;
using HtmlAgilityPack;
using CuttingEdge.Conditions;
using System.Collections.Specialized;
using System.Collections;


namespace Ramone.HyperMedia.Html
{
  public class Form : IKeyValueForm
  {
    public Uri Action { get; protected set; }

    public string Method { get; protected set; }

    public MediaType EncodingType { get; protected set; }

    public string AcceptCharset { get; protected set; }


    protected Hashtable Values { get; set; }

    protected object AlternateValues { get; set; }


    #region IKeyValueForm Members

    public void Value(string key, object value)
    {
      Condition.Requires(key, "key").IsNotNullOrEmpty();
      Condition.Requires(value, "value").IsNotNull();

      Values.Add(key, value);
    }


    public void Value(object value)
    {
      Condition.Requires(value, "value").IsNotNull();

      AlternateValues = value;
    }


    public RamoneResponse Submit(RamoneResponse current)
    {
      RamoneResponse response = current.Session.Bind(Action, Values).Execute(Method);
      return response;
    }


    public RamoneResponse<T> Submit<T>(RamoneResponse current) where T : class
    {
      RamoneResponse<T> response = current.Session
                                          .Bind(Action)
                                          .ContentType(EncodingType)
                                          .Execute<T>(Method, AlternateValues ?? Values);
      return response;
    }

    #endregion


    public Form(HtmlNode formNode)
    {
      Condition.Requires(formNode, "formNode").IsNotNull();
      if (!formNode.Name.Equals("form", StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException(string.Format("Cannot create HTML form from '{0}' node.", formNode.Name));

      // FIXME: needs response to get default URI if it is not supplied
      Action = new Uri(formNode.GetAttributeValue("action", null));

      Method = formNode.GetAttributeValue("method", "get");

      string enctype = formNode.GetAttributeValue("enctype", null);
      EncodingType = (enctype != null ? new MediaType(enctype) : MediaType.ApplicationFormUrlEncoded);

      // FIXME: needs response in order to get default charset
      AcceptCharset = formNode.GetAttributeValue("accept-charset", "UNKNOWN");

      Values = new Hashtable();
    }
  }
}
