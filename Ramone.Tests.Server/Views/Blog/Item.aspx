<%@ Import Namespace="Ramone.Tests.Server.Handlers.Blog" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<BlogItem>" %>
<html>
  <body>
    <h1><%=Resource.Title %></h1>
    <p>Author: <a rel="author" href="<%=Resource.AuthorLink%>"><%=Resource.AuthorName%></a></p>
    <p><%=Resource.Text %></p>
    <p><a href="<%=Resource.SelfLink %>" rel="self">Permalink</a></p>
    <p><a href="<%=Resource.UpLink %>" rel="up">Bloglist</a></p>
  </body>
</html>