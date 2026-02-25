using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TemplateGenerator.Template;

/// <summary>
/// Loads template assets from JSON files on disk.
/// </summary>
public sealed class FileSystemTemplateAssetProvider : ITemplateAssetProvider
{
    public const string DefaultSearchPattern = "*.template.json";

    private readonly string _rootDirectory;
    private readonly string _searchPattern;
    private readonly SearchOption _searchOption;

    public FileSystemTemplateAssetProvider(
        string rootDirectory,
        string searchPattern = DefaultSearchPattern,
        bool recursive = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(searchPattern);

        _rootDirectory = rootDirectory;
        _searchPattern = searchPattern;
        _searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
    }

    public IEnumerable<TemplateAssetDescriptor> GetDescriptors()
    {
        if (!Directory.Exists(_rootDirectory))
        {
            return Array.Empty<TemplateAssetDescriptor>();
        }

        return Directory
            .EnumerateFiles(_rootDirectory, _searchPattern, _searchOption)
            .OrderBy(static path => path, StringComparer.Ordinal)
            .Select(static path => new TemplateAssetDescriptor(
                InferNameFromFilePath(path),
                TemplateAssetSourceKind.FileSystem,
                path))
            .ToArray();
    }

    public TemplateAsset LoadAsset(TemplateAssetDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        if (descriptor.SourceKind != TemplateAssetSourceKind.FileSystem)
        {
            throw new ArgumentException("Descriptor source kind must be FileSystem.", nameof(descriptor));
        }

        return TemplateAssetJsonLoader.LoadFromFile(descriptor.SourceId);
    }

    private static string InferNameFromFilePath(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var fileName = Path.GetFileName(filePath);
        const string suffix = ".template.json";

        return fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
            ? fileName[..^suffix.Length]
            : Path.GetFileNameWithoutExtension(filePath);
    }
}
