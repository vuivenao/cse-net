namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    public class ResultCollection : Collection<Result>
    {
        public string Context { get; internal set; }

        public string Time { get; internal set; }

        public QueryParameters Parameters { get; internal set; }

        internal ResultCollection(QueryParameters parameters)
        {
            this.Parameters = parameters;
        }
    }
}
