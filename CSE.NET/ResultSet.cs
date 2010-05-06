namespace Google.CSE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ResultSet : List<Result>
    {
        public string Context { get; internal set; }

        public TimeSpan Time { get; internal set; }

        public QueryParameters Params { get; internal set; }

        internal ResultSet(QueryParameters parameters)
        {
            this.Params = parameters;
        }
    }
}
