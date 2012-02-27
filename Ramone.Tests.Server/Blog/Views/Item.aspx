<%@ Import Namespace="Ramone.Tests.Server.Blog.Resources" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<BlogItem>" %>
<html>
  <body>
    <h1 class="post-title"><%=Resource.Title %></h1>
    <p>Author: <a rel="author" href="<%=Resource.AuthorLink%>"><%=Resource.AuthorName%></a></p>
    <% if (Resource.ImageLink != null) { %>
      <img src="<%=Resource.ImageLink %>" />
    <% } %>
    <p class="post-content"><%=Resource.Text %></p>
    <p><a href="<%=Resource.SelfLink %>" rel="self">Permalink</a></p>
    <p><a href="<%=Resource.UpLink %>" rel="up">Bloglist</a></p>
  </body>
</html>