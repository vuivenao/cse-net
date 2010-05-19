namespace CseTestRig
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using Google.CustomSearch;

    class Program
    {
        static void Main(string[] args)
        {
            CustomSearchClient client = new CustomSearchClient();

            SearchResult results = client.Search(new QueryParameters()
            {
                Count = 20,
                //Format = OutputFormat.XmlNoDtd,
                SearchTerm = "concrette",
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
                // Console.WriteLine(f.GetAvailableCount() + " results available for " + f.AnchorText);
                var p = results.Parameters.Clone();
                p.Filter = f;
                client.SearchAsync(p, new AsyncCallback(Callback));
            }

            Console.WriteLine();

            foreach (Result r in results.Results)
            {
                FormatAndDisplay(r);
            }

            Console.ReadKey();
        }

        public static void Callback(IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncResult result = (AsyncResult)ar;
            AsyncSearch caller = (AsyncSearch)result.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            string someState = (string)ar.AsyncState;

            // Call EndInvoke to retrieve the results.
            SearchResult returnValue = caller.EndInvoke(ar);

            Console.WriteLine("Callback for " + returnValue.Parameters.Filter.Label);
            // Use the format string to format the output message.
            Console.Write(someState + " ");
            Console.Write(returnValue.Time + " ");
            Console.WriteLine(returnValue.Total);
        }

        private static void FormatAndDisplay(Result r)
        {
            Console.WriteLine("Result " + r.Id + " of type " + r.MimeType.ToString());
            Console.WriteLine(r.Title + " Crawled on " + r.CrawlDate.ToLongDateString());
            Console.WriteLine(r.Excerpt);
            Console.WriteLine(r.Uri);
            Console.WriteLine();
        }
    }
}
