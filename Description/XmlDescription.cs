using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateGenerator.Description
{
    /// <summary>
    /// Represents an description of a XML-file (or element).
    /// </summary>
    [Serializable]
    public class XmlDescription : IDescription
    {
        /// <summary>
        /// Default constructor ; initialized new description.
        /// </summary>
        /// <param name="xmlName">Name of the template description</param>
        /// <param name="xmlDescription">Template</param>
        /// <param name="properties">Template properties</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor parameter's is null.</exception>
        public XmlDescription(string xmlName, string xmlDescription, IEnumerable<IPropertyDescription> properties)
        {
            if (xmlName == null)
            {
                throw new ArgumentNullException("xmlName");
            }
            if (xmlDescription == null)
            {
                throw new ArgumentNullException("xmlDescription");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            Name = xmlName;
            Description = xmlDescription;
            Properties = new ReadOnlyCollection<IPropertyDescription>(properties.ToList());
        }

        #region Implementation of IDescription

        /// <summary>
        /// Gets or sets the template file's used data storage's table or collection name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the template file's name (usually a class or XM-element name etc.).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the template file's description (or summary for XML-documentation).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the template file's full path.
        /// </summary>
        public string FileFullPath { get; set; }

        /// <summary>
        /// Gets an read-only collection of template's defined property descriptions.
        /// </summary>
        public ReadOnlyCollection<IPropertyDescription> Properties { get; private set; }

        #endregion

        public string Namespace { get; set; }

        #region Implementation of IComparable<in IDescription>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(IDescription other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IEquatable<IDescription>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IDescription other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}