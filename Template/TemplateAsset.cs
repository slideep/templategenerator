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

        Name = name;
        DescriptionType = descriptionType;
        ClassTemplate = classTemplate;
        PropertyTemplate = propertyTemplate;
        ParameterTemplate = parameterTemplate;
        SovitusParameterTemplate = sovitusParameterTemplate;
        Descriptor = descriptor ?? new TemplateAssetDescriptor(name, TemplateAssetSourceKind.InMemory, name);
    }

    public string Name { get; init; }

    public TemplateDescriptionTypes DescriptionType { get; init; }

    public string ClassTemplate { get; init; }

    public string PropertyTemplate { get; init; }

    public string ParameterTemplate { get; init; }

    public string SovitusParameterTemplate { get; init; }

    public TemplateAssetDescriptor Descriptor { get; init; }
}
