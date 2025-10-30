using POS_qu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger()
        {
            string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            _filePath = Path.Combine(logDir, "activity_log.txt");
        }


        // Versi baru untuk log terstruktur
        public void Log(string userId, string actionType, int? referenceId, string? desc, string details)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | " +
                              $"{(string.IsNullOrEmpty(userId) ? "NULL" : userId)} | " +
                              $"{(actionType ?? "APP_LOG")} | " +
                              $"{(referenceId.HasValue ? referenceId.Value.ToString() : "NULL")} | " +
                              $"{(string.IsNullOrEmpty(details) ? "{}" : details)}";

            File.AppendAllText(_filePath, logEntry + Environment.NewLine);
        }

    }


}
