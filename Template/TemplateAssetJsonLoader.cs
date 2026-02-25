using System;
using System.IO;
using System.Text.Json;

namespace TemplateGenerator.Template;

internal static class TemplateAssetJsonLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static TemplateAsset LoadFromFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        try
        {
            using var stream = File.OpenRead(filePath);
            return LoadFromStream(stream, TemplateAssetSourceKind.FileSystem, filePath);
        }
        catch (TemplateAssetLoadException)
        {
            throw;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw TemplateAssetLoadException.ReadFailed(TemplateAssetSourceKind.FileSystem, filePath, ex);
        }
    }

    public static TemplateAsset LoadFromEmbeddedResource(Stream stream, string resourceName)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);

        try
        {
            return LoadFromStream(stream, TemplateAssetSourceKind.EmbeddedResource, resourceName);
        }
        catch (TemplateAssetLoadException)
        {
            throw;
        }
    }

    private static TemplateAsset LoadFromStream(Stream stream, TemplateAssetSourceKind sourceKind, string sourceId)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceId);

        TemplateAssetDocument document;
        try
        {
            document = JsonSerializer.Deserialize<TemplateAssetDocument>(stream, JsonOptions)
                ?? throw TemplateAssetLoadException.EmptyPayload(sourceKind, sourceId);
        }
        catch (TemplateAssetLoadException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw TemplateAssetLoadException.InvalidJson(sourceKind, sourceId, ex);
        }

        if (!Enum.TryParse<TemplateDescriptionTypes>(document.DescriptionType, ignoreCase: true, out var descriptionType))
        {
            throw TemplateAssetLoadException.InvalidDescriptionType(sourceKind, sourceId, document.DescriptionType);
        }

        var name = RequiredField(document.Name, "name", sourceKind, sourceId);
        var classTemplate = RequiredField(document.ClassTemplate, "classTemplate", sourceKind, sourceId);
        var propertyTemplate = RequiredField(document.PropertyTemplate, "propertyTemplate", sourceKind, sourceId);

        return new TemplateAsset(
            name,
            descriptionType,
            classTemplate,
            propertyTemplate,
            document.ParameterTemplate ?? string.Empty,
            document.SovitusParameterTemplate ?? string.Empty,
            new TemplateAssetDescriptor(
                name,
                sourceKind,
                sourceId));
    }

    private static string RequiredField(
        string? value,
        string fieldName,
        TemplateAssetSourceKind sourceKind,
        string sourceId)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw TemplateAssetLoadException.MissingField(sourceKind, sourceId, fieldName);
        }

        return value;
    }

    private sealed class TemplateAssetDocument
    {
        public string? Name { get; init; }

        public string? DescriptionType { get; init; }

        public string? ClassTemplate { get; init; }

        public string? PropertyTemplate { get; init; }

        public string? ParameterTemplate { get; init; }

        public string? SovitusParameterTemplate { get; init; }
    }
}
