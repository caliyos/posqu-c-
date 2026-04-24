using Microsoft.Web.WebView2.Core;
using POS_qu.Helpers;
using System;
using System.Diagnostics;
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

                var resp = await LicenseApiClient.ActivateAsync(LicenseServerBaseUrl, key, _deviceId);
                var file = new LocalLicenseFile
                {
                    Key = key,
                    Plan = resp.Plan,
                    ExpiredAt = resp.ExpiredAt,
                    Status = resp.Status,
                    DeviceId = _deviceId
                };

                await LicenseManager.SaveAsync(file);
                UpdateStatus(file);

                if (LicenseManager.IsValidNow(file))
                    MessageBox.Show("License aktif. Menu pro sudah terbuka.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("License tidak valid.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnActivate.Enabled = true;
                btnActivate.Text = "Activate";
            }
        }

        private void UpdateStatus(LocalLicenseFile file)
        {
            var ok = LicenseManager.IsValidNow(file);
            lblStatus.Text = $"Status: {(file.Status ?? "-")}\nPlan: {(file.Plan ?? "-")}\nExpired: {(file.ExpiredAt ?? "-")}\nValid: {(ok ? "YES" : "NO")}\n\nSaved: {LocalFirstPaths.LicenseFilePath}";
        }
    }
}
