using System.Collections.Generic;

namespace TemplateGenerator.Template
{
    /// <summary>
    /// A simple container for templates.
    /// </summary>
    internal class TemplateStorage
    {
        private static TemplateStorage _instance;

        /// <summary>
        /// Default constructor ; initialize new dictionary for holding templates.
        /// </summary>
        public TemplateStorage()
        {
            Storage = new Dictionary<string, TemplateBase>();
        }

        /// <summary>
        /// Gets the dictionary
        /// </summary>
        private IDictionary<string, TemplateBase> Storage { get; set; }

        /// <summary>
        /// Gets an "singleton" instance of <see cref="TemplateStorage"/>.
        /// </summary>
        public static TemplateStorage Instance
        {
            get { return _instance ?? (_instance = new TemplateStorage()); }
        }

        /// <summary>
        /// Gets the dictionary of templates.
        /// </summary>
        public IDictionary<string, TemplateBase> Templates
        {
            get { return Storage; }
        }
    }
}