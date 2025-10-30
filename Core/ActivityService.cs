using PdfSharp.Logging;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace POS_qu.Core
{
    // === Core Implementation ===
    public class ActivityService : IActivityService
    {
        private readonly List<ILogger> _loggers;

        // constructor terima 1..n logger
        public ActivityService(params ILogger[] loggers)
        {
            _loggers = new List<ILogger>(loggers);
        }

        //public void LogAction(string actionType, string message, object? details = null)
        //{
        //    string detaildata = string.Empty;

        //    if (details != null)
        //    {
        //        detaildata = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
        //    }

        //    string logMessage = $"[{actionType}] {message} | {detaildata}";

        //    // loop semua logger (file, db, console, dll)
        //    foreach (var logger in _loggers)
        //    {
        //        logger.Log(logMessage);
        //    }
        //}

        public void LogAction(string userId, string actionType, int? referenceId, string? desc = null, object? details = null)
        {
            string detailData = "{}";

            if (details != null)
            {
                detailData = System.Text.Json.JsonSerializer.Serialize(details, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            }

            // loop semua logger, pakai versi baru jika tersedia
            foreach (var logger in _loggers)
            {
                logger.Log(userId, actionType, referenceId, desc, detailData);
                // cek apakah logger support overload structured log
                //switch (logger)
                //{
                //    case DbLogger dbLogger:
                //        dbLogger.Log(userId, actionType, referenceId, detailData);
                //        break;
                //    case FileLogger fileLogger:
                //        fileLogger.Log(userId, actionType, referenceId, detailData);
                //        break;
                //    default:
                //        // fallback ke versi lama
                //        logger.Log($"[{actionType}] {detailData}");
                //        break;
                //}
            }
        }

    }
}
