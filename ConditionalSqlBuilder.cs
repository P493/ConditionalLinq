using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ConditionalLinq
{
    public class ConditionalSqlBuilder
    {
        public StringBuilder Builder { get; private set; }

        public List<SqlParameter> ParameterList { get; private set; }

        public ConditionalSqlBuilder(string sql)
        {
            Builder = new StringBuilder(sql);
            ParameterList = new List<SqlParameter>();
        }

        public SqlCommand ToSqlCommand(SqlConnection con)
        {
            var sqlCommand = new SqlCommand(Builder.ToString(), con);
            sqlCommand.Parameters.AddRange(ParameterList.ToArray());
            return sqlCommand;
        }

        public ConditionalSqlBuilder MayAppend(string value, string parameterName, string sql)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                Builder.Append(sql);
                ParameterList.Add(new SqlParameter(parameterName, value));
            }
            return this;
        }
    }
}
