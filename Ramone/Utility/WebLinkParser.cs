using System;
using System.Collections.Generic;
using Ramone.HyperMedia;
using System.IO;
using CuttingEdge.Conditions;


namespace Ramone.Utility
{
  public class WebLinkParser
  {
    public static IList<ILink> ParseLinks(string links)
    {
      WebLinkParser parser = new WebLinkParser();
      return parser.Parse(links);
    }


    public IList<ILink> Parse(string linkHeader)
    {
      InputString = linkHeader;
      InputPos = 0;

      IList<ILink> links = new List<ILink>();

      do
      {
        GetNextToken();

        if (NextToken.Type == TokenType.Url)
          links.Add(ParseLink());
        else if (NextToken.Type == TokenType.EOF)
          break;
        else
          Error(string.Format("Unexpected token '{0}' (expected URL)", NextToken.Type));
      }
      while (NextToken.Type == TokenType.Comma);

      return links;
    }


    #region Parser

    class WebLink : LinkBase
    {
      public WebLink()
      {
      }


      public WebLink(Uri href, string relationshipType, string mediaType, string title)
        : this(href.ToString(), relationshipType, mediaType, title)
      {
      }


      public WebLink(string href, string relationshipType, string mediaType, string title)
        : base(href, relationshipType, mediaType, title)
      {
      }
    }


    protected ILink ParseLink()
    {
      Condition.Requires(NextToken.Type, "CurrentToken.Type").IsEqualTo(TokenType.Url);
      string url = NextToken.Value;
      string rel = null;
      string title = null;
      string type = null;

      GetNextToken();

      while (NextToken.Type == TokenType.Semicolon)
      {
        GetNextToken();
        KeyValuePair<string, string> p = ParseParameter();

        if (p.Key == "rel")
          rel = p.Value;
        else if (p.Key == "title")
          title = p.Value;
        else if (p.Key == "title*")
          title = p.Value;
        else if (p.Key == "type")
          type = p.Value;
      }

      WebLink link = new WebLink(url, rel, type, title);
      return link;
    }


    protected KeyValuePair<string, string> ParseParameter()
    {
      if (NextToken.Type != TokenType.Identifier && NextToken.Type != TokenType.ExtendedIdentifier)
        Error(string.Format("Unexpected token '{0}' (expected an identifier)", NextToken.Type));
      string id = NextToken.Value;
      bool isExtended = (NextToken.Type == TokenType.ExtendedIdentifier);
      GetNextToken();

      if (NextToken.Type != TokenType.Assignment)
        Error(string.Format("Unexpected token '{0}' (expected an assignment)", NextToken.Type));
      GetNextToken();

      if (NextToken.Type != TokenType.String)
        Error(string.Format("Unexpected token '{0}' (expected an string)", NextToken.Type));
      string value = NextToken.Value;
      if (isExtended)
        value = HeaderEncodingParser.ParseExtendedHeader(value);
      GetNextToken();

      return new KeyValuePair<string, string>(id, value);
    }


    #endregion


    #region Token scanner

    protected Token NextToken { get; set; }

    protected enum TokenType { Url, Semicolon, Comma, Assignment, Identifier, ExtendedIdentifier, String, EOF }

    protected class Token
    {
      public TokenType Type { get; set; }
      public string Value { get; set; }
    }

    protected string InputString { get; set; }
    protected int InputPos { get; set; }


    protected void GetNextToken()
    {
      NextToken = ReadToken();
    }


    protected Token ReadToken()
    {
      while (true)
      {
        char? c = ReadNextChar();

        if (c == null)
          return new Token { Type = TokenType.EOF };

        if (c == ';')
          return new Token { Type = TokenType.Semicolon };

        if (c == ',')
          return new Token { Type = TokenType.Comma };

        if (c == '=')
          return new Token { Type = TokenType.Assignment };

        if (c == '"')
          return new Token { Type = TokenType.String, Value = ReadString() };

        if (c == '<')
          return new Token { Type = TokenType.Url, Value = ReadUrl() };

        if (Char.IsWhiteSpace(c.Value))
          continue;

        if (Char.IsLetter(c.Value))
          return ReadIdentifier(c.Value);

        Error(string.Format("Unrecognized character '{0}'", c));
      }
    }


    protected string ReadString()
    {
      string result = "";

      while (true)
      {
        char? c = ReadNextChar();
        if (c == null)
          break;
        if (c == '"')
          break;
        result += c;
      }

      return result;
    }


    protected string ReadUrl()
    {
      string result = "";

      while (true)
      {
        char? c = ReadNextChar();
        if (c == null)
          break;
        if (c == '>')
          break;
        result += c;
      }

      return result;
    }


    protected Token ReadIdentifier(char c)
    {
      string id = "" + c;

      while (Char.IsLetterOrDigit(InputString[InputPos]))
      {
        id += InputString[InputPos++];
      }

      if (InputString[InputPos] == '*')
      {
        InputPos++;
        return new Token { Type = TokenType.ExtendedIdentifier, Value = id };
      }
      else
      {
        return new Token { Type = TokenType.Identifier, Value = id };
      }
    }


    protected char? ReadNextChar()
    {
      if (InputPos == InputString.Length)
        return null;
      return InputString[InputPos++];
    }

    #endregion


    protected void Error(string msg)
    {
      throw new FormatException(string.Format("Invalid HTTP Web Link. {0} in '{1}' (around pos {2}).", msg, InputString, InputPos));
    }
  }
}
