using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using HtmlAgilityPack;
using Ramone.HyperMedia;
using Ramone.Utility;


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

    protected List<SubmitElement> SubmitElements { get; set; }

    protected object AlternateValues { get; set; }

    protected IRamoneSession Session { get; set; }


    #region IKeyValueForm Members

    public IKeyValueForm Value(string key, object value)
    {
      Condition.Requires(key, "key").IsNotNullOrEmpty();
      Condition.Requires(value, "value").IsNotNull();

      Values[key] = value;

      return this;
    }


    public IKeyValueForm Value(object value)
    {
      Condition.Requires(value, "value").IsNotNull();

      AlternateValues = value;

      return this;
    }


    public RamoneResponse Submit(string button = null)
    {
      string oldFormat = Session.SerializerSettings.ArrayFormat;

      try
      {
        Session.SerializerSettings.ArrayFormat = "{0}";
        return Session.Bind(Action)
                      .ContentType(EncodingType)
                      .Execute(Method, GetSubmitData(button));
      }
      finally
      {
        Session.SerializerSettings.ArrayFormat = oldFormat;
      }
    }


    public RamoneResponse<T> Submit<T>(string button = null) where T : class
    {
      string oldFormat = Session.SerializerSettings.ArrayFormat;

      try
      {
        Session.SerializerSettings.ArrayFormat = "{0}";
        return Session.Bind(Action)
                      .ContentType(EncodingType)
                      .Execute<T>(Method, GetSubmitData(button));
      }
      finally
      {
        Session.SerializerSettings.ArrayFormat = oldFormat;
      }
    }

    #endregion


    //protected RamoneRequest CreateRequest(string button = null, out Hashtable data)
    //{
    //  string oldFormat = Session.SerializerSettings.ArrayFormat;

    //  try
    //  {
    //    Session.SerializerSettings.ArrayFormat = "{0}";
    //    return Session.Bind(Action)
    //                  .ContentType(EncodingType);
    //  }
    //  finally
    //  {
    //    Session.SerializerSettings.ArrayFormat = oldFormat;
    //  }
    //}


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
      SubmitElements = new List<SubmitElement>();

      ParseInputs(formNode);
    }


    protected void ParseInputs(HtmlNode formNode)
    {
      foreach (HtmlNode inputNode in formNode.SelectNodes(".//*") ?? Enumerable.Empty<HtmlNode>())
      {
        string id = inputNode.GetAttributeValue("id", null);
        if (inputNode.Name == "input")
        {
          string type = inputNode.GetAttributeValue("type", "text");
          string value = inputNode.GetAttributeValue("value", null);
          string name = inputNode.GetAttributeValue("name", null);

          if (name != null)
          {
            if (type == "submit")
            {
              // Register submit buttons
              if (value != null)
              {
                SubmitElements.Add(new SubmitElement { Name = name, Value = value, Id = id });
              }
            }
            else if (type == "radio")
            {
              string isChecked = inputNode.GetAttributeValue("checked", null);
              if (isChecked != null && value != null)
                Values[name] = value;
            }
            else
            {
              // Set default values for input
              if (value != null)
                Values[name] = value;
            }
          }
        }
        else if (inputNode.Name == "textarea")
        {
          string name = inputNode.GetAttributeValue("name", null);
          string value = inputNode.InnerText;
          if (value != null)
            Values[name] = value;
        }
        else if (inputNode.Name == "select")
        {
          string name = inputNode.GetAttributeValue("name", null);
          //string multiple = inputNode.GetAttributeValue("multiple", null);
          //List<object> multiValues = null;
          //if (multiple != null)
            //Values[name] = multiValues = new List<object>();

          foreach (HtmlNode optionNode in inputNode.SelectNodes(".//option") ?? Enumerable.Empty<HtmlNode>())
          {
            string value = optionNode.GetAttributeValue("value", null);
            string selected = optionNode.GetAttributeValue("selected", null);
            if (selected != null && value != null)
            {
              //if (multiple == null)
              //{
                Values[name] = value;
              //}
              //else
              //{
              //  multiValues.Add(value);
              //}
            }
          }
        }
      }
    }


    protected Hashtable GetSubmitData(string button = null)
    {
      if (AlternateValues == null)
        return Values;

      Hashtable serializedValues = HashtableConverter.ConvertObjectPropertiesToHashtable(AlternateValues);
      foreach (string key in serializedValues.Keys)
      {
        object value = serializedValues[key];
        if (value != null)
          Values[key] = value;
      }

      AssignSubmitButton(button);

      return Values;
    }


    protected void AssignSubmitButton(string button = null)
    {
      SubmitElement submit = null;

      if (button != null)
      {
        if (button.StartsWith("#"))
        {
          string id = button.Substring(1);
          submit = SubmitElements.Where(s => s.Id == id).FirstOrDefault();
        }
        else
          submit = SubmitElements.Where(s => s.Name == button).FirstOrDefault();
      }
      else if (SubmitElements.Count > 0)
        submit = SubmitElements[0];

      if (submit != null)
        Values[submit.Name] = submit.Value;
    }


    protected class SubmitElement
    {
      public string Id { get; set; }
      public string Name { get; set; }
      public object Value { get; set; }
    }
  }
}
