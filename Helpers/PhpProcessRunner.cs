using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace POS_qu.Helpers
{
    public static class PhpProcessRunner
    {
        public static (int exitCode, string stdout, string stderr) RunScript(string rootDir, string scriptFileName, Dictionary<string, string>? env = null)
        {
            string scriptPath = Path.Combine(rootDir, scriptFileName);
            if (!File.Exists(scriptPath))
                throw new FileNotFoundException($"PHP script tidak ditemukan: {scriptPath}", scriptPath);

            var psi = new ProcessStartInfo
            {
                FileName = "php",
                Arguments = scriptFileName,
                WorkingDirectory = rootDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            if (env != null)
            {
                foreach (var kv in env)
                {
                    psi.EnvironmentVariables[kv.Key] = kv.Value;
                }
            }
            using var proc = Process.Start(psi);
            string stdout = proc!.StandardOutput.ReadToEnd();
            string stderr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            return (proc.ExitCode, stdout, stderr);
        }
    }
}
