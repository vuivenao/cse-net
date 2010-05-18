namespace Google.CustomSearch
{
    /// <summary>
    /// A SearchResult contains the results of a request to Google CSE.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.SearchResult class specifying 
        /// the ResultCollection to populate this SearchResult.
        /// </summary>
        /// <param name="results">The ResultCollection to populate this SearchResult</param>
        public SearchResult(ResultCollection results)
            : this()
        {
            this.Results = results;
        }

        internal SearchResult()
        {
            this.Facets = new FacetCollection();
            this.Results = new ResultCollection();
            this.Suggestions = new SuggestionCollection();
            this.Parameters = new QueryParameters();
        }

        internal SearchResult(QueryParameters parameters)
            : this()
        {
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the index (1-based) of the first search result returned in this result set.
        /// </summary>
        public int StartIndex { get; internal set; }

        /// <summary>
        /// Gets the index (1-based) of the last search result returned in this result set.
        /// </summary>
        public int EndIndex { get; internal set; }

        /// <summary>
        /// Gets a boolean indicating that the estimated total number of results, as specified by the Total property, 
        /// actually represents the exact total number of results. 
        /// See the <see cref="http://www.google.com/cse/docs/resultsxml.html#automaticFiltering">Automatic Filtering</see> section of this document for more details.
        /// </summary>
        public bool Exact { get; internal set; }

        /// <summary>
        /// Gets a flag that indicates whether document filtering was performed for the search. 
        /// See the <see cref="http://www.google.com/cse/docs/resultsxml.html#automaticFiltering">Automatic Filtering</see> section of this document for more information about Google's search results filters.
        /// </summary>
        public bool Filtered { get; internal set; }

        /// <summary>
        /// Gets the estimated total number of results for the search.
        /// </summary>
        public int Total { get; internal set; }

        /// <summary>
        /// Gets the total server time needed to return search results, measured in seconds.
        /// </summary>
        public string Time { get; internal set; }

        /// <summary>
        /// Gets the name of your Custom Search Engine.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the relative link to the next page of search results.
        /// </summary>
        public string NextPageLink { get; internal set; }

        /// <summary>
        /// Gets the relative link to the previous page of search results.
        /// </summary>
        public string PreviousPageLink { get; internal set; }

        /// <summary>
        /// Gets the search results.
        /// </summary>
        public ResultCollection Results { get; private set; }

        /// <summary>
        /// Gets the refinement labels associated with this set of search results.
        /// </summary>
        public FacetCollection Facets { get; private set; }

        /// <summary>
        /// Gets the parameters that were used to generate this set of search results.
        /// </summary>
        public QueryParameters Parameters { get; internal set; }

        /// <summary>
        /// Gets the alternate spelling suggestions for the submitted query.
        /// </summary>
        public SuggestionCollection Suggestions { get; internal set; }
    }
}
