namespace TemplateGenerator.Description
{
    public interface IPropertyDescription : IDescription
    {
        /// <summary>
        /// Gets or sets the datatype of the property.
        /// </summary>
        string DataType { get; set; }

        /// <summary>
        /// Gets or sets the default value of the property.
        /// </summary>
        dynamic DefaultValue { get; set; }

        /// <summary>
        /// Gets the datatype in textual form for the property.
        /// </summary>
        string DotNetDataType { get; }
    }
}