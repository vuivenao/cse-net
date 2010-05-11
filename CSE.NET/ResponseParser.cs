namespace Google.CustomSearch
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;

    internal class ResponseParser
    {
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

        private Result ParseResult(XPathNavigator navigator)
        {
            XPathNavigator crawlNode = navigator.SelectSingleNode("CRAWLDATE");
            string crawlDate = string.Empty;
            if (null != crawlNode)
                crawlDate = crawlNode.Value;

            Result result = new Result()
            {
                Id = int.Parse(navigator.GetAttribute("N", string.Empty)),
                Title = navigator.SelectSingleNode("T").Value,
                Excerpt = navigator.SelectSingleNode("S").Value,
                Uri = new Uri(navigator.SelectSingleNode("U").Value),
                EscapedUrl = navigator.SelectSingleNode("UE").Value
            };

            if (!string.IsNullOrEmpty(crawlDate))
            {
                // TODO: Needs to check if the date stamp from Google is UTC or PDT/EST or anything else for that matter.
                result.CrawlDate = DateTime.Parse(crawlDate, new CultureInfo("en-US"), DateTimeStyles.AssumeUniversal);
            }

            return result;
        }
    }
}
