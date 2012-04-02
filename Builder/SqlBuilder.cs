using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TemplateGenerator.Builder
{
    public class SqlBuilder : ISqlBuilder<IDictionary<string, IDataParameter>, IDataParameter>
    {
        public const string SqlRow = "SELECT * FROM {0} WHERE ROWNUM < 1";
        public const string SqlSelect = "\"SELECT {0} FROM {1} WHERE {2}\"";
        public const string SqlInsert = "\"INSERT INTO {0} ({1}) VALUES ({2})\"";
        public const string SqlUpdate = "\"UPDATE {0} SET {1} WHERE {2}\"";

        public SqlBuilder()
        {
            ColumnNames = new Collection<string>();
        }

        #region Implementation of ISqlBuilder<out OracleParameterCollection>

        /// <summary>
        /// Gets the column names from the database table.
        /// </summary>
        public ICollection<string> ColumnNames { get; private set; }

        /// <summary>
        /// Gets the data parameters.
        /// </summary>
        public IDictionary<string, IDataParameter> Parameters
        {
            get { return null; }
        }

        /// <summary>
        /// Fetches the named and value given <see cref="IDataParameter"/> implementation.
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="value">Default value</param>
        /// <exception cref="ArgumentNullException">Thrown when parameter name is null.</exception>
        /// <returns><see cref="IDataParameter"/> implementation</returns>
        public IDataParameter FetchParameter(string parameterName, object value)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            return null;
        }

        /// <summary>
        /// Builds the SQL-clause from given table name and <see cref="SqlBuilderOperationTypes"/> type.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="operationTypes"><see cref="SqlBuilderOperationTypes"/></param>
        /// <returns>Built SQL-clause</returns>
        public string BuildSql(string tableName, SqlBuilderOperationTypes operationTypes)
        {
            var columnNames = new StringBuilder();
            var parameters = new StringBuilder();

            switch (operationTypes)
            {
                case SqlBuilderOperationTypes.Select:
                    {
                        parameters.Clear();
                        parameters.Append("USERNAME = :username");
                        return string.Format(CultureInfo.InvariantCulture, SqlSelect, columnNames, tableName, parameters);
                    }
                case SqlBuilderOperationTypes.Insert:
                    return string.Format(CultureInfo.InvariantCulture, SqlInsert, tableName, columnNames, parameters);
                case SqlBuilderOperationTypes.Update:
                    {
                        const string formatString =
                            @"(@paramName@ = :ed_@paramName@ OR (@paramName@ IS NULL AND :ed_@paramName@ IS NULL)) AND";

                        columnNames.Clear();
                        columnNames.Append(string.Join(",",
                                                       ColumnNames.Select(s => formatString.Replace("@paramName@", s))));

                        return string.Format(CultureInfo.InvariantCulture, SqlUpdate, tableName, columnNames, parameters);
                    }
            }

            return string.Empty;
        }

        protected static ICollection<string> FetchColumnNames(DataColumnCollection columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            return
                Array.AsReadOnly(
                    columns.OfType<DataColumn>().ToList().ConvertAll(dc => dc.ColumnName.ToUpperInvariant()).ToArray());
        }

        protected void SetColumnNames(IEnumerable columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            ColumnNames =
                Array.AsReadOnly(columns.OfType<DataColumn>()
                                     .ToList()
                                     .ConvertAll(dc => dc.ColumnName.ToUpperInvariant()).ToArray());
        }

        #endregion

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName
        {
            get { return string.Empty; }
        }
    }
}