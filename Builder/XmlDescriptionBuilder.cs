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
        /// <summary>
        /// Default constant name for propertyDescription-element.
        /// </summary>
        public const string PropertyDescriptionElement = "propertyDescription";

        #region Overrides of DescriptionBuilderBase

        /// <summary>
        /// Builds <see cref="IDescription"/> based on input XML-string.
        /// </summary>
        /// <param name="xml">XML-string</param>
        /// <exception cref="ArgumentNullException">Thrown when XML-string is null.</exception>
        /// <returns>IDescription</returns>
        protected override IDescription BuildDescription(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
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
                                       Namespace = meta.Namespace
                                   };
                    }
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Fetch enumerable over <see cref="PropertyDescription"/> types.
        /// </summary>
        /// <param name="templateNode">Node type</param>
        /// <param name="propertyDescription">Property description</param>
        /// <exception cref="InvalidOperationException">Thrown when XML-description has a duplicate element defined.</exception>
        /// <returns>IEnumerable{PropertyDescription}</returns>
        protected override IEnumerable<PropertyDescription> FetchProperties(XElement templateNode,
                                                                            string propertyDescription)
        {
            if (templateNode == null)
            {
                throw new ArgumentNullException(nameof(templateNode));
            }
            if (propertyDescription == null)
            {
                throw new ArgumentNullException(nameof(propertyDescription));
            }

            var propertyDescriptions = new Collection<PropertyDescription>();

            var propertyNodes = templateNode.XPathSelectElements(propertyDescription).ToList();

            if (propertyNodes.Any())
            {
                propertyNodes.TakeWhile(HasElement).ToList().ForEach(propertyNode =>
                {
                    var elementName = TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.Name);

                    if (elementName == null)
                    {
                        return;
                    }

                    var description = new PropertyDescription(elementName, string.Empty, string.Empty);

                    if (propertyDescriptions.Contains(description))
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
                throw new ArgumentNullException(nameof(propertyNode));
            }
            
            // TODO: fix hardcoded value or provide a better way to get enum's stringified value
            return
                TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.MetaInformation).Equals(
                    Enum.GetName(typeof (MetadataTypes), 5));
        }
    }
}