using System;
using System.Globalization;

namespace TemplateGenerator.Template;

/// <summary>
/// Represents a structured template asset loading failure.
/// </summary>
public sealed class TemplateAssetLoadException : InvalidOperationException
{
    public TemplateAssetLoadException(
        TemplateAssetLoadErrorCode errorCode,
        TemplateAssetSourceKind sourceKind,
        string sourceId,
        string message,
        string? fieldName = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceId);

        ErrorCode = errorCode;
        SourceKind = sourceKind;
        SourceId = sourceId;
        FieldName = fieldName;
    }

    public TemplateAssetLoadErrorCode ErrorCode { get; }

    public TemplateAssetSourceKind SourceKind { get; }

    public string SourceId { get; }

    public string? FieldName { get; }

    internal static TemplateAssetLoadException ResourceNotFound(TemplateAssetSourceKind sourceKind, string sourceId)
        => new(
            TemplateAssetLoadErrorCode.ResourceNotFound,
            sourceKind,
            sourceId,
            string.Format(CultureInfo.InvariantCulture, "Template asset resource '{0}' was not found.", sourceId));

    internal static TemplateAssetLoadException ReadFailed(
        TemplateAssetSourceKind sourceKind,
        string sourceId,
        Exception innerException)
        => new(
            TemplateAssetLoadErrorCode.ReadFailed,
            sourceKind,
            sourceId,
            string.Format(CultureInfo.InvariantCulture, "Failed to read template asset '{0}'.", sourceId),
            innerException: innerException);

    internal static TemplateAssetLoadException InvalidJson(
        TemplateAssetSourceKind sourceKind,
        string sourceId,
        Exception innerException)
        => new(
            TemplateAssetLoadErrorCode.InvalidJson,
            sourceKind,
            sourceId,
            string.Format(CultureInfo.InvariantCulture, "Template asset '{0}' contains invalid JSON.", sourceId),
            innerException: innerException);

    internal static TemplateAssetLoadException EmptyPayload(TemplateAssetSourceKind sourceKind, string sourceId)
        => new(
            TemplateAssetLoadErrorCode.EmptyPayload,
            sourceKind,
            sourceId,
            string.Format(CultureInfo.InvariantCulture, "Template asset '{0}' payload was empty.", sourceId));

    internal static TemplateAssetLoadException MissingField(
        TemplateAssetSourceKind sourceKind,
        string sourceId,
        string fieldName)
        => new(
            TemplateAssetLoadErrorCode.MissingField,
            sourceKind,
            sourceId,
            string.Format(CultureInfo.InvariantCulture, "Template asset '{0}' is missing required field '{1}'.", sourceId, fieldName),
            fieldName: fieldName);

    internal static TemplateAssetLoadException InvalidDescriptionType(
        TemplateAssetSourceKind sourceKind,
        string sourceId,
        string? descriptionType)
        => new(
            TemplateAssetLoadErrorCode.InvalidDescriptionType,
            sourceKind,
            sourceId,
            string.Format(
                CultureInfo.InvariantCulture,
                "Template asset '{0}' has invalid descriptionType '{1}'.",
                sourceId,
                descriptionType ?? "<null>"),
            fieldName: "descriptionType");
}
