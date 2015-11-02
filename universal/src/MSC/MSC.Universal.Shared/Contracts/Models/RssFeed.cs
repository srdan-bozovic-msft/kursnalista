using System;
using System.Xml.Serialization;

namespace MSC.Universal.Shared.Contracts.Models
{
    [XmlRoot(ElementName="rss")]
    public class RssFeed
    {
        public class RssFeedChannel
        {
            public class RssFeedItem
            {
                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement(ElementName="encoded", Namespace = "http://purl.org/rss/1.0/modules/content/")]
                public string EncodedContent { get; set; }

                [XmlElement("pubDate")]
                public string PubDate { get; set; }

                [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
                public string Creator { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement(ElementName = "commentRss", Namespace = "http://wellformedweb.org/CommentAPI/")]
                public string CommentRss { get; set; }

                [XmlElement(ElementName = "category")]
                public string[] Categories { get; set; }

                [XmlIgnore]
                public DateTime Date
                {
                    get
                    {
                        DateTime date;
                        DateTime.TryParse(PubDate, out date);
                        return date;
                    }
                }
            }
            
            [XmlElement(ElementName="item")]
            public RssFeedItem[] Items { get; set; }
        }

        [XmlElement(ElementName="channel")]
        public RssFeedChannel Channel { get; set; }

    }
}
