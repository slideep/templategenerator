using System.Collections.Generic;
using System.Data;

namespace TemplateGenerator.Builder;

/// <summary>
/// An interface for derived SQL-clause builders.
/// </summary>
/// <typeparam name="TParameters">Type constraint for dictionary of <see cref="TUDataParameter"/> types.</typeparam>
/// <typeparam name="TUDataParameter">The types to build or use</typeparam>
internal interface ISqlBuilder<out TParameters, TUDataParameter>
    where TParameters : IDictionary<string, TUDataParameter>
{
        /// <summary>
        /// Gets the column names from the database table.
        /// </summary>
        ICollection<string> ColumnNames { get; }

        /// <summary>
        /// Gets the data parameters.
        /// </summary>
    TParameters? Parameters { get; }

        /// <summary>
        /// Fetches the named and value given <see cref="IDataParameter"/> implementation.
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="value">Default value</param>
        /// <returns><see cref="IDataParameter"/> implementation</returns>
    IDataParameter? FetchParameter(string parameterName, object? value);

        /// <summary>
        /// Builds the SQL-clause from given table name and <see cref="SqlBuilderOperationTypes"/> type.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="operationTypes"><see cref="SqlBuilderOperationTypes"/></param>
        /// <returns>Built SQL-clause</returns>
    string BuildSql(string? tableName, SqlBuilderOperationTypes operationTypes);
}
