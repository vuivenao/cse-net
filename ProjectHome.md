# CSE.NET - C# API wrapper for Google Custom Search #
CSE.NET is an open source client library for [Google Custom Search](http://www.google.co.uk/cse/) and [Google Site Search](http://www.google.com/sitesearch/) for business.

This project is still in its early stages but fulfils the need for a .NET client library for Google Site Search and Google Custom Search.

As of writing ([revision 31](http://code.google.com/p/cse-net/source/browse/?r=31)) the library implements searching using the `CustomSearchClient` and filtering via Facets.

There are asynchronous versions of the main `Search()` method and around half of the classes have documentation comments.

It's still early days and suggestions and contributions are more than welcome.

**TODO:**
  * Implement all features as defined by [XML tags in Google docs](http://www.google.com/cse/docs/resultsxml.html#XML_Results_for_Regular_and_Advanced_Search_Queries).
  * Refine the implementation based on real world usage.
  * Code documentation (mostly done, needs publishing to HTML).
  * Usage documentation and examples.
  * Package binary releases.
  * Project website.