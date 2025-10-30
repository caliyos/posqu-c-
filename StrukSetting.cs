using POS_qu.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Helpers;
using POS_qu.Controllers;

namespace POS_qu
{
    public partial class StrukSetting : Form
    {
        public StrukSetting()
        {
            InitializeComponent();
            var setting = StrukSettingHelper.Load();
            if (setting != null)
            {
                label1.Text = setting.Judul;
                label2.Text = setting.Alamat;
                label3.Text = setting.NomorTelepon;
                label8.Text = setting.Footer;
                pictureBox1.Image = StrukSettingHelper.GetLogoImage(setting.LogoBytes);

                //MessageBox.Show("setting.IsVisibleNamaToko");
                chkTampilkanNama.Checked = setting.IsVisibleNamaToko;
            }
            txtNamaManual.Enabled = false; // Disable

            txtNamaManual.Leave += txtNamaManual_Leave;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)  // Jika radio "Gunakan Nama Toko" dipilih
            {
                txtNamaManual.Enabled = false;

                var settingController = new SettingController();
                string namaToko = settingController.GetNamaToko(); // Ambil nama toko dari tabel settingtoko

                txtNamaManual.Text = namaToko;
                label1.Text = namaToko;

                // Update judul di tabel struk_setting
                var strukController = new SettingController();
                strukController.UpdateJudul(namaToko);
            }
            else if (radioButton2.Checked)  // Jika radio "Input Manual"
            {
                txtNamaManual.Enabled = true;
                txtNamaManual.Text = "";  // Kosongkan agar bisa input manual

            }
        }

        private void chkTampilkanNama_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = chkTampilkanNama.Checked;

            // Simpan ke database
            var controller = new SettingController();
            controller.UpdateVisibilitySetting("is_visible_nama_toko", chkTampilkanNama.Checked);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtNamaManual_Leave(object sender, EventArgs e)
        {
            var controller = new SettingController();
            controller.UpdateNamaTokoManual(txtNamaManual.Text);

            string namaToko = controller.GetNamaTokoDariStrukSetting(); // Ambil nama toko dari tabel struk_setting
            txtNamaManual.Text = namaToko;
            label1.Text = namaToko;

        }
    }
}
