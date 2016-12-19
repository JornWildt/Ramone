using System.Linq;
using System.Text;
using NUnit.Framework;
using Ramone.MediaTypes.Xml;
using Ramone.Tests.Codecs;
using Ramone.Tests.Common;
using Ramone.Tests.Common.CMS;
using Ramone.MediaTypes;
using System.Collections.Generic;
using System;
using Ramone.Hypermedia.Mason;


namespace Ramone.Hypermedia.Tests
{
  [SetUpFixture]
  class SetupFixture
  {
    public static Resource SharedProject { get; set; }


    [OneTimeSetUp]
    public void Setup()
    {
      TestHelper.TestService = RamoneConfiguration.NewService(TestHelper.BaseUrl);

      TestHelper.TestService.DefaultEncoding = Encoding.GetEncoding("iso-8859-1");
      // FIXME: move this to some utility feature in Hypermedia namespace
      TestHelper.TestService.CodecManager.AddCodec<Resource, MasonCodec>(new MediaType("application/vnd.mason+json"));

      ICodecManager cm = TestHelper.TestService.CodecManager;
    }


    [OneTimeTearDown]
    public void TearDown()
    {
      IList<ConnectionStatistics.ConnectionInfo> connections = ConnectionStatistics.GetOpenConnections().ToList();
      if (connections.Count > 0)
      {
        foreach (ConnectionStatistics.ConnectionInfo c in connections)
          Console.WriteLine("Open connection to {0} ({1}).", c.Url, c.Method);
        Assert.AreEqual(0, connections.Count, "All connections must have been closed (showing currently number of open connections).");
      }
    }
  }
}
