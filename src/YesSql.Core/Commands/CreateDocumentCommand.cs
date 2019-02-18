using Dapper;
using System.Data.Common;
using System.Threading.Tasks;
using YesSql.Collections;

namespace YesSql.Commands
{
    public class CreateDocumentCommand : DocumentCommand
    {
        private readonly string _tablePrefix;

        public override int ExecutionOrder { get; } = 0;

        public CreateDocumentCommand(Document document, string tablePrefix) : base(document)
        {
            _tablePrefix = tablePrefix;
        }

        public override Task ExecuteAsync(DbConnection connection, DbTransaction transaction, ISqlDialect dialect)
        {
            var documentTable = CollectionHelper.Current.GetPrefixedName(Store.DocumentTable);
            var insertCmd = "insert into " + dialect.QuoteForTableName(_tablePrefix + documentTable) + " (" + dialect.QuoteForColumnName("Id") + ", " + dialect.QuoteForColumnName("Type") + ", " + dialect.QuoteForColumnName("Content") + ") " +
                "values (" + dialect.QuoteForParameter("Id") + ", " + dialect.QuoteForParameter("Type") + ", " + dialect.QuoteForParameter("Content") + ")" + dialect.StatementEnd;
            return connection.ExecuteScalarAsync<int>(insertCmd, Document, transaction);
        }
    }
}
