namespace Google.CustomSearch
{
    using System;
    using System.Configuration;

    public class QueryParameters : ICloneable
    {
        public QueryParameters()
        {
            this.Format = OutputFormat.XmlNoDtd;
            this.Count = this.GetCountFromConfig();
        }

        public int Start { get; set; }

        public int Count { get; set; }

        public string SearchTerm { get; set; }

        public Facet Filter { get; set; }

        public OutputFormat Format { get; set; }

        // TODO: Possibly swap encoding for a built in type
        public string Encoding { get; set; }

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

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        private int GetCountFromConfig()
        {
            GoogleCustomSearchConfigSection section = ConfigurationManager.GetSection("googleCustomSearch") as GoogleCustomSearchConfigSection;
            if (section == null)
            {
                // TODO: Throw the proper exception
                throw new Exception("needs the section in the config");
            }

            return section.CountPerPage;
        }
    }
}
