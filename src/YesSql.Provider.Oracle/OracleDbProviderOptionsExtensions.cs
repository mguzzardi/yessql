using System;
using System.Data;
#if RUN_DOTNET451
using Oracle.ManagedDataAccess.Client;
#endif
#if RUN_NETSTANDARD
using Devart.Data.Oracle;
#endif


namespace YesSql.Provider.Oracle
{
    public static class OracleDbProviderOptionsExtensions
    {
        public static IConfiguration RegisterOracle(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["oracleconnection"] = new OracleDialect();
            CommandInterpreterFactory.CommandInterpreters["oracleconnection"] = d => new OracleCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UseOracle(
            this IConfiguration configuration,
            string connectionString)
        {
            return UseOracle(configuration, connectionString, IsolationLevel.ReadCommitted);
        }

        public static IConfiguration UseOracle(
            this IConfiguration configuration,
            string connectionString,
            IsolationLevel isolationLevel)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterOracle(configuration);
            configuration.ConnectionFactory = new DbConnectionFactory<OracleConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}
