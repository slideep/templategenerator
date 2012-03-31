using System;

namespace TemplateGenerator.Template
{
    [Flags]
    public enum MetadataTypes
    {
        None = 0,
        Attribute = 1,
        Class = 2,
        Element = 4,
        Operation = 8,
        Xml = 16,
    }
}