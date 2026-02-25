using System.Collections.Generic;

namespace TemplateGenerator.Template;

/// <summary>
/// Provides template descriptors and assets from a source (filesystem, embedded resources, NuGet packs).
/// </summary>
public interface ITemplateAssetProvider
{
    IReadOnlyList<TemplateAssetDescriptor> GetDescriptors();

    IReadOnlyList<TemplateAsset> GetAssets();
}
