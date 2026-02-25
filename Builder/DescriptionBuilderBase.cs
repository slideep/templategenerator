using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TemplateGenerator;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Builder;

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

    protected DescriptionBuilderBase()
        : this(TemplateGeneratorOptions.DefaultTemplateDirectory)
    {
    }

    protected DescriptionBuilderBase(string templateDirectory)
    {
        if (string.IsNullOrWhiteSpace(templateDirectory))
        {
            throw new ArgumentException("Template directory must be a non-empty path.", nameof(templateDirectory));
        }

        TemplateDirectory = templateDirectory;
    }

    protected string TemplateDirectory { get; }

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
                Dictionary<string, IDescription> classDescriptions = [];

                if (!Directory.Exists(TemplateDirectory))
                {
                    return classDescriptions;
                }

                var queryCondition = string.Format(CultureInfo.InvariantCulture, "*{0}", Extension);

                // Keep this single-threaded: builders and the shared dictionary are not thread-safe.
                foreach (var fileName in Directory.EnumerateFiles(
                             TemplateDirectory,
                             queryCondition,
                             SearchOption.AllDirectories))
                {
                    var xml = File.ReadAllText(fileName);
                    if (string.IsNullOrWhiteSpace(xml))
                    {
                        continue;
                    }

                    try
                    {
                        var description = BuildDescription(xml, fileName);
                        if (description == null)
                        {
                            continue;
                        }

                        classDescriptions.Add(description.Name, description);
                    }
                    catch (TemplateParseException ex)
                    {
                        ex.AttachFileFullPath(fileName);
                        throw;
                    }
                }

                return classDescriptions;
            }
            catch (TemplateParseException)
            {
                throw;
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
    protected abstract IDescription? BuildDescription(string xml, string? fileFullPath);

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
 
