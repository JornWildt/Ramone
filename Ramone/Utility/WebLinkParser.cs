using System;
using System.Collections.Generic;
using CuttingEdge.Conditions;
using Ramone.HyperMedia;


namespace Ramone.Utility
{
  public class WebLinkParser
  {
    public static IList<WebLink> ParseLinks(Uri baseUrl, string linkHeader)
    {
      WebLinkParser parser = new WebLinkParser();
      return parser.Parse(baseUrl, linkHeader);
    }


    public Uri BaseUrl { get; protected set; }


    public IList<WebLink> Parse(Uri baseUrl, string linkHeader)
    {
      BaseUrl = baseUrl;
      InputString = linkHeader;
      InputPos = 0;

      IList<WebLink> links = new List<WebLink>();

      while (true)
      {
        try
        {
          GetNextToken();

          if (NextToken.Type == TokenType.Url)
            links.Add(ParseLink());
          else if (NextToken.Type == TokenType.EOF)
            break;
          else
            Error(string.Format("Unexpected token '{0}' (expected URL)", NextToken.Type));

          if (NextToken.Type == TokenType.Comma)
            continue;
          else if (NextToken.Type == TokenType.EOF)
            break;
          else
            Error(string.Format("Unexpected token '{0}' (expected comma)", NextToken.Type));
        }
        catch (FormatException)
        {
          while (NextToken.Type != TokenType.Comma && NextToken.Type != TokenType.EOF)
          {
            try
            {
              GetNextToken();
            }
            catch (FormatException)
            {
            }
          }
        }
      }

      return links;
    }


    #region Parser

    protected WebLink ParseLink()
    {
      Condition.Requires(NextToken.Type, "CurrentToken.Type").IsEqualTo(TokenType.Url);

      string url = NextToken.Value;
      string rel = null;
      string title = null;
      string title_s = null;
      string type = null;

      GetNextToken();

      while (NextToken.Type == TokenType.Semicolon)
      {
        try
        {
          GetNextToken();
          bool isExtended;
          KeyValuePair<string, string> p = ParseParameter(out isExtended);

          if (p.Key == "rel" && rel == null)
            rel = p.Value;
          else if (p.Key == "title" && title == null && !isExtended)
            title = p.Value;
          else if (p.Key == "title" && title_s == null && isExtended)
            title_s = p.Value;
          else if (p.Key == "type" && type == null)
            type = p.Value;
        }
        catch (FormatException)
        {
          while (NextToken.Type != TokenType.Semicolon && NextToken.Type != TokenType.Comma && NextToken.Type != TokenType.EOF)
          {
            try
            {
              GetNextToken();
            }
            catch (FormatException)
            {
            }
          }
        }
      }

      WebLink link = new WebLink(BaseUrl, url, rel, type, title_s ?? title);
      return link;
    }


    protected KeyValuePair<string, string> ParseParameter(out bool isExtended)
    {
      if (NextToken.Type != TokenType.Identifier && NextToken.Type != TokenType.ExtendedIdentifier)
        Error(string.Format("Unexpected token '{0}' (expected an identifier)", NextToken.Type));
      string id = NextToken.Value;
      isExtended = (NextToken.Type == TokenType.ExtendedIdentifier);
      GetNextToken();

      if (NextToken.Type != TokenType.Assignment)
        Error(string.Format("Unexpected token '{0}' (expected an assignment)", NextToken.Type));

      if (id == "rel")
      {
        GetNextStringOrRelType();
      }
      else
      {
        GetNextToken();
      }

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


    protected void GetNextStringOrRelType()
    {
      NextToken = ReadNextStringOrRelType();
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


    protected Token ReadNextStringOrRelType()
    {
      while (true)
      {
        char? c = ReadNextChar();

        if (c == null)
          return new Token { Type = TokenType.EOF };

        if (c == '"')
          return new Token { Type = TokenType.String, Value = ReadString() };

        if (Char.IsLetter(c.Value))
          return new Token { Type = TokenType.String, Value = ReadRelType(c.Value) };

        Error(string.Format("Unrecognized character '{0}' for string or rel-type", c));
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


    protected string ReadRelType(char c)
    {
      string id = "" + c;

      while (Char.IsLetterOrDigit(InputString[InputPos]) || InputString[InputPos] == '.' || InputString[InputPos] == '-')
      {
        id += InputString[InputPos++];
      }

      return id;
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
