using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TemplateGenerator.Template;

/// <summary>
/// Loads template assets from embedded JSON resources.
/// </summary>
public sealed class EmbeddedResourceTemplateAssetProvider : ITemplateAssetProvider
{
    private readonly Assembly _assembly;
    private readonly string? _resourcePrefix;

    public EmbeddedResourceTemplateAssetProvider(Assembly assembly, string? resourcePrefix = null)
    {
        _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        _resourcePrefix = resourcePrefix;
    }

    public IEnumerable<TemplateAssetDescriptor> GetDescriptors()
        => [.. GetResourceNames().Select(CreateDescriptor)];

    public TemplateAsset LoadAsset(TemplateAssetDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        if (descriptor.SourceKind != TemplateAssetSourceKind.EmbeddedResource)
        {
            throw new ArgumentException("Descriptor source kind must be EmbeddedResource.", nameof(descriptor));
        }

        return LoadAsset(descriptor.SourceId);
    }

    private IEnumerable<string> GetResourceNames()
    {
        return _assembly
            .GetManifestResourceNames()
            .Where(name => name.EndsWith(".template.json", StringComparison.Ordinal))
            .Where(name => string.IsNullOrEmpty(_resourcePrefix) || name.StartsWith(_resourcePrefix, StringComparison.Ordinal))
            .OrderBy(static name => name, StringComparer.Ordinal);
    }

    private TemplateAsset LoadAsset(string resourceName)
    {
        using var stream = _assembly.GetManifestResourceStream(resourceName)
            ?? throw TemplateAssetLoadException.ResourceNotFound(TemplateAssetSourceKind.EmbeddedResource, resourceName);

        try
        {
            return TemplateAssetJsonLoader.LoadFromEmbeddedResource(stream, resourceName);
        }
        catch (TemplateAssetLoadException)
        {
            throw;
        }
        catch (Exception ex) when (ex is IOException or NotSupportedException or ObjectDisposedException)
        {
            throw TemplateAssetLoadException.ReadFailed(TemplateAssetSourceKind.EmbeddedResource, resourceName, ex);
        }
    }

    private static TemplateAssetDescriptor CreateDescriptor(string resourceName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);

        return new TemplateAssetDescriptor(
            InferNameFromResourceName(resourceName),
            TemplateAssetSourceKind.EmbeddedResource,
            resourceName);
    }

    private static string InferNameFromResourceName(string resourceName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);

        const string suffix = ".template.json";
        var withoutSuffix = resourceName.EndsWith(suffix, StringComparison.Ordinal)
            ? resourceName[..^suffix.Length]
            : resourceName;

        var lastDotIndex = withoutSuffix.LastIndexOf('.');
        return lastDotIndex >= 0 ? withoutSuffix[(lastDotIndex + 1)..] : withoutSuffix;
    }
}
