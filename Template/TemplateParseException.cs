using System;
using System.Globalization;

namespace TemplateGenerator.Template;

/// <summary>
/// Represents a structured template parsing error.
/// </summary>
public sealed class TemplateParseException : InvalidOperationException
{
    public TemplateParseException(
        TemplateParseErrorCode errorCode,
        string message,
        string? propertyName = null,
        string? fileFullPath = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        PropertyName = propertyName;
        FileFullPath = fileFullPath;
    }

    public TemplateParseErrorCode ErrorCode { get; }

    public string? PropertyName { get; }

    public string? FileFullPath { get; private set; }

    internal static TemplateParseException MissingRootNode()
    {
        return new TemplateParseException(
            TemplateParseErrorCode.MissingRootNode,
            "Template metadata root node is missing.");
    }

    internal static TemplateParseException MissingPropertyElement(string propertyName)
    {
        return new TemplateParseException(
            TemplateParseErrorCode.MissingPropertyElement,
            string.Format(CultureInfo.InvariantCulture, "Missing property element '{0}'.", propertyName),
            propertyName: propertyName);
    }

    internal static TemplateParseException MissingPropertyValueAttribute(string propertyName)
    {
        return new TemplateParseException(
            TemplateParseErrorCode.MissingPropertyValueAttribute,
            string.Format(CultureInfo.InvariantCulture, "Missing value attribute for property '{0}'.", propertyName),
            propertyName: propertyName);
    }

    internal void AttachFileFullPath(string? fileFullPath)
    {
        if (string.IsNullOrWhiteSpace(fileFullPath) || !string.IsNullOrWhiteSpace(FileFullPath))
        {
            return;
        }

        FileFullPath = fileFullPath;
    }
}
