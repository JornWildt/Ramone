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


namespace Ramone.Hypermedia.Tests
{
  [SetUpFixture]
  class SetupFixture
  {
    [SetUp]
    public void Setup()
    {
      TestHelper.TestService = RamoneConfiguration.NewService(TestHelper.BaseUrl);

      TestHelper.TestService.DefaultEncoding = Encoding.GetEncoding("iso-8859-1");

      ICodecManager cm = TestHelper.TestService.CodecManager;
    }


    [TearDown]
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
