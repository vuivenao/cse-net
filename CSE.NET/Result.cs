namespace Google.CustomSearch
{
    using System;

    public class Result
    {
        /// <summary>
        /// Gets the index of this result within the result set.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Gets the URI of this search result.
        /// </summary>
        public Uri Uri { get; internal set; }

        /// <summary>
        /// Gets the URL-escaped so that it is suitable for passing as a query parameter in a URL.
        /// </summary>
        public string EscapedUrl { get; internal set; }

        /// <summary>
        /// Gets the title of this search result.
        /// </summary>
        public string Title { get; internal set; }

        /// <summary>
        /// Gets an excerpt for a search result that shows query terms highlighted in bold. Line breaks are included in the excerpt for proper text wrapping.
        /// </summary>
        public string Excerpt { get; internal set; }

        public DateTime CrawlDate { get; internal set; }
    }
}