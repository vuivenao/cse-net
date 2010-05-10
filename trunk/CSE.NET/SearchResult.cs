namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SearchResult
    {
        public int StartIndex { get; internal set; }

        public int EndIndex { get; internal set; }

        public bool Exact { get; internal set; }

        public int Total { get; internal set; }

        public string Time { get; internal set; }

        public string Title { get; internal set; }

        public Uri NextPageLink { get; internal set; }

        public Uri PreviousPageLink { get; internal set; }

        public ResultCollection Results { get; private set; }

        public FacetCollection Facets { get; private set; }

        public QueryParameters Parameters { get; internal set; }

        public SuggestionCollection Suggestions { get; internal set; }

        internal SearchResult()
        {
            this.Facets = new FacetCollection();
            this.Results = new ResultCollection();
            this.Suggestions = new SuggestionCollection();
        }

        internal SearchResult(QueryParameters parameters)
            : this()
        {
            this.Parameters = parameters;
        }

        public SearchResult(ResultCollection results)
        {
            this.Results = results;
        }
    }
}
