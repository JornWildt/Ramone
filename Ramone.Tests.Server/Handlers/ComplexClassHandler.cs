using Ramone.Tests.Common;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Tests.Server.Handlers
{
  public class ComplexClassHandler
  {
    public object Get(ComplexClassForOpenRastaSerializationTests data)
    {
      ObjectSerializer serializer = new ObjectSerializer(data.GetType());
      ObjectToStringPropertyVisitor visitor = new ObjectToStringPropertyVisitor();
      serializer.Serialize(data, visitor);
      return visitor.Result;
    }


    public object Post(ComplexClassForOpenRastaSerializationTests data)
    {
      ObjectSerializer serializer = new ObjectSerializer(data.GetType());
      ObjectToStringPropertyVisitor visitor = new ObjectToStringPropertyVisitor();
      serializer.Serialize(data, visitor);
      return visitor.Result;
    }
  }
}