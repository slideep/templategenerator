// Example header text. Can be configured in the options.
using System;

namespace TemplateGenerator
{
    internal class GeneratorParameters : IEquatable<GeneratorParameters>
    {
        /// <summary>
        /// Gets or sets the malli pohja.
        /// </summary>
        /// <value>The malli pohja.</value>
        public string ModelTemplate { get; set; }

        /// <summary>
        /// Gets or sets the ominaisuus pohja.
        /// </summary>
        /// <value>The ominaisuus pohja.</value>
        public string PropertyTemplate { get; set; }

        /// <summary>
        /// Gets or sets the surrogaatti pohja.
        /// </summary>
        /// <value>The surrogaatti pohja.</value>
        public string SurrogateTemplate { get; set; }

        /// <summary>
        /// Gets or sets the parametri pohja.
        /// </summary>
        /// <value>The parametri pohja.</value>
        public string ParameterTemplate { get; set; }

        /// <summary>
        /// Gets or sets the generoitava kuvaus tyyppi pohja.
        /// </summary>
        /// <value>The generoitava kuvaus tyyppi pohja.</value>
        public ITemplate GeneroitavaKuvausTyyppiPohja { get; set; }

        public override string ToString()
        {
            return string.Format("ModelTemplate: {0}, PropertyTemplate: {1}, SurrogateTemplate: {2}, ParameterTemplate: {3}, GeneratedTemplate: {4}", 
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