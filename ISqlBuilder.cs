using System.Collections.Generic;
using System.Data;

namespace TemplateGenerator
{   
    internal interface ISqlBuilder<out TParameters, UDataParameter> where TParameters : IDictionary<string, UDataParameter>
    {
        ICollection<string> ColumnNames { get; }
        TParameters Parameters { get; }
        IDataParameter FetchParameter(string parameterName, object value);
        string BuildSql(string tableName, SqlBuilderOperation operation);        
    }
}
