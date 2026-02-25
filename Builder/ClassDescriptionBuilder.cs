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

/// <summary>
/// A builder for class descriptions.
/// </summary>
public class ClassDescriptionBuilder : DescriptionBuilderBase<XElement>
{
    public ClassDescriptionBuilder()
    {
    }

    public ClassDescriptionBuilder(string templateDirectory)
        : base(templateDirectory)
    {
    }

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
        var meta = new TemplateMetadata(xmlDocument);

        if (meta.Node == null)
        {
            throw TemplateParseException.MissingRootNode();
        }

        if (meta.MetadataType != MetadataTypes.Class)
        {
            return null;
        }

        var classTypeName = TemplateMetadata.SearchProperty(meta.Node, MetadataParameters.ClassType);
        var className = meta.Name;
        var classDescription = meta.Description;

        if (string.IsNullOrWhiteSpace(classTypeName) || string.IsNullOrWhiteSpace(className))
        {
            return null;
        }

        var classType = Enum.Parse<TemplateDescriptionTypes>(classTypeName, true);
        var propertyDescriptions = FetchProperties(meta.Node, XmlDescriptionBuilder.PropertyDescriptionElement);

        return new ClassDescription(
            className,
            classDescription,
            propertyDescriptions,
            Array.Empty<OperationDescription>(),
            isDataAccessClass: classType == TemplateDescriptionTypes.DataAccess,
            tableName: meta.TableName,
            fileFullPath: fileFullPath);
    }

    /// <summary>
    /// Fetch enumerable over <see cref="PropertyDescription"/> types.
    /// </summary>
    /// <param name="templateNode">Node type</param>
    /// <param name="propertyDescription">Property description</param>
    /// <exception cref="ArgumentNullException">Thrown when template node or property description is null.</exception>
    /// <returns>A IEnumerable{PropertyDescription} sequence with (or without) property descriptions.</returns>
    protected override IEnumerable<PropertyDescription> FetchProperties(XElement templateNode, string propertyDescription)
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
            var isPublic = string.Equals(
                TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.Visibility),
                MetadataParameters.Public,
                StringComparison.Ordinal);
            var hasAttribute = string.Equals(
                TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.MetaInformation),
                MetadataParameters.Attribute,
                StringComparison.Ordinal);

            if (isPublic && hasAttribute)
            {
                AddPropertyDescription(propertyDescriptions, propertyNode);
            }
        }

        return propertyDescriptions;
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
    private static void AddPropertyDescription(ICollection<PropertyDescription> propertyDescriptions, XElement propertyNode)
    {
        ArgumentNullException.ThrowIfNull(propertyDescriptions);
        ArgumentNullException.ThrowIfNull(propertyNode);

        var propertyName = TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.Name);
        var propertyDescription = TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.Description);
        var propertyType = TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.DataType);
        var propertyDefaultValue = TemplateMetadata.SearchProperty(propertyNode, MetadataParameters.DefaultValue);

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new InvalidOperationException("Property name is missing.");
        }

        var property = new PropertyDescription(propertyName, propertyDescription, propertyType)
            .WithDefaultValue(propertyDefaultValue);

        if (propertyDescriptions.Contains(property))
        {
            throw new InvalidOperationException(
                string.Format(CultureInfo.InvariantCulture, ": '{0}'", propertyName));
        }

        propertyDescriptions.Add(property);
    }
}
 
