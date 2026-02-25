using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator;
using TemplateGenerator.Builder;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator;
    /// <summary>
    /// Represents a generators and builders for creating templates and descriptions.
    /// </summary>
    public class GeneratorController
    {
        private IDictionary<string, IDescription>? _descriptions;
        private readonly string _templateDirectory;
        private readonly object _descriptionsLock = new();

        public GeneratorController()
            : this(new TemplateGeneratorOptions())
        {
        }

        public GeneratorController(TemplateGeneratorOptions options)
        {
            options ??= new TemplateGeneratorOptions();

            if (string.IsNullOrWhiteSpace(options.TemplateDirectory))
            {
                throw new ArgumentException("Template directory must be a non-empty path.", nameof(options));
            }

            _templateDirectory = options.TemplateDirectory;
        }

        /// <summary>
        /// Gets an list of available <see cref="IDescription"/> implementations.
        /// </summary>
        public IList<IDescription> Descriptions
            => [..TemplateNamesDescriptions.Values];

        /// <summary>
        /// Gets an list of available template implementations (derived from <see cref="TemplateBase"/>).
        /// </summary>
        public static IList<TemplateBase> Templates => new List<TemplateBase>(TemplateStorage.Instance.Templates.Values);

        /// <summary>
        /// Gets and builds an dictionary of templates. 
        /// </summary>
        protected IDictionary<string, IDescription> TemplateNamesDescriptions
        {
            get
            {
                if (_descriptions == null)
                {
                    lock (_descriptionsLock)
                    {
                        if (_descriptions == null)
                        {
                            // TODO: ioc / autodiscovery / reflect every builder available on the assembly
                            var buildDescriptions = new List<IDictionary<string, IDescription>>
                            {
                                new ClassDescriptionBuilder(_templateDirectory).BuiltTemplates,
                                new XmlDescriptionBuilder(_templateDirectory).BuiltTemplates
                            };

                            _descriptions = buildDescriptions
                                .SelectMany(descriptions => descriptions)
                                .ToDictionary(descriptionPair => descriptionPair.Key, pair => pair.Value);
                        }
                    }
                }

                return _descriptions!;
            }
        }

        /// <summary>
        /// Generates an description based on template's name (interface or class type name is used as a key).
        /// </summary>
        /// <param name="description"><see cref="IDescription"/>'s concrete implementation.</param>
        /// <param name="templateName">Template's name</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when derived <see cref="IDescription"/> interface implementation is null or template name wasn't given.
        /// </exception>
        /// <returns>Generated template's description (a class, interface, XML-file etc.) or null.</returns>
        public static string? GenerateDescription(IDescription description, string templateName)
        {
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(templateName);

            return TemplateStorage.Instance.Templates.TryGetValue(templateName, out var template)
                ? new ClassGenerator(template).Generate(description)
                : null;
        }
    }
 
