<%@ Import Namespace="Ramone.Tests.Server.Handlers.Blog" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<BlogList>" %>
<html>
  <body>
    <h1><%=Resource.Title %></h1>
    <p>Author: <a rel="author" href="<%=Resource.AuthorLink%>"><%=Resource.AuthorName%></a></p>
    <% foreach (BlogItem item in Resource.Items) { %>
      <div class="post">
        <h2><a class="post-title" href="<%=item.SelfLink%>"><%=item.Title %></a></h2>
        <p class="post-content"><%=item.Text %></p>
      </div>
    <% } %>
  </body>
</html>