using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace TemplateGenerator.Description
{
    /// <summary>
    /// Represents an description of a property.
    /// </summary>
    [Serializable]
    public class PropertyDescription : IPropertyDescription
    {
        /// <summary>
        /// Default constructor ; initialize this instance with name, description and datatype.
        /// </summary>
        /// <remarks>Notice! Name-property is only compulsory.</remarks>
        /// <param name="name">Name of the property</param>
        /// <param name="description">Summary of the property</param>
        /// <param name="dataType">Datatype of the property</param>
        /// <exception cref="ArgumentNullException">Thrown when name parameter is null.</exception>
        public PropertyDescription(string name, string description, string dataType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            Description = description;
            DataType = dataType;
        }

        #region IPropertyDescription Members

        /// <summary>
        /// Gets or sets the template file's used data storage's table or collection name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the summary of the property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the template file's full path.
        /// </summary>
        public string FileFullPath { get; set; }

        /// <summary>
        /// Gets an read-only collection of template's defined property descriptions.
        /// </summary>
        public ReadOnlyCollection<IPropertyDescription> Properties
        {
            get { return Enumerable.Empty<IPropertyDescription>().ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Gets or sets the datatype of the property.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the default value of the property.
        /// </summary>
        public dynamic DefaultValue { get; set; }

        /// <summary>
        /// Gets the datatype in textual form for the property.
        /// </summary>
        public string DotNetDataType
        {
            get
            {
                switch (DataType)
                {
                    case "Long":
                    case "long":
                        return "long";
                    case "Double":
                    case "decimal":
                    case "Decimal":
                        return "decimal";
                    case "Integer":
                    case "int":
                        return "int";
                    case "String":
                    case "string":
                        return "string";
                    case "Date":
                    case "DateTime":
                        return "DateTime";
                    case "bool":
                    case "Boolean":
                        return "bool";
                    default:
                        return DataType;
                }
            }
        }

        #endregion

        /// <summary>
        /// Sets the given default value for the property. 
        /// Based on it's data type converted to the conforming type.
        /// </summary>
        /// <param name="defaultValue">The default value for the property.</param>
        public void SetDefaultValue(string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                return;
            }

            switch (DotNetDataType)
            {
                case "long":
                    DefaultValue = Convert.ToInt64(defaultValue, CultureInfo.InvariantCulture);
                    break;
                case "Double":
                    DefaultValue = Convert.ToDouble(defaultValue, CultureInfo.InvariantCulture);
                    break;
                case "decimal":
                case "Decimal":
                    DefaultValue = Convert.ToDecimal(defaultValue, CultureInfo.InvariantCulture);
                    break;
                case "Integer":
                case "int":
                    DefaultValue = Convert.ToInt32(defaultValue, CultureInfo.InvariantCulture);
                    break;
                case "String":
                case "string":
                    DefaultValue = Convert.ToString(defaultValue, CultureInfo.InvariantCulture);
                    break;
                case "Date":
                case "DateTime":
                    DefaultValue = Convert.ToDateTime(defaultValue, CultureInfo.InvariantCulture);
                    break;
                case "bool":
                case "Boolean":
                    DefaultValue = Convert.ToBoolean(defaultValue, CultureInfo.InvariantCulture);
                    break;
            }
        }

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