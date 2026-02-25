namespace TemplateGenerator;

/// <summary>
/// Options for configuring template discovery.
/// </summary>
public sealed class TemplateGeneratorOptions
{
    public const string DefaultTemplateDirectory = "Templates";

    public string TemplateDirectory { get; init; } = DefaultTemplateDirectory;
}
