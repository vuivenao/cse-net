namespace Google.CustomSearch
{
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// A collection of Google.CustomSearch.Facet objects.
    /// </summary>
    public class FacetCollection : Collection<Facet>
    {
        /// <summary>
        /// Gets a Facet contained in the collection.
        /// </summary>
        /// <param name="label">The label of the Facet to get.</param>
        /// <returns>Returns a Facet or null.</returns>
        public Facet this[string label]
        {
            get
            {
                return this.FirstOrDefault(f => f.Label == label);
            }
        }

        /// <summary>
        /// Adds an object to the end of the Google.CustomSearch.FacetCollection.
        /// </summary>
        /// <param name="label">The label of the Facet to add to the collection.</param>
        /// <param name="anchor">The anchor text of the Facet to add to the collection.</param>
        public void Add(string label, string anchor)
        {
            base.Add(new Facet(label, anchor));
        }

        /// <summary>
        /// Determines whether an element is in the Google.CustomSearch.FacetCollection.
        /// </summary>
        /// <param name="label">The label of the Google.CustomSearch.Facet to locate.</param>
        /// <returns>Returns true if Facet is found; otherwise, false.</returns>
        public bool Contains(string label)
        {
            foreach (var item in this)
                if (item.Label == label) return true;

            return false;
        }
    }
}
