namespace TemplateGenerator.Template;

/// <summary>
/// Identifies where a template asset originates.
/// </summary>
public enum TemplateAssetSourceKind
{
    InMemory,
    FileSystem,
    EmbeddedResource,
    NuGetPackage
}
