---------------------------------------------------------
RastaClient - A .NET framework for working with HTTP APIs
---------------------------------------------------------

The purpose of the RastaClient framework is to simplify access to HTTP based
APIs and make it easier to consume both truely REST APIs as well as the more
POX-ish RPC style APIs.

What RastaClient does is to wrap the inner workings of HTTP and make 
serialization and de-serialization easier through the use of codecs for the
various formats used on the web.

RastaClient supports plain XML and JSON out of the box, but where it really
excels is when it comes to object serialization. In its most simple form you
can transfer an object like this:

  class Cat
  {
    public string Name { get; set; }
    public DateTime Born { get; set; }
  }

  RastaRequest req = Session.Bind(CatUrlTemplate, new { id = 8 });

  Cat c = req.Get<Cat>().Body;


SIMILAR PROJECTS
----------------


CREDITS
-------
Thanks to the OpenRasta (openrasta.org) and Restfulie 
(restfulie.caelum.com.br) projects for inspiration.

Thanks to cBrain (www.cbrain.com) for invensting some hours in the project.


