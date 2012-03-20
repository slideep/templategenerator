using System;
using System.Globalization;
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
                Node = xmlDoc.Root.XPathSelectElement("/classTemplate/class[1]");
            }
        }

        public XElement Node { get; set; }

        public MetadataTypes MetadataTypes
        {
            get { return GetMetadataType(MetadataTypeString); }
        }

        private string MetadataTypeString
        {
            get { return SearchProperty(Node, MetadataParameters.MetaInformation); }
        }

        public string Name
        {
            get { return SearchProperty(Node, MetadataParameters.Name); }
        }

        public string Description
        {
            get { return SearchProperty(Node, MetadataParameters.Description); }
        }

        public TemplateDescriptionTypes ClassTypes
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
            get { return SearchProperty(Node, MetadataParameters.EventType); }
        }

        public string Namespace
        {
            get { return SearchProperty(Node, MetadataParameters.Namespace); }
        }

        public string Visibility
        {
            get { return SearchProperty(Node, MetadataParameters.Visibility); }
        }

        public string DefaultValue
        {
            get { return SearchProperty(Node, MetadataParameters.DefaultValue); }
        }

        public static MetadataTypes GetMetadataType(string metadataType)
        {
            return (MetadataTypes) Enum.Parse(typeof (MetadataTypes), metadataType, true);
        }

        public static TemplateDescriptionTypes GetClassDescriptionType(string luokkakuvausTyyppi)
        {
            return (TemplateDescriptionTypes) Enum.Parse(typeof (TemplateDescriptionTypes), luokkakuvausTyyppi, true);
        }

        private static string SearchProperty(XNode node, string property)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            return
                node.XPathSelectElement(string.Format(CultureInfo.InvariantCulture, "property[@name='{0}']", property)).
                    Attributes("value").First().Value;
        }
    }
}