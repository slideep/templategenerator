using TemplateGenerator.Description;

namespace TemplateGenerator.Generator;

/// <summary>
/// An interface for creating concrete <see cref="IGenerator"/> implementations.
/// </summary>
internal interface IGenerator
{
        /// <summary>
        /// Gets the class template text.
        /// </summary>
        string ClassTemplate { get; }

        /// <summary>
        /// Gets the property template text.
        /// </summary>
        string PropertyTemplate { get; }

        /// <summary>
        /// Gets the generated template text based on <see cref="IDescription"/> implementation.
        /// </summary>
        /// <param name="description"><see cref="IDescription"/> implementation.</param>
        /// <returns>Generated template text</returns>
        string Generate(IDescription description);
}
