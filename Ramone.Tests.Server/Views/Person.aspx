<%@ Import Namespace="Ramone.Tests.Common" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<Person>" %>
<html>
  <body>
    <h1>Person</h1>
    <div class="Person">
      <ul>
        <li class="Name"><%=Resource.Name%></li>
        <li class="Address"><%=Resource.Address%></li>
      </ul>
    </div>
  </body>
</html>