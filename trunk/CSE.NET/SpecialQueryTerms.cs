namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SpecialQueryTerms
    {
        /// <summary>
        /// Gets or sets a query term that retrieves a set of Web pages that link to a particular URL.<para/>
        /// You can also use QueryParameters.AdvancedSearchLinkQuery to submit a BackLinks request.<para/>
        /// Note: You cannot specify any other query terms when using BackLinks. <para/>
        /// If a value is specified for BackLinks all other QueryParameters will be ignored.
        /// </summary>
        public Uri BackLinks { get; set; }

        public List<string> ExcludeQueryTerms { get; private set; }

        /// <summary>
        /// Gets a list of words which restricts search results to documents that contain a particular word in the document URL.
        /// </summary>
        public List<string> InUrl { get; private set; }

        public SpecialQueryTerms()
        {
            this.ExcludeQueryTerms = new List<string>();
            this.InUrl = new List<string>();
        }
    }
}
