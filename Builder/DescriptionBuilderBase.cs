using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TemplateGenerator.Description;
using TemplateGenerator.Properties;

namespace TemplateGenerator.Builder
{
    /// <summary>
    /// A base class for creating template builders of given <typeparam name="TNodeType"/> type.
    /// Deriving class has to implement abstract members of this class.
    /// </summary>
    /// <typeparam name="TNodeType">Node type</typeparam>
    public abstract class DescriptionBuilderBase<TNodeType>
    {
        /// <summary>
        /// Default extension for description builder template (.xml).
        /// </summary>
        public const string Extension = ".xml";

        /// <summary>
        /// Build all available templates from the given default class template directory.
        /// </summary>
        /// <value> IDictionary{string, IDescription} </value>
        public virtual IDictionary<string, IDescription> BuiltTemplates
        {
            get
            {
                try
                {
                    var classDescriptions = new Dictionary<string, IDescription>();

                    var templateDirectory = Settings.Default.TemplateDirectory;
                    if (templateDirectory != null && Directory.Exists(templateDirectory))
                    {
                        var queryCondition = string.Format(CultureInfo.InvariantCulture, "*{0}", Extension);

                        Directory.EnumerateFiles(
                            templateDirectory, queryCondition, SearchOption.AllDirectories)
                            .AsParallel()
                            .ToList().ForEach(fileName =>
                            {
                                var xml = File.ReadAllText(fileName);
                                if (string.IsNullOrWhiteSpace(xml))
                                {
                                    return;
                                }

                                var description = BuildDescription(xml);
                                if (description == null)
                                {
                                    return;
                                }

                                description.FileFullPath = fileName;

                                classDescriptions.Add(description.Name, description);
                            });
                    }

                    return classDescriptions;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error reading template descriptions.", ex);
                }
            }
        }

        /// <summary>
        /// Builds <see cref="IDescription"/> based on input XML-string.
        /// </summary>
        /// <param name="xml">XML-string</param>
        /// <returns>IDescription</returns>
        protected abstract IDescription BuildDescription(string xml);

        /// <summary>
        /// Fetch enumerable over <see cref="PropertyDescription"/> types.
        /// </summary>
        /// <param name="templateNode">Node type</param>
        /// <param name="propertyDescription">Property description</param>
        /// <typeparam name="TNodeType">Node type</typeparam>
        /// <returns>IEnumerable{PropertyDescription}</returns>
        protected abstract IEnumerable<PropertyDescription> FetchProperties(TNodeType templateNode,
                                                                            string propertyDescription);
    }
}