namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Sort/bias capabilities of the Google search engine.
    /// </summary>
    /// <see cref="http://code.google.com/apis/customsearch/docs/structured_search.html#sort_by_attribute"/>
    /// <see cref="http://code.google.com/apis/customsearch/docs/structured_search.html#bias_by_attribute"/>
    public class Sort : ICloneable
    {
        #region SortType

        /// <summary>
        /// The types of sort that results can be order by.
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// Sort with the most relevant result first, using the ranking determined by Google.
            /// </summary>
            Relevance,
            /// <summary>
            /// Sort by date, using the date determined by Google.
            /// </summary>
            Date,
            /// <summary>
            /// Sort by a custom attribute.
            /// When this sort type is used the CustomAttribute property must be set.
            /// </summary>
            Attribute
        }

        #endregion

        #region SortDirection

        /// <summary>
        /// The directions of sort that results can be ordered by.
        /// Only applies when the SortType is Date or Attribute.
        /// </summary>
        public enum SortDirection
        {
            /// <summary>
            /// Sort with highest/newest value first.
            /// </summary>
            Descending,
            /// <summary>
            /// Sort with lowest/oldest value first.
            /// </summary>
            Ascending
        }

        #endregion

        #region SortStrength

        /// <summary>
        /// The strengths of sort that results can be ordered by.
        /// Only applies when the SortType is Attribute.
        /// </summary>
        public enum SortStrength
        {
            /// <summary>
            /// A hard sort.
            /// Results are ordered strictly by the sort attribute.
            /// Results which lack a parsable value for the sort attribute will be excluded from the results.
            /// </summary>
            /// <see cref="http://code.google.com/apis/customsearch/docs/structured_search.html#sort_by_attribute"/>
            Hard,
            /// <summary>
            /// A strong biased sort.
            /// Results are ordered with strong promotion by the sort attribute.
            /// Results which lack a parsable value for the sort attribute will be included in the results.
            /// </summary>
            /// <see cref="http://code.google.com/apis/customsearch/docs/structured_search.html#bias_by_attribute"/>
            Strong,
            /// <summary>
            /// A weak biased sort.
            /// Results are ordered with weak promotion by the sort attribute.
            /// Results which lack a parsable value for the sort attribute will be included in the results.
            /// </summary>
            /// <see cref="http://code.google.com/apis/customsearch/docs/structured_search.html#bias_by_attribute"/>
            Weak
        }

        #endregion

        public Sort()
        {
            this.Type = SortType.Relevance;
            this.Direction = SortDirection.Descending;
            this.Strength = SortStrength.Hard;
            this.Attribute = null;
        }

        /// <summary>
        /// The type of sort the results will be ordered by.
        /// </summary>
        public SortType Type { get; set; }

        /// <summary>
        /// The direction of sort the results will be ordered by.
        /// Only applies when the Type is Date or Attribute.
        /// </summary>
        public SortDirection Direction { get; set; }

        /// <summary>
        /// The strength of sort the results will be ordered by.
        /// Only applies when the Type is Attribute.
        /// </summary>
        public SortStrength Strength { get; set; }

        /// <summary>
        /// The custom sort attribute.
        /// Only applies when the Type is Attribute.
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        public Sort Clone()
        {
            return (Sort)this.MemberwiseClone();
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a copy of this instance.
        /// </summary>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
