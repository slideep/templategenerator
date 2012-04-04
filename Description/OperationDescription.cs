using System;
using System.Collections.Generic;

namespace TemplateGenerator.Description
{
    /// <summary>
    /// Represents an description of an operation.
    /// </summary>
    [Serializable]
    public class OperationDescription
    {
        public OperationDescription(string name, string description, string returnType,
                                    IDictionary<string, Type> parameters)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            Description = description;
            ReturnDataType = returnType;
            Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets the parameters for the operation (a dictionary containing names and types).
        /// </summary>
        public IDictionary<string, Type> Parameters { get; private set; }

        /// <summary>
        /// Gets or sets the name of the operation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the XML-documentation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the return datatype of the operation.
        /// </summary>
        public string ReturnDataType { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the operation.
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// Gets or sets the datatype of the operation.
        /// </summary>
        public string DotNetDataType
        {
            get
            {
                switch (ReturnDataType)
                {
                    case "void":
                        return "void";
                    case "Long":
                    case "long":
                        return "long";
                    case "Double":
                        return "decimal";
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
                        return ReturnDataType;
                }
            }
        }
    }
}