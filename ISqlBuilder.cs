using System.Collections.Generic;
using System.Data;

namespace TemplateGenerator
{
    internal interface ISqlBuilder<out TParameters, TUDataParameter>
        where TParameters : IDictionary<string, TUDataParameter>
    {
        ICollection<string> ColumnNames { get; }

        TParameters Parameters { get; }

        IDataParameter FetchParameter(string parameterName, object value);

        string BuildSql(string tableName, SqlBuilderOperationTypes operationTypes);
    }
}