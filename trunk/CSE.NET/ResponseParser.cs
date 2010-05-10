namespace Google.CustomSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
            while (iterator.MoveNext())
            {
                result.Results.Add(this.ParseResult(iterator.Current));
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
            int startIndex;
            if (int.TryParse(resultContainer.GetAttribute("SN", string.Empty), out startIndex))
            {
                result.StartIndex = startIndex;
            }

            int endIndex;
            if (int.TryParse(resultContainer.GetAttribute("EN", string.Empty), out endIndex))
            {
                result.EndIndex = endIndex;
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

            this.ParseFacets(result, nav);
        }

        private void ParseFacets(SearchResult result, XPathNavigator nav)
        {
            XPathNodeIterator facets = nav.Select("Context/Facet/FacetItem");
            while (facets.MoveNext())
            {
                string label = facets.Current.SelectSingleNode("label").Value;
                string anchor = facets.Current.SelectSingleNode("anchor_text").Value;
                result.Facets.Add(label, anchor);
            }
        }

        private Result ParseResult(XPathNavigator navigator)
        {
            return new Result()
            {
                Id = int.Parse(navigator.GetAttribute("N", string.Empty)),
                Title = navigator.SelectSingleNode("T").Value,
                Excerpt = navigator.SelectSingleNode("S").Value,
                Uri = new Uri(navigator.SelectSingleNode("U").Value),
                EscapedUrl = navigator.SelectSingleNode("UE").Value
            };
        }
    }
}
