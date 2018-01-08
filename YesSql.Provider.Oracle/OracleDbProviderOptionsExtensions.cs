using System;
using System.Data;

namespace YesSql.Provider.Oracle
{
    public static class OracleDbProviderOptionsExtensions
    {
        public static IConfiguration RegisterOracle(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["npgsqlconnection"] = new OracleDialect();
            CommandInterpreterFactory.CommandInterpreters["npgsqlconnection"] = d => new OracleCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UseOracle(
            this IConfiguration configuration,
            string connectionString)
        {
            return UseOracle(configuration, connectionString, IsolationLevel.ReadUncommitted);
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

            RegisterUseOracle(configuration);
            configuration.ConnectionFactory = new DbConnectionFactory<NpgsqlConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}
