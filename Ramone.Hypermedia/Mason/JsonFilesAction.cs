using Newtonsoft.Json;
using Ramone.IO;
using Ramone.Utility.ObjectSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ramone.Hypermedia.Mason
{
  public class JsonFilesAction : ActionBase
  {
    public string JsonFile { get; protected set; }


    public JsonFilesAction(string name, string href, string method, string jsonFile)
      : base(name, href, method)
    {
      JsonFile = jsonFile;
    }


    public override Request Bind(ISession session)
    {
      return session.Bind(HRef).Method(Method).AsMultipartFormData();
    }


    public override Request Bind(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).AsMultipartFormData().Body(args);
    }


    public override Request Bind(ISession session, object args, object files)
    {
      var payload = new Dictionary<string, object>();

      string jsonArgs = SerializeArgsToJson(args);
      payload[JsonFile] = new StringFile { ContentType = "application/json", Data = jsonArgs, Filename = "args" };

      JsonFilesPropertiesVisitor v = new JsonFilesPropertiesVisitor(payload);
      ObjectSerializer serializer = new ObjectSerializer(files.GetType()); // FIXME: null check
      serializer.Serialize(files, v);

      return session.Bind(HRef).Method(Method).AsMultipartFormData().Body(payload);
    }


    public override Response Invoke(ISession session)
    {
      throw new InvalidOperationException("A json-files action must have arguments applied");
    }


    public override Response Invoke(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).AsMultipartFormData().Body(args).Submit();
    }


    public override Response Upload(ISession session, object args, object files)
    {
      return Bind(session, args, files).Submit();
    }


    public override Response<T> Invoke<T>(ISession session)
    {
      throw new InvalidOperationException("A json-files action must have arguments applied");
    }


    public override Response<T> Invoke<T>(ISession session, object args)
    {
      return session.Bind(HRef).Method(Method).AsMultipartFormData().Body(args).Submit<T>();
    }


    public override Response<T> Upload<T>(ISession session, object args, object files)
    {
      return Bind(session, args, files).Submit<T>();
    }

    
    private string SerializeArgsToJson(object args)
    {
      using (StringWriter sw = new StringWriter())
      {
        using (JsonWriter jsw = new JsonTextWriter(sw))
        {
          JsonSerializer serializer = new JsonSerializer();
          serializer.Serialize(jsw, args);
        }
        return sw.ToString();
      }
    }
  }


  class JsonFilesPropertiesVisitor : IPropertyVisitor
  {
    Dictionary<string, object> FoundFiles = new Dictionary<string, object>();

    public JsonFilesPropertiesVisitor(Dictionary<string, object> foundFiles)
    {
      FoundFiles = foundFiles;
    }


    #region IPropertyVisitor Members

    public void Begin()
    {
    }

    public void SimpleValue(string name, object value, string formatedValue)
    {
    }

    public void File(IFile file, string name)
    {
      FoundFiles[name] = file;
    }

    public void End()
    {
    }

    #endregion
  }

}
