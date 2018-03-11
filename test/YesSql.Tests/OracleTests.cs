using System;
using YesSql.Provider.Oracle;
using YesSql.Sql;

namespace YesSql.Tests
{
    public class OracleTests : CoreTests
    {
        public static string ConnectionString => Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING") ?? @"User Id=YesSQL;Password=PWD;Server=localhost;port=1521;sid=orcl;direct=true";
        public OracleTests()
        {
            _store = new Store(new Configuration().UseOracle(ConnectionString));

            CleanDatabase(false);
            CreateTables();
        }

        protected override void OnCleanDatabase(SchemaBuilder builder, ISession session)
        {
            base.OnCleanDatabase(builder, session);

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
