using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateGenerator
{
    public class ClassDescription : IDescription
    {
        public ClassDescription(string name, string description, IEnumerable<PropertyDescription> properties)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            Name = name;
            Description = description;
            Properties = new ReadOnlyCollection<PropertyDescription>(properties.ToList());
            Builder = new SqlBuilder();
        }

        public bool IsDataAccessClass { get; set; }

        public string BuildSQLSelect
        {
            get { return Builder.BuildSql(TableName, SqlBuilderOperation.Select); }
        }

        public string BuildSQLUpdate
        {
            get { return Builder.BuildSql(TableName, SqlBuilderOperation.Update); }
        }

        public string BuildSQLInsert
        {
            get { return Builder.BuildSql(TableName, SqlBuilderOperation.Insert); }
        }

        public ReadOnlyCollection<OperationDescription> Operations { get; set; }
        public SqlBuilder Builder { get; private set; }

        #region IDescription Members

        public string TableName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ReadOnlyCollection<PropertyDescription> Properties { get; set; }

        public string FileFullPath { get; set; }

        public string SurrogateValue { get; set; }

        #endregion
    }
}