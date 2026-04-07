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
using POS_qu.Controllers;
using POS_qu.Models;
using POS_qu.Core.Interfaces;
using POS_qu.Services;
using POS_qu.Repositories;

namespace POS_qu
{
    public partial class UnitVariantForm : Form
    {
        public List<UnitVariant> UnitVariants { get; private set; }
        private Item _item;
        private IProductService _productService;
        private string baseUnitName;
        private decimal baseSellPrice;

        public UnitVariantForm(Item item, string unitName, decimal sellPrice)
        {
            InitializeComponent();
            _item = item;
            baseUnitName = unitName;
            baseSellPrice = sellPrice;
            
            UnitVariants = new List<UnitVariant>(_item.UnitVariants ?? new List<UnitVariant>());
            _productService = new ProductService(new ProductRepository());

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void UnitVariantForm_Load(object sender, EventArgs e)
        {
            ApplyProfessionalStyle();
            
            // Reconfigure form bounds for better UI
        
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            DataTable unitTable = _productService.GetUnits();
            cmbUnitVariant.DataSource = unitTable;
            cmbUnitVariant.DisplayMember = "display";
            cmbUnitVariant.ValueMember = "id";
            cmbUnitVariant.SelectedIndex = -1;

            lblConvertionRate.Text = baseUnitName;

            foreach (var variant in UnitVariants)
            {
                AddVariantToPanel(variant);
            }
            
        }

        private void ApplyProfessionalStyle()
        {
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            var btnSave = this.Controls.Find("btnSaveVariant", true).FirstOrDefault() as Button;
            if (btnSave != null)
            {
                btnSave.BackColor = Color.FromArgb(40, 167, 69);
                btnSave.ForeColor = Color.White;
                btnSave.FlatStyle = FlatStyle.Flat;
                btnSave.FlatAppearance.BorderSize = 0;
            }

            var btnDone = this.Controls.Find("btnDone", true).FirstOrDefault() as Button;
            if (btnDone != null)
            {
                btnDone.BackColor = Color.FromArgb(0, 122, 255);
                btnDone.ForeColor = Color.White;
                btnDone.FlatStyle = FlatStyle.Flat;
                btnDone.FlatAppearance.BorderSize = 0;
                btnDone.Text = "Simpan";
            }
            
         
        }

        private void BtnSaveVariant_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtConvertionRate.Text, out int conversion) || conversion <= 0)
            {
                MessageBox.Show("Konversi harus berupa angka positif.");
                return;
            }

            if (!decimal.TryParse(txtSellingPrice.Text, out decimal sellingPrice))
            {
                MessageBox.Show("Harga Jual harus berupa angka yang valid.");
                return;
            }

            decimal actualSellPrice = baseSellPrice * conversion;
            decimal profit = sellingPrice - actualSellPrice;

            int unitId = Convert.ToInt32(cmbUnitVariant.SelectedValue);
            string unitName = cmbUnitVariant.Text;

            decimal MinQty;
            if (!decimal.TryParse(txtMinQty.Text, out MinQty))
            {
                MinQty = 0; 
                MessageBox.Show("Masukkan angka yang valid untuk MinQty.");
                return;
            }


            var variant = new UnitVariant
            {
                UnitId = unitId,
                UnitName = unitName,
                Conversion = conversion, 
                actualSellPrice = actualSellPrice,
                SellPrice = sellingPrice,
                Profit = profit,
                MinQty = MinQty
            };

            UnitVariants.Add(variant);
            AddVariantToPanel(variant);

            txtConvertionRate.Text = "";
            txtSellingPrice.Text = "";
            txtMinQty.Text = "";
            cmbUnitVariant.SelectedIndex = -1;
        }

        private void AddVariantToPanel(UnitVariant variant)
        {
            var panel = new Panel
            {
                Width = flpVariantLog.Width - 30,
                Height = 80,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.White
            };

            var labelEquation = new Label
            {
                Text = $"1 {variant.UnitName} = {variant.Conversion} {lblConvertionRate.Text}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 15)
            };

            var labelMinQty = new Label
            {
                Text = $"Min Qty: {variant.MinQty}",
                AutoSize = true,
                Location = new Point(15, 45),
                ForeColor = Color.DimGray
            };
            
            var labelSelling = new Label
            {
                Text = $"Harga Jual: Rp {variant.SellPrice:N0}",
                AutoSize = true,
                Location = new Point(250, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255)
            };

            var labelActualSelling = new Label
            {
                Text = $"Total Harga Dasar: Rp {variant.actualSellPrice:N0}",
                AutoSize = true,
                Location = new Point(250, 45),
                ForeColor = Color.DimGray
            };

            var labelProfit = new Label
            {
                Text = $"Profit: Rp {variant.Profit:N0}",
                AutoSize = true,
                Location = new Point(480, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = variant.Profit >= 0 ? Color.Green : Color.Red
            };

            var btnDelete = new Button
            {
                Text = "Hapus",
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = variant,
                Location = new Point(panel.Width - 95, 20)
            };
            btnDelete.FlatAppearance.BorderSize = 0;

            btnDelete.Click += (s, ev) =>
            {
                var result = MessageBox.Show("Apakah Anda yakin ingin menghapus varian ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    UnitVariant toRemove = (UnitVariant)btnDelete.Tag;
                    UnitVariants.Remove(toRemove);
                    flpVariantLog.Controls.Remove(panel);
                }
            };

            panel.Controls.Add(labelEquation);
            panel.Controls.Add(labelMinQty);
            panel.Controls.Add(labelSelling);
            panel.Controls.Add(labelActualSelling);
            panel.Controls.Add(labelProfit);
            panel.Controls.Add(btnDelete);

            flpVariantLog.Controls.Add(panel);
        }

        private void TxtConvertionRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; this.Close();
        }
    }
}
