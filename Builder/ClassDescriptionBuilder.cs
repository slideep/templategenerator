using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

// TODO: Moving code away from XmlDocument is first priority -> XmlDescriptionBuilder contains pretty much all necessary functionality.

namespace TemplateGenerator.Builder
{
    /// <summary>
    /// A builder for class descriptions.
    /// </summary>
    public class ClassDescriptionBuilder : DescriptionBuilderBase<XmlNode>
    {
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

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var classNode = xmlDocument.SelectSingleNode("/classTemplate/class[1]");

            if (SearchProperty(classNode, MetadataParameters.MetaInformation).Equals("Class", StringComparison.Ordinal))
            {
                var classType =
                    (TemplateDescriptionTypes)Enum.Parse(typeof(TemplateDescriptionTypes), SearchProperty(classNode, MetadataParameters.ClassType), true);

                var className = SearchProperty(classNode, MetadataParameters.Name);
                var classDescription = SearchProperty(classNode, MetadataParameters.Description);
                var tableName = SearchProperty(classNode, MetadataParameters.TableName);
            
                if (classNode != null)
                {
                    var propertyDescriptions = FetchProperties(classNode, XmlDescriptionBuilder.PropertyDescriptionElement);

                    return new ClassDescription(className, classDescription, propertyDescriptions, Enumerable.Empty<OperationDescription>().ToList().AsReadOnly())
                           {
                               IsDataAccessClass = classType == TemplateDescriptionTypes.DataAccess,
                               TableName = tableName,
                           };
                }
            }

            return null;
        }

        /// <summary>
        /// Fetch enumerable over <see cref="PropertyDescription"/> types. 
        /// </summary>
        /// <param name="templateNode">Node type</param>
        /// <param name="propertyDescription">Property description</param>
        /// <exception cref="ArgumentNullException">Thrown when template node or property description is null.</exception>
        /// <returns>A IEnumerable{PropertyDescription} sequence with (or without) property descriptions.</returns>
        protected override IEnumerable<PropertyDescription> FetchProperties(XmlNode templateNode, string propertyDescription)
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

            var propertyNodes = templateNode.SelectNodes(propertyDescription);            
            if (propertyNodes != null && propertyNodes.Count > 0)
            {
                foreach (XmlNode propertyNode in propertyNodes)
                {
                    var isPublic =
                        SearchProperty(propertyNode, MetadataParameters.Visibility)
                        .Equals(MetadataParameters.Public, StringComparison.Ordinal);
                    var hasAttribute =
                        SearchProperty(propertyNode, MetadataParameters.MetaInformation)
                        .Equals(MetadataParameters.Attribute, StringComparison.Ordinal);

                    if (isPublic && hasAttribute)
                    {
                        AddPropertyDescription(propertyDescriptions, propertyNode);
                    }
                }

                return propertyDescriptions;
            }

            return Enumerable.Empty<PropertyDescription>();
        }

        /// <summary>
        /// Adds new <see cref="PropertyDescription"/> to return collection based on property node's values.
        /// </summary>
        /// <param name="propertyDescriptions">A collection where to add <see cref="PropertyDescription"/></param>
        /// <param name="propertyNode">A node containing property information.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if property descriptions contains already the property.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if collection where to add property is null or property node is null.
        /// </exception>
        private static void AddPropertyDescription(ICollection<PropertyDescription> propertyDescriptions, XmlNode propertyNode)
        {
            if (propertyDescriptions == null)
            {
                throw new ArgumentNullException(nameof(propertyDescriptions));
            }
            if (propertyNode == null)
            {
                throw new ArgumentNullException(nameof(propertyNode));
            }

            var propertyName = SearchProperty(propertyNode, MetadataParameters.Name);
            var propertyDescription = SearchProperty(propertyNode, MetadataParameters.Description);
            var propertyType = SearchProperty(propertyNode, MetadataParameters.DataType);
            var propertyDefaultValue = SearchProperty(propertyNode, MetadataParameters.DefaultValue);

            var property = new PropertyDescription(propertyName, propertyDescription, propertyType);
            property.SetDefaultValue(propertyDefaultValue);

            if (propertyDescriptions.Contains(property))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, ": '{0}'", propertyName));
            }

            propertyDescriptions.Add(property);
        }

        private static string SearchProperty(XmlNode node, string property)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            try
            {
                var childNode =
                    node.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, @"property[@name='{0}']", property));

                if (childNode == null)
                {
                    return null;
                }

                if (childNode.Attributes != null)
                {
                    var attribute = childNode.Attributes["value"];

                    return attribute?.Value;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Error occurred while reading property '{0}'", property), ex);
            }

            return null;
        }
    }
}