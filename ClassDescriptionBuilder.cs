using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace TemplateGenerator
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

            var classNode = xmlDocument.SelectSingleNode("/luokkaPohja/luokka[1]");

            if (SearchProperty(classNode, MetadataParameters.Metainformation).Equals("Class", StringComparison.Ordinal))
            {
                var classType =
                    (TemplateDescriptionType)Enum.Parse(typeof(TemplateDescriptionType), SearchProperty(classNode, MetadataParameters.ClassType), true);

                string className = SearchProperty(classNode, MetadataParameters.Name);
                string classDescription = SearchProperty(classNode, MetadataParameters.Description);
                string tableName = SearchProperty(classNode, MetadataParameters.TableName);
            
                if (classNode != null)
                {
                    var propertyDescriptions = FetchProperties(classNode, XmlDescriptionBuilder.PropertyDescriptionElement);

                    return new ClassDescription(className, classDescription, propertyDescriptions)
                           {
                               IsDataAccessClass = classType == TemplateDescriptionType.DataAccess,
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
                        SearchProperty(propertyNode, MetadataParameters.Metainformation)
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

        private static void AddPropertyDescription(Collection<PropertyDescription> propertyDescriptions, XmlNode propertyNode)
        {
            if (propertyDescriptions == null)
            {
                throw new ArgumentNullException("propertyDescriptions");
            }
            if (propertyNode == null)
            {
                throw new ArgumentNullException("propertyNode");
            }

            string propertyName = SearchProperty(propertyNode, MetadataParameters.Name);
            string propertyDescription = SearchProperty(propertyNode, MetadataParameters.Description);
            string propertyType = SearchProperty(propertyNode, MetadataParameters.DataType);
            string propertyDefaultValue = SearchProperty(propertyNode, MetadataParameters.DefaultValue);

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
                XmlNode childNode =
                    node.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "property[@name='{0}']", property));

                if (childNode == null)
                {
                    return null;
                }

                if (childNode.Attributes != null)
                {
                    XmlAttribute attribute = childNode.Attributes["value"];

                    if (attribute == null)
                    {
                        return null;
                    }

                    return attribute.Value;
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