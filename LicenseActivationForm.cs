using POS_qu.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class LicenseActivationForm : Form
    {
        private const string LicenseServerBaseUrl = "http://localhost:10002";
        private string _deviceId = "";

        public LicenseActivationForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            Load += LicenseActivationForm_Load;
            btnClose.Click += (s, e) => Close();
            btnActivate.Click += async (s, e) => await ActivateAsync();
        }

        private async void LicenseActivationForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!LocalFirstPaths.TryEnsureWritable(out var err))
                {
                    MessageBox.Show($"Tidak bisa simpan license ke:\n{LocalFirstPaths.DataDir}\n\n{err}", "Akses Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                _deviceId = await DeviceIdProvider.GetOrCreateAsync();
                lblDeviceId.Text = "Device ID: " + _deviceId;

                var existing = await LicenseManager.LoadAsync();
                if (existing != null)
                {
                    txtLicenseKey.Text = existing.Key ?? "";
                    UpdateStatus(existing);
                }

                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.Settings.AreDevToolsEnabled = true;
                webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
                webView.CoreWebView2.Navigate(LicenseServerBaseUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ActivateAsync()
        {
            try
            {
                btnActivate.Enabled = false;
                btnActivate.Text = "Activating...";

                var key = (txtLicenseKey.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(key))
                    throw new InvalidOperationException("License key wajib diisi.");

                if (string.IsNullOrWhiteSpace(_deviceId))
                    _deviceId = await DeviceIdProvider.GetOrCreateAsync();

                var call = await LicenseApiClient.ActivateAsync(LicenseServerBaseUrl, key, _deviceId);
                var resp = call.Data;

                if (resp.Body == null)
                    throw new InvalidOperationException("Response body kosong.\n\nRAW:\n" + (call.RawBody ?? ""));

                if (resp.Body.License == null)
                {
                    var full = BuildActivationDebugText(
                        title: "Aktivasi gagal (license null)",
                        call: call,
                        resp: resp,
                        inputKey: key,
                        deviceId: _deviceId
                    );
                    ShowActivationError("Gagal", full);
                    return;
                }

                bool serverValid = resp.Body.License.Valid ?? true;
                if (!resp.Body.Success || !serverValid)
                {
                    var full = BuildActivationDebugText(
                        title: "Aktivasi gagal (success/valid false)",
                        call: call,
                        resp: resp,
                        inputKey: key,
                        deviceId: _deviceId
                    );
                    ShowActivationError("Gagal", full);
                    return;
                }

                if (string.IsNullOrWhiteSpace(resp.Body.Signature) ||
                    string.IsNullOrWhiteSpace(resp.Body.SigAlg) ||
                    !string.Equals(resp.Body.SigAlg, "RS256", StringComparison.OrdinalIgnoreCase))
                {
                    var full = BuildActivationDebugText(
                        title: "Aktivasi gagal (signature belum tersedia dari server)",
                        call: call,
                        resp: resp,
                        inputKey: key,
                        deviceId: _deviceId
                    );
                    ShowActivationError("Gagal", full);
                    return;
                }

                var file = new LocalLicenseFile
                {
                    Status = resp.Status,
                    Success = resp.Body.Success,
                    Message = resp.Body.Message,
                    ActivationId = resp.Body.ActivationId,
                    Key = string.IsNullOrWhiteSpace(resp.Body.License.Key) ? key : resp.Body.License.Key,
                    LicenseValid = serverValid,
                    ActivatedAt = resp.Body.License.ActivatedAt,
                    ExpiresAt = resp.Body.License.ExpiresAt,
                    LicenseType = resp.Body.License.LicenseType,
                    StoreId = resp.Body.License.StoreId,
                    MachineId = resp.Body.License.MachineId,
                    DeviceId = _deviceId
                };

                file.Signature = resp.Body.Signature;
                file.SigAlg = resp.Body.SigAlg;
                file.SigKid = resp.Body.SigKid;

                if (!LicenseManager.TryValidateNow(file, out var validateErr))
                {
                    var full = BuildActivationDebugText(
                                   title: "Aktivasi gagal (validasi signature/machine/expired gagal)",
                                   call: call,
                                   resp: resp,
                                   inputKey: key,
                                   deviceId: _deviceId) +
                               "\n\nvalidate_error:\n" + validateErr;
                    ShowActivationError("Gagal", full);
                    return;
                }

                try
                {
                    await LicenseManager.SaveAsync(file);
                }
                catch (Exception saveEx)
                {
                    var full = "Aktivasi sukses dari server, tapi gagal menyimpan license lokal.\n\n" +
                               $"Path: {LocalFirstPaths.LicenseFilePath}\n\n" +
                               saveEx;
                    ShowActivationError("Save Error", full);
                    return;
                }
                var saved = await LicenseManager.LoadAsync();
                if (saved != null)
                    UpdateStatus(saved);
                else
                    UpdateStatus(file);

                MessageBox.Show("License activated. Menu pro sudah terbuka.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                var full = ex.ToString();
                ShowActivationError("Error", full);
            }
            finally
            {
                btnActivate.Enabled = true;
                btnActivate.Text = "Activate";
            }
        }

        private static string BuildActivationDebugText(
            string title,
            POS_qu.Helpers.LicenseApiClient.LicenseApiCallResult call,
            POS_qu.Helpers.LicenseApiResponse resp,
            string inputKey,
            string deviceId)
        {
            var body = resp.Body;
            var lic = body?.License;
            string msg = body?.Message ?? "-";
            bool success = body?.Success ?? false;
            string activationId = body?.ActivationId ?? "-";
            string licenseKey = lic?.Key ?? "-";
            string expiresAt = lic?.ExpiresAt ?? "-";
            string storeId = lic?.StoreId ?? "-";
            string machineId = lic?.MachineId ?? "-";
            string valid = lic?.Valid.HasValue == true ? lic.Valid.Value.ToString() : "(null)";
            string signature = body?.Signature ?? "-";
            string sigAlg = body?.SigAlg ?? "-";
            string sigKid = body?.SigKid ?? "-";

            return
                $"{title}\n\n" +
                $"HTTP: {call.HttpStatusCode}\n" +
                $"status(json): {resp.Status}\n" +
                $"success: {success}\n" +
                $"message: {msg}\n" +
                $"activation_id: {activationId}\n\n" +
                $"input_key: {inputKey}\n" +
                $"device_id: {deviceId}\n\n" +
                $"license.key: {licenseKey}\n" +
                $"license.valid: {valid}\n" +
                $"license.expires_at: {expiresAt}\n" +
                $"license.machine_id: {machineId}\n" +
                $"license.store_id: {storeId}\n\n" +
                $"sig_alg: {sigAlg}\n" +
                $"sig_kid: {sigKid}\n" +
                $"signature: {signature}\n\n" +
                $"request_json:\n{call.RequestJson}\n\n" +
                $"raw_response:\n{call.RawBody}";
        }

        private void ShowActivationError(string caption, string fullText)
        {
            lblStatus.Text = fullText;
            try
            {
                Clipboard.SetText(fullText);
            }
            catch
            {
            }
            MessageBox.Show(fullText, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void UpdateStatus(LocalLicenseFile file)
        {
            var ok = LicenseManager.TryValidateNow(file, out var err);
            lblStatus.Text = $"Success: {(file.Success ? "YES" : "NO")}\nMessage: {(file.Message ?? "-")}\nActivation ID: {(file.ActivationId ?? "-")}\nKey: {(file.Key ?? "-")}\nExpires: {(file.ExpiresAt ?? "-")}\nMachine ID: {(file.MachineId ?? "-")}\nSigAlg: {(file.SigAlg ?? "-")}\nValid: {(ok ? "YES" : "NO")}\nValid Error: {(ok ? "-" : err)}\n\nSaved: {LocalFirstPaths.LicenseFilePath}";
        }
    }
}

namespace POS_qu.Helpers
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public sealed class LicenseApiResponse
    {
        public int Status { get; set; }
        public LicenseApiBody Body { get; set; }
    }

    public sealed class LicenseApiBody
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonPropertyName("activation_id")]
        public string ActivationId { get; set; }
        public LicenseApiLicense License { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
        [JsonPropertyName("sig_alg")]
        public string SigAlg { get; set; }
        [JsonPropertyName("sig_kid")]
        public string SigKid { get; set; }
    }

    public sealed class LicenseApiLicense
    {
        public string Key { get; set; }
        [JsonPropertyName("machine_id")]
        public string MachineId { get; set; }
        [JsonPropertyName("store_id")]
        public string StoreId { get; set; }
        [JsonPropertyName("activated_at")]
        public string ActivatedAt { get; set; }
        [JsonPropertyName("expires_at")]
        public string ExpiresAt { get; set; }
        public bool? Valid { get; set; }
        [JsonPropertyName("license_type")]
        public string LicenseType { get; set; }
    }

    public sealed class LocalLicenseFile
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ActivationId { get; set; }
        public string Key { get; set; }
        public bool LicenseValid { get; set; }
        public string ActivatedAt { get; set; }
        public string ExpiresAt { get; set; }
        public string LicenseType { get; set; }
        public string StoreId { get; set; }
        public string MachineId { get; set; }
        public string DeviceId { get; set; }
        public string Signature { get; set; }
        public string SigAlg { get; set; }
        public string SigKid { get; set; }
        public DateTime SavedAt { get; set; }
    }

    public static class LocalFirstPaths
    {
        public static string DataDir
        {
            get
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Posqu");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public static string LicenseFilePath => Path.Combine(DataDir, "license.json");
        public static string DeviceIdFilePath => Path.Combine(DataDir, "device_id.txt");
        public static string PublicKeyPemPath => Path.Combine(DataDir, "license_pubkey.pem");

        public static bool TryEnsureWritable(out string error)
        {
            error = "";
            try
            {
                var dir = DataDir;
                var testPath = Path.Combine(dir, ".__write_test");
                File.WriteAllText(testPath, DateTime.UtcNow.ToString("O"));
                File.Delete(testPath);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
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

            string deviceId;
            try
            {
                deviceId = Utility.GetPcId();
            }
            catch
            {
                deviceId = "";
            }

            if (string.IsNullOrWhiteSpace(deviceId))
                deviceId = Guid.NewGuid().ToString("N");

            await File.WriteAllTextAsync(path, deviceId);
            return deviceId;
        }
    }

    public static class LicenseSignatureVerifier
    {
        public static string BuildCanonical(string key, string machineId, string expiresAt, string activationId)
        {
            return $"key={key}&machine_id={machineId}&expires_at={expiresAt}&activation_id={activationId}";
        }

        public static string GetPublicKeyPem()
        {
            var env = Environment.GetEnvironmentVariable("POSQU_LICENSE_PUBLIC_KEY_PEM");
            if (!string.IsNullOrWhiteSpace(env)) return env;
            try
            {
                if (File.Exists(LocalFirstPaths.PublicKeyPemPath))
                    return File.ReadAllText(LocalFirstPaths.PublicKeyPemPath);
            }
            catch
            {
            }
            return "";
        }

        public static bool VerifyRs256(string publicKeyPem, string canonical, string signatureBase64)
        {
            try
            {
                var sig = Convert.FromBase64String(signatureBase64);
                using var rsa = RSA.Create();
                rsa.ImportFromPem(publicKeyPem.AsSpan());
                var data = Encoding.UTF8.GetBytes(canonical);
                return rsa.VerifyData(data, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch
            {
                return false;
            }
        }
    }

    public static class LicenseManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        private sealed class ProtectedLicenseEnvelope
        {
            public int V { get; set; }
            public string Scope { get; set; }
            [JsonPropertyName("protected_payload")]
            public string ProtectedPayload { get; set; }
        }

        public static async Task<LocalLicenseFile?> LoadAsync()
        {
            var path = LocalFirstPaths.LicenseFilePath;
            if (!File.Exists(path)) return null;

            var json = await File.ReadAllTextAsync(path);
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("protected_payload", out _))
                {
                    var env = JsonSerializer.Deserialize<ProtectedLicenseEnvelope>(json, JsonOptions);
                    if (env == null || string.IsNullOrWhiteSpace(env.ProtectedPayload)) return null;
                    var blob = Convert.FromBase64String(env.ProtectedPayload);
                    var plain = ProtectedData.Unprotect(blob, GetEntropy(), DataProtectionScope.LocalMachine);
                    var plainJson = Encoding.UTF8.GetString(plain);
                    return JsonSerializer.Deserialize<LocalLicenseFile>(plainJson, JsonOptions);
                }
            }
            catch
            {
            }

            var legacy = JsonSerializer.Deserialize<LocalLicenseFile>(json, JsonOptions);
            if (legacy != null)
            {
                try { await SaveAsync(legacy); } catch { }
            }
            return legacy;
        }

        public static async Task SaveAsync(LocalLicenseFile file)
        {
            file.SavedAt = DateTime.UtcNow;
            var plainJson = JsonSerializer.Serialize(file, JsonOptions);
            var plain = Encoding.UTF8.GetBytes(plainJson);
            var blob = ProtectedData.Protect(plain, GetEntropy(), DataProtectionScope.LocalMachine);
            var env = new ProtectedLicenseEnvelope
            {
                V = 1,
                Scope = "LocalMachine",
                ProtectedPayload = Convert.ToBase64String(blob)
            };
            var json = JsonSerializer.Serialize(env, JsonOptions);
            await File.WriteAllTextAsync(LocalFirstPaths.LicenseFilePath, json);
        }

        public static bool IsValidNow(LocalLicenseFile? file)
        {
            return TryValidateNow(file, out _);
        }

        public static bool TryValidateNow(LocalLicenseFile? file, out string error)
        {
            error = "";
            if (file == null) { error = "license file kosong"; return false; }
            if (!file.Success) { error = "success=false"; return false; }
            if (!file.LicenseValid) { error = "license.valid=false"; return false; }
            if (string.IsNullOrWhiteSpace(file.Key)) { error = "license.key kosong"; return false; }
            if (string.IsNullOrWhiteSpace(file.ExpiresAt)) { error = "expires_at kosong"; return false; }
            if (!DateTimeOffset.TryParse(file.ExpiresAt, out var exp)) { error = "expires_at invalid"; return false; }
            if (exp.UtcDateTime < DateTime.UtcNow) { error = "expired"; return false; }

            var deviceId = "";
            try { deviceId = Utility.GetPcId(); } catch { deviceId = ""; }
            var machineId = string.IsNullOrWhiteSpace(file.MachineId) ? deviceId : file.MachineId;
            if (!string.IsNullOrWhiteSpace(machineId) && !string.Equals(machineId, deviceId, StringComparison.OrdinalIgnoreCase))
            {
                error = "machine_id mismatch";
                return false;
            }

            if (string.IsNullOrWhiteSpace(file.Signature)) { error = "signature kosong"; return false; }
            if (!string.Equals(file.SigAlg ?? "", "RS256", StringComparison.OrdinalIgnoreCase)) { error = "sig_alg bukan RS256"; return false; }
            var canonical = LicenseSignatureVerifier.BuildCanonical(file.Key, machineId, file.ExpiresAt, file.ActivationId);
            var pub = LicenseSignatureVerifier.GetPublicKeyPem();
            if (string.IsNullOrWhiteSpace(pub)) { error = "public key pem belum diset"; return false; }
            if (!LicenseSignatureVerifier.VerifyRs256(pub, canonical, file.Signature)) { error = "signature invalid"; return false; }
            return true;
        }

        private static byte[] GetEntropy() => Encoding.UTF8.GetBytes("POS-qu-license-v1");
    }

    public static class LicenseApiClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private sealed class LicenseApiFlatResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            [JsonPropertyName("activation_id")]
            public string ActivationId { get; set; }
            public LicenseApiLicense License { get; set; }
            public string Signature { get; set; }
            [JsonPropertyName("sig_alg")]
            public string SigAlg { get; set; }
            [JsonPropertyName("sig_kid")]
            public string SigKid { get; set; }
        }

        public sealed class LicenseApiCallResult
        {
            public int HttpStatusCode { get; set; }
            public string RawBody { get; set; }
            public string RequestJson { get; set; }
            public LicenseApiResponse Data { get; set; }
        }

        public static async Task<LicenseApiCallResult> ActivateAsync(string baseUrl, string licenseKey, string deviceId)
        {
            using var http = new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) };
            var payload = new
            {
                key = licenseKey,
                license_key = licenseKey,
                device_id = deviceId,
                deviceId = deviceId,
                machine_id = deviceId,
                machineId = deviceId
            };
            var json = JsonSerializer.Serialize(payload);
            using var resp = await http.PostAsync("/api/license/activate", new StringContent(json, Encoding.UTF8, "application/json"));
            var body = await resp.Content.ReadAsStringAsync();

            LicenseApiResponse data;
            try
            {
                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("body", out _))
                {
                    data = JsonSerializer.Deserialize<LicenseApiResponse>(body, JsonOptions);
                    if (data == null) throw new InvalidOperationException("Response license wrapper tidak valid.");
                    if (data.Status == 0) data.Status = (int)resp.StatusCode;
                }
                else if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("success", out _))
                {
                    var flat = JsonSerializer.Deserialize<LicenseApiFlatResponse>(body, JsonOptions);
                    if (flat == null) throw new InvalidOperationException("Response license flat tidak valid.");
                    data = new LicenseApiResponse
                    {
                        Status = (int)resp.StatusCode,
                        Body = new LicenseApiBody
                        {
                            Success = flat.Success,
                            Message = flat.Message,
                            ActivationId = flat.ActivationId,
                            License = flat.License,
                            Signature = flat.Signature,
                            SigAlg = flat.SigAlg,
                            SigKid = flat.SigKid
                        }
                    };
                }
                else
                {
                    data = JsonSerializer.Deserialize<LicenseApiResponse>(body, JsonOptions);
                    if (data == null) throw new InvalidOperationException("Response license tidak valid.");
                    if (data.Status == 0) data.Status = (int)resp.StatusCode;
                }
            }
            catch (Exception parseEx)
            {
                throw new InvalidOperationException("Response license tidak valid: " + parseEx.Message + "\n\nRAW:\n" + body);
            }

            if (data.Body == null) data.Body = new LicenseApiBody();
            if (data.Body.License == null) data.Body.License = new LicenseApiLicense();

            return new LicenseApiCallResult
            {
                HttpStatusCode = (int)resp.StatusCode,
                RawBody = body,
                RequestJson = json,
                Data = data
            };
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
