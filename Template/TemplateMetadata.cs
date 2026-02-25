using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TemplateGenerator.Template;

/// <summary>
/// A wrapper class for parsed template's <see cref="XDocument"/>.
/// </summary>
internal class TemplateMetadata
{
    /// <summary>
    /// Default constructor ; initializes metadata from the given <see cref="XDocument"/>'s root element.
    /// </summary>
    /// <param name="xmlDoc"><see cref="XDocument"/></param>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="XDocument"/> is null.</exception>
    public TemplateMetadata(XDocument xmlDoc)
    {
        ArgumentNullException.ThrowIfNull(xmlDoc);

        Node = xmlDoc.Root?.XPathSelectElement("/classTemplate/class[1]");
    }

    /// <summary>
    /// Gets or sets the root node of template's description.
    /// </summary>
    public XElement? Node { get; }

    /// <summary>
    /// Gets the metadata type information (one of the following: Attribute | Class | Element | Operation | Xml).
    /// </summary>
    public MetadataTypes MetadataType => GetMetadataType(MetadataTypeString);

    /// <summary>
    /// Gets (searches) the metadata type as a string value.
    /// </summary>
    private string MetadataTypeString => SearchProperty(RequiredNode, MetadataParameters.MetaInformation);

    /// <summary>
    /// Gets the name of the metadata type (used to describe class's or maybe XML-element's name).
    /// </summary>
    public string Name => SearchProperty(RequiredNode, MetadataParameters.Name);

    /// <summary>
    /// Gets the description of the metadata type (used to describe XML-documentation).
    /// </summary>
    public string Description => SearchProperty(RequiredNode, MetadataParameters.Description);

    /// <summary>
    /// Gets the template description type which is about to be generated or templated.
    /// </summary>
    public TemplateDescriptionTypes ClassTypes => GetClassDescriptionType(ClassTypeString);

    /// <summary>
    /// Gets the class type which is about to be generated or templated.
    /// </summary>
    private string ClassTypeString => SearchProperty(RequiredNode, MetadataParameters.ClassType);

    /// <summary>
    /// Gets the table or collection name where to be generated or templated data is fetched.
    /// </summary>
    public string TableName => SearchProperty(RequiredNode, MetadataParameters.TableName);

    /// <summary>
    /// Gets the event type.
    /// </summary>
    public string EventType => SearchProperty(RequiredNode, MetadataParameters.EventType);

    /// <summary>
    /// Gets the namespace (<see cref="MetadataTypes"/> is usually set to <see cref="MetadataTypes.Xml"/>.
    /// </summary>
    public string Namespace => SearchProperty(RequiredNode, MetadataParameters.Namespace);

    /// <summary>
    /// Gets the visibility for the operation or property.
    /// </summary>
    public string Visibility => SearchProperty(RequiredNode, MetadataParameters.Visibility);

    /// <summary>
    /// Gets the default value assigned to the property or operation return value.
    /// </summary>
    public string DefaultValue => SearchProperty(RequiredNode, MetadataParameters.DefaultValue);

    private XElement RequiredNode => Node ?? throw TemplateParseException.MissingRootNode();

    private static MetadataTypes GetMetadataType(string metadataType)
    {
        ArgumentNullException.ThrowIfNull(metadataType);
        return Enum.Parse<MetadataTypes>(metadataType, true);
    }

    private static TemplateDescriptionTypes GetClassDescriptionType(string classDescriptionType)
    {
        ArgumentNullException.ThrowIfNull(classDescriptionType);
        return Enum.Parse<TemplateDescriptionTypes>(classDescriptionType, true);
    }

    /// <summary>
    /// Searches the attribute value from the given <see cref="XNode"/>'s named property.
    /// </summary>
    /// <param name="node"><see cref="XNode"/></param>
    /// <param name="property">Property element's attribute name.</param>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="XNode"/> or property element's name is null.</exception>
    /// <returns>Property attribute's value</returns>
    public static string SearchProperty(XNode node, string property)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(property);

        var element = node.XPathSelectElement(
                          string.Format(CultureInfo.InvariantCulture, "property[@name='{0}']", property))
                      ?? throw TemplateParseException.MissingPropertyElement(property);

        var attribute = element.Attributes("value").FirstOrDefault()
                        ?? throw TemplateParseException.MissingPropertyValueAttribute(property);

        return attribute.Value;
    }
}
 
