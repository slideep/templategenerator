using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TemplateGenerator.Builder;

namespace TemplateGenerator.Description
{
    [Serializable]
    public class ClassDescription : IDescription
    {
        public ClassDescription(string name, string description, IEnumerable<IPropertyDescription> properties,
                                ReadOnlyCollection<OperationDescription> operations)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            Name = name;
            Description = description;

            if (properties != null)
            {
                Properties = new ReadOnlyCollection<IPropertyDescription>(properties.ToList());
            }

            Operations = operations;
            Builder = new SqlBuilder();
        }

        public bool IsDataAccessClass { get; set; }

        public string BuildSqlSelect => Builder.BuildSql(TableName, SqlBuilderOperationTypes.Select);

        public string BuildSqlUpdate => Builder.BuildSql(TableName, SqlBuilderOperationTypes.Update);

        public string BuildSqlInsert => Builder.BuildSql(TableName, SqlBuilderOperationTypes.Insert);

        public ReadOnlyCollection<OperationDescription> Operations { get; private set; }

        public SqlBuilder Builder { get; }

        #region IDescription Members

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
        public ReadOnlyCollection<IPropertyDescription> Properties { get; }

        #endregion

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