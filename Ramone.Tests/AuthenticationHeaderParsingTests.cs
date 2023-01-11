using System;
using System.Net.Http.Headers;
using NUnit.Framework;


namespace Ramone.Tests
{
  [TestFixture]
  public class AuthenticationHeaderValueTests
  {
    private void CanParseBase(string authenticationHeader, string expectedScheme, string expectedParameters)
    {
      // Act
      AuthenticationHeaderValue authenticationHeaderValue = AuthenticationHeaderValue.Parse(authenticationHeader);

      // Assert
      Assert.That(authenticationHeaderValue.Scheme, Is.EqualTo(expectedScheme));
      Assert.That(authenticationHeaderValue.Parameter, Is.EqualTo(expectedParameters));
    }


    [Test]
    public void CanParseWithNoParameters() => CanParseBase(
      authenticationHeader: "Basic",
      expectedScheme: "Basic",
      expectedParameters: null);


    [Test]
    public void CanParseSchemeWithTrailingWhitespace() => CanParseBase(
      authenticationHeader: "Basic ",
      expectedScheme: "Basic",
      expectedParameters: null);


    [Test]
    public void CanParseSchemeWithTrailingWhitespaces() => CanParseBase(
      authenticationHeader: "Basic  ",
      expectedScheme: "Basic",
      expectedParameters: null);


    [Test]
    public void CanParseSchemeAndParameter() => CanParseBase(
      authenticationHeader: "Basic dGVzdDoxMjPCow==",
      expectedScheme: "Basic",
      expectedParameters: "dGVzdDoxMjPCow==");


    [Test]
    public void CanParseSchemeAndParameters() => CanParseBase(
      authenticationHeader: "Basic dGVzdDoxMjPCow==, charset=\"UTF-8\"",
      expectedScheme: "Basic",
      expectedParameters: "dGVzdDoxMjPCow==, charset=\"UTF-8\"");


    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void CannotParse(string authenticationHeader) =>
      Assert.That(() => AuthenticationHeaderValue.Parse(authenticationHeader), Throws.InstanceOf<FormatException>());
  }
}
