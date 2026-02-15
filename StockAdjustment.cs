using POS_qu.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_qu.Models;
using POS_qu.Core;
using POS_qu.Helpers;

namespace POS_qu
{
    public partial class StockAdjustment : Form
    {
        private IActivityService activityService;
        private ILogger flogger = new FileLogger();
        private ILogger dlogger = new DbLogger();

        private ItemController itemController;
        Item item;
        public StockAdjustment(int id)
        {
            itemController = new ItemController();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            item = itemController.GetItemById(id);


            // Inject the FileLogger into ActivityService
            activityService = new ActivityService(flogger, dlogger);

            // 🔗 Hook event di sini
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            textBox4.TextChanged += textBox4_TextChanged;


            textBox1.Text = item.name;
            textBox2.Text = item.stock.ToString();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox4.ReadOnly = false;   // stok fisik aktif
                textBox3.ReadOnly = true;    // selisih auto
                textBox4.Clear();
                textBox3.Text = "0";
                textBox4.Focus();
            }
        }



        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked &&
                int.TryParse(textBox4.Text, out int stokFisik) &&
                int.TryParse(textBox2.Text, out int stokSistem))
            {
                int selisih = stokFisik - stokSistem;
                textBox3.Text = selisih.ToString();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox4.ReadOnly = true;    // stok fisik tidak dipakai
                textBox3.ReadOnly = false;   // selisih diisi manual
                textBox3.Clear();
                textBox4.Text = "0";
                textBox3.Focus();
            }
        }

     

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int stokSistem = int.Parse(textBox2.Text);
            int selisih = int.Parse(textBox3.Text);
            int? stokFisik = null;
            string metode;

            if (radioButton1.Checked) // Selisih Fisik
            {
                metode = "selisih_fisik";
                stokFisik = int.Parse(textBox4.Text);
            }
            else // Qty Manual
            {
                metode = "qty_manual";
            }

            string alasan = richTextBox1.Text;
            string catatan = richTextBox2.Text;

            var user = SessionUser.GetCurrentUser();

            itemController.AdjustStock(
                item.id,
                stokSistem,
                selisih,
                metode,
                stokFisik,
                alasan,
                catatan,
                user.UserId,
                user.LoginId
            );

            // Activity Log
            activityService.LogAction(
                userId: user.UserId.ToString(),
                actionType: ActivityType.StockAdjusment.ToString(),
                referenceId: item.id,
                details: new
                {
                    itemId = item.id,
                    oldStock = stokSistem,
                    selisih = selisih,
                    newStock = stokSistem + selisih,
                    metode = metode,
                    alasan = alasan,
                    terminal = user.TerminalId
                }
            );

            MessageBox.Show("Stock berhasil di-adjust!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        private void bntCancel_Click(object sender, EventArgs e)
        {
            // Reset ke data awal item
            textBox1.Text = item.name;
            textBox2.Text = item.stock.ToString();

            // Kosongkan input user
            textBox4.Clear();      // Stok fisik
            textBox3.Clear();      // Selisih
            richTextBox1.Clear();  // Alasan
            richTextBox2.Clear();  // Catatan

            // Reset radio button
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            // Fokus balik ke input stok fisik
            textBox4.Focus();
        }

    }
}
