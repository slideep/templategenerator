using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateGenerator
{
    public class GeneratorController
    {
        private IDictionary<string, IDescription> _descriptions;

        public IList<IDescription> Descriptions
        { 
            get { return new List<IDescription>(TemplateNamesDescriptions.Values); }
        }

        public IList<ITemplate> Templates
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

        public string GenerateDescription(IDescription description, string templateName)
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
                switch (template.DescriptionType)
                {
                    case TemplateDescriptionType.DataAccess:
                    case TemplateDescriptionType.BusinessEntity:
                    case TemplateDescriptionType.Controller:
                    case TemplateDescriptionType.ControllerInterface:
                    case TemplateDescriptionType.ConstantClass:
                        return new ClassGenerator(template).Generate(description);
                    default:
                        break;
                }
            }

            return null;
        }
    }
}