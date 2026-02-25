using System.Collections.Generic;

namespace TemplateGenerator.Template;

/// <summary>
/// Stores named templates used for rendering descriptions.
/// </summary>
public interface ITemplateRegistry
{
    IReadOnlyList<TemplateAssetDescriptor> Descriptors { get; }

    IReadOnlyList<TemplateAsset> Assets { get; }

    bool TryGet(string templateName, out TemplateAsset? templateAsset);

    void Register(TemplateAsset templateAsset);

    void LoadFrom(ITemplateAssetProvider provider);

    bool Remove(string templateName);

    void Clear();
}
