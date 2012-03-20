using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateGenerator
{
    public class ClassDescription : IDescription
    {
        public ClassDescription(string name, string description, IEnumerable<PropertyDescription> properties,
                                ReadOnlyCollection<OperationDescription> operations)
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
            Operations = operations;
            Builder = new SqlBuilder();
        }

        public bool IsDataAccessClass { get; set; }

        public string BuildSqlSelect
        {
            get { return Builder.BuildSql(TableName, SqlBuilderOperationTypes.Select); }
        }

        public string BuildSqlUpdate
        {
            get { return Builder.BuildSql(TableName, SqlBuilderOperationTypes.Update); }
        }

        public string BuildSqlInsert
        {
            get { return Builder.BuildSql(TableName, SqlBuilderOperationTypes.Insert); }
        }

        public ReadOnlyCollection<OperationDescription> Operations { get; private set; }

        public SqlBuilder Builder { get; private set; }

        #region IDescription Members

        public string TableName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FileFullPath { get; set; }

        public ReadOnlyCollection<PropertyDescription> Properties { get; private set; }

        #endregion
    }
}