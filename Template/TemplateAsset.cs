using System;

namespace TemplateGenerator.Template;

/// <summary>
/// Immutable template content and metadata used for code generation.
/// </summary>
public sealed record TemplateAsset
{
    public TemplateAsset(
        string name,
        TemplateDescriptionTypes descriptionType,
        string classTemplate,
        string propertyTemplate,
        string parameterTemplate,
        string sovitusParameterTemplate,
        TemplateAssetDescriptor? descriptor = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(classTemplate);
        ArgumentNullException.ThrowIfNull(propertyTemplate);
        ArgumentNullException.ThrowIfNull(parameterTemplate);
        ArgumentNullException.ThrowIfNull(sovitusParameterTemplate);

        var normalizedDescriptor = descriptor is null
            ? new TemplateAssetDescriptor(name, TemplateAssetSourceKind.InMemory, name)
            : NormalizeDescriptorName(descriptor, name);

        Name = name;
        DescriptionType = descriptionType;
        ClassTemplate = classTemplate;
        PropertyTemplate = propertyTemplate;
        ParameterTemplate = parameterTemplate;
        SovitusParameterTemplate = sovitusParameterTemplate;
        Descriptor = normalizedDescriptor;
    }

    public string Name { get; init; }

    public TemplateDescriptionTypes DescriptionType { get; init; }

    public string ClassTemplate { get; init; }

    public string PropertyTemplate { get; init; }

    public string ParameterTemplate { get; init; }

    public string SovitusParameterTemplate { get; init; }

    public TemplateAssetDescriptor Descriptor { get; init; }

    private static TemplateAssetDescriptor NormalizeDescriptorName(TemplateAssetDescriptor descriptor, string name)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return string.Equals(descriptor.Name, name, StringComparison.Ordinal)
            ? descriptor
            : descriptor with { Name = name };
    }
}
