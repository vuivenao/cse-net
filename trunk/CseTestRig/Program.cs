namespace CseTestRig
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Google.CustomSearch;

    class Program
    {
        static void Main(string[] args)
        {
            CustomSearchClient client = new CustomSearchClient();

            SearchResult results = client.Search(new QueryParameters()
            {
                Count = 10,
                Format = OutputFormat.XmlNoDtd,
                SearchTerm = "concrete",
                Start = 0,
                // TODO: Possibly swap encoding for a built in type
                Encoding = "utf8"
            });

            Console.WriteLine(results.Title);
            Console.WriteLine(results.Total + " results found in " + results.Time + " seconds");
            Console.WriteLine("Displaying results " + results.StartIndex + " to " + results.EndIndex);
            Console.WriteLine();

            Console.WriteLine("Facets");
            foreach (Facet f in results.Facets)
            {
                Console.WriteLine(f.GetAvailableCount() + " results available for " + f.AnchorText);
            }

            Console.WriteLine();

            foreach (Result r in results.Results)
            {
                FormatAndDisplay(r);
            }

            Console.ReadKey();
        }

        private static void FormatAndDisplay(Result r)
        {
            Console.WriteLine("Result " + r.Id);
            Console.WriteLine(r.Title);
            Console.WriteLine(r.Excerpt);
            Console.WriteLine(r.Uri);
            Console.WriteLine();
        }
    }
}
