using System;
using System.Threading.Tasks;
using Xunit;
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

            CleanDatabase();
            CreateTables();
        }

        protected override void OnCleanDatabase(ISession session)
        {
            base.OnCleanDatabase(session);

            var builder = new SchemaBuilder(session);

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
