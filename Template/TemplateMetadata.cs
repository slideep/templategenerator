using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TemplateGenerator.Template
{
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
            if (xmlDoc == null)
            {
                throw new ArgumentNullException(nameof(xmlDoc));
            }

            if (xmlDoc.Root != null)
            {
                Node = xmlDoc.Root.XPathSelectElement("/classTemplate/class[1]");
            }
        }

        /// <summary>
        /// Gets or sets the root node of template's description.
        /// </summary>
        public XElement Node { get; }

        /// <summary>
        /// Gets the metadata type information (one of the following: Attribute | Class | Element | Operation | Xml).
        /// </summary>
        public MetadataTypes MetadataType => GetMetadataType(MetadataTypeString);

        /// <summary>
        /// Gets (searches) the metadata type as a string value.
        /// </summary>
        private string MetadataTypeString => SearchProperty(Node, MetadataParameters.MetaInformation);

        /// <summary>
        /// Gets the name of the metadata type (used to describe class's or maybe XML-element's name).
        /// </summary>
        public string Name => SearchProperty(Node, MetadataParameters.Name);

        /// <summary>
        /// Gets the description of the metadata type (used to describe XML-documentation).
        /// </summary>
        public string Description => SearchProperty(Node, MetadataParameters.Description);

        /// <summary>
        /// Gets the template description type which is about to be generated or templated.
        /// </summary>
        public TemplateDescriptionTypes ClassTypes => GetClassDescriptionType(ClassTypeString);

        /// <summary>
        /// Gets the class type which is about to be generated or templated.
        /// </summary>
        private string ClassTypeString => SearchProperty(Node, MetadataParameters.ClassType);

        /// <summary>
        /// Gets the table or collection name where to be generated or templated data is fetched.
        /// </summary>
        public string TableName => SearchProperty(Node, MetadataParameters.TableName);

        /// <summary>
        /// Gets the event type.
        /// </summary>
        public string EventType => SearchProperty(Node, MetadataParameters.EventType);

        /// <summary>
        /// Gets the namespace (<see cref="MetadataTypes"/> is usually set to <see cref="MetadataTypes.Xml"/>.
        /// </summary>
        public string Namespace => SearchProperty(Node, MetadataParameters.Namespace);

        /// <summary>
        /// Gets the visibility for the operation or property.
        /// </summary>
        public string Visibility => SearchProperty(Node, MetadataParameters.Visibility);

        /// <summary>
        /// Gets the default value assigned to the property or operation return value.
        /// </summary>
        public string DefaultValue => SearchProperty(Node, MetadataParameters.DefaultValue);

        private static MetadataTypes GetMetadataType(string metadataType)
        {
            if (metadataType == null)
                throw new ArgumentNullException(nameof(metadataType));

            return (MetadataTypes)Enum.Parse(typeof(MetadataTypes), metadataType, true);
        }

        private static TemplateDescriptionTypes GetClassDescriptionType(string classDescriptionType)
        {
            if (classDescriptionType == null)
                throw new ArgumentNullException(nameof(classDescriptionType));

            return (TemplateDescriptionTypes)Enum.Parse(typeof(TemplateDescriptionTypes), classDescriptionType, true);
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
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return
                node.XPathSelectElement(string.Format(CultureInfo.InvariantCulture, "property[@name='{0}']", property)).
                    Attributes("value").First().Value;
        }
    }
} 