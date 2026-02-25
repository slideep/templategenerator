namespace TemplateGenerator.Description;

public interface IPropertyDescription : IDescription
{
        /// <summary>
        /// Gets or sets the datatype of the property.
        /// </summary>
    string DataType { get; }

        /// <summary>
        /// Gets or sets the default value of the property.
        /// </summary>
    object? DefaultValue { get; }

        /// <summary>
        /// Gets the datatype in textual form for the property.
        /// </summary>
    string DotNetDataType { get; }
}
