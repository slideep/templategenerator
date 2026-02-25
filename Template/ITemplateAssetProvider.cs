using System.Collections.Generic;

namespace TemplateGenerator.Template;

/// <summary>
/// Provides template descriptors and assets from a source (filesystem, embedded resources, NuGet packs).
/// </summary>
public interface ITemplateAssetProvider
{
    IEnumerable<TemplateAssetDescriptor> GetDescriptors();

    TemplateAsset LoadAsset(TemplateAssetDescriptor descriptor);
}
