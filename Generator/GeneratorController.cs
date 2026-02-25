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
        private readonly ITemplateRegistry _templateRegistry;
        private readonly TemplateGeneratorService _templateGeneratorService;
        private readonly object _descriptionsLock = new();

        public GeneratorController()
            : this(new TemplateGeneratorOptions(), TemplateRegistry.Default)
        {
        }

        public GeneratorController(TemplateGeneratorOptions options)
            : this(options, TemplateRegistry.Default)
        {
        }

        public GeneratorController(TemplateGeneratorOptions options, ITemplateRegistry templateRegistry)
        {
            options ??= new TemplateGeneratorOptions();
            _templateRegistry = templateRegistry ?? throw new ArgumentNullException(nameof(templateRegistry));
            _templateGeneratorService = new TemplateGeneratorService(_templateRegistry);

            if (string.IsNullOrWhiteSpace(options.TemplateDirectory))
            {
                throw new ArgumentException("Template directory must be a non-empty path.", nameof(options.TemplateDirectory));
            }

            _templateDirectory = options.TemplateDirectory;
        }

        /// <summary>
        /// Gets an list of available <see cref="IDescription"/> implementations.
        /// </summary>
        public IList<IDescription> Descriptions
            => [..TemplateNamesDescriptions.Values];

        /// <summary>
        /// Gets an list of available template assets from the default registry.
        /// </summary>
        /// <remarks>
        /// This uses the shared default registry. Prefer instance usage with an injected
        /// <see cref="ITemplateRegistry"/> in tests and concurrent scenarios.
        /// </remarks>
        public static IList<TemplateAsset> Templates => [.. TemplateRegistry.Default.Assets];

        /// <summary>
        /// Gets template descriptors from the shared default registry used by legacy static APIs.
        /// Prefer instance usage with an injected <see cref="ITemplateRegistry"/> for isolation.
        /// </summary>
        public static IList<TemplateAssetDescriptor> TemplateDescriptors => [.. TemplateRegistry.Default.Descriptors];

        public ITemplateRegistry RegisteredTemplateRegistry => _templateRegistry;

        /// <summary>
        /// Gets and builds an dictionary of templates. 
        /// </summary>
        protected IDictionary<string, IDescription> TemplateNamesDescriptions
        {
            get
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

                    return _descriptions;
                }
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
            return new TemplateGeneratorService(TemplateRegistry.Default).GenerateDescription(description, templateName);
        }

        public string? Generate(IDescription description, string templateName)
            => _templateGeneratorService.GenerateDescription(description, templateName);

        /// <summary>
        /// Registers a template into the shared default registry used by legacy static APIs.
        /// Prefer injecting a registry and calling instance methods in tests and concurrent scenarios.
        /// </summary>
        public static void RegisterTemplate(TemplateAsset template)
            => TemplateRegistry.Default.Register(template);

        /// <summary>
        /// Loads templates into the shared default registry used by legacy static APIs.
        /// Prefer injecting a registry and calling instance methods in tests and concurrent scenarios.
        /// </summary>
        public static void LoadTemplates(ITemplateAssetProvider provider)
            => TemplateRegistry.Default.LoadFrom(provider);

        /// <summary>
        /// Clears the shared default registry used by legacy static APIs.
        /// </summary>
        public static void ClearTemplates()
            => TemplateRegistry.Default.Clear();
    }
 
