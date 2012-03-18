namespace TemplateGenerator
{
    public interface ITemplate
    {
        string Name { get; }
        TemplateDescriptionType DescriptionType { get; }
        string ClassTemplate { get; }
        string PropertyTemplate { get; }
        string SurrogateTemplate { get; }
        string ParameterTemplate { get; }
        string SovitusParameterTemplate { get; }
    }
}