using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace POS_qu.Helpers
{
    public sealed class LicenseActivationResponse
    {
        public string Status { get; set; }
        public string Plan { get; set; }
        public string ExpiredAt { get; set; }
    }

    public sealed class LocalLicenseFile
    {
        public string Key { get; set; }
        public string Plan { get; set; }
        public string ExpiredAt { get; set; }
        public string Status { get; set; }
        public string DeviceId { get; set; }
        public DateTime SavedAt { get; set; }
    }

    public static class LocalFirstPaths
    {
        public static string DataDir
        {
            get
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "POS-qu");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public static string LicenseFilePath => Path.Combine(DataDir, "license.json");
        public static string DeviceIdFilePath => Path.Combine(DataDir, "device_id.txt");
    }

    public static class DeviceIdProvider
    {
        public static async Task<string> GetOrCreateAsync()
        {
            var path = LocalFirstPaths.DeviceIdFilePath;
            if (File.Exists(path))
            {
                var s = (await File.ReadAllTextAsync(path)).Trim();
                if (!string.IsNullOrWhiteSpace(s)) return s;
            }

            var deviceId = Guid.NewGuid().ToString("N");
            await File.WriteAllTextAsync(path, deviceId);
            return deviceId;
        }
    }

    public static class LicenseManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static async Task<LocalLicenseFile?> LoadAsync()
        {
            var path = LocalFirstPaths.LicenseFilePath;
            if (!File.Exists(path)) return null;

            var json = await File.ReadAllTextAsync(path);
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonSerializer.Deserialize<LocalLicenseFile>(json, JsonOptions);
        }

        public static async Task SaveAsync(LocalLicenseFile file)
        {
            file.SavedAt = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(file, JsonOptions);
            await File.WriteAllTextAsync(LocalFirstPaths.LicenseFilePath, json);
        }

        public static bool IsValidNow(LocalLicenseFile? file)
        {
            if (file == null) return false;
            if (!string.Equals(file.Status?.Trim(), "ok", StringComparison.OrdinalIgnoreCase)) return false;
            if (string.IsNullOrWhiteSpace(file.Key)) return false;
            if (string.IsNullOrWhiteSpace(file.ExpiredAt)) return false;
            if (!DateTime.TryParse(file.ExpiredAt, out var exp)) return false;
            return exp.Date >= DateTime.Now.Date;
        }
    }

    public static class LicenseApiClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<LicenseActivationResponse> ActivateAsync(string baseUrl, string licenseKey, string deviceId)
        {
            using var http = new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) };
            var payload = new { license_key = licenseKey, device_id = deviceId };
            var json = JsonSerializer.Serialize(payload);
            using var resp = await http.PostAsync("/api/license/activate", new StringContent(json, Encoding.UTF8, "application/json"));
            var body = await resp.Content.ReadAsStringAsync();
            resp.EnsureSuccessStatusCode();
            var data = JsonSerializer.Deserialize<LicenseActivationResponse>(body, JsonOptions);
            return data ?? new LicenseActivationResponse { Status = "error" };
        }
    }

    public static class SyncApiClient
    {
        public static async Task SyncDummyItemsAsync(string baseUrl)
        {
            using var http = new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) };
            var now = DateTime.UtcNow;
            var payload = new
            {
                items = new[]
                {
                    new { id = 1, name = "Item A", price = 10000, updated_at = now },
                    new { id = 2, name = "Item B", price = 15000, updated_at = now },
                    new { id = 3, name = "Item C", price = 20000, updated_at = now },
                }
            };

            using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var resp = await http.PostAsync("/api/sync/items", content);
            resp.EnsureSuccessStatusCode();
        }
    }
}

