using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Template;

namespace TemplateGenerator.Builder;
    /// <summary>
    /// Creates derived <see cref="DescriptionBuilderBase{TNodeType}"/> builders depending
    /// on the <see cref="MetadataTypes"/>-type specified.
    /// </summary>
    /// <remarks>
    /// This class implements a sortish of factory design pattern in conjunction with the
    /// FactoryGirl.NET ripoff fitted into this implementation.
    /// </remarks>
    public class DescriptionBuilderFactory<TNodeType>
    {
        private readonly object _buildersLock = new();

        /// <summary>
        /// Default constructor - initialize our dictionary for builders.
        /// </summary>
        public DescriptionBuilderFactory()
        {
            Builders = new Dictionary<Type, Func<object>>();
        }

        /// <summary>
        /// Gets or sets builders (either defined or built).
        /// </summary>
        public IDictionary<Type, Func<object>> Builders { get; }

        /// <summary>
        /// Gets and sequence of builders.
        /// </summary>
        public IEnumerable<Type> DefinedBuilders => Builders.Keys;

        /// <summary>
        /// Builds an builder.
        /// </summary>
        /// <typeparam name="TBuilder">The type parametized builder.</typeparam>
        /// <returns><see cref="TBuilder"/></returns>
        public TBuilder Build<TBuilder>() where TBuilder : class, new()
            => Build<TBuilder>(_ => { });

        /// <summary>
        /// Builds an builder and allows customization of the definition.
        /// </summary>
        /// <typeparam name="TBuilder">The type parametized builder.</typeparam>
        /// <param name="customization">Customization of the builder.</param>
        /// <returns><see cref="TBuilder"/></returns>
        public TBuilder Build<TBuilder>(Action<TBuilder> customization) where TBuilder : class, new()
        {
            ArgumentNullException.ThrowIfNull(customization);

            var result = Builders[typeof(TBuilder)]() as TBuilder;

            if (result != null)
            {
                customization(result);

                return result;
            }

            return new TBuilder();
        }

        /// <summary>
        /// Defines an builder factory.
        /// </summary>
        /// <typeparam name="TBuilder">The type parametized builder.</typeparam>
        /// <param name="builder">Builder definition.</param>
        /// <exception cref="ArgumentException">Thrown when builder doesn't inherit from base class.</exception>
        /// <exception cref="InvalidOperationException">Thrown when builder is already registered.</exception>
        /// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
        public void Define<TBuilder>(Func<TBuilder> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            if (!typeof(DescriptionBuilderBase<TNodeType>).IsAssignableFrom(typeof(TBuilder)))
            {
                throw new ArgumentException(
                    "A builder doesn't inherit from '" + typeof(DescriptionBuilderBase<TNodeType>).Name + "'.",
                    nameof(builder));
            }

            lock (_buildersLock)
            {
                if (Builders.ContainsKey(typeof(TBuilder)))
                {
                    throw new InvalidOperationException(
                        typeof(TBuilder).Name + " is already registered. You can only register one builder per type.");
                }

                Builders.Add(typeof(TBuilder), () => builder() ?? throw new InvalidOperationException("Builder factory returned null."));
            }
        }

        /// <summary>
        /// Clears (empties) builders from the dictionary.
        /// </summary>
        public void Empty()
        {
            lock (_buildersLock)
            {
                Builders.Clear();
            }
        }
    }
 
