namespace TemplateGenerator.Template;

/// <summary>
/// Structured error codes for template asset loading failures.
/// </summary>
public enum TemplateAssetLoadErrorCode
{
    ResourceNotFound,
    ReadFailed,
    InvalidJson,
    EmptyPayload,
    MissingField,
    InvalidDescriptionType
}
