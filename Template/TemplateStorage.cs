using System;
using System.Collections.Generic;

namespace TemplateGenerator.Template;
    /// <summary>
    /// A simple container for templates.
    /// </summary>
    internal sealed class TemplateStorage
    {
        private static readonly Lazy<TemplateStorage> InstanceFactory = new(() => new TemplateStorage());

        /// <summary>
        /// Default constructor ; initialize new dictionary for holding templates.
        /// </summary>
        private TemplateStorage()
        {
            Storage = new Dictionary<string, TemplateBase>();
        }

        /// <summary>
        /// Gets the dictionary
        /// </summary>
        private IDictionary<string, TemplateBase> Storage { get; }

        /// <summary>
        /// Gets an "singleton" instance of <see cref="TemplateStorage"/>.
        /// </summary>
        public static TemplateStorage Instance => InstanceFactory.Value;

        /// <summary>
        /// Gets the dictionary of templates.
        /// </summary>
        public IDictionary<string, TemplateBase> Templates => Storage;
    }
 
