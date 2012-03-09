using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateGenerator
{
    public class XmlDescription : IDescription
    {
        public string EventType
        {
            get;
            set;
        }

        public string Namespace { get; set; }

        public XmlDescription(string xmlName, string xmlDescription, IEnumerable<PropertyDescription> properties)
        {
            if (xmlName == null)
            {
                throw new ArgumentNullException("xmlName");
            }
            if (xmlDescription == null)
            {
                throw new ArgumentNullException("xmlDescription");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            Name = xmlName;
            Description = xmlDescription;
            Properties = new ReadOnlyCollection<PropertyDescription>(properties.ToList());
        }

        #region Implementation of IDescription

        public string TableName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FileFullPath { get; set; }

        public string SurrogateValue { get; set; }

        public int? Year { get; set; }

        public ReadOnlyCollection<PropertyDescription> Properties { get; private set; }

        #endregion
    }
}