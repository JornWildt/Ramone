using System;
using System.IO;
using HtmlAgilityPack;
using Ramone.MediaTypes;


namespace Ramone.Tests.Blog.Codecs.Html
{
  public abstract class BaseCodec_Html<T> : TextCodecBase<T> where T : class
  {
    protected override T ReadFrom(TextReader reader, ReaderContext context)
    {
      HtmlDocument doc = new HtmlDocument();
      doc.Load(reader);
      return ReadFromHtml(doc);
    }


    protected override void WriteTo(T item, System.IO.TextWriter writer, WriterContext context)
    {
      throw new NotImplementedException();
    }


    public abstract T ReadFromHtml(HtmlDocument html);
  }
}
