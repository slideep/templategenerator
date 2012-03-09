namespace TemplateGenerator
{
    internal interface IGenerator
    {
        string ClassTemplate { get; }
        string PropertyTemplate { get; }
        string Generate(IDescription description);
    }
}
