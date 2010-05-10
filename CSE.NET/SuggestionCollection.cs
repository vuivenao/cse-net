namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SuggestionCollection
    {
        public string Label { get; internal set; }

        public string Query { get; internal set; }

        public SuggestionCollection()
        {

        }

        public SuggestionCollection(string label, string query)
        {
            this.Label = label;
            this.Query = query;
        }
    }
}
