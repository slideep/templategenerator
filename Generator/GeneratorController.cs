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

        public static IList<ITemplate> Templates
        {
            get { return new List<ITemplate>(TemplateStorage.Instance.Templates.Values); }
        }

        protected IDictionary<string, IDescription> TemplateNamesDescriptions
        {
            get
            {
                if (_descriptions == null)
                {
                    var buildDescriptions = new List<IDictionary<string, IDescription>>
                                                {
                                                    new ClassDescriptionBuilder().BuildTemplates(),
                                                    new XmlDescriptionBuilder().BuildTemplates()
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

            ITemplate template;
            if (TemplateStorage.Instance.Templates.TryGetValue(templateName, out template))
            {
                switch (template.DescriptionTypes)
                {
                    case TemplateDescriptionTypes.DataAccess:
                    case TemplateDescriptionTypes.BusinessEntity:
                    case TemplateDescriptionTypes.Controller:
                    case TemplateDescriptionTypes.ControllerInterface:
                    case TemplateDescriptionTypes.ConstantClass:
                        return new ClassGenerator(template).Generate(description);
                }
            }

            return null;
        }
    }
}