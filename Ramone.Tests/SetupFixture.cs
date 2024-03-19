using System.Text;
using NUnit.Framework;

namespace Ramone.Tests
{
  [SetUpFixture]
  public class SetupFixture : Common.SetupFixture
  {
    public override void Setup()
    {
      base.Setup();
#if NET6_0_OR_GREATER
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
    }
  }
}
