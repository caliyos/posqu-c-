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

        public void Log(string message)
        {
            File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
        }
    }


}
