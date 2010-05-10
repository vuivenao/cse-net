namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Suggestion
    {
        public Suggestion()
        {
        }

        public Suggestion(string label, string query)
        {
            this.Label = label;
            this.Query = query;
        }

        public string Label { get; internal set; }

        public string Query { get; internal set; }
    }
}
