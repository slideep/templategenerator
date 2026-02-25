using System;

namespace TemplateGenerator.Template;

/// <summary>
/// Metadata describing a template asset without requiring its contents.
/// </summary>
public sealed record TemplateAssetDescriptor
{
    public TemplateAssetDescriptor(string name, TemplateAssetSourceKind sourceKind, string sourceId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceId);

        Name = name;
        SourceKind = sourceKind;
        SourceId = sourceId;
    }

    public string Name { get; init; }

    public TemplateAssetSourceKind SourceKind { get; init; }

    public string SourceId { get; init; }
}
