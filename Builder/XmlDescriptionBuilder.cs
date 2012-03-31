using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Builder
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

                    if (meta.MetadataType == MetadataTypes.Xml)
                    {
                        var properties = FetchProperties(meta.Node, PropertyDescriptionElement);

                        return new XmlDescription(meta.Name, meta.Description, properties)
                                   {
                                       TableName = meta.TableName,
                                       EventType = meta.EventType,
                                       Namespace = meta.Namespace
                                   };
                    }
                }
            }

            return null;
        }

        #endregion

        protected override IEnumerable<PropertyDescription> FetchProperties(XElement templateNode,
                                                                            string propertyDescription)
        {
            if (templateNode == null)
            {
                throw new ArgumentNullException("templateNode");
            }
            if (propertyDescription == null)
            {
                throw new ArgumentNullException("propertyDescription");
            }

            var propertyDescriptions = new Collection<PropertyDescription>();

            var propertyNodes = templateNode.XPathSelectElements(propertyDescription);

            if (propertyNodes.Any())
            {
                propertyNodes.TakeWhile(HasElement).ToList().ForEach(propertyNode =>
                                                                         {
                                                                             string elementName =
                                                                                 SearchProperty(propertyNode,
                                                                                                MetadataParameters.Name);

                                                                             if (elementName == null) return;

                                                                             var description =
                                                                                 new PropertyDescription(elementName, string.Empty,
                                                                                                         string.Empty);

                                                                             if (
                                                                                 propertyDescriptions.Contains(
                                                                                     description))
                                                                             {
                                                                                 throw new InvalidOperationException(
                                                                                     string.Format(
                                                                                         CultureInfo.InvariantCulture,
                                                                                         "XML-description has a duplicate element defined: '{0}'",
                                                                                         elementName));
                                                                             }

                                                                             propertyDescriptions.Add(description);
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
            return
                SearchProperty(propertyNode, MetadataParameters.MetaInformation).Equals(
                    Enum.GetName(typeof (MetadataTypes), 5));
        }

        private static string SearchProperty(XNode node, string propertyDescription)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (propertyDescription == null)
            {
                throw new ArgumentNullException("propertyDescription");
            }

            return
                node.XPathSelectElement(string.Format(CultureInfo.InvariantCulture, "propertyDescription[@name='{0}']",
                                                      propertyDescription)).Attributes("value").First().Value;
        }
    }
}