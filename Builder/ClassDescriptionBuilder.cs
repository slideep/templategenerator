using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Builder
{
    public class ClassDescriptionBuilder : DescriptionBuilderBase<XmlNode>
    {
        protected override IDescription BuildDescription(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
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

        protected override IEnumerable<PropertyDescription> FetchProperties(XmlNode templateNode, string propertyDescription)
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

        private static void AddPropertyDescription(ICollection<PropertyDescription> propertyDescriptions, XmlNode propertyNode)
        {
            if (propertyDescriptions == null)
            {
                throw new ArgumentNullException("propertyDescriptions");
            }
            if (propertyNode == null)
            {
                throw new ArgumentNullException("propertyNode");
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
                throw new ArgumentNullException("node");
            }
            if (property == null)
            {
                throw new ArgumentNullException("property");
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

                    return attribute == null ? null : attribute.Value;
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