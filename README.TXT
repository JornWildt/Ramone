-------------------------------------------------------
Ramone - A C# framework for working with HTTP/REST APIs
-------------------------------------------------------

The purpose of Ramone is to simplify access to HTTP based APIs and make it 
easier to consume both truely REST APIs as well as the more POX-ish RPC 
style APIs.

What Ramone does is to wrap the inner workings of HTTP and make encoding and 
decoding easier through the use of codecs for the various formats used on the 
web.

Ramone supports plain XML, JSON, and Atom out of the box, but where it really
excels is when it comes to working with typed objects. In its most simple form 
you can transfer an object like this:

  class Cat
  {
    public string Name { get; set; }
    public DateTime Born { get; set; }
  }

  RamoneRequest req = Session.Bind(CatUrlTemplate, new { id = 8 });

  Cat c = req.Get<Cat>().Body;

The name "Ramone" was inspired by the character "Ramone" in the movie "Cars".
In the movie Ramone is an easy going car. Hopefully you will find using the 
Ramone library just as easy going. Happy hacking.


SIMILAR PROJECTS
----------------
- http://restsharp.org/
- http://www.restlet.org/
- http://restfulie.caelum.com.br/
- http://restagent.codeplex.com/
- https://github.com/Huddle/dynamicrest
- http://msdn.microsoft.com/en-us/netframework/dd547388


CREDITS
-------
Thanks to the OpenRasta (openrasta.org) and Restfulie 
(restfulie.caelum.com.br) projects for inspiration.

Thanks to cBrain (www.cbrain.com) for investing some hours in the project.


