namespace Google.CSE
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class GoogleCustomSearchConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("countPerPage", DefaultValue = 10)]
        public int CountPerPage
        {
            get { return (int)this["CountPerPage"]; }
            set { this["CountPerPage"] = value; }
        }

        [ConfigurationProperty("cseId", IsRequired = true)]
        public string CseId
        {
            get { return (string)this["CseId"]; }
            set { this["CseId"] = value; }
        }
    }
}
