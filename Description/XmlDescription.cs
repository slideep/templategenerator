using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateGenerator.Description
{
    [Serializable]
    public class XmlDescription : IDescription
    {
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

        public int? Year { get; set; }

        public string TableName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FileFullPath { get; set; }

        public ReadOnlyCollection<IPropertyDescription> Properties { get; private set; }

        #endregion

        public string EventType { get; set; }

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