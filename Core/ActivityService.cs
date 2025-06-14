using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace POS_qu.Core
{
    // === Core Implementation ===
    public class ActivityService : IActivityService
    {
        // Inject ILogger through the constructor
        private readonly ILogger _logger;
        public ActivityService(ILogger logger)
        {
            _logger = logger;
        }
        public void LogAction(string actionType, string message, object? details = null)
        {
            // Use the injected logger to log to the destination (file, console, etc.)
            // If additional details are provided, serialize and log them
            string detaildata = string.Empty; // or use "No details" or any default string

            // If additional details are provided, serialize and log them
            if (details != null)
            {
                detaildata = JsonSerializer.Serialize(details, new JsonSerializerOptions { WriteIndented = true });
            }

            _logger.Log($"[{actionType}] {message} | {detaildata}");

          
        }
    }

}
