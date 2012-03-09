using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TemplateGenerator.Properties;

namespace TemplateGenerator
{
    public abstract class DescriptionBuilderBase<TNodeType>
    {
        private const string Extension = ".genClass";

        public virtual IDictionary<string, IDescription> BuildTemplates()
        {
            try
            {
                var classDescriptions = new Dictionary<string, IDescription>();

                string templateDirectory = Settings.Default.ClassTemplateDirectory;				
                if (templateDirectory != null && Directory.Exists(templateDirectory))
                {
                    string queryCondition = string.Format("*{0}", Extension);
                    Directory.GetFiles(templateDirectory, queryCondition).ToList().ForEach(fileName =>
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

        protected abstract IDescription BuildDescription(string xml);

        protected abstract IEnumerable<PropertyDescription> FetchProperties(TNodeType templateNode, string propertyDescription);
    }
}