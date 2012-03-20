using System;
using System.Globalization;

namespace TemplateGenerator
{
    public class PropertyDescription
    {
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

        public string Name { get; set; }

        public string Description { get; set; }

        public string DataType { get; set; }

        protected dynamic DefaultValue { get; set; }

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