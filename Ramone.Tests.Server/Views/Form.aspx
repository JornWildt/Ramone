<%@ Import Namespace="Ramone.Tests.Server.Handlers" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<TestForm>" %>
<html>
  <body>
    <h1>Create new blog item</h1>
    <div>
      <form id="create" method="post" action="<%=Resource.ActionUrl %>" enctype="multipart/form-data">
        <p>
          <label for="Title">Title:</label>
          <input id="Title" name="Title" />
        </p>
        <p>
          <label for="Text">Text:</label>
          <textarea id="Text" name="Text"></textarea>
        </p>
        <p>
          <input type="submit" name="Create" Value="Create"/>
          <input type="submit" name="Cancel" Value="Cancel"/>
        </p>
      </form>
    </div>
  </body>
</html>