using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class HelperDatabase
    {

        public static string GetSqliteDatabaseConnectionString(string dbName)
        {
            var builder = new SqliteConnectionStringBuilder { DataSource = GetSqliteFilePath(dbName) };

            return builder.ConnectionString;
        }
        public static async Task ExecuteSqliteScriptAsync(string dbName, string script)
        {
            using var connection = new SqliteConnection(GetSqliteDatabaseConnectionString(dbName));
            connection.Open();
            using (var cmdDb = new SqliteCommand(script, connection))
            {
                await cmdDb.ExecuteNonQueryAsync();
            }
            connection.Close();
        }

        public static string GetSqliteFilePath(string dbName)
        {
            var fi = new FileInfo(dbName);

            if (string.IsNullOrEmpty(fi.Extension))
                dbName = $"{dbName}.db";

            return Path.Combine(Directory.GetCurrentDirectory(), dbName);

        }

        public static void DropSqliteDatabase(string dbName)
        {
            string filePath = null;
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                filePath = GetSqliteFilePath(dbName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

            }
            catch (Exception)
            {
                Console.WriteLine($"Sqlite file seems loked. ({filePath})");
            }

        }

        public static string GetRandomName(string pref = default)
        {
            var str1 = Path.GetRandomFileName().Replace(".", "").ToLowerInvariant();
            return $"{pref}{str1}";
        }
    }
}
