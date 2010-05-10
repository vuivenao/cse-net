namespace Google.CustomSearch
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class FacetCollection : Dictionary<string, string>
    {
        public new void Add(string label, string anchor)
        {
            base.Add(label, anchor);
        }

        public void Add(Facet facet)
        {
            this.Add(facet.Label, facet.AnchorText);
        }

        public new string this[string label]
        {
            get
            {
                return base[label];
            }
        }
    }
}
