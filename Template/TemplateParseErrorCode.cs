namespace TemplateGenerator.Template;

/// <summary>
/// Identifies structured template parsing failures.
/// </summary>
public enum TemplateParseErrorCode
{
    MissingRootNode,
    MissingPropertyElement,
    MissingPropertyValueAttribute
}
