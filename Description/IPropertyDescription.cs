namespace TemplateGenerator.Description
{
    public interface IPropertyDescription
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the summary of the property.
        /// </summary>
        string Description { get; set; }

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