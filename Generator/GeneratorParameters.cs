using System;
using System.Globalization;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator;

[Serializable]
internal class GeneratorParameters : IEquatable<GeneratorParameters>
{
    public string? ModelTemplate { get; set; }

    public string? PropertyTemplate { get; set; }

    public string? SurrogateTemplate { get; set; }

    public string? ParameterTemplate { get; set; }

    public TemplateAsset? GeneroitavaKuvausTyyppiPohja { get; set; }

        #region IEquatable<GeneratorParameters> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
    public bool Equals(GeneratorParameters? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(ModelTemplate, other.ModelTemplate, StringComparison.Ordinal) &&
               string.Equals(PropertyTemplate, other.PropertyTemplate, StringComparison.Ordinal) &&
               string.Equals(SurrogateTemplate, other.SurrogateTemplate, StringComparison.Ordinal) &&
               string.Equals(ParameterTemplate, other.ParameterTemplate, StringComparison.Ordinal) &&
               Equals(GeneroitavaKuvausTyyppiPohja, other.GeneroitavaKuvausTyyppiPohja);
    }

        #endregion

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "ModelTemplate: {0}, PropertyTemplate: {1}, SurrogateTemplate: {2}, ParameterTemplate: {3}, GeneratedTemplate: {4}",
            ModelTemplate,
            PropertyTemplate,
            SurrogateTemplate,
            ParameterTemplate,
            GeneroitavaKuvausTyyppiPohja);
    }

    public override bool Equals(object? obj) => obj is GeneratorParameters other && Equals(other);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            ModelTemplate,
            PropertyTemplate,
            SurrogateTemplate,
            ParameterTemplate,
            GeneroitavaKuvausTyyppiPohja);
    }
}
