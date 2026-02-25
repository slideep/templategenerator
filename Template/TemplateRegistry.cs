using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateGenerator.Template;

/// <summary>
/// Thread-safe in-memory template registry.
/// </summary>
public sealed class TemplateRegistry : ITemplateRegistry
{
    private readonly object _lock = new();
    private readonly Dictionary<string, TemplateAsset> _templates = new(StringComparer.Ordinal);
    private TemplateAsset[]? _assetsSnapshot;
    private TemplateAssetDescriptor[]? _descriptorSnapshot;

    /// <summary>
    /// Shared process-wide registry used by legacy static APIs. Prefer injecting an
    /// <see cref="ITemplateRegistry"/> into <c>GeneratorController</c> for isolation.
    /// </summary>
    public static TemplateRegistry Default { get; } = new();

    public IReadOnlyList<TemplateAssetDescriptor> Descriptors
    {
        get
        {
            lock (_lock)
            {
                if (_templates.Count == 0)
                {
                    return Array.Empty<TemplateAssetDescriptor>();
                }

                return _descriptorSnapshot ??= BuildDescriptorSnapshot();
            }
        }
    }

    public IReadOnlyList<TemplateAsset> Assets
    {
        get
        {
            lock (_lock)
            {
                if (_templates.Count == 0)
                {
                    return Array.Empty<TemplateAsset>();
                }

                return _assetsSnapshot ??= BuildAssetSnapshot();
            }
        }
    }

    public bool TryGet(string templateName, out TemplateAsset? templateAsset)
    {
        ArgumentNullException.ThrowIfNull(templateName);

        lock (_lock)
        {
            if (_templates.TryGetValue(templateName, out var found))
            {
                templateAsset = found;
                return true;
            }
        }

        templateAsset = null;
        return false;
    }

    public void Register(TemplateAsset templateAsset)
    {
        ArgumentNullException.ThrowIfNull(templateAsset);

        if (string.IsNullOrWhiteSpace(templateAsset.Name))
        {
            throw new ArgumentException("Template name must be a non-empty value.", nameof(templateAsset));
        }

        lock (_lock)
        {
            if (_templates.ContainsKey(templateAsset.Name))
            {
                throw new InvalidOperationException($"Template '{templateAsset.Name}' is already registered.");
            }

            _templates.Add(templateAsset.Name, templateAsset);
            InvalidateSnapshots();
        }
    }

    public void LoadFrom(ITemplateAssetProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        var loadedAssets = new List<TemplateAsset>();
        var loadedNames = new HashSet<string>(StringComparer.Ordinal);

        foreach (var descriptor in provider.GetDescriptors())
        {
            var asset = provider.LoadAsset(descriptor);

            if (string.IsNullOrWhiteSpace(asset.Name))
            {
                throw new ArgumentException("Template name must be a non-empty value.", nameof(provider));
            }

            if (!loadedNames.Add(asset.Name))
            {
                throw new InvalidOperationException($"Template '{asset.Name}' is already registered.");
            }

            loadedAssets.Add(asset);
        }

        if (loadedAssets.Count == 0)
        {
            return;
        }

        lock (_lock)
        {
            foreach (var asset in loadedAssets)
            {
                if (_templates.ContainsKey(asset.Name))
                {
                    throw new InvalidOperationException($"Template '{asset.Name}' is already registered.");
                }
            }

            foreach (var asset in loadedAssets)
            {
                _templates.Add(asset.Name, asset);
            }

            InvalidateSnapshots();
        }
    }

    public bool Remove(string templateName)
    {
        ArgumentNullException.ThrowIfNull(templateName);

        lock (_lock)
        {
            var removed = _templates.Remove(templateName);
            if (removed)
            {
                InvalidateSnapshots();
            }

            return removed;
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            if (_templates.Count == 0)
            {
                return;
            }

            _templates.Clear();
            InvalidateSnapshots();
        }
    }

    private TemplateAsset[] BuildAssetSnapshot()
        => [.. _templates.Values];

    private TemplateAssetDescriptor[] BuildDescriptorSnapshot()
        => [.. _templates.Values.Select(static asset => asset.Descriptor)];

    private void InvalidateSnapshots()
    {
        _assetsSnapshot = null;
        _descriptorSnapshot = null;
    }
}
