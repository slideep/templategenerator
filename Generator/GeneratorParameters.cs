using System;
using System.Globalization;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator
{
    [Serializable]
    internal class GeneratorParameters : IEquatable<GeneratorParameters>
    {
        public string ModelTemplate { get; set; }

        public string PropertyTemplate { get; set; }

        public string SurrogateTemplate { get; set; }

        public string ParameterTemplate { get; set; }

        public TemplateBase GeneroitavaKuvausTyyppiPohja { get; set; }

        #region IEquatable<GeneratorParameters> Members

        public bool Equals(GeneratorParameters other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            return other.Equals(this);
        }

        #endregion

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "ModelTemplate: {0}, PropertyTemplate: {1}, SurrogateTemplate: {2}, ParameterTemplate: {3}, GeneratedTemplate: {4}",
                                 ModelTemplate, PropertyTemplate, SurrogateTemplate, ParameterTemplate,
                                 GeneroitavaKuvausTyyppiPohja);
        }
    }
}