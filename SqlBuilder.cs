using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TemplateGenerator
{
    public class SqlBuilder : ISqlBuilder<IDictionary<string, IDataParameter>, IDataParameter>
    {
        public const string SqlRow = "SELECT * FROM {0} WHERE ROWNUM < 1";
        public const string SqlSelect = "\"SELECT {0} FROM {1} WHERE {2}\"";
        public const string SqlInsert = "\"INSERT INTO {0} ({1}) VALUES ({2})\"";
        public const string SqlUpdate = "\"UPDATE {0} SET {1} WHERE {2}\"";

        public string DatabaseName
        {
            get { return ""; }
        }

        public SqlBuilder()
        {
            ColumnNames = new Collection<string>();
        }

        #region Implementation of ISqlBuilder<out OracleParameterCollection>

        public ICollection<string> ColumnNames { get; private set; }

        public IDictionary<string, IDataParameter> Parameters
        {
            get { return null; }
        }

        public IDataParameter FetchParameter(string parameterName, object value)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            //return new OracleParameter(parameterName, value);
            return null;
        }

        public string BuildSql(string tableName, SqlBuilderOperation operation)
        {
            var columnNames = new StringBuilder();
            var parameters = new StringBuilder();
            
            //var command = new OracleCommand(DatabaseName, string.Format(SqlRivi, tableName));
            //var columns = command.FetchTable().Columns;

            //columnNames.Append(
            //    string.Join(",", 
            //        columns.OfType<DataColumn>()
            //        .ToList()
            //        .ConvertAll<string>(dc => dc.ColumnName.ToUpper()).ToArray()));

            //parameters.Append(
            //    string.Join(",", 
            //        columns.OfType<DataColumn>()
            //        .ToList()
            //        .ConvertAll<string>(dc => ":" + dc.ColumnName.ToUpper()).ToArray()));

            //SetColumnNames(columns);

            switch (operation)
            {
                case SqlBuilderOperation.Select:
                    {
                        parameters.Clear();
                        parameters.Append("USERNAME = :username");
                        return string.Format(CultureInfo.InvariantCulture, SqlSelect, columnNames, tableName, parameters);
                    }
                case SqlBuilderOperation.Insert:
                    return string.Format(CultureInfo.InvariantCulture, SqlInsert, tableName, columnNames, parameters);
                case SqlBuilderOperation.Update:
                    {
                        const string formatString =
                            @"(@paramName@ = :ed_@paramName@ OR (@paramName@ IS NULL AND :ed_@paramName@ IS NULL)) AND";

                        columnNames.Clear();
                        columnNames.Append(string.Join(",", ColumnNames.Select(s => formatString.Replace("@paramName@", s))));

                        return string.Format(CultureInfo.InvariantCulture, SqlUpdate, tableName, columnNames, parameters);
                    }
            }

            return string.Empty;
        }

		private ICollection<string> FetchColumnNames(DataColumnCollection columns)
		{
			if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            return Array.AsReadOnly<string>(columns.OfType<DataColumn>().ToList().ConvertAll<string>(dc => dc.ColumnName.ToLowerInvariant()).ToArray());
		}
		
        private void SetColumnNames(DataColumnCollection columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            ColumnNames = 
                Array.AsReadOnly<string>(columns.OfType<DataColumn>()
                .ToList()
                .ConvertAll<string>(dc => dc.ColumnName.ToLowerInvariant()).ToArray());
        }

        #endregion
    }
}
