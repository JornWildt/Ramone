-----------------------------------------------------------------
Ramone - A C# library for working with REST services and Web APIs
-----------------------------------------------------------------

Ramone is a C# library that simplifies access to HTTP based Web APIs and REST
services. It has a strong focus on REST and implements elements of the Uniform
Interface as first class citizens of the API.

This means natural support for

- URIs as identifiers for resources.
- The standard HTTP methods GET, POST, PUT and more.
- Multiple media types (XML, JSON, HTML, ATOM and more).
- User defined media types.
- Hyper media controls (linking and key/value forms).

What Ramone does is to wrap the inner workings of HTTP (using .NET's 
HttpWebRequest/HttpWebResponse classes) and make encoding and decoding easier 
through the use of codecs for the various formats used on the web.

In its most simple form you can GET a resource like this:

  // Define resource type
  class Cat
  {
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
  }

  // Create session pointing to service root
  ISession Session = RamoneConfiguration.NewSession(new Uri("http://cat-example.com"));

  // Setup HTTP request
  Request req = Session.Bind("/cat/{name}", new { name = "Mike" });

  // Make actual request
  using (var resp = req.AcceptXml().Get<Cat>())
  {
    Cat c = resp.Body;
    Console.WriteLine("Cat with name={0} was born on={1:d}.", c.Name, c.DateOfBirth);
  }

See more examples here: https://github.com/JornWildt/Ramone/wiki/Examples.

Happy hacking.

Jorn Wildt


DOWNLOAD
--------
Binaries are available on NuGet and at these links:

  Version 1.0: http://elfisk.dk/Ramone/Ramone-1.0.0.zip

  Nightly build: http://elfisk.dk/Ramone/Ramone.zip


DOCUMENTATION
-------------
Some of the documentation is available as a PDF (distributed with the binaries).

In addition to this there is a few blog posts:

  Consuming Web APIs in C# with Ramone
  http://soabits.blogspot.com/2012/04/consuming-web-apis-in-c-with-ramone.html

  Introducing the Ramone C# Library for Web API Clients 
  http://soabits.blogspot.com/2012/04/introducing-ramone-c-library-for-web.html

  Ramone: Consuming Hyper-Media REST Services in C#
  http://soabits.blogspot.com/2012/04/ramone-consuming-hyper-media-rest.html

  JSON-Patch support in Ramone 
  http://soabits.blogspot.dk/2013/01/json-patch-support-in-ramone.html

  Using Ramone for OAuth2 authorization with Google APIs
  http://soabits.blogspot.dk/2013/03/using-ramone-for-oauth2-authorization.html

The name "Ramone" was inspired by the character "Ramone" in the movie "Cars".
In the movie Ramone is an easy going car. Hopefully you will find using the 
Ramone library just as easy going. You are of course also free to associate 
Ramone with the punk band "The Ramones" if that suits you better :-)


USING
-----
To use Ramone in a .NET project you must first reference the Ramone.dll
assembly. This assembly contains both the core Ramone code as well as supporting
libraries (JsonFx, HtmlAgilityPack and more).

If you already have the supporting libraries in your code base then you should
reference Ramone.Core.dll which contains only the core Ramone code.


BUILDING
--------
Here is how to build Ramone from scratch:

1) Download the source from GitHub (https://github.com/JornWildt/Ramone).
2) Open "Ramone.sln" and build the whole project.
   - This builds the core Ramone.dll and additional test projects
3) Merge Ramone.dll and supporting libraries into one dll.
   a) Install ilmerge.exe from Microsoft.
   b) Fix ilmerge path in post-build.bat.
   c) Run post-build.bat.


SIMILAR PROJECTS
----------------
- RestSharp http://restsharp.org/
- RestFulie http://restfulie.caelum.com.br/
- RESTAgent http://restagent.codeplex.com/
- Dynamic REST https://github.com/Huddle/dynamicrest
- Spring.NET Rest client http://www.springframework.net/rest/
- Microsoft WCF HttpClient http://wcf.codeplex.com/
- Hammock (discontinued by the developer) https://github.com/danielcrenna/hammock


CREDITS
-------
Thanks to the OpenRasta (openrasta.org) and Restfulie (restfulie.caelum.com.br)
projects for inspiration.

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
