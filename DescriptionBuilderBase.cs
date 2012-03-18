using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TemplateGenerator.Properties;

namespace TemplateGenerator
{
	/// <summary>
	/// A base class for creating template builders of given <typeparam name="TNodeType"/> type.
	/// Deriving class has to implement 
	/// </summary>
    public abstract class DescriptionBuilderBase<TNodeType>
    {
        private const string Extension = ".genClass";

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
                    
                    Directory.EnumerateFiles(templateDirectory, queryCondition, SearchOption.AllDirectories).AsParallel().ToList().ForEach(fileName =>
                    {
                        string xml = File.ReadAllText(fileName);
                        if (!string.IsNullOrWhiteSpace(xml))
                        {
                            var description = BuildDescription(xml);
                            if (description != null)
                            {
                                description.FileFullPath = fileName;
                                classDescriptions.Add(description.Name, description);
                            }
                        }
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
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        protected abstract IDescription BuildDescription(string xml);

        protected abstract IEnumerable<PropertyDescription> FetchProperties(TNodeType templateNode, string propertyDescription);
    }
}