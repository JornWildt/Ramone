<%@ Import Namespace="Ramone.Tests.Server.Handlers" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<TestForm>" %>
<html>
<!-- Using different kinds of input closing tags, just to see what happens -->
  <body>
    <h1>Create new blog item</h1>
    <div>
      <form id="create" method="post" action="<%=Resource.ActionUrl %>" enctype="multipart/form-data">
      <div>
        <p>
          <label for="InputText">InputText:</label>
          <input id="InputText" name="InputText" value="text"></input>
        </p>
        <p>
          <label for="InputPassword">InputPassword:</label>
          <input id="InputPassword" name="InputPassword" type="password" value="password"/>
        </p>
        <p>
          <label for="InputCheckbox">InputCheckbox:</label>
          <input id="InputCheckbox" name="InputCheckbox" value="checkbox">
        </p>
        <input name="InputHidden" type="hidden" value="hidden"></input>
        <p>
          <input type="submit" name="Save" value="Save">
          <input type="submit" name="Cancel" value="Cancel"/>
          <input type="submit" name="Help" value="Help" id="help-button"/>
        </p>
      </div>
      </form>
    </div>
  </body>
</html>