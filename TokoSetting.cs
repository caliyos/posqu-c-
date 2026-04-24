using POS_qu.Controllers;
using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Helpers;

namespace POS_qu
{
    public partial class TokoSetting : Form
    {
        private SettingController settingController;
        private string selectedImagePath = "";
        public TokoSetting()
        {
            InitializeComponent();
            settingController = new SettingController();
            settingController.EnsureSettingTokoSchema();
            LoadSettingData(); ;
            button2.Click += button2_Click;
            chkPKP.CheckedChanged += chkPKP_CheckedChanged;
        }


        private void LoadSettingData()
        {
            DataRow setting = settingController.GetSettingToko();
            Debug.WriteLine("setting : " + setting["nama"]?.ToString());
            if (setting != null)
            {
                textBox1.Text = setting["nama"]?.ToString() ?? "";
                richTextBox1.Text = setting["alamat"]?.ToString() ?? "";
                textBox2.Text = setting["npwp"]?.ToString() ?? "";
                chkPKP.Checked = setting.Table.Columns.Contains("is_pkp") && setting["is_pkp"] != DBNull.Value && (bool)setting["is_pkp"];
                if (setting.Table.Columns.Contains("ppn_rate") && setting["ppn_rate"] != DBNull.Value)
                    numPpnRate.Value = Convert.ToDecimal(setting["ppn_rate"]);
                else
                    numPpnRate.Value = 11;

                if (setting.Table.Columns.Contains("purchase_prefix") && setting["purchase_prefix"] != DBNull.Value)
                    txtPurchasePrefix.Text = setting["purchase_prefix"]?.ToString() ?? "";
                else
                    txtPurchasePrefix.Text = "PB";

                if (setting["logo"] != DBNull.Value)
                {
                    byte[] logoBytes = (byte[])setting["logo"];
                    using (var ms = new MemoryStream(logoBytes))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }

                }

                ApplyTaxUiState();
              
            }
            else
            {
                MessageBox.Show("Data setting toko tidak ditemukan.");
            }
        }

        private void chkPKP_CheckedChanged(object sender, EventArgs e)
        {
            ApplyTaxUiState();
        }

        private void ApplyTaxUiState()
        {
            numPpnRate.Enabled = chkPKP.Checked;
            if (!chkPKP.Checked)
                numPpnRate.Value = 11;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CLicker");
            // Untuk gambar logo aplikasi
            selectedImagePath = ImageHelper.SelectAndSaveImage(pictureBox, "applogo");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string nama = textBox1.Text.Trim();
                string alamat = richTextBox1.Text.Trim();
                string npwp = textBox2.Text.Trim();

                byte[] logoBytes = null;
                if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
                {
                    logoBytes = File.ReadAllBytes(selectedImagePath);
                }

                var prefix = (txtPurchasePrefix.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(prefix)) prefix = "PB";
                settingController.UpdateSettingToko(
                    nama,
                    alamat,
                    npwp,
                    logoBytes,
                    isPkp: chkPKP.Checked,
                    ppnRate: Convert.ToDecimal(numPpnRate.Value),
                    purchasePrefix: prefix
                );

                MessageBox.Show("Data berhasil disimpan.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
