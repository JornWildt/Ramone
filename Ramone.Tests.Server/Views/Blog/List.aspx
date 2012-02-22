<%@ Import Namespace="Ramone.Tests.Server.Handlers.Blog" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<BlogList>" %>
<html>
  <body>
    <h1><%=Resource.Title %></h1>
    <% foreach (BlogItem item in Resource.Items) { %>
      <h2><a href="<%=item.SelfLink%>"><%=item.Title %></a></h2>
      <p><%=item.Text %></p>
    <% } %>
  </body>
</html>