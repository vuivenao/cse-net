namespace Google.CustomSearch
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the parameters that can be used when making a search request.
    /// </summary>
    public class QueryParameters : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the QueryParameters class.
        /// </summary>
        public QueryParameters()
        {
            this.Count = CustomSearchClient.Count;
            this.FileTypes = new Dictionary<string, bool>(); 
            this.Format = OutputFormat.XmlNoDtd;
            this.Safe = SafeSearch.Off;
        }

        /// <summary>
        /// Gets or sets the Count parameter which identifies the number of search results to return.
        /// This parameter is optional.
        /// The default Count value is 10, and the maximum value is 20. If you request more than 20 results, only 20 results will be returned.
        /// Note: If the total number of search results is less than the requested number of results, all available search results will be returned.
        /// </summary>
        /// <see cref="http://www.google.com/cse/docs/resultsxml.html#numsp"/>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the Filter property.
        /// TODO: Invent this documentation.
        /// </summary>
        /// <see cref="http://www.google.com/cse/docs/resultsxml.html#results_xml_tag_label"/>
        public Facet Filter { get; set; }

        /// <summary>
        /// Gets or sets the safe parameter indicates how search results should be filtered for adult and pornographic content. 
        /// The default value for the safe parameter is off.
        /// See the <see cref="http://www.google.com/cse/docs/resultsxml.html#safeSearchLevels">Filtering Adult Content with SafeSearch</see> section for more details about this feature.
        /// </summary>
        public SafeSearch Safe { get; set; }

        /// <summary>
        /// Gets or sets the SearchTerm parameter which specifies the search query entered by the user. 
        /// Even though this parameter is optional, you must specify a value for at least one 
        /// of the query parameters (as_epq, as_lq, as_oq, as_q, as_rq) to get search results.
        /// TODO: Implement as_eqp, as_lq etc and update these docs
        /// </summary>
        /// <see cref="http://www.google.com/cse/docs/resultsxml.html#qsp"/>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the start parameter which indicates the first matching result that should be included in the search results.
        /// The start parameter uses a zero-based index, meaning the first result is 0, the second result is 1 and so forth.
        /// </summary>
        /// <see cref="http://www.google.com/cse/docs/resultsxml.html#startsp"/>
        public int Start { get; set; }

        // TODO: implement the as_dt parameter and update this documentation

        /// <summary>
        /// Gets or sets a URL that specifies that all search results should be pages from a given site. 
        /// By setting the as_dt parameter, you can also use it to exclude pages from a given site from your search resutls.
        /// </summary>
        public Uri AdvancedSearchSite { get; set; }

        /// <summary>
        /// <para>Gets a list of filetypes with which to restrict the search results to documents with a particular file extension, such as ".pdf" or ".doc".</para>
        /// Filetypes with a true value will be included, false will be excluded.
        /// </summary>
        public Dictionary<string, bool> FileTypes { get; private set; }




        // TODO: Possibly swap encoding for a built in type
        
        /// <summary>
        /// Gets or sets the Encoding parameter which sets the character encoding scheme that should be used to decode the XML result.
        /// This parameter is optional and the default Encoding value is 'latin1'.
        /// See the <see cref="http://www.google.com/cse/docs/resultsxml.html#wsCharacterEncoding">Character Encoding</see> section for a discussion of when you might need to use this parameter.
        /// See the <see cref="http://www.google.com/cse/docs/resultsxml.html#characterEncodings">Character Encoding Schemes</see> section for the list of possible oe values.
        /// </summary>
        /// <see cref="http://www.google.com/cse/docs/resultsxml.html#oesp"/>
        public string Encoding { get; set; }
        
        // TODO: Format is not really something the user needs to wory about. As long as the system works.
        // TODO: Need to research if there is any benefit to using the DTD or not.

        /// <summary>
        /// Gets or sets the Format parameter which specifies the format of the XML results. 
        /// The only valid values for this parameter are specified in the OutputFormat enumeration as OutputFormat.XML
        /// and OutputFormat.XmlNoDtd.
        /// This value is required, default value is OutputFormat.XmlNoDtd.
        /// </summary>
        /// <see cref="http://www.google.com/cse/docs/resultsxml.html#outputsp"/>
        internal OutputFormat Format { get; set; }

        /// <summary>
        /// Creates a new QueryParameters object that is a copy of the current instance.
        /// </summary>
        /// <returns>Returns new QueryParameters object that is a copy of this instance.</returns>
        public QueryParameters Clone()
        {
            return new QueryParameters()
            {
                Count = this.Count,
                Encoding = this.Encoding,
                Filter = this.Filter,
                Format = this.Format,
                SearchTerm = this.SearchTerm,
                Start = this.Start
            };
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>Returns new object that is a copy of this instance.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion      
    }
}