<%@ Import Namespace="Ramone.Tests.Server.Handlers" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<TestForm>" %>
<html>
<!-- Using different kinds of input closing tags, just to see what happens -->
  <body>
    <h1>Create new blog item</h1>
    <div>
      <form id="create" method="post" <%= Resource.ActionUrl != null ? "action=\"" + Resource.ActionUrl + "\"" : "" %> enctype="multipart/form-data">
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
          <SELECT name="Select" id="Select">
            <option value="1">One</option>
            <option value="2" selected>Two</option>
            <option value="3">Three</option>
          </SELECT>
        </p>
        <p>
          <label for="MultiSelect">MultiSelect:</label>
          <SELECT name="MultiSelect" id="MultiSelect" multiple="multiple">
            <option value="A">Aaa</option>
            <option value="B" selected>Bbb</option>
            <option value="C" selected="selected">Ccc</option>
            <option value="D">Ddd</option>
          </SELECT>
        </p>
        <p>
          <label for="Radio1a">Radio1a:</label>
          <input type="radio" name="Radio1" value="1a" id="Radio1a"></input>
          <label for="Radio1b">Radio1b:</label>
          <input type="radio" name="Radio1" value="1b" id="Radio1b" checked="checked"></input>
          <label for="Radio1c">Radio1c:</label>
          <input type="radio" name="Radio1" value="1c" id="Radio1c"></input>
        </p>
        <p>
          <label for="Radio2a">Radio2a:</label>
          <input type="radio" name="Radio2" value="2a" id="Radio2a"></input>
          <label for="Radio2b">Radio2b:</label>
          <input type="radio" name="Radio2" value="2b" id="Radio2b"></input>
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