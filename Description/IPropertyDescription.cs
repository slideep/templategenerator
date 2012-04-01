namespace TemplateGenerator.Description
{
    public interface IPropertyDescription
    {
        string Name { get; set; }
        string Description { get; set; }
        string DataType { get; set; }
        dynamic DefaultValue { get; set; }
        string DotNetDataType { get; }
    }
}