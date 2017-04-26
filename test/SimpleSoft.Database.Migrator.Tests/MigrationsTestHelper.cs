using System;

namespace SimpleSoft.Database.Migrator.Tests
{
    public static class MigrationsTestHelper
    {
        public static string GenerateMigrationId(DateTimeOffset seed)
        {
            var migrationId = string.Concat(
                "Migration", seed.ToString("yyyyMMdd_HHmmss_fffffff"));

            return migrationId;
        }

        public static string GenerateMigrationId()
        {
            return GenerateMigrationId(DateTimeOffset.UtcNow);
        }

        public static string GenerateMigrationClassName(string migrationId)
        {
            return string.Concat("SimpleSoft.Database.Migrator.Tests.", migrationId);
        }

        public static void GenerateMigrationInfo(DateTimeOffset seed, out string migrationId, out string className)
        {
            migrationId = GenerateMigrationId(seed);
            className = GenerateMigrationClassName(migrationId);
        }

        public static void GenerateMigrationInfo(out string migrationId, out string className)
        {
            GenerateMigrationInfo(DateTimeOffset.UtcNow, out migrationId, out className);
        }
    }
}