using System;
using System.Globalization;

namespace TemplateGenerator
{
    internal class GeneratorParameters : IEquatable<GeneratorParameters>
    {
        public string ModelTemplate { get; set; }
        public string PropertyTemplate { get; set; }
        public string SurrogateTemplate { get; set; }
        public string ParameterTemplate { get; set; }
        public ITemplate GeneroitavaKuvausTyyppiPohja { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "ModelTemplate: {0}, PropertyTemplate: {1}, SurrogateTemplate: {2}, ParameterTemplate: {3}, GeneratedTemplate: {4}", 
                ModelTemplate, PropertyTemplate, SurrogateTemplate, ParameterTemplate, GeneroitavaKuvausTyyppiPohja);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + ((ModelTemplate != null) ? this.ModelTemplate.GetHashCode() : 0);
                return result;
            }
        }

        public override bool Equals(object obj)
        {
            GeneratorParameters temp = obj as GeneratorParameters;
            if (temp == null)
                return false;
            return this.Equals(temp);
        }

        public bool Equals(GeneratorParameters other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            return other.Equals(this);
        }
    }
}