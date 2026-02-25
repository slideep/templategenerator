using System;
using System.Linq;
using System.Text;
using TemplateGenerator.Description;
using TemplateGenerator.Template;

namespace TemplateGenerator.Generator;
    internal class ClassGenerator : IGenerator
    {
        public ClassGenerator(TemplateAsset template)
        {
            ArgumentNullException.ThrowIfNull(template);

            GeneratedTemplate = template;
            ClassTemplate = GeneratedTemplate.ClassTemplate;
            PropertyTemplate = GeneratedTemplate.PropertyTemplate;
            ParameterTemplate = GeneratedTemplate.ParameterTemplate;
            DescriptionTypes = GeneratedTemplate.DescriptionType;
            SovitusParametriPohja = GeneratedTemplate.SovitusParameterTemplate;
        }

        public string ParameterTemplate { get; }

        protected string SovitusParametriPohja { get; }

        protected TemplateDescriptionTypes DescriptionTypes { get; }

        protected TemplateAsset GeneratedTemplate { get; }

        #region IGenerator Members

        /// <summary>
        /// Gets the class template text.
        /// </summary>
        public string ClassTemplate { get; }

        /// <summary>
        /// Gets the property template text.
        /// </summary>
        public string PropertyTemplate { get; }

        /// <summary>
        /// Gets the generated template text based on <see cref="IDescription"/> implementation.
        /// </summary>
        /// <param name="description"><see cref="IDescription"/> implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="IDescription"/> implementation is null.</exception>
        /// <returns>Generated template text</returns>
        public string Generate(IDescription description)
        {
            ArgumentNullException.ThrowIfNull(description);

            var classTemplateString = ClassTemplate;

            classTemplateString = classTemplateString.Replace(MetadataParameters.ClassName, description.Name);
            classTemplateString = classTemplateString.Replace(MetadataParameters.DescriptionName,
                                                              description.Description);

            if (description is ClassDescription classDescription)
            {
                if (classDescription.IsDataAccessClass)
                {
                    classTemplateString = classTemplateString.Replace(MetadataParameters.TableNameName,
                                                                      classDescription.TableName ?? string.Empty);
                    classTemplateString = classTemplateString.Replace(MetadataParameters.SelectSqlName,
                                                                      BuildSqlSelect(classDescription));
                    classTemplateString = classTemplateString.Replace(MetadataParameters.InsertSqlName,
                                                                      BuildSqlInsert(classDescription));
                    classTemplateString = classTemplateString.Replace(MetadataParameters.UpdateSqlName,
                                                                      BuildSqlUpdate(classDescription));
                    classTemplateString = classTemplateString.Replace(MetadataParameters.ParametersName,
                                                                      CreateParameters(classDescription).ToString());
                }

                classTemplateString = classTemplateString.Replace(MetadataParameters.PropertiesName,
                                                                  CreateProperties(classDescription).ToString());
                classTemplateString = classTemplateString.Replace(
                    MetadataParameters.SovitusParametriName,
                    CreateColumnNameParameters(classDescription).ToString());
            }

            return ClearOptionalClassPlaceholders(classTemplateString);
        }

        #endregion

        private StringBuilder CreateColumnNameParameters(ClassDescription description)
        {
            ArgumentNullException.ThrowIfNull(description);

            var parameters = new StringBuilder();

            foreach (var parameterName in GetColumnNames(description))
            {
                var parameterString = SovitusParametriPohja;
                parameterString = GetParameterString(description, parameterString, parameterName);
                parameters.Append(parameterString);
            }

            return parameters;
        }

        private StringBuilder CreateParameters(ClassDescription description)
        {
            ArgumentNullException.ThrowIfNull(description);

            var parameters = new StringBuilder();

            foreach (var parameterName in GetColumnNames(description))
            {
                var parameterString = ParameterTemplate;

                switch (DescriptionTypes)
                {
                    case TemplateDescriptionTypes.Controller:
                        parameterString = parameterString.Replace(MetadataParameters.ParameterName, ToUpperLeadingAndTail(parameterName));
                        parameterString = parameterString.Replace(MetadataParameters.ClassName, description.Name);
                        break;
                    default:
                        parameterString = parameterString.Replace(MetadataParameters.ParameterName, parameterName);
                        break;
                }

                parameters.Append(parameterString);
            }

            return parameters;
        }

        private StringBuilder CreateProperties(ClassDescription description)
        {
            ArgumentNullException.ThrowIfNull(description);

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
            ArgumentNullException.ThrowIfNull(description);

            parameterString =
                parameterString.Replace("@sovitusParametri1@",
                                        ToUpperLeadingAndTail(parametriNimi));
            parameterString =
                parameterString.Replace("@sovitusParametri2@",
                                        ToUpperLeadingAndTail(parametriNimi));

            parameterString = parameterString.Replace(MetadataParameters.ClassName, description.Name);

            return parameterString;
        }

        private static string ToUpperLeadingAndTail(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            return value.Length == 1
                ? value.ToUpperInvariant()
                : string.Concat(value[0].ToString().ToUpperInvariant(), value.Substring(1));
        }

        private static string[] GetColumnNames(ClassDescription description)
        {
            ArgumentNullException.ThrowIfNull(description);

            return description.Properties
                .Select(property => property.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }

        private static string BuildSqlSelect(ClassDescription description)
        {
            var tableName = description.TableName;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            EnsureSqlIdentifier(tableName, nameof(description.TableName));
            var columns = JoinColumnsOrWildcard(description);
            return $"\"SELECT {columns} FROM {tableName}\"";
        }

        private static string BuildSqlInsert(ClassDescription description)
        {
            var tableName = description.TableName;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            var columnNames = GetColumnNames(description);
            if (columnNames.Length == 0)
            {
                return string.Empty;
            }

            EnsureSqlIdentifier(tableName, nameof(description.TableName));
            EnsureSqlIdentifiers(columnNames, "Properties");
            var columns = string.Join(",", columnNames);
            var parameters = string.Join(",", columnNames.Select(static name => $":{name}"));
            return $"\"INSERT INTO {tableName} ({columns}) VALUES ({parameters})\"";
        }

        private static string BuildSqlUpdate(ClassDescription description)
        {
            var tableName = description.TableName;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            var columnNames = GetColumnNames(description);
            if (columnNames.Length == 0)
            {
                return string.Empty;
            }

            EnsureSqlIdentifier(tableName, nameof(description.TableName));
            EnsureSqlIdentifiers(columnNames, "Properties");
            var assignments = string.Join(",", columnNames.Select(static name => $"{name} = :{name}"));
            return $"\"UPDATE {tableName} SET {assignments}\"";
        }

        private static string JoinColumnsOrWildcard(ClassDescription description)
        {
            var columnNames = GetColumnNames(description);
            EnsureSqlIdentifiers(columnNames, "Properties");
            return columnNames.Length == 0 ? "*" : string.Join(",", columnNames);
        }

        private static void EnsureSqlIdentifiers(string[] identifiers, string parameterName)
        {
            foreach (var identifier in identifiers)
            {
                EnsureSqlIdentifier(identifier, parameterName);
            }
        }

        private static void EnsureSqlIdentifier(string identifier, string parameterName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);

            foreach (var segment in identifier.Split('.'))
            {
                if (!IsValidSqlIdentifierSegment(segment))
                {
                    throw new InvalidOperationException(
                        $"Invalid SQL identifier '{identifier}' in '{parameterName}'. Expected alphanumeric/underscore segments.");
                }
            }
        }

        private static bool IsValidSqlIdentifierSegment(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (!(char.IsLetter(value[0]) || value[0] == '_'))
            {
                return false;
            }

            for (var i = 1; i < value.Length; i++)
            {
                var character = value[i];
                if (!(char.IsLetterOrDigit(character) || character == '_'))
                {
                    return false;
                }
            }

            return true;
        }

        private static string ClearOptionalClassPlaceholders(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            return value
                .Replace(MetadataParameters.TableNameName, string.Empty)
                .Replace(MetadataParameters.SelectSqlName, string.Empty)
                .Replace(MetadataParameters.InsertSqlName, string.Empty)
                .Replace(MetadataParameters.UpdateSqlName, string.Empty)
                .Replace(MetadataParameters.ParametersName, string.Empty)
                .Replace(MetadataParameters.SovitusParametriName, string.Empty);
        }

        #endregion
    }
 
