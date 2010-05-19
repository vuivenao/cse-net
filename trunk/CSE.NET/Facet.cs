namespace Google.CustomSearch
{
    using System;

    /// <summary>
    /// Facet objects encapsulate information about a refinement label associated with a set of search results.
    /// </summary>
    public class Facet
    {
        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.Facet class specifying the label.
        /// </summary>
        /// <param name="label">The label of this Facet.</param>
        public Facet(string label)
        {
            this.Label = label;
        }

        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.Facet class specifying the 
        /// QueryParameters that were used to generate the search to which this Facet is related.
        /// </summary>
        /// <param name="parameters">The QueryParameters to which this Facet is related.</param>
        public Facet(QueryParameters parameters)
        {
            this.Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.Facet class specifying the label
        /// and the anchor text of this Facet.
        /// </summary>
        /// <param name="label">The label of this Facet.</param>
        /// <param name="anchorText">The anchor text of this Facet.</param>
        public Facet(string label, string anchorText)
        {
            this.Label = label;
            this.AnchorText = anchorText;
        }

        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.Facet class with no information specified.
        /// </summary>
        internal Facet()
        {
        }

        /// <summary>
        /// Gets the refinement label that you can use to filter the search results that you receive.
        /// </summary>
        public string Label { get; internal set; }

        /// <summary>
        /// Gets the text that you should display to users to identify a refinement label associated with a search result set.
        /// Since refinement Facet.Label replaces nonalphanumeric characters with underscores, 
        /// you should not display the value of the Facet.Label in your user interface. 
        /// Instead, you should display the value of the Facet.AnchorText.
        /// </summary>
        public string AnchorText { get; internal set; }

        /// <summary>
        /// Gets the QueryParameters that were used to generate the search to which this Facet is related.
        /// </summary>
        public QueryParameters Parameters { get; internal set; }
    }
}
