using Microsoft.EntityFrameworkCore.Migrations;

namespace Timeoff.Extensions
{
    internal static class MigrationExtensions
    {
        public static void AddRowVersion(this MigrationBuilder migrationBuilder, string tableName)
        {
            migrationBuilder.Sql(
@$"CREATE TRIGGER Set{tableName}TimestampOnUpdate
AFTER UPDATE ON {tableName}
BEGIN
    UPDATE {tableName}
    SET RowVersion = randomblob(8)
    WHERE rowid = NEW.rowid;
END");
        }
    }
}