namespace Google.CustomSearch
{
    /// <summary>
    /// Represents an alternate spelling suggestion for the submitted query.
    /// </summary>
    public class Suggestion
    {
        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.Suggestion class.
        /// </summary>
        public Suggestion()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Google.CustomSearch.Suggestion class
        /// specifying the label and query for the suggestion.
        /// </summary>
        /// <param name="label">The label to use when displaying this suggestion.</param>
        /// <param name="query">The query value of this suggestion.</param>
        public Suggestion(string label, string query)
        {
            this.Label = label;
            this.Query = query;
        }

        /// <summary>
        /// Gets the text to display to the user when using this suggestion.
        /// </summary>
        public string Label { get; internal set; }

        /// <summary>
        /// Gets the query value to use when this suggestion is applied.
        /// </summary>
        public string Query { get; internal set; }
    }
}
