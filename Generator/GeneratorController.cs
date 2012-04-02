using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator.Builder;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator
{
    public class GeneratorController
    {
        private IDictionary<string, IDescription> _descriptions;

        public IList<IDescription> Descriptions
        {
            get { lock (TemplateNamesDescriptions) { return new List<IDescription>(TemplateNamesDescriptions.Values);} }
        }

        public static IList<TemplateBase> Templates
        {
            get { return new List<TemplateBase>(TemplateStorage.Instance.Templates.Values); }
        }

        protected IDictionary<string, IDescription> TemplateNamesDescriptions
        {
            get
            {
                if (_descriptions == null)
                {
                    var buildDescriptions = new List<IDictionary<string, IDescription>>
                                                {
                                                    new ClassDescriptionBuilder().BuiltTemplates,
                                                    new XmlDescriptionBuilder().BuiltTemplates
                                                };

                    _descriptions =
                        buildDescriptions
                            .SelectMany(descriptions => descriptions)
                            .ToDictionary(descriptionPair => descriptionPair.Key, pair => pair.Value);
                }

                return _descriptions;
            }
        }

        public static string GenerateDescription(IDescription description, string templateName)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            if (templateName == null)
            {
                throw new ArgumentNullException("templateName");
            }

            TemplateBase template;
            if (TemplateStorage.Instance.Templates.TryGetValue(templateName, out template))
            {
                switch (template.DescriptionType)
                {
                    case TemplateDescriptionTypes.DataAccess:
                    case TemplateDescriptionTypes.Entity:
                    case TemplateDescriptionTypes.Controller:
                    case TemplateDescriptionTypes.ControllerInterface:
                    case TemplateDescriptionTypes.Constant:
                        return new ClassGenerator(template).Generate(description);
                }
            }

            return null;
        }
    }
}