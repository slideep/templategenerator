using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Builder;

namespace TemplateGenerator.Description;

[Serializable]
public class ClassDescription : IDescription
{
    public ClassDescription(
        string name,
        string description,
        IEnumerable<IPropertyDescription>? properties,
        IReadOnlyList<OperationDescription>? operations,
        bool isDataAccessClass = false,
        string? tableName = null,
        string? fileFullPath = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);

        Name = name;
        Description = description;
        Properties = properties?.ToArray() ?? Array.Empty<IPropertyDescription>();
        Operations = operations ?? Array.Empty<OperationDescription>();
        IsDataAccessClass = isDataAccessClass;
        TableName = tableName;
        FileFullPath = fileFullPath;
        Builder = new SqlBuilder();
    }

    public bool IsDataAccessClass { get; }

    public string BuildSqlSelect => Builder.BuildSql(TableName, SqlBuilderOperationTypes.Select);

    public string BuildSqlUpdate => Builder.BuildSql(TableName, SqlBuilderOperationTypes.Update);

    public string BuildSqlInsert => Builder.BuildSql(TableName, SqlBuilderOperationTypes.Insert);

    public IReadOnlyList<OperationDescription> Operations { get; }

    public SqlBuilder Builder { get; }

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
        return other is ClassDescription classDescription &&
               StringComparer.Ordinal.Equals(Name, classDescription.Name);
    }

    public override bool Equals(object? obj) => obj is IDescription other && Equals(other);

    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);
}
