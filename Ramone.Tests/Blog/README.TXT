These tests illustrates the difference between using one of the generic formats
(HTML as HtmlDocument) and a dedicated codec. In addition to this you can see
how Ramone handles hyper-media elements such as links (HTML anchors) and 
key/value forms (HTML forms) with file upload.

In both examples the actual wire format is HTML. The difference lies in how the
resources are represented internally on the client.

In HtmlDocumentTests.cs you can see how it is done when everything is 
represented as HtmlDocument internally.

In TypedHtmlTests you can see how the interaction with the blogging website
looks when the actually parsing of the page content is moved from the 
application code into dedicated codecs.

The codecs can be found in the folder "Codecs" and the client side 
representation of the resources in "Resources".

By working with codecs you gain a few advantages over the raw interaction with
the wire format:

  1) The application code is completely decoupled from the wire format. This
     gives you the ability to work with different wire formats without changing
     the application code

  2) It results in more readable application code.

  3) It makes the parsing code resuable accross difference pieces of 
     application code.

The downside of codes is that you have to write a few more pieces of boiler-
plate code to create and register the codecs. You also have to implement the
client side representation of the resources as a specific class. But in the end
it all pays off, in my opinion, and yields more readable and maintainable code.
