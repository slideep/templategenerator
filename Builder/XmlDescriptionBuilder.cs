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

namespace TemplateGenerator.Builder;
    public class XmlDescriptionBuilder : DescriptionBuilderBase<XElement>
    {
        public XmlDescriptionBuilder()
        {
        }

        public XmlDescriptionBuilder(string templateDirectory)
            : base(templateDirectory)
        {
        }

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
        protected override IDescription? BuildDescription(string xml, string? fileFullPath)
        {
            ArgumentNullException.ThrowIfNull(xml);

            using var reader = new StringReader(xml);
            var xmlDocument = XDocument.Load(reader);

            if (xmlDocument.Root == null)
            {
                return null;
            }

            var meta = new TemplateMetadata(xmlDocument);

            if (meta.Node == null)
            {
                throw TemplateParseException.MissingRootNode();
            }

            if (meta.MetadataType != MetadataTypes.Xml)
            {
                return null;
            }

            var properties = FetchProperties(meta.Node, PropertyDescriptionElement);

            return new XmlDescription(
                meta.Name,
                meta.Description,
                properties,
                tableName: meta.TableName,
                xmlNamespace: meta.Namespace,
                fileFullPath: fileFullPath);
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
            ArgumentNullException.ThrowIfNull(templateNode);
            ArgumentNullException.ThrowIfNull(propertyDescription);

            var propertyDescriptions = new Collection<PropertyDescription>();

            var propertyNodes = templateNode.XPathSelectElements(propertyDescription).ToList();

            if (!propertyNodes.Any())
            {
                return Enumerable.Empty<PropertyDescription>();
            }

            foreach (var propertyNode in propertyNodes)
            {
                // Non-element property descriptions (for example, Attributes) are intentionally
                // skipped. The legacy TakeWhile(HasElement) behavior truncated parsing at the
                // first non-element node and dropped valid later elements.
                if (!HasElement(propertyNode))
                {
                    continue;
                }

                var elementName = TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.Name);

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
            }

            return propertyDescriptions;
        }

        private static bool HasElement(XElement propertyNode)
        {
            ArgumentNullException.ThrowIfNull(propertyNode);
            
            return TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.MetaInformation)
                .Equals(nameof(MetadataTypes.Element), StringComparison.Ordinal);
        }
    }
 
