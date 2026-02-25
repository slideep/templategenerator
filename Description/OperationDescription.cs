using System;
using System.Collections.Generic;

namespace TemplateGenerator.Description;

/// <summary>
/// Represents a description of an operation.
/// </summary>
[Serializable]
public class OperationDescription
{
    public OperationDescription(
        string name,
        string? description,
        string? returnType,
        IReadOnlyDictionary<string, Type>? parameters)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Description = description ?? string.Empty;
        ReturnDataType = returnType ?? string.Empty;
        Parameters = parameters ?? new Dictionary<string, Type>();
    }

    /// <summary>
    /// Gets the parameters for the operation (a dictionary containing names and types).
    /// </summary>
    public IReadOnlyDictionary<string, Type> Parameters { get; }

    /// <summary>
    /// Gets the name of the operation.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the description of the XML-documentation.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the return datatype of the operation.
    /// </summary>
    public string ReturnDataType { get; }

    /// <summary>
    /// Gets or sets the visibility of the operation.
    /// </summary>
    public string? Visibility { get; set; }

    /// <summary>
    /// Gets the datatype of the operation.
    /// </summary>
    public string DotNetDataType => ReturnDataType switch
    {
        "void" => "void",
        "Long" or "long" => "long",
        "Double" => "decimal",
        "decimal" or "Decimal" => "decimal",
        "Integer" or "int" => "int",
        "String" or "string" => "string",
        "Date" or "DateTime" => "DateTime",
        "bool" or "Boolean" => "bool",
        _ => ReturnDataType
    };
}
