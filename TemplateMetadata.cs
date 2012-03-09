using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TemplateGenerator
{
    internal class TemplateMetadata
    {
        public TemplateMetadata(XDocument xmlDoc)
        {
            if (xmlDoc == null)
            {
                throw new ArgumentNullException("xmlDoc");
            }
            if (xmlDoc.Root != null)
            {
                Node = xmlDoc.Root.XPathSelectElement("/luokkaPohja/luokka[1]");
            }
        }

        public XElement Node { get; set; }

        public MetadataType MetadataType
        {
            get { return GetMetadataType(MetadataTypeString); }
        }

        private string MetadataTypeString
        {
            get { return SearchProperty(Node, MetadataParameters.Metainformation); }
        }

        public string Name
        {
            get { return SearchProperty(Node, MetadataParameters.Name); }
        }

        public string Description
        {
            get { return SearchProperty(Node, MetadataParameters.Description); }
        }

        public TemplateDescriptionType ClassType
        {
            get { return GetClassDescriptionType(ClassTypeString); }
        }

        private string ClassTypeString
        {
            get { return SearchProperty(Node, MetadataParameters.ClassType); }
        }

        public string TableName
        {
            get { return SearchProperty(Node, MetadataParameters.TableName); }
        }

        public string Surrogate
        {
            get { return SearchProperty(Node, MetadataParameters.Surrogate); }
        }

        public string EventType
        {
            get { return SearchProperty(Node, MetadataParameters.Tapahtumalaji); }
        }

        public string Namespace
        {
            get { return SearchProperty(Node, MetadataParameters.Nimiavaruus); }
        }

        public string Visibility
        {
            get { return SearchProperty(Node, MetadataParameters.Visibility); }
        }
        
        public string DefaultValue
        {
            get { return SearchProperty(Node, MetadataParameters.DefaultValue); }
        }

        public static MetadataType GetMetadataType(string metadataType)
        {
            return (MetadataType)Enum.Parse(typeof (MetadataType), metadataType, true);
        }

        public static TemplateDescriptionType GetClassDescriptionType(string luokkakuvausTyyppi)
        {
            return (TemplateDescriptionType)Enum.Parse(typeof (TemplateDescriptionType), luokkakuvausTyyppi, true);
        }

        private static string SearchProperty(XElement node, string property)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return node.XPathSelectElement(string.Format("property[@name='{0}']", property)).Attributes("value").First().Value;
        }
    }
}