using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace POS_qu.Helpers
{
    public static class PhpTranslationRunner
    {
        public static void RunMigrationsFromController(string host, string port, string user, string pass, string dbName, string rootMigrationDir)
        {
            string controller = Path.Combine(rootMigrationDir, "posqumigration.php");
            if (!File.Exists(controller))
                throw new FileNotFoundException("posqumigration.php tidak ditemukan", controller);
            string connString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbName}";
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var php = File.ReadAllText(controller);
                foreach (var sql in ExtractExecSqlBlocks(php))
                {
                    var cleaned = CleanSqlForPostgres(UnescapePhpSql(sql));
                    if (string.IsNullOrWhiteSpace(cleaned)) continue;
                    foreach (var stmt in SplitStatements(cleaned))
                    {
                        if (string.IsNullOrWhiteSpace(stmt)) continue;
                        using var cmd = new NpgsqlCommand(stmt, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                }
                var migrationsDir = Path.Combine(rootMigrationDir, "migrations");
                if (Directory.Exists(migrationsDir))
                {
                    var files = Directory.GetFiles(migrationsDir, "*.php", SearchOption.TopDirectoryOnly)
                                         .OrderBy(f => f, new NaturalFileNameComparer())
                                         .ToArray();
                    foreach (var file in files)
                    {
                        var phpFile = File.ReadAllText(file);
                        foreach (var sql in ExtractExecSqlBlocks(phpFile))
                        {
                            var cleaned = CleanSqlForPostgres(UnescapePhpSql(sql));
                            if (string.IsNullOrWhiteSpace(cleaned)) continue;
                            foreach (var stmt in SplitStatements(cleaned))
                            {
                                if (string.IsNullOrWhiteSpace(stmt)) continue;
                                using var cmd = new NpgsqlCommand(stmt, conn, tran);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void RunSeedersFromController(string host, string port, string user, string pass, string dbName, string rootMigrationDir)
        {
            string controller = Path.Combine(rootMigrationDir, "posquseeder.php");
            if (!File.Exists(controller))
                throw new FileNotFoundException("posquseeder.php tidak ditemukan", controller);
            string connString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbName}";
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var php = File.ReadAllText(controller);
                foreach (var sql in ExtractExecSqlBlocks(php))
                {
                    var cleaned = CleanSqlForPostgres(UnescapePhpSql(sql));
                    if (string.IsNullOrWhiteSpace(cleaned)) continue;
                    foreach (var stmt in SplitStatements(cleaned))
                    {
                        if (string.IsNullOrWhiteSpace(stmt)) continue;
                        using var cmd = new NpgsqlCommand(stmt, conn, tran);
                        cmd.ExecuteNonQuery();
                    }
                }
                var seedersDir = Path.Combine(rootMigrationDir, "seeders");
                if (Directory.Exists(seedersDir))
                {
                    var files = Directory.GetFiles(seedersDir, "*.php", SearchOption.TopDirectoryOnly)
                                         .OrderBy(f => f, new NaturalFileNameComparer())
                                         .ToArray();
                    foreach (var file in files)
                    {
                        var phpFile = File.ReadAllText(file);
                        foreach (var sql in ExtractExecSqlBlocks(phpFile))
                        {
                            var cleaned = CleanSqlForPostgres(UnescapePhpSql(sql));
                            if (string.IsNullOrWhiteSpace(cleaned)) continue;
                            foreach (var stmt in SplitStatements(cleaned))
                            {
                                if (string.IsNullOrWhiteSpace(stmt)) continue;
                                using var cmd = new NpgsqlCommand(stmt, conn, tran);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void RunMigrations(string host, string port, string user, string pass, string dbName, string rootMigrationDir)
        {
            string connString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbName}";
            string migrationsDir = Path.Combine(rootMigrationDir, "migrations");
            if (!Directory.Exists(migrationsDir))
                throw new DirectoryNotFoundException($"Folder migrations tidak ditemukan: {migrationsDir}");

            var files = Directory.GetFiles(migrationsDir, "*.php", SearchOption.TopDirectoryOnly)
                                 .OrderBy(f => f, new NaturalFileNameComparer())
                                 .ToArray();
            if (files.Length == 0)
                throw new InvalidOperationException("Tidak ada file migrasi .php");

            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                foreach (var file in files)
                {
                    var php = File.ReadAllText(file);
                    foreach (var sql in ExtractExecSqlBlocks(php))
                    {
                        var cleaned = CleanSqlForPostgres(UnescapePhpSql(sql));
                        if (string.IsNullOrWhiteSpace(cleaned)) continue;
                        foreach (var stmt in SplitStatements(cleaned))
                        {
                            if (string.IsNullOrWhiteSpace(stmt)) continue;
                            using var cmd = new NpgsqlCommand(stmt, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void RunSeeders(string host, string port, string user, string pass, string dbName, string rootMigrationDir)
        {
            string connString = $"Host={host};Port={port};Username={user};Password={pass};Database={dbName}";
            string seedersDir = Path.Combine(rootMigrationDir, "seeders");
            if (!Directory.Exists(seedersDir))
                throw new DirectoryNotFoundException($"Folder seeders tidak ditemukan: {seedersDir}");

            var files = Directory.GetFiles(seedersDir, "*.php", SearchOption.TopDirectoryOnly)
                                 .OrderBy(f => f, new NaturalFileNameComparer())
                                 .ToArray();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                foreach (var file in files)
                {
                    var php = File.ReadAllText(file);
                    foreach (var sql in ExtractExecSqlBlocks(php))
                    {
                        var cleaned = CleanSqlForPostgres(UnescapePhpSql(sql));
                        if (string.IsNullOrWhiteSpace(cleaned)) continue;
                        foreach (var stmt in SplitStatements(cleaned))
                        {
                            if (string.IsNullOrWhiteSpace(stmt)) continue;
                            using var cmd = new NpgsqlCommand(stmt, conn, tran);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static IEnumerable<string> ExtractExecSqlBlocks(string php)
        {
            var results = new List<string>();
            ExtractByQuote(php, '"', results);
            ExtractByQuote(php, '\'', results);
            return results;
        }

        private static void ExtractByQuote(string php, char quote, List<string> results)
        {
            string marker = $"$db->exec({quote}";
            int idx = 0;
            while (true)
            {
                int start = php.IndexOf(marker, idx, StringComparison.Ordinal);
                if (start < 0) break;
                int s = start + marker.Length;
                var sb = new StringBuilder();
                bool escaped = false;
                for (int i = s; i < php.Length; i++)
                {
                    char c = php[i];
                    if (escaped)
                    {
                        sb.Append(c);
                        escaped = false;
                        continue;
                    }
                    if (c == '\\')
                    {
                        escaped = true;
                        sb.Append(c);
                        continue;
                    }
                    if (c == quote)
                    {
                        results.Add(sb.ToString());
                        idx = i + 1;
                        break;
                    }
                    sb.Append(c);
                    if (i == php.Length - 1)
                        idx = php.Length;
                }
                if (idx < s) break;
            }
        }

        private static string UnescapePhpSql(string s)
        {
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\' && i + 1 < s.Length)
                {
                    char n = s[i + 1];
                    if (n == '"' || n == '\\' || n == '\'')
                    {
                        sb.Append(n);
                        i++;
                        continue;
                    }
                    if (n == '$')
                    {
                        sb.Append('$');
                        i++;
                        continue;
                    }
                    sb.Append(n);
                    i++;
                    continue;
                }
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        // Remove line comments (//, --, #) and block comments /* ... */ outside quotes
        private static string CleanSqlForPostgres(string s)
        {
            var sb = new StringBuilder(s.Length);
            bool inSingle = false, inDouble = false;
            bool inLineComment = false, inBlockComment = false;
            bool inDollarTag = false;
            string currentTag = "";
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                char next = i + 1 < s.Length ? s[i + 1] : '\0';

                if (inLineComment)
                {
                    if (c == '\n')
                    {
                        inLineComment = false;
                        sb.Append(c);
                    }
                    continue;
                }
                if (inBlockComment)
                {
                    if (c == '*' && next == '/')
                    {
                        inBlockComment = false;
                        i++;
                    }
                    continue;
                }

                if (!inDouble && c == '\'' )
                {
                    inSingle = !inSingle;
                    sb.Append(c);
                    continue;
                }
                if (!inSingle && c == '"' )
                {
                    inDouble = !inDouble;
                    sb.Append(c);
                    continue;
                }

                if (!inSingle && !inDouble)
                {
                    if (!inDollarTag && c == '$')
                    {
                        int j = i + 1;
                        while (j < s.Length && s[j] != '$' && (char.IsLetterOrDigit(s[j]) || s[j] == '_'))
                        {
                            j++;
                        }
                        if (j < s.Length && s[j] == '$')
                        {
                            currentTag = s.Substring(i, j - i + 1);
                            inDollarTag = true;
                            sb.Append(currentTag);
                            i = j;
                            continue;
                        }
                    }
                    if (inDollarTag && c == '$')
                    {
                        int endLen = currentTag.Length;
                        if (i + endLen - 1 < s.Length)
                        {
                            string endTag = s.Substring(i, endLen);
                            if (endTag == currentTag)
                            {
                                sb.Append(endTag);
                                i = i + endLen - 1;
                                inDollarTag = false;
                                currentTag = "";
                                continue;
                            }
                        }
                    }
                    if (inDollarTag)
                    {
                        sb.Append(c);
                        continue;
                    }
                    if (c == '/' && next == '*')
                    {
                        inBlockComment = true;
                        i++;
                        continue;
                    }
                    if (c == '/' && next == '/')
                    {
                        inLineComment = true;
                        i++;
                        continue;
                    }
                    if (c == '-' && next == '-')
                    {
                        inLineComment = true;
                        i++;
                        continue;
                    }
                    if (c == '#')
                    {
                        inLineComment = true;
                        continue;
                    }
                }

                sb.Append(c);
            }
            return sb.ToString().Trim();
        }

        private static IEnumerable<string> SplitStatements(string s)
        {
            var results = new List<string>();
            var sb = new StringBuilder();
            bool inSingle = false, inDouble = false, inDollar = false, inDollarTag = false;
            string currentTag = "";
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                char next = i + 1 < s.Length ? s[i + 1] : '\0';
                if (!inDouble && c == '\'') { inSingle = !inSingle; sb.Append(c); continue; }
                if (!inSingle && c == '"') { inDouble = !inDouble; sb.Append(c); continue; }
                if (!inSingle && !inDouble)
                {
                    if (c == '$' && next == '$') { inDollar = !inDollar; sb.Append(c); i++; sb.Append('$'); continue; }
                    if (!inDollarTag && c == '$')
                    {
                        int j = i + 1;
                        while (j < s.Length && s[j] != '$' && (char.IsLetterOrDigit(s[j]) || s[j] == '_'))
                        {
                            j++;
                        }
                        if (j < s.Length && s[j] == '$')
                        {
                            currentTag = s.Substring(i, j - i + 1);
                            inDollarTag = true;
                            sb.Append(currentTag);
                            i = j;
                            continue;
                        }
                    }
                    if (inDollarTag && c == '$')
                    {
                        int endLen = currentTag.Length;
                        if (i + endLen - 1 < s.Length)
                        {
                            string endTag = s.Substring(i, endLen);
                            if (endTag == currentTag)
                            {
                                sb.Append(endTag);
                                i = i + endLen - 1;
                                inDollarTag = false;
                                currentTag = "";
                                continue;
                            }
                        }
                    }
                    if (!inDollar && !inDollarTag && c == ';')
                    {
                        results.Add(sb.ToString().Trim());
                        sb.Clear();
                        continue;
                    }
                }
                sb.Append(c);
            }
            var tail = sb.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(tail)) results.Add(tail);
            return results;
        }

        private class NaturalFileNameComparer : IComparer<string>
        {
            public int Compare(string? x, string? y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                var a = Path.GetFileName(x);
                var b = Path.GetFileName(y);
                return CompareNatural(a, b);
            }
            private static int CompareNatural(string a, string b)
            {
                int ia = 0, ib = 0;
                while (ia < a.Length && ib < b.Length)
                {
                    char ca = a[ia], cb = b[ib];
                    if (char.IsDigit(ca) && char.IsDigit(cb))
                    {
                        long va = 0, vb = 0;
                        while (ia < a.Length && char.IsDigit(a[ia])) { va = va * 10 + (a[ia] - '0'); ia++; }
                        while (ib < b.Length && char.IsDigit(b[ib])) { vb = vb * 10 + (b[ib] - '0'); ib++; }
                        if (va != vb) return va < vb ? -1 : 1;
                    }
                    else
                    {
                        int cmp = char.ToUpperInvariant(ca).CompareTo(char.ToUpperInvariant(cb));
                        if (cmp != 0) return cmp;
                        ia++; ib++;
                    }
                }
                return (a.Length - ia).CompareTo(b.Length - ib);
            }
        }
    }
}
    
