using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using YesSql.Sql;

namespace YesSql.Provider.Oracle
{
    public class OracleDialect : BaseDialect
    {
        private static Dictionary<DbType, string> ColumnTypes = new Dictionary<DbType, string>
        {
            {DbType.Guid, "raw(16)"},
            {DbType.Binary, "raw"},
            {DbType.Date, "date"},
            {DbType.Time, "date"},
            {DbType.DateTime, "timestamp" },
            {DbType.DateTime2, "timestamp" },
            {DbType.DateTimeOffset, "timestamp with time Zone" },
            {DbType.Boolean, "number(1)"},
            {DbType.Byte, "number(3)"},
            {DbType.Decimal, "number"},
            {DbType.Single, "binary_float"},
            {DbType.Double, "binary_double"},
            {DbType.Int16, "number(5,0)"},
            {DbType.Int32, "number(9,0)"},
            {DbType.Int64, "number(19,0)"},
            {DbType.UInt16, "number(5,0)"},
            {DbType.UInt32, "number(9,0)"},
            {DbType.UInt64, "number(19,0)"},
            {DbType.AnsiStringFixedLength, "char(255)"},
            {DbType.AnsiString, "varchar2(255)"},
            {DbType.StringFixedLength, "nchar(255)"},
            {DbType.String, "nvarchar(255)"},
            {DbType.Currency, "number"}
        };

        public OracleDialect()
        {
            Methods.Add("second", new TemplateFunction("extract(second from {0})"));
            Methods.Add("minute", new TemplateFunction("extract(minute from {0})"));
            Methods.Add("hour", new TemplateFunction("extract(hour from {0})"));
            Methods.Add("day", new TemplateFunction("extract(day from {0})"));
            Methods.Add("month", new TemplateFunction("extract(month from {0})"));
            Methods.Add("year", new TemplateFunction("extract(year from {0})"));
        }

        public override string Name => "Oracle";

        public override ISqlBuilder CreateBuilder(string tablePrefix)
        {
            return new OracleSqlBuilder(tablePrefix, this);
        }

        public override string GetTypeName(DbType dbType, int? length, byte precision, byte scale)
        {
            if (length.HasValue)
            {
                if (length.Value > 4000)
                {
                    if (dbType == DbType.String)
                    {
                        return "nclob";
                    }

                    if (dbType == DbType.AnsiString)
                    {
                        return "clob";
                    }

                    if (dbType == DbType.Binary)
                    {
                        return "blob";
                    }
                }
                else
                {
                    if (dbType == DbType.String)
                    {
                        return "nvarchar2(" + length + ")";
                    }

                    if (dbType == DbType.AnsiString)
                    {
                        return "varchar2(" + length + ")";
                    }

                    if (dbType == DbType.Binary)
                    {
                        return "raw";
                    }
                }
            }

            if (ColumnTypes.TryGetValue(dbType, out string value))
            {
                return value;
            }

            throw new Exception("DbType not found for: " + dbType);
        }

        public override string GetDropForeignKeyConstraintString(string name)
        {
            return " drop foreign key " + name;
        }

        public override string DefaultValuesInsert => "DEFAULT VALUES";

        public override void Page(ISqlBuilder sqlBuilder, int offset, int limit)
        {
            var sb = new StringBuilder();

            sb.Append(" rownum <= ");

            if (limit != 0)
            {
                sb.Append(limit);
            }

            if (offset != 0)
            {
                sb.Append(" rownum > ");
                sb.Append(offset);
            }

            sqlBuilder.Trail = sb.ToString();
        }

        public override string QuoteForColumnName(string columnName)
        {
            return QuoteString + columnName + QuoteString;
        }

        public override string QuoteForTableName(string tableName)
        {
            return QuoteString + tableName + QuoteString;
        }

        protected override string Quote(string value)
        {
            return SingleQuoteString + value.Replace(SingleQuoteString, DoubleSingleQuoteString) + SingleQuoteString;
        }

        public override string CascadeConstraintsString => " cascade ";

        public override string IdentitySelectString => throw new NotImplementedException();

        public override string GetSqlValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            switch (Convert.GetTypeCode(value))
            {
                case TypeCode.Boolean:
                    return (bool)value ? "TRUE" : "FALSE";
                default:
                    return base.GetSqlValue(value);
            }
        }

    }
}
