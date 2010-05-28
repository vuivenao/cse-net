namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Net;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Encapsulates an asynchronous search call.
    /// </summary>
    /// <param name="queryParameters">The query parameters to be used to perform the search.</param>
    /// <returns>Returns a SearchResult object containing the results of the search.</returns>
    public delegate SearchResult AsyncSearch(QueryParameters queryParameters);

    /// <summary>
    /// Allows applications to search using the Google Custom Search API.
    /// </summary>
    public class CustomSearchClient
    {
        /// <summary>
        /// The root URL or the Google CSE request.
        /// </summary>
        private const string GOOGLE_URL = "http://www.google.com/search?";

        /// <summary>
        /// Initializes static members of the CustomSearchClient class.
        /// </summary>
        static CustomSearchClient()
        {
            CustomSearchClient.Count = GetCountFromConfig();
        }

        /// <summary>
        /// Initializes a new instance of the CustomSearchClient class with 
        /// default values taken from the application config file.
        /// </summary>
        public CustomSearchClient()
        {
            GoogleCustomSearchSection section = ConfigurationManager.GetSection("googleCustomSearch") as GoogleCustomSearchSection;
            if (section == null)
            {
                throw new NullReferenceException("The value 'null' was found where an instance of GoogleCustomSearchConfigSection was expected. Please check the applications config file and ensure thre is a googleCustomSearch configuration section.");
            }

            this.CseID = section.CseId;
        }

        /// <summary>
        /// Initializes a new instance of the CustomSearchClient class specifying the CSE ID to be used.
        /// </summary>
        /// <param name="cseId">The Google CSE ID to use when initializing this instance of the CustomSearchClient.</param>
        public CustomSearchClient(string cseId)
        {
            this.CseID = cseId;
        }

        /// <summary>
        /// Gets the number of results to return per search.
        /// This is initialized to the value in the application config or 10 if it is not present.
        /// </summary>
        public static int Count { get; private set; }

        /// <summary>
        /// Gets the Google Custom Search-engine ID.
        /// </summary>
        public string CseID { get; private set; }

        /// <summary>
        /// Performs a search using the query parameters specified.
        /// </summary>
        /// <param name="queryParameters">A Google.CustomSearch.QueryParameters that specifies the search to be performed.</param>
        /// <returns>Returns an Google.CustomSearch.SearchResult containing the results of the search.</returns>
        public SearchResult Search(QueryParameters queryParameters)
        {
            Uri requestUri = this.FormatRequest(queryParameters);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return this.ProcessResponse(queryParameters, response);
        }

        /// <summary>
        /// Performs an asynchronous search using the query parameters specified.
        /// On completion the AsyncCallback specified as callback is notified.
        /// </summary>
        /// <param name="queryParameters">A Google.CustomSearch.QueryParameters that specifies the search to be performed.</param>
        /// <param name="callback">A System.AsyncCallback object that will be notified when the call completes.</param>
        /// <returns>Returns a System.IAsyncResult that can be used to monitor the status of the asynchronous call.</returns>
        public IAsyncResult SearchAsync(QueryParameters queryParameters, AsyncCallback callback)
        {
            AsyncSearch caller = new AsyncSearch(this.Search);

            // TODO: What should be passed as the state object? is the Filter.Label appropriate?
            return caller.BeginInvoke(queryParameters, callback, queryParameters.Filter.Label);
        }

        /// <summary>
        /// Gets the default number of results per search from the applicaiton config.
        /// </summary>
        /// <returns>Returns the number of resutls to return per search.</returns>
        private static int GetCountFromConfig()
        {
            GoogleCustomSearchSection section = ConfigurationManager.GetSection("googleCustomSearch") as GoogleCustomSearchSection;
            if (section == null)
            {
                // TODO: Throw the proper exception
                throw new ConfigurationErrorsException("The application configuration file must contain an 'googleCustomSearch' section");
            }

            return section.CountPerPage;
        }

        /// <summary>
        /// Processes the HTTP Response from Google CSE and produces a 
        /// Google.CustomSearch.SearchResult containing the supplied data.
        /// </summary>
        /// <param name="queryParameters">The parameters which were used to perform the query.</param>
        /// <param name="response">The HttpWebResponse recieved from Google CSE.</param>
        /// <returns>Returns a SearchResult containing the search results.</returns>
        private SearchResult ProcessResponse(QueryParameters queryParameters, HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("The request was borked");
            }

            SearchResult retval = new SearchResult(queryParameters);
            new ResponseParser().Parse(retval, response.GetResponseStream());

            return retval;
        }

        /// <summary>
        /// Generates the Uri to be used for the request to Google CSE.
        /// </summary>
        /// <param name="input">The query parameters to format in the URL.</param>
        /// <returns>Returns a System.Uri to request the search results from Google CSE.</returns>
        private Uri FormatRequest(QueryParameters input)
        {
            StringBuilder url = new StringBuilder(GOOGLE_URL);
            Dictionary<string, string> queryParameters = new Dictionary<string, string>();

            queryParameters.Add("cx", this.CseID);
            queryParameters.Add("client", "google-csbe");
            queryParameters.Add("q", input.SearchTerm);
            if (input.Filter != null)
            {
                queryParameters["q"] += "+more:" + input.Filter.Label;
            }

            foreach (string word in input.SpecialQueryTerms.InUrl)
            {
                queryParameters["q"] += "+inurl:" + word;
            }

            queryParameters.Add("num", input.Count.ToString());
            queryParameters.Add("start", input.Start.ToString());

            if (OutputFormat.Xml == input.Format)
            {
                queryParameters.Add("output", "xml");
            }
            else
            {
                queryParameters.Add("output", "xml_no_dtd");
            }

            if (SafeSearch.Off != input.Safe)
            {
                string value = "medium";
                if (SafeSearch.High == input.Safe)
                {
                    value = "high";
                }

                queryParameters.Add("safe", value);
            }

            // Special search options
            bool firstType = true;
            foreach (var pair in input.FileTypes)
            {
                string type = string.Format(" filetype:{0}", pair.Key.TrimStart('.'));
                if (!pair.Value) type = " -" + type.TrimStart(); 
                if (!firstType) type = " OR" + type;
                queryParameters["q"] += type;
                firstType = false;
            }

            // Advanced search options
            if (null != input.AdvancedSearchSite)
            {
                queryParameters.Add("as_sitesearch", input.AdvancedSearchSite.ToString());
            }

            if (null != input.AdvancedSearchSite)
            {
                string value = "i";
                if (DomainInclusion.Exclude == input.AdvancedSearchDomainInclusion)
                {
                    value = "e";
                }

                queryParameters.Add("as_dt", value);
            }

            if (!string.IsNullOrEmpty(input.AdvancedSearchPhraseQuery))
            {
                queryParameters.Add("as_epq", input.AdvancedSearchPhraseQuery);
            }

            if (!string.IsNullOrEmpty(input.AdvancedSearchExcludePhraseQuery))
            {
                queryParameters.Add("as_eq", input.AdvancedSearchExcludePhraseQuery);
            }

            if (null != input.AdvancedSearchRequiredLink)
            {
                queryParameters.Add("as_lq", input.AdvancedSearchRequiredLink.ToString());
            }

            foreach (var pair in queryParameters)
            {
                url.Append(pair.Key + "=" + HttpUtility.UrlEncode(pair.Value) + "&");
            }

            return new Uri(url.ToString().TrimEnd('&'));
        }
    }
}
