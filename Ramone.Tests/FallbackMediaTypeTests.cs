using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Ramone.MediaTypes;
using Ramone.Tests.Common;

namespace Ramone.Tests
{
  [TestFixture]
  public class FallbackMediaTypeTests : TestHelper
  {
    public class SomeData
    {
      public string X { get; set; }
    }



    public class SomedataCodec : TextCodecBase<SomeData>
    {
      protected override SomeData ReadFrom(TextReader reader, ReaderContext context)
      {
        string x = reader.ReadToEnd();
        return new SomeData { X = x };
      }


      protected override void WriteTo(SomeData data, TextWriter writer, WriterContext context)
      {
        writer.Write(data.X);
      }
    }


    [Test]
    public void ItCannotUseWildcardMediaTypesInWriters()
    {
      // Arrange
      Session.Service.CodecManager.AddCodec<SomeData, SomedataCodec>(new MediaType("*/*"));

      Request fileReq = Session.Bind(Constants.HeaderEchoPath);

      // Act + Assert
      Assert.Throws<InvalidOperationException>(() => fileReq.Post<HeaderList>(new SomeData { X = "hello" }));
    }
  }
}
