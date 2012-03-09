using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TemplateGenerator
{
    public class XmlDescriptionBuilder : DescriptionBuilderBase<XElement>
    {
        public const string PropertyDescriptionElement = "propertyDescription";

        #region Overrides of DescriptionBuilderBase

        protected override IDescription BuildDescription(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            using (var reader = new StringReader(xml))
            {
                var xmlDocument = XDocument.Load(reader);

                if (xmlDocument.Root != null)
                {
                    var meta = new TemplateMetadata(xmlDocument);

                    if (meta.MetadataType == MetadataType.Xml)
                    {
                        return new XmlDescription(meta.Name, meta.Description, 
                            FetchProperties(meta.Node, PropertyDescriptionElement))
                               {
                                   TableName = meta.TableName,
                                   SurrogateValue = meta.Surrogate,
                                   EventType = meta.EventType,
                                   Namespace = meta.Namespace
                               };
                    }
                }
            }

            return null;
        }

        #endregion

        protected override IEnumerable<PropertyDescription> FetchProperties(XElement templateNode, string propertyDescription)
        {
            if (templateNode == null)
            {
                throw new ArgumentNullException("templateNode");
            }
            if (propertyDescription == null)
            {
                throw new ArgumentNullException("descriptionType");
            }

            var propertyDescriptions = new Collection<PropertyDescription>();

            var propertyNodes = templateNode.XPathSelectElements(propertyDescription);

            if (propertyNodes.Count() > 0)
            {
                propertyNodes.TakeWhile(e => HasElement(e)).ToList().ForEach(propertyNode =>
                {
                    string elementName = SearchProperty(propertyNode, MetadataParameters.Name);
                    if (elementName != null)
                    {
                        var propertyDescription = new PropertyDescription(elementName, "", "");
                        
                        if (propertyDescriptions.Contains(propertyDescription))
                        {
                            throw new InvalidOperationException(
                                string.Format("XML-description has a duplicate element defined: '{0}'", elementName));
                        }

                        propertyDescriptions.Add(propertyDescription);
                    }
                });

                return propertyDescriptions;
            }

            return Enumerable.Empty<PropertyDescription>();
        }

        private static bool HasElement(XElement propertyNode)
        {
            if (propertyNode == null)
            {
                throw new ArgumentNullException("propertyNode");
            }

			// TODO: fix hardcoded value or provide a better way to get enum's stringified value
            return SearchProperty(propertyNode, MetadataParameters.Metainformation).Equals(Enum.GetName(typeof (MetadataType), 5));
        }

        private static string SearchProperty(XElement node, string propertyDescription)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (propertyDescription == null)
            {
                throw new ArgumentNullException("propertyDescription");
            }

            return node.XPathSelectElement(string.Format("propertyDescription[@name='{0}']", propertyDescription)).Attributes("value").First().Value;
        }
    }
}