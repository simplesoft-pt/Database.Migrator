using System;

namespace SimpleSoft.Database.Migrator.Tests
{
    public static class MigrationsTestHelper
    {
        public static string GenerateMigrationIdUsingDateSeed(DateTimeOffset date)
        {
            var migrationId = string.Concat(
                "Migration", date.ToString("yyyyMMdd_HHmmss_fffffff"));

            return migrationId;
        }

        public static string GenerateMigrationId()
        {
            return GenerateMigrationIdUsingDateSeed(DateTimeOffset.UtcNow);
        }
    }
}