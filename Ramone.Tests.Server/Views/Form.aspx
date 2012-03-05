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
        <p>
          <label for="TextArea">TextArea:</label>
          <textarea name="TextArea">textarea</textarea>
        </p>
        <p>
          <label for="Select">Select:</label>
          <SELECT name="Select">
            <option value="1">One</option>
            <option value="2" selected>Two</option>
            <option value="3">Three</option>
          </SELECT>
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