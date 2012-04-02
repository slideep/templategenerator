using System;

namespace TemplateGenerator.Builder
{
    /// <summary>
    /// Enumeration types for SQL builder operations.
    /// </summary>
	[Flags]
    public enum SqlBuilderOperationTypes
    {
        /// <summary>
        /// None (probably an error)
        /// </summary>
        None = 0,

        /// <summary>
        /// SQL SELECT - clause operation
        /// </summary>
        Select = 1,

        /// <summary>
        /// SQL INSERT - clause operation
        /// </summary>
        Insert = 2,

        /// <summary>
        /// SQl UPDATE - clause operation
        /// </summary>
        Update = 4,

        /// <summary>
        /// SQL DELETE - clause operation
        /// </summary>
		Delete = 8
    }
}
