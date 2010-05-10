﻿namespace Google.CustomSearch
{
    public class Facet
    {
        public Facet(string label)
        {
            this.Label = label;
        }

        public Facet(QueryParameters parameters)
        {
            this.Parameters = parameters;
        }

        public Facet(string label, string anchorText)
        {
            this.Label = label;
            this.AnchorText = anchorText;
        }

        internal Facet()
        {
        }

        public string Label { get; internal set; }

        public string AnchorText { get; internal set; }

        public QueryParameters Parameters { get; internal set; }

        public int GetAvailableCount()
        {
            QueryParameters temp = this.Parameters.Clone();
            temp.Count = 100;
            temp.Filter = this;
            return new CustomSearchClient().Search(temp).Total;
        }
    }
}