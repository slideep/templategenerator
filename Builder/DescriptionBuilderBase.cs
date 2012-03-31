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
    /// Deriving class has to implement 
    /// </summary>
    /// <typeparam name="TNodeType">Node type</typeparam>
    public abstract class DescriptionBuilderBase<TNodeType>
    {
        /// <summary>
        /// Default extension for description builder template (.genClass).
        /// </summary>
        public const string Extension = ".genClass";

        /// <summary>
        /// Build all available templates from the given default class template directory.
        /// </summary>
        /// <returns>IDictionary{string, IDescription}</returns>
        public virtual IDictionary<string, IDescription> BuildTemplates()
        {
            try
            {
                var classDescriptions = new Dictionary<string, IDescription>();

                string templateDirectory = Settings.Default.ClassTemplateDirectory;
                if (templateDirectory != null && Directory.Exists(templateDirectory))
                {
                    string queryCondition = string.Format(CultureInfo.InvariantCulture, "*{0}", Extension);

                    Directory.EnumerateFiles(templateDirectory, queryCondition, SearchOption.AllDirectories).AsParallel()
                        .ToList().ForEach(fileName =>
                                              {
                                                  string xml = File.ReadAllText(fileName);
                                                  if (string.IsNullOrWhiteSpace(xml))
                                                      return;

                                                  IDescription description = BuildDescription(xml);
                                                  if (description == null)
                                                      return;

                                                  description.FileFullPath = fileName;

                                                  classDescriptions.Add(description.Name, description);
                                              });
                }

                return classDescriptions;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error reading class template descriptions.", ex);
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