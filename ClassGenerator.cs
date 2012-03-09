using System;
using System.Text;

namespace TemplateGenerator
{
    internal class ClassGenerator : IGenerator
    {
        public string ClassTemplate { get; private set; }
        public string PropertyTemplate { get; private set; }
        public string SurrogateTemplate { get; private set; }
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
            SurrogateTemplate = GeneratedTemplate.SurrogateTemplate;   
            ParameterTemplate = GeneratedTemplate.ParameterTemplate;
            DescriptionType = GeneratedTemplate.DescriptionType;
            SovitusParametriPohja = GeneratedTemplate.SovitusParametriPohja;
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

            string luokkaPohja = ClassTemplate;

            luokkaPohja = luokkaPohja.Replace("@class@", description.Name);
            luokkaPohja = luokkaPohja.Replace("@description@", description.Description);
            
            var luokkaKuvaus = description as ClassDescription;
            if (luokkaKuvaus != null)
            {
                if (luokkaKuvaus.IsDataAccessClass)
                {
                    luokkaPohja = luokkaPohja.Replace("@tablename@", luokkaKuvaus.TableName);
                    luokkaPohja = luokkaPohja.Replace("@selectsql@", luokkaKuvaus.BuildSQLSelect);
                    luokkaPohja = luokkaPohja.Replace("@insertsql@", luokkaKuvaus.BuildSQLInsert);
                    luokkaPohja = luokkaPohja.Replace("@updatesql@", luokkaKuvaus.BuildSQLUpdate);

                    if (!string.IsNullOrEmpty(description.SurrogateValue))
                    {
                        luokkaPohja = luokkaPohja.Replace("@surrogate@", CreateSurrogate(luokkaKuvaus).ToString());
                    }

                    luokkaPohja = luokkaPohja.Replace("@parameters@", CreateParameters(luokkaKuvaus).ToString());
                }

                luokkaPohja = luokkaPohja.Replace("@properties@", CreateProperties(luokkaKuvaus).ToString());
                luokkaPohja = luokkaPohja.Replace("@sovitusParametri@", LuoSovitusParametrit(luokkaKuvaus).ToString());
            }

            return luokkaPohja;
        }

        private StringBuilder LuoSovitusParametrit(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var parameters = new StringBuilder();

            foreach (var parametriNimi in description.Builder.ColumnNames)
            {
                string parametriString = SovitusParametriPohja;
                parametriString = GetParameterString(description, parametriString, parametriNimi);
                parameters.Append(parametriString);
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

            foreach (var parametriNimi in description.Builder.ColumnNames)
            {
                string parametriString = ParameterTemplate;

                switch (DescriptionType)
                {
                    case TemplateDescriptionType.Controller:
                        {
                            parametriString =
                                parametriString.Replace("@parametriNimi@",
                                                        string.Concat(parametriNimi.Substring(0, 1).ToUpper(), parametriNimi.Substring(1, parametriNimi.Length - 1).ToLower()));
                            parametriString = parametriString.Replace("@class@", description.Name);
                        }
                        break;
                    default:
                        parametriString = parametriString.Replace("@parametriNimi@", parametriNimi);
                        break;
                }

                parameters.Append(parametriString);
            }

            return parameters;
        }

        private StringBuilder CreateSurrogate(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            var surrogate = new StringBuilder();

            string surrogaattiString = SurrogateTemplate;            
            surrogaattiString = surrogaattiString.Replace("@tablename@", description.TableName);

            switch (this.DescriptionType)
            {
                case TemplateDescriptionType.Controller:
                    surrogaattiString = surrogaattiString.Replace("@surrogatevalue@",
                                                      string.Concat(description.SurrogateValue.Substring(0, 1).ToUpper(),
                                                                    description.SurrogateValue.Substring(1, description.SurrogateValue.Length - 1).ToLower()));
                    break;
                default:
                    surrogaattiString = surrogaattiString.Replace("@surrogatevalue@", description.SurrogateValue);
                    break;
            }            

            surrogaattiString = surrogaattiString.Replace("@class@", description.Name);
            surrogate.Append(surrogaattiString);
                                    
            return surrogate;
        }

        public StringBuilder CreateProperties(ClassDescription description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            
            var properties = new StringBuilder();

            foreach (var ominaisuusKuvaus in description.Properties)
            {
                string ominaisuusString = PropertyTemplate;

                ominaisuusString = ominaisuusString.Replace("@name@", ominaisuusKuvaus.Name);
                ominaisuusString = ominaisuusString.Replace("@description@", ominaisuusKuvaus.Description);
                ominaisuusString = ominaisuusString.Replace("@datatype@", ominaisuusKuvaus.GetNetDatatype());
                ominaisuusString = ominaisuusString.Replace("@datatypeLTK@", ominaisuusKuvaus.GetNetDatatypeLTK());

                properties.Append(ominaisuusString);
            }

            return properties;
        }

        #region Staattiset apujäsenet

        private static string GetParameterString(ClassDescription description, string parametriString, string parametriNimi)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            parametriString =
                parametriString.Replace("@sovitusParametri1@",
                                        string.Concat(
                                            parametriNimi.Substring(0, 1).ToUpper(), 
                                            parametriNimi.Substring(1, parametriNimi.Length - 1).ToLower()));
            parametriString =
                parametriString.Replace("@sovitusParametri2@",
                                        string.Concat(
                                            parametriNimi.Substring(0, 1).ToUpper(), 
                                            parametriNimi.Substring(1, parametriNimi.Length - 1).ToLower()));
											
            parametriString = parametriString.Replace("@class@", description.Name);

            return parametriString;
        }

        #endregion
    }
}