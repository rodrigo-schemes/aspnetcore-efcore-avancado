using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreModulo19.Extensions
{
    public static partial class SnakeCaseExtensions
    {
        public static void ToSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName().ToSnakeCase();
                entity.SetTableName(tableName);

                foreach (var property in entity.GetProperties())
                {
                    var storeObjectIdentifier = StoreObjectIdentifier.Table(tableName, null);
                    
                    var columnName = property.GetColumnName(storeObjectIdentifier).ToSnakeCase();

                    property.SetColumnName(columnName);
                }

                foreach (var key in entity.GetKeys())
                {
                    var keyName = key.GetName().ToSnakeCase();
                    key.SetName(keyName);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    var foreignKeyName = key.GetConstraintName().ToSnakeCase();
                    key.SetConstraintName(foreignKeyName);
                }

                foreach (var index in entity.GetIndexes())
                {
                    var indexName = index.GetDatabaseName().ToSnakeCase();
                    index.SetDatabaseName(indexName);
                }
            }
        }

        //UserId -> user_id
        private static string ToSnakeCase(this string name)
            => MyRegex().Replace(name, "$1_$2").ToLower();
        [GeneratedRegex(@"([a-z0-9])([A-Z])")]
        private static partial Regex MyRegex();
    }
}