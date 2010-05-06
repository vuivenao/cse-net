using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Google.CSE
{
    public class QueryParameters
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public string SearchTerm { get; set; }
        public OutputFormat Format { get; set; }

        // TODO: Possibly swap encoding for a built in type
        public string Encoding { get; set; }
    }
}
