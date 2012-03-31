namespace TemplateGenerator.Template
{
    public interface ITemplate
    {
        string Name { get; }

        TemplateDescriptionTypes DescriptionTypes { get; }

        string ClassTemplate { get; }

        string PropertyTemplate { get; }

        string SurrogateTemplate { get; }

        string ParameterTemplate { get; }

        string SovitusParameterTemplate { get; }
    }
}