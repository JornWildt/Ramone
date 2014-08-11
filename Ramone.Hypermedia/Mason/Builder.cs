using Newtonsoft.Json.Linq;
using Ramone.Hypermedia.Utilities;
using System;
using System.Linq;


namespace Ramone.Hypermedia.Mason
{
  public class Builder
  {
    private NamespaceManager Namespaces = new NamespaceManager();


    public MasonResource Build(JObject json)
    {
      if (json == null)
        return null;

      JObject namespaceJson = json[MasonProperties.Namespaces] as JObject;
      if (namespaceJson != null)
        BuildNamespaces(namespaceJson);

      MasonResource result = new MasonResource();

      foreach (var pair in json)
      {
        if (pair.Key == MasonProperties.Namespaces && pair.Value is JObject)
        {
          // Ignore - already handled
        }
        else if (pair.Key == MasonProperties.Links && pair.Value is JObject)
        {
          foreach (JProperty linkJson in pair.Value.Children().OfType<JProperty>())
            result.Controls.Add(BuildLink(linkJson));
        }
        else if (pair.Key == MasonProperties.LinkTemplates && pair.Value is JObject)
        {
          //LinkTemplatesJsonValue = pair.Value;
          //LinkTemplates = new ObservableCollection<LinkTemplateViewModel>(
          //  pair.Value.Children().OfType<JProperty>().Select(l => new LinkTemplateViewModel(this, l, context)));
        }
        else if (pair.Key == MasonProperties.Actions && pair.Value is JObject)
        {
          //ActionsJsonValue = pair.Value;
          //Actions = new ObservableCollection<ActionViewModel>(
          //  pair.Value.Children().OfType<JProperty>().Select(a => ActionViewModel.CreateAction(this, a, context)));
        }
        else if (pair.Key == MasonProperties.Meta && pair.Value is JObject)
        {
          //MetaJsonValue = pair.Value;
          //Description = GetValue<string>(pair.Value, MasonProperties.MetaProperties.Description);
          //JToken metaLinksProperty = pair.Value[MasonProperties.Links];
          //if (metaLinksProperty is JObject)
          //{
          //  MetaLinksJsonValue = metaLinksProperty;
          //  MetaLinks = new ObservableCollection<LinkViewModel>(
          //    metaLinksProperty.Children().OfType<JProperty>().Select(l => new LinkViewModel(this, l, context)));
          //}
        }
        else if (pair.Key == MasonProperties.Error && pair.Value is JObject)
        {
          //ResourcePropertyViewModel error = new ResourcePropertyViewModel(this, pair.Value, pair.Key, new ResourceViewModel(this, (JObject)pair.Value, context));
          //error.IsError = true;
          //Properties.Add(error);
        }
        else
        {
          result.RegisterPropertyValue(pair.Key, CreatePropertyRecursively(pair.Value));
        }
      }

      return result;
    }


    private IControl BuildLink(JProperty linkJson)
    {
      JObject linkObject = linkJson.Value as JObject;
      if (linkObject == null)
        return null;

      string prefix;
      string reference;
      string nsname;
      string name = Namespaces.Expand(linkJson.Name, out prefix, out reference, out nsname);

      string href = GetValue<string>(linkObject, "href");

      Link link = new Link(name, href);
      return link;
    }


    private IControl BuildAction(JProperty actionJson)
    {
      JObject actionObject = actionJson.Value as JObject;
      if (actionObject == null)
        return null;

      string prefix;
      string reference;
      string nsname;
      string name = Namespaces.Expand(actionJson.Name, out prefix, out reference, out nsname);

      string type = GetValue<string>(actionObject, "type");
      string method = GetValue<string>(actionObject, "method");
      string href = GetValue<string>(actionObject, "href");

      if (type == MasonProperties.ActionTypes.JSON)
      {
        return new JsonAction(name, href, method);
      }
      
      throw new NotImplementedException();
    }


    private object CreatePropertyRecursively(JToken json)
    {
      if (json == null)
      {
        return null;
      }
      else if (json is JArray)
      {
        return ((JArray)json).Select(item => CreatePropertyRecursively(item)).ToArray();
      }
      else if (json is JObject)
      {
        return Build((JObject)json);
      }
      else if (json is JValue)
      {
        return ((JValue)json).Value;
      }
      else
        throw new NotImplementedException(string.Format("Cannot build property value from JSON type '{0}'", json.GetType()));
    }



    private void BuildNamespaces(JObject namespaces)
    {
      foreach (JProperty ns in namespaces.Properties())
      {
        JObject nsDef = ns.Value as JObject;
        if (nsDef != null)
        {
          JToken jsonName = nsDef[MasonProperties.NamespaceProperties.Name];
          string ns_name = (jsonName != null && jsonName.Type == JTokenType.String ? jsonName.Value<string>() : null);
          string ns_prefix = ns.Name;
          if (!string.IsNullOrWhiteSpace(ns_prefix) && !string.IsNullOrWhiteSpace(ns_name))
            Namespaces.Namespace(ns_prefix, ns_name);
        }
      }
    }


    protected T GetValue<T>(JToken t, string name)
      where T : class
    {
      JToken value = t[name];
      if (value != null)
        return value.Value<T>();
      return null;
    }
  }
}
