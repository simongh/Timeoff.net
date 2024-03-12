using HtmlAgilityPack;
using System.Text.Json.Serialization;

namespace Timeoff.Application.EmailAudit
{
    public record EmailResult
    {
        public int Id { get; init; }

        public ResultModels.ListResult User { get; init; } = null!;

        public string Email { get; init; } = null!;

        public string Subject { get; init; } = null!;

        [JsonIgnore]
        internal string Content { get; init; } = null!;

        public string Body
        {
            get
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(Content);

                using var writer = new StringWriter();
                ConvertTo(doc.DocumentNode, writer);
                writer.Flush();

                return writer.ToString();
            }
        }

        public DateTimeOffset CreatedAt { get; init; }

        private static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;

                        case "br":
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
    }
}