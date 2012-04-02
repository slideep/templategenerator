using System;

namespace TemplateGenerator.Template
{
    /// <summary>
    /// Enumeration types for the templating/generating metadata.
    /// </summary>
    [Flags]
    public enum MetadataTypes
    {
        /// <summary>
        /// None (probably an error).
        /// </summary>
        None = 0,

        /// <summary>
        /// Attribute type
        /// </summary>
        Attribute = 1,

        /// <summary>
        /// Class type
        /// </summary>
        Class = 2,

        /// <summary>
        /// Element type
        /// </summary>
        Element = 4,

        /// <summary>
        /// Operation type
        /// </summary>
        Operation = 8,

        /// <summary>
        /// XML type
        /// </summary>
        Xml = 16,
    }
}