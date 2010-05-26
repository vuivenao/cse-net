namespace Google.CustomSearch
{
    using System.Configuration;

    /// <summary>
    /// Represents the configuration section for Google Custom Search Engine. 
    /// This class cannot be inherited.
    /// </summary>
    public sealed class GoogleCustomSearchSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the number of results to display per page.
        /// </summary>
        [ConfigurationProperty("countPerPage", DefaultValue = 10)]
        public int CountPerPage
        {
            get { return (int)this["countPerPage"]; }
            set { this["countPerPage"] = value; }
        }

        /// <summary>
        /// Gets or sets the Google Custom Search account ID.
        /// </summary>
        [ConfigurationProperty("cseId", IsRequired = true)]
        public string CseId
        {
            get { return (string)this["cseId"]; }
            set { this["cseId"] = value; }
        }
    }
}
