using System;
using System.Globalization;

namespace TemplateGenerator.Description
{
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

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the summary of the property.
        /// </summary>
        public string Description { get; set; }

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
                    case "long":
                        return "long";
                    case "Double":
                        return "decimal";
                    case "decimal":
                        return "decimal";
                    case "Decimal":
                        return "decimal";
                    case "Integer":
                        return "int";
                    case "int":
                        return "int";
                    case "String":
                        return "string";
                    case "string":
                        return "string";
                    case "Date":
                        return "DateTime";
                    case "DateTime":
                        return "DateTime";
                    case "bool":
                        return "bool";
                    case "Boolean":
                        return "bool";
                    default:
                        return DataType;
                }
            }
        }

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
    }
}