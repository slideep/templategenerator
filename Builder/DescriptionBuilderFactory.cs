﻿using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Template;

namespace TemplateGenerator.Builder
{
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
        /// <summary>
        /// Default constructor - initialize our dictionary for builders.
        /// </summary>
        public DescriptionBuilderFactory()
        {
            Builders = new Dictionary<Type, Func<object>>();
        }

        /// <summary>
        /// Gets and sequence of builders.
        /// </summary>
        public IEnumerable<Type> DefinedBuilders
        {
            get { return Builders.Select(x => x.Key); }
        }

        /// <summary>
        /// Gets or sets builders (either defined or built).
        /// </summary>
        public IDictionary<Type, Func<object>> Builders { get; private set; }

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
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (Builders.ContainsKey(typeof (TBuilder)))
            {
                throw new InvalidOperationException(typeof (TBuilder).Name +
                                                    " is already registered. You can only register one builder per type.");
            }
            if (typeof (TBuilder).BaseType != typeof (DescriptionBuilderBase<TNodeType>))
            {
                throw new ArgumentException(
                    "A builder doesn't inherit from '" + typeof (DescriptionBuilderBase<TNodeType>).Name + "'.",
                    "builder");
            }

            lock (Builders)
            {
                Builders.Add(typeof (TBuilder), () => builder());
            }
        }

        /// <summary>
        /// Builds an builder.
        /// </summary>
        /// <typeparam name="TBuilder">The type parametized builder.</typeparam>
        /// <returns><see cref="TBuilder"/></returns>
        public TBuilder Build<TBuilder>() where TBuilder : class, new()
        {
            return Build<TBuilder>(x => { });
        }

        /// <summary>
        /// Builds an builder and allows customization of the definition.
        /// </summary>
        /// <typeparam name="TBuilder">The type parametized builder.</typeparam>
        /// <param name="overrides">Customization of the builder.</param>
        /// <returns><see cref="TBuilder"/></returns>
        public TBuilder Build<TBuilder>(Action<TBuilder> overrides) where TBuilder : class, new()
        {
            if (overrides == null)
            {
                throw new ArgumentNullException("overrides");
            }

            var result = Builders[typeof (TBuilder)]() as TBuilder;

            if (result != null)
            {
                overrides(result);

                return result;
            }

            return new Func<TBuilder>(() => new TBuilder()).Invoke();
        }

        /// <summary>
        /// Clears (empties) builders from the dictionary.
        /// </summary>
        public void Empty()
        {
            lock (Builders)
            {
                Builders.Clear();
            }
        }

        [Obsolete]
        public static DescriptionBuilderBase<TNodeType> Create(MetadataTypes metadataType) 
        {
            if (metadataType == MetadataTypes.Class)
            {
                var instanceTypeFunc = new Func<ClassDescriptionBuilder>(() => new ClassDescriptionBuilder());
            }

            return null;
        }
    }
}