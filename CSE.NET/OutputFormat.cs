namespace Google.CustomSearch
{
    /// <summary>
    /// The valid values for the output format parameter of a CSE request.
    /// </summary>
    public enum OutputFormat
    {
        /// <summary>
        /// <para>The XML results will contain a Google DTD reference. 
        /// The second line of the result will identify the document definition type (DTD) that the results use:</para>
        /// &lt;!DOCTYPE GSP SYSTEM "google.dtd"&gt;
        /// </summary>
        /// <example>&lt;!DOCTYPE GSP SYSTEM "google.dtd"&gt;</example>
        Xml,

        /// <summary>
        /// The XML results will not include a !DOCTYPE statement (recommended).
        /// </summary>
        XmlNoDtd
    }
}
