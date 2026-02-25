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

    public static TemplateRegistry Default { get; } = new();

    public IReadOnlyList<TemplateAssetDescriptor> Descriptors
    {
        get
        {
            lock (_lock)
            {
                return _templates.Count == 0
                    ? Array.Empty<TemplateAssetDescriptor>()
                    : [.. _templates.Values.Select(static asset => asset.Descriptor)];
            }
        }
    }

    public IReadOnlyList<TemplateAsset> Assets
    {
        get
        {
            lock (_lock)
            {
                return _templates.Count == 0 ? Array.Empty<TemplateAsset>() : [.. _templates.Values];
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
        }
    }

    public void LoadFrom(ITemplateAssetProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        foreach (var asset in provider.GetAssets())
        {
            Register(asset);
        }
    }

    public bool Remove(string templateName)
    {
        ArgumentNullException.ThrowIfNull(templateName);

        lock (_lock)
        {
            return _templates.Remove(templateName);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _templates.Clear();
        }
    }
}
