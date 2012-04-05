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


DOCUMENTATION
-------------
Not much so far. There's a few blog posts:

  Consuming Web APIs in C# with Ramone
  http://soabits.blogspot.com/2012/04/consuming-web-apis-in-c-with-ramone.html

  Introducing the Ramone C# Library for Web API Clients 
  http://soabits.blogspot.com/2012/04/introducing-ramone-c-library-for-web.html

  Ramone: Consuming Hyper-Media REST Services in C#
  http://soabits.blogspot.com/2012/04/ramone-consuming-hyper-media-rest.html


SIMILAR PROJECTS
----------------
- RestSharp http://restsharp.org/
- Ramone https://github.com/JornWildt/Ramone (Disclaimer: my project)
- RestFulie http://restfulie.caelum.com.br/
- RESTAgent http://restagent.codeplex.com/
- Dynamic REST https://github.com/Huddle/dynamicrest
- Spring.NET Rest client http://www.springframework.net/rest/
- Microsoft WCF HttpClient http://wcf.codeplex.com/
- Hammock (discontinued by the developer) https://github.com/danielcrenna/hammock


CREDITS
-------
Thanks to the OpenRasta (openrasta.org) and Restfulie 
(restfulie.caelum.com.br) projects for inspiration.

Thanks to cBrain (www.cbrain.com) for investing some hours in the project.

Ramone includes distributions of:

  JsonFx.Net
    Source: https://github.com/jsonfx/jsonfx
    licens (MIT): http://www.opensource.org/licenses/MIT

  Html Agility Pack
    Source: http://htmlagilitypack.codeplex.com/
    License (Ms-PL): http://htmlagilitypack.codeplex.com/license

  Cutting Edge Conditions
    Source: http://conditions.codeplex.com/
    License (MIT): http://conditions.codeplex.com/license


LICENSE
-------
Ramone is distributed under the MIT License: http://www.opensource.org/licenses/MIT
A copy of this license is included in the file LICENSE.TXT.
