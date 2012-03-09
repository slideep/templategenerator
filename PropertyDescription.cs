using System;

namespace TemplateGenerator
{
    public class PropertyDescription
    {
        public PropertyDescription(string name, string description, string datatype)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            Description = description;
            Datatype = datatype;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Datatype { get; set; }

        protected dynamic DefaultValue { get; set; }

        public void SetDefaultValue(string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                return;
            }

            switch (GetNetDatatype())
            {
                case "long":
                    DefaultValue = Convert.ToInt64(defaultValue);
                    break;
                case "Double":
                    DefaultValue = Convert.ToDouble(defaultValue);
                    break;
                case "decimal":                    
                case "Decimal":
                    DefaultValue = Convert.ToDecimal(defaultValue);
                    break;
                case "Integer":
                case "int":
                    DefaultValue = Convert.ToInt32(defaultValue);
                    break;
                case "String":
                case "string":
                    DefaultValue = Convert.ToString(defaultValue);
                    break;
                case "Date":
                case "DateTime":
                    DefaultValue = Convert.ToDateTime(defaultValue);
                    break;
                case "bool":
                case "Boolean":
                    DefaultValue = Convert.ToBoolean(defaultValue);
                    break;
            }
        }

        public string GetNetDatatype()
        {
            switch (Datatype)
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
                    return Datatype;
            }
        }

        public string GetNetDatatypeLTK()
        {
            switch (Datatype)
            {
                case "long":
                    return "Long";
                case "Long":
                    return "Long";
                case "Double":
                    return "Decimal";
                case "decimal":
                    return "Decimal";
                case "Decimal":
                    return "Decimal";
                case "Integer":
                    return "Int32";
                case "int":
                    return "Int32";
                case "String":
                    return "String";
                case "string":
                    return "String";
                case "Date":
                    return "DateTime";
                case "DateTime":
                    return "DateTime";
                case "bool":
                    return "Bool";
                case "Boolean":
                    return "Bool";
                default:
                    return Datatype;
            }
        }
    }
}