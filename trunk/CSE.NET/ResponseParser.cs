namespace Google.CustomSearch
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Mime;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Utility class used to parse the response of a Google CSE request into a
    /// Google.CustomSearch.SearchResult object.
    /// </summary>
    internal class ResponseParser
    {
        /// <summary>
        /// Parses the given stream and populates the given SearchResult object.
        /// </summary>
        /// <param name="result">The Google.CustomSearch.SearchResult to be populated.</param>
        /// <param name="responseStream">The stream containing the response from the CSE request.</param>
        public void Parse(SearchResult result, Stream responseStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(responseStream);
            XPathNavigator nav = doc.CreateNavigator();

            this.ParseResponseProperties(result, nav);

            // Process the results
            XPathNodeIterator iterator = nav.Select("/GSP/RES/R");
            if (iterator != null)
            {
                while (iterator.MoveNext())
                {
                    result.Results.Add(this.ParseResult(iterator.Current));
                }
            }
        }

        /// <summary>
        /// Parses the various properties from the XML document and populates the given SearchResult.
        /// </summary>
        /// <param name="result">The Google.CustomSearch.SearchResult to populate.</param>
        /// <param name="nav">The XPathNavigator for the response document.</param>
        private void ParseResponseProperties(SearchResult result, XPathNavigator nav)
        {
            XPathNavigator timeNode = nav.SelectSingleNode("/GSP/TM");
            if (timeNode != null)
                result.Time = timeNode.Value;

            XPathNavigator titleNode = nav.SelectSingleNode("/GSP/Context/title");
            if (titleNode != null)
                result.Title = titleNode.Value;

            XPathNavigator resultContainer = nav.SelectSingleNode("/GSP/RES");
            if (resultContainer != null)
            {
                // See http://www.google.com/cse/docs/resultsxml.html#results_xml_tag_XT
                result.Exact = resultContainer.SelectSingleNode("XT") != null;

                result.Filtered = resultContainer.SelectSingleNode("FI") != null;

                int startIndex;
                string start = resultContainer.GetAttribute("SN", string.Empty);
                if (int.TryParse(start, out startIndex))
                {
                    result.StartIndex = startIndex;
                }

                int endIndex;
                string end = resultContainer.GetAttribute("EN", string.Empty);
                if (int.TryParse(end, out endIndex))
                {
                    result.EndIndex = endIndex;
                }

                // Next and previous URLs
                XPathNavigator navigation = resultContainer.SelectSingleNode("NB");
                if (null != navigation)
                {
                    XPathNavigator nextLink = navigation.SelectSingleNode("NU");
                    if (null != nextLink)
                        result.NextPageLink = nextLink.Value;

                    XPathNavigator previousLink = navigation.SelectSingleNode("NP");
                    if (null != previousLink)
                        result.PreviousPageLink = previousLink.Value;
                }

                XPathNavigator totalNode = resultContainer.SelectSingleNode("M");
                if (totalNode != null)
                {
                    int total;
                    if (int.TryParse(totalNode.Value, out total))
                    {
                        result.Total = total;
                    }
                }
            }

            this.ParseSpellings(result, nav);

            this.ParseFacets(result, nav);
        }

        /// <summary>
        /// Parses spelling suggestions from the CSE request into the Suggestions property of the given SearchResult object.
        /// </summary>
        /// <param name="result">The Google.CustomSearch.SearchResult object whos Suggestions property is to be populated.</param>
        /// <param name="nav">THe XPathNavigator for the root of the response document.</param>
        private void ParseSpellings(SearchResult result, XPathNavigator nav)
        {
            XPathNodeIterator spellings = nav.Select("/GSP/Spelling/Suggestion");
            while (spellings.MoveNext())
            {
                string query = spellings.Current.GetAttribute("q", string.Empty);
                string suggestion = spellings.Current.Value;
                result.Suggestions.Add(new Suggestion(suggestion, query));
            }
        }

        /// <summary>
        /// Parses the site labels from the CSE response.
        /// </summary>
        /// <param name="result">The Google.CustomSearch.SearchResult whos Facets property is to be populated.</param>
        /// <param name="nav">THe XPathNavigator for the root of the response document.</param>
        private void ParseFacets(SearchResult result, XPathNavigator nav)
        {
            XPathNodeIterator facets = nav.Select("/GSP/Context/Facet/FacetItem");
            while (facets.MoveNext())
            {
                string label = facets.Current.SelectSingleNode("label").Value;
                string anchor = facets.Current.SelectSingleNode("anchor_text").Value;
                result.Facets.Add(new Facet(result.Parameters)
                {
                    Label = label,
                    AnchorText = anchor
                });
            }
        }

        /// <summary>
        /// Parses a result node into a Google.CustomSearch.Result object.
        /// </summary>
        /// <param name="navigator">The XPathNavigator for the &lt;R&gt; node.</param>
        /// <returns>Returns a populated Google.CustomSearch.Result object.</returns>
        private Result ParseResult(XPathNavigator navigator)
        {
            // Get result values in a cautious manner as not all attributes/nodes always exist

            int id = 0;
            int.TryParse(navigator.GetAttribute("N", string.Empty), out id);

            string title = null;
            XPathNavigator titleNode = navigator.SelectSingleNode("T");
            if (titleNode != null)
                title = titleNode.Value;

            string excerpt = null;
            XPathNavigator excerptNode = navigator.SelectSingleNode("S");
            if (excerptNode != null)
                excerpt = excerptNode.Value;

            Uri uri = null;
            XPathNavigator uriNode = navigator.SelectSingleNode("U");
            if (uriNode != null)
                uri = new Uri(uriNode.Value);

            string escapedUrl = null;
            XPathNavigator escapedUrlNode = navigator.SelectSingleNode("UE");
            if (escapedUrlNode != null)
                escapedUrl = escapedUrlNode.Value;

            string mimeTypeStr = navigator.GetAttribute("MIME", string.Empty);
            if (string.IsNullOrEmpty(mimeTypeStr))
                mimeTypeStr = "text/html";
            ContentType mimeType = new ContentType(mimeTypeStr);

            Result result = new Result()
            {
                Id = id,
                Title = title,
                Excerpt = excerpt,
                Uri = uri,
                EscapedUrl = escapedUrl,
                MimeType = mimeType
            };

            string crawlDate = null;
            XPathNavigator crawlDateNode = navigator.SelectSingleNode("CRAWLDATE");
            if (crawlDateNode != null)
                crawlDate = crawlDateNode.Value;

            if (!string.IsNullOrEmpty(crawlDate))
            {
                DateTime crawlDateTime;
                if (DateTime.TryParse(crawlDate, out crawlDateTime))
                {
                    // TODO: Needs to check if the date stamp from Google is UTC or PDT/EST or anything else for that matter.
                    result.CrawlDate = DateTime.Parse(crawlDate, new CultureInfo("en-US"), DateTimeStyles.AssumeUniversal);
                }
            }

            return result;
        }
    }
}
