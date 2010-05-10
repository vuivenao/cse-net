namespace Google.CustomSearch
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Facet
    {
        public string Label { get; internal set; }

        public string AnchorText { get; internal set; }

        public Facet() { }

        public Facet(string label, string anchorText)
        {
            this.Label = label;
            this.AnchorText = anchorText;
        }
    }
}
