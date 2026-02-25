using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateGenerator.Description;

/// <summary>
/// Represents a description of an XML file (or element).
/// </summary>
[Serializable]
public class XmlDescription : IDescription
{
    public XmlDescription(
        string xmlName,
        string xmlDescription,
        IEnumerable<IPropertyDescription> properties,
        string? tableName = null,
        string? xmlNamespace = null,
        string? fileFullPath = null)
    {
        ArgumentNullException.ThrowIfNull(xmlName);
        ArgumentNullException.ThrowIfNull(xmlDescription);
        ArgumentNullException.ThrowIfNull(properties);

        Name = xmlName;
        Description = xmlDescription;
        Properties = properties.ToArray();
        TableName = tableName;
        Namespace = xmlNamespace;
        FileFullPath = fileFullPath;
    }

    /// <summary>
    /// Gets or sets the template file's used data storage's table or collection name.
    /// </summary>
    public string? TableName { get; }

    /// <summary>
    /// Gets the template file's name (usually a class or XML-element name etc.).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the template file's description (or summary for XML-documentation).
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the template file's full path.
    /// </summary>
    public string? FileFullPath { get; }

    /// <summary>
    /// Gets a read-only collection of template-defined property descriptions.
    /// </summary>
    public IReadOnlyList<IPropertyDescription> Properties { get; }

    public string? Namespace { get; }

    public int CompareTo(IDescription? other)
    {
        if (other is null)
        {
            return 1;
        }

        return StringComparer.Ordinal.Compare(Name, other.Name);
    }

    public bool Equals(IDescription? other)
    {
        return other is XmlDescription xmlDescription &&
               StringComparer.Ordinal.Equals(Name, xmlDescription.Name);
    }

    public override bool Equals(object? obj) => obj is IDescription other && Equals(other);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);
}
