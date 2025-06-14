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
            LoadSettingData(); ;
            button2.Click += button2_Click;
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

                if (setting["logo"] != DBNull.Value)
                {
                    byte[] logoBytes = (byte[])setting["logo"];
                    using (var ms = new MemoryStream(logoBytes))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }

                }

              
            }
            else
            {
                MessageBox.Show("Data setting toko tidak ditemukan.");
            }
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

                settingController.UpdateSettingToko(nama, alamat, npwp, logoBytes);

                MessageBox.Show("Data berhasil disimpan.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
