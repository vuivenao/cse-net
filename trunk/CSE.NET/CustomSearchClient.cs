namespace Google.CSE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Web;

    public class CustomSearchClient
    {
        private const string GOOGLE_URL = "http://www.google.com/search?";

        public string CseId { get; private set; }

        public CustomSearchClient(string cseId)
        {
            this.CseId = cseId;
        }

        public ResultSet Search(QueryParameters queryParameters)
        {
            Uri requestUri = this.FormatRequest(queryParameters);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ResultSet retval = this.ProcessResponse(queryParameters, response);
            return retval;
        }

        private ResultSet ProcessResponse(QueryParameters queryParameters, HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("The request was borked");
            }

            ResultSet retval = new ResultSet(queryParameters);
            new RespnseParser().Parse(retval, response.GetResponseStream());
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
