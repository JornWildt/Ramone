<%@ Import Namespace="Ramone.Tests.Server.Handlers.Blog" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<BlogItemCreationDescriptor>" %>
<html>
  <body>
    <h1>Create new blog item</h1>
    <div>
      <form id="create" method="post" action="<%=Resource.PostLink %>" >
        <p>
          <label for="Title">Title:</label>
          <input id="Title" name="Title" />
        </p>
        <p>
          <label for="Text">Text:</label>
          <textarea id="Text" name="Text"></textarea>
        </p>
        <p>
          <input type="submit" name="Create" />
        </p>
      </form>
    </div>
  </body>
</html>