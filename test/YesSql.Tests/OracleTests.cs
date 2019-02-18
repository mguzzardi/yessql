using System;
using System.Data.Common;
using YesSql.Provider.Oracle;
using YesSql.Sql;

namespace YesSql.Tests
{
    public class OracleTests : CoreTests
    {
        public static string ConnectionString => Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING") ?? @"User Id=YesSQL;Password=PWD;Server=localhost;port=1521;sid=orcl;direct=true";
        protected override IConfiguration CreateConfiguration()
        {
            return new Configuration()
                .UseOracle(ConnectionString)
                .SetTablePrefix(TablePrefix)
                .UseBlockIdGenerator()
                ;
        }
        public OracleTests()
        {
        }

        protected override void OnCleanDatabase(SchemaBuilder builder, DbTransaction transaction)
        {
            base.OnCleanDatabase(builder, transaction);

            try
            {
                builder.DropTable("Content");
            }
            catch { }

            try
            {
                builder.DropTable("Collection1_Content");
            }
            catch { }
        }
    }
}
