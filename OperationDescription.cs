using System;
using System.Collections.Generic;

namespace TemplateGenerator
{
    public class OperationDescription
    {
        public IDictionary<string, Type> Parameters { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ReturnDataType { get; set; }

        public string Visibility { get; set; }

        public OperationDescription(string name, string description, string returnType, IDictionary<string, Type> parameters)
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

        public string DotNetDataType
        {
        	get
        	{
            switch (ReturnDataType)
            {
                case "void":
                    return "void";
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
                    return ReturnDataType;
            }
        	}
        }
    }
}
