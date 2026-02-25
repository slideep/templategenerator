using System;
using System.Collections.Generic;
using System.Globalization;

namespace TemplateGenerator.Description;

/// <summary>
/// Represents a description of a property.
/// </summary>
[Serializable]
public class PropertyDescription : IPropertyDescription
{
    public PropertyDescription(
        string name,
        string? description,
        string? dataType,
        string? tableName = null,
        string? fileFullPath = null,
        object? defaultValue = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Description = description ?? string.Empty;
        DataType = dataType ?? string.Empty;
        TableName = tableName;
        FileFullPath = fileFullPath;
        DefaultValue = defaultValue;
    }

    /// <summary>
    /// Gets or sets the template file's used data storage's table or collection name.
    /// </summary>
    public string? TableName { get; }

    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the summary of the property.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the template file's full path.
    /// </summary>
    public string? FileFullPath { get; }

    /// <summary>
    /// Property descriptions do not contain nested property descriptions.
    /// </summary>
    public IReadOnlyList<IPropertyDescription> Properties => Array.Empty<IPropertyDescription>();

    /// <summary>
    /// Gets the datatype of the property.
    /// </summary>
    public string DataType { get; }

    /// <summary>
    /// Gets the default value of the property.
    /// </summary>
    public object? DefaultValue { get; }

    /// <summary>
    /// Gets the datatype in textual form for the property.
    /// </summary>
    public string DotNetDataType => DataType switch
    {
        "Long" or "long" => "long",
        "Double" or "decimal" or "Decimal" => "decimal",
        "Integer" or "int" => "int",
        "String" or "string" => "string",
        "Date" or "DateTime" => "DateTime",
        "bool" or "Boolean" => "bool",
        _ => DataType
    };

    /// <summary>
    /// Sets the given default value for the property.
    /// Based on its data type, converted to the conforming type.
    /// </summary>
    /// <param name="defaultValue">The default value for the property.</param>
    public PropertyDescription WithDefaultValue(string? defaultValue)
    {
        if (string.IsNullOrWhiteSpace(defaultValue))
        {
            return this;
        }

        return new PropertyDescription(
            Name,
            Description,
            DataType,
            TableName,
            FileFullPath,
            ConvertDefaultValue(defaultValue));
    }

    private object? ConvertDefaultValue(string defaultValue)
    {
        return DotNetDataType switch
        {
            "long" => Convert.ToInt64(defaultValue, CultureInfo.InvariantCulture),
            "decimal" => Convert.ToDecimal(defaultValue, CultureInfo.InvariantCulture),
            "int" => Convert.ToInt32(defaultValue, CultureInfo.InvariantCulture),
            "string" => Convert.ToString(defaultValue, CultureInfo.InvariantCulture),
            "DateTime" => Convert.ToDateTime(defaultValue, CultureInfo.InvariantCulture),
            "bool" => Convert.ToBoolean(defaultValue, CultureInfo.InvariantCulture),
            _ => DefaultValue
        };
    }

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
        return other is PropertyDescription propertyDescription &&
               StringComparer.Ordinal.Equals(Name, propertyDescription.Name);
    }

    public override bool Equals(object? obj) => obj is IDescription other && Equals(other);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);
}
