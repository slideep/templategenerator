using System;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator;

/// <summary>
/// Renders descriptions using templates from a registry.
/// </summary>
public sealed class TemplateGeneratorService
{
    private readonly ITemplateRegistry _templateRegistry;

    public TemplateGeneratorService(ITemplateRegistry templateRegistry)
    {
        _templateRegistry = templateRegistry ?? throw new ArgumentNullException(nameof(templateRegistry));
    }

    public string? GenerateDescription(IDescription description, string templateName)
    {
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(templateName);

        if (!_templateRegistry.TryGet(templateName, out var template) || template is null)
        {
            return null;
        }

        return new ClassGenerator(template).Generate(description);
    }
}
