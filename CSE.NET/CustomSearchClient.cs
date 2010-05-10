namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Web;
    using System.Configuration;

    public class CustomSearchClient
    {
        private const string GOOGLE_URL = "http://www.google.com/search?";

        public string CseId { get; private set; }

        public CustomSearchClient()
        {
            GoogleCustomSearchConfigSection section = ConfigurationManager.GetSection("googleCustomSearch") as GoogleCustomSearchConfigSection;
            if (section == null)
            {
                throw new NullReferenceException("The value 'null' was found where an instance of GoogleCustomSearchConfigSection was expected. Please check the applications config file and ensure thre is a googleCustomSearch configuration section.");
            }

            this.CseId = section.CseId;
        }

        public CustomSearchClient(string cseId)
        {
            this.CseId = cseId;
        }

        public ResultCollection Search(QueryParameters queryParameters)
        {
            Uri requestUri = this.FormatRequest(queryParameters);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ResultCollection retval = this.ProcessResponse(queryParameters, response);
            return retval;
        }

        private ResultCollection ProcessResponse(QueryParameters queryParameters, HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("The request was borked");
            }

            ResultCollection retval = new ResultCollection(queryParameters);
            new ResponseParser().Parse(retval, response.GetResponseStream());
            return retval;
        }

        private Uri FormatRequest(QueryParameters queryParameters)
        {
            StringBuilder url = new StringBuilder(GOOGLE_URL);
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            queryParams.Add("cx", this.CseId);
            queryParams.Add("client", "google-csbe");
            queryParams.Add("q", queryParameters.SearchTerm);
            queryParams.Add("num", queryParameters.Count.ToString());
            queryParams.Add("start", queryParameters.Start.ToString());

            if (queryParameters.Format == OutputFormat.Xml)
            {
                queryParams.Add("output", "xml");
            }
            else
            {
                queryParams.Add("output", "xml_no_dtd");
            }

            foreach (var pair in queryParams)
            {
                url.Append(pair.Key + "=" + HttpUtility.UrlEncode(pair.Value) + "&");
            }

            return new Uri(url.ToString().TrimEnd('&'));
        }
    }
}
