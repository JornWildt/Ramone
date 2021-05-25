using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Ramone.HyperMedia;
using Ramone.Utility;
using Ramone.Utility.Validation;

namespace Ramone.MediaTypes.Html
{
  public class Form : IKeyValueForm
  {
    public Uri Action { get; set; }

    public string Method { get; set; }

    public MediaType EncodingType { get; set; }

    public string AcceptCharset { get; set; }

    public Uri BaseUrl { get; set; }

    public string ResponseCharset { get; protected set; }

    public Hashtable Values { get; protected set; }


    protected List<SubmitElement> SubmitElements { get; set; }

    protected object AlternateValues { get; set; }

    protected ISession Session { get; set; }


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


    public Request Bind(string button = null)
    {
      string charset = GetCharset();
      Uri action = Action;

      if (!MethodDescription.GetMethod(Method).BodyAllowed && EncodingType == MediaType.ApplicationFormUrlEncoded)
        action = Action.AddQueryParameters(GetSubmitData(button));

      Request request = Session.Bind(action)
                              .ContentType(EncodingType)
                              .Method(Method);

      if (MethodDescription.GetMethod(Method).BodyAllowed)
        request.Body(GetSubmitData(button));

      if (charset != null)
        request.CodecParameter("Charset", charset);

      return request;
    }

    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="formNode"></param>
    /// <param name="session"></param>
    /// <param name="baseUrl"></param>
    /// <param name="charset">The character set used in the previoius response (from which the form originates).</param>
    public Form(HtmlNode formNode, ISession session, Uri baseUrl, string charset)
    {
      Condition.Requires(formNode, "formNode").IsNotNull();
      Condition.Requires(session, "session").IsNotNull();
      Condition.Requires(baseUrl, "baseUrl").IsNotNull();

      if (!formNode.Name.Equals("form", StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException(string.Format("Cannot create HTML form from '{0}' node.", formNode.Name));

      Action = new Uri(baseUrl, formNode.GetAttributeValue("action", ""));
      Method = formNode.GetAttributeValue("method", "get");
      Session = session;
      BaseUrl = baseUrl;
      ResponseCharset = charset;

      string enctype = formNode.GetAttributeValue("enctype", null);
      EncodingType = (enctype != null ? new MediaType(enctype) : MediaType.ApplicationFormUrlEncoded);

      AcceptCharset = formNode.GetAttributeValue("accept-charset", null);

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
          if (value != null && name != null)
            Values[name] = value;
        }
        else if (inputNode.Name == "select")
        {
          string name = inputNode.GetAttributeValue("name", null);
          if (name != null)
          {
            foreach (HtmlNode optionNode in inputNode.SelectNodes(".//option") ?? Enumerable.Empty<HtmlNode>())
            {
              string value = optionNode.GetAttributeValue("value", null);
              string selected = optionNode.GetAttributeValue("selected", null);
              if (selected != null && value != null)
              {
                Values[name] = value;
              }
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


    protected string GetCharset()
    {
      if (AcceptCharset != null)
        return AcceptCharset;
      else if (Session.DefaultEncoding != null)
        return Session.DefaultEncoding.WebName;
      return null;
    }


    protected class SubmitElement
    {
      public string Id { get; set; }
      public string Name { get; set; }
      public object Value { get; set; }
    }
  }
}
