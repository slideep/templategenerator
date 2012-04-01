using System;
using System.Collections.ObjectModel;

namespace TemplateGenerator.Description
{
    /// <summary>
    /// An interface that template's description classes has to implement.
    /// </summary>
    public interface IDescription : IComparable<IDescription>, IEquatable<IDescription>
    {
        /// <summary>
        /// Gets or sets the template file's used data storage's table or collection name.
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// Gets or sets the template file's name (usually a class or XM-element name etc.).
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the template file's description (or summary for XML-documentation).
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the template file's full path.
        /// </summary>
        string FileFullPath { get; set; }

        /// <summary>
        /// Gets an read-only collection of template's defined property descriptions.
        /// </summary>
        ReadOnlyCollection<IPropertyDescription> Properties { get; }
    }
}