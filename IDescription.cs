using System.Collections.ObjectModel;

namespace TemplateGenerator
{
    public interface IDescription
    {
        string TableName { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string FileFullPath { get; set; }
        ReadOnlyCollection<PropertyDescription> Properties { get; }
    }
}