using System;
using System.Text;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator
{
    internal class ClassGenerator : IGenerator
    {
        public ClassGenerator(TemplateBase template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            GeneratedTemplate = template;
            ClassTemplate = GeneratedTemplate.ClassTemplate;
            PropertyTemplate = GeneratedTemplate.PropertyTemplate;
            ParameterTemplate = GeneratedTemplate.ParameterTemplate;
            DescriptionTypes = GeneratedTemplate.DescriptionType;
            SovitusParametriPohja = GeneratedTemplate.SovitusParameterTemplate;
        }

        public string ParameterTemplate { get; private set; }

        protected string SovitusParametriPohja { get; set; }

        protected TemplateDescriptionTypes DescriptionTypes { get; set; }

        protected TemplateBase GeneratedTemplate { get; set; }

        #region IGenerator Members

        /// <summary>
        /// Gets the class template text.
        /// </summary>
        public string ClassTemplate { get; private set; }

        /// <summary>
        /// Gets the property template text.
        /// </summary>
        public string PropertyTemplate { get; private set; }

        /// <summary>
        /// Gets the generated template text based on <see cref="IDescription"/> implementation.
        /// </summary>
        /// <param name="description"><see cref="IDescription"/> implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="IDescription"/> implementation is null.</exception>
        /// <returns>Generated template text</returns>
        public string Generate(IDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var classTemplateString = ClassTemplate;

            classTemplateString = classTemplateString.Replace(MetadataParameters.ClassName, description.Name);
            classTemplateString = classTemplateString.Replace(MetadataParameters.DescriptionName,
                                                              description.Description);

            var classDescription = description as ClassDescription;
            if (classDescription != null)
            {
                if (classDescription.IsDataAccessClass)
                {
                    classTemplateString = classTemplateString.Replace(MetadataParameters.TableNameName,
                                                                      classDescription.TableName);
                    classTemplateString = classTemplateString.Replace(MetadataParameters.SelectSqlName,
                                                                      classDescription.BuildSqlSelect);
                    classTemplateString = classTemplateString.Replace(MetadataParameters.InsertSqlName,
                                                                      classDescription.BuildSqlInsert);
                    classTemplateString = classTemplateString.Replace(MetadataParameters.UpdateSqlName,
                                                                      classDescription.BuildSqlUpdate);
                    classTemplateString = classTemplateString.Replace(MetadataParameters.ParametersName,
                                                                      CreateParameters(classDescription).ToString());
                }

                classTemplateString = classTemplateString.Replace(MetadataParameters.PropertiesName,
                                                                  CreateProperties(classDescription).ToString());
                classTemplateString = classTemplateString.Replace(MetadataParameters.SovitusParametriName,
                                                                  CreateColumnNameParameters(classDescription).ToString());
            }

            return classTemplateString;
        }

        #endregion

        private StringBuilder CreateColumnNameParameters(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var parameters = new StringBuilder();

            foreach (var parameterName in description.Builder.ColumnNames)
            {
                var parameterString = SovitusParametriPohja;
                parameterString = GetParameterString(description, parameterString, parameterName);
                parameters.Append(parameterString);
            }

            return parameters;
        }

        private StringBuilder CreateParameters(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var parameters = new StringBuilder();

            foreach (var parameterName in description.Builder.ColumnNames)
            {
                var parameterString = ParameterTemplate;

                switch (DescriptionTypes)
                {
                    case TemplateDescriptionTypes.Controller:
                        {
                            parameterString =
                                parameterString.Replace(MetadataParameters.ParameterName,
                                                        string.Concat(parameterName.Substring(0, 1).ToUpperInvariant(),
                                                                      parameterName.Substring(1,
                                                                                              parameterName.Length - 1).
                                                                          ToUpperInvariant()));
                            parameterString = parameterString.Replace(MetadataParameters.ClassName, description.Name);
                        }
                        break;
                    default:
                        parameterString = parameterString.Replace(MetadataParameters.ParameterName, parameterName);
                        break;
                }

                parameters.Append(parameterString);
            }

            return parameters;
        }

        public StringBuilder CreateProperties(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var properties = new StringBuilder();

            foreach (var propertyDescription in description.Properties)
            {
                var propertyString = PropertyTemplate;

                propertyString = propertyString.Replace(MetadataParameters.NameName, propertyDescription.Name);
                propertyString = propertyString.Replace(MetadataParameters.DescriptionName,
                                                        propertyDescription.Description);
                propertyString = propertyString.Replace(MetadataParameters.DataTypeName,
                                                        propertyDescription.DotNetDataType);

                properties.Append(propertyString);
            }

            return properties;
        }

        #region Static members

        private static string GetParameterString(IDescription description, string parameterString,
                                                 string parametriNimi)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            parameterString =
                parameterString.Replace("@sovitusParametri1@",
                                        string.Concat(
                                            parametriNimi.Substring(0, 1).ToUpperInvariant(),
                                            parametriNimi.Substring(1, parametriNimi.Length - 1).ToUpperInvariant()));
            parameterString =
                parameterString.Replace("@sovitusParametri2@",
                                        string.Concat(
                                            parametriNimi.Substring(0, 1).ToUpperInvariant(),
                                            parametriNimi.Substring(1, parametriNimi.Length - 1).ToUpperInvariant()));

            parameterString = parameterString.Replace(MetadataParameters.ClassName, description.Name);

            return parameterString;
        }

        #endregion
    }
}