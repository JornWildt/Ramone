<%@ Import Namespace="Ramone.Tests.Server.Blog.Resources" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<Search>" %>
 <?xml version="1.0" encoding="UTF-8"?>
 <OpenSearchDescription xmlns="http://a9.com/-/spec/opensearch/1.1/">
   <ShortName>Blog Search</ShortName>
   <Description>Searching for blogs.</Description>
   <Contact>jw@fjeldgruppen.dk</Contact>
   <Url type="application/rss+xml" 
        template="<%=Resource.Template %>"/>
 </OpenSearchDescription>