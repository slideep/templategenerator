using System;
using System.Globalization;
using System.Text;

namespace TemplateGenerator
{
    internal class ClassGenerator : IGenerator
    {
        public string ClassTemplate { get; private set; }
        public string PropertyTemplate { get; private set; }
        public string ParameterTemplate { get; private set; }

        public ClassGenerator(ITemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            GeneratedTemplate = template;
            ClassTemplate = GeneratedTemplate.ClassTemplate;
            PropertyTemplate = GeneratedTemplate.PropertyTemplate;
            ParameterTemplate = GeneratedTemplate.ParameterTemplate;
            DescriptionType = GeneratedTemplate.DescriptionType;
            SovitusParametriPohja = GeneratedTemplate.SovitusParameterTemplate;
        }

        protected string SovitusParametriPohja { get; set; }

        protected TemplateDescriptionType DescriptionType { get; set; }

        protected ITemplate GeneratedTemplate { get; set; }

        public string Generate(IDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            string classTemplateString = ClassTemplate;

            classTemplateString = classTemplateString.Replace("@class@", description.Name);
            classTemplateString = classTemplateString.Replace("@description@", description.Description);
            
            var classDescription = description as ClassDescription;
            if (classDescription != null)
            {
                if (classDescription.IsDataAccessClass)
                {
                    classTemplateString = classTemplateString.Replace("@tablename@", classDescription.TableName);
                    classTemplateString = classTemplateString.Replace("@selectsql@", classDescription.BuildSqlSelect);
                    classTemplateString = classTemplateString.Replace("@insertsql@", classDescription.BuildSqlInsert);
                    classTemplateString = classTemplateString.Replace("@updatesql@", classDescription.BuildSqlUpdate);
                    classTemplateString = classTemplateString.Replace("@parameters@", CreateParameters(classDescription).ToString());
                }

                classTemplateString = classTemplateString.Replace("@properties@", CreateProperties(classDescription).ToString());
                classTemplateString = classTemplateString.Replace("@sovitusParametri@", LuoSovitusParametrit(classDescription).ToString());
            }

            return classTemplateString;
        }

        private StringBuilder LuoSovitusParametrit(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var parameters = new StringBuilder();

            foreach (var parameterName in description.Builder.ColumnNames)
            {
                string parameterString = SovitusParametriPohja;
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
                string parameterString = ParameterTemplate;

                switch (DescriptionType)
                {
                    case TemplateDescriptionType.Controller:
                        {
                            parameterString =
                                parameterString.Replace("@parameterName@",
                            	                        string.Concat(parameterName.Substring(0, 1).ToUpperInvariant(), parameterName.Substring(1, parameterName.Length - 1).ToLowerInvariant()));
                            parameterString = parameterString.Replace("@class@", description.Name);
                        }
                        break;
                    default:
                        parameterString = parameterString.Replace("@parameterName@", parameterName);
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
                string propertyString = PropertyTemplate;

                propertyString = propertyString.Replace("@name@", propertyDescription.Name);
                propertyString = propertyString.Replace("@description@", propertyDescription.Description);
                propertyString = propertyString.Replace("@datatype@", propertyDescription.DotNetDataType);
           
                properties.Append(propertyString);
            }

            return properties;
        }

        #region Static members

        private static string GetParameterString(ClassDescription description, string parameterString, string parametriNimi)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            parameterString =
                parameterString.Replace("@sovitusParametri1@",
                                        string.Concat(
            	                        	parametriNimi.Substring(0, 1).ToUpperInvariant(),
            	                        	parametriNimi.Substring(1, parametriNimi.Length - 1).ToLowerInvariant()));
            parameterString =
                parameterString.Replace("@sovitusParametri2@",
                                        string.Concat(
                                            parametriNimi.Substring(0, 1).ToUpperInvariant(), 
                                            parametriNimi.Substring(1, parametriNimi.Length - 1).ToLowerInvariant()));
											
            parameterString = parameterString.Replace("@class@", description.Name);

            return parameterString;
        }

        #endregion
    }
}