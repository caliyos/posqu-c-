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
        private ProductService _productService;
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
            if (cmbUnitVariant.SelectedValue == null)
            {
                MessageBox.Show("Pilih satuan terlebih dahulu.");
                return;
            }

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

            decimal sellIfFollowBase = baseSellPrice * conversion;
            decimal hppTotal = _item.buy_price * conversion;
            decimal profitReal = sellingPrice - hppTotal;

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
                SellPrice = sellingPrice,
                Profit = profitReal,
                MinQty = MinQty,
                BarcodeSuffix = string.IsNullOrWhiteSpace(txtBarcodeSuffix.Text) ? null : txtBarcodeSuffix.Text.Trim(),
                actualSellPrice = sellIfFollowBase
            };

            UnitVariants.Add(variant);
            AddVariantToPanel(variant);

            txtConvertionRate.Text = "";
            txtSellingPrice.Text = "";
            txtMinQty.Text = "";
            txtBarcodeSuffix.Text = "";
            cmbUnitVariant.SelectedIndex = -1;
        }

        private void AddVariantToPanel(UnitVariant variant)
        {
            var sellIfFollowBase = baseSellPrice * variant.Conversion;
            var hppTotal = _item.hpp_avg * variant.Conversion;
            var priceDiff = variant.SellPrice - sellIfFollowBase;
            var priceDiffPercent = sellIfFollowBase == 0m ? 0m : (priceDiff / sellIfFollowBase) * 100m;
            var hppDiff = variant.SellPrice - hppTotal;
            var hppDiffPercent = hppTotal == 0m ? 0m : (hppDiff / hppTotal) * 100m;

            var panel = new Panel
            {
                Width = flpVariantLog.Width - 30,
                Height = 118,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.White
            };

            var labelEquation = new Label
            {
                Text = $"1 {variant.UnitName} = {variant.Conversion} {lblConvertionRate.Text}",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(15, 12)
            };

            var labelMinQty = new Label
            {
                Text = $"Min Qty: {variant.MinQty}",
                AutoSize = true,
                Location = new Point(15, 34),
                ForeColor = Color.DimGray
            };
            
            var labelBarcodeSuffix = new Label
            {
                Text = $"Barcode Suffix: {(string.IsNullOrWhiteSpace(variant.BarcodeSuffix) ? "-" : variant.BarcodeSuffix)}",
                AutoSize = true,
                Location = new Point(15, 56),
                ForeColor = Color.DimGray
            };

            var labelSellVariant = new Label
            {
                Text = $"Harga varian: Rp {variant.SellPrice:N0}",
                AutoSize = true,
                Location = new Point(250, 12),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 255)
            };

            var labelCompareBase = new Label
            {
                Text = $"Perbandingan dengan harga jual base (Rp {sellIfFollowBase:N0}): {(priceDiff >= 0 ? "+" : "-")}Rp {Math.Abs(priceDiff):N0} ({(priceDiff >= 0 ? "+" : "-")}{Math.Abs(priceDiffPercent):N1}%)",
                AutoSize = true,
                Location = new Point(250, 34),
                ForeColor = Color.DimGray
            };

            var labelCompareHpp = new Label
            {
                Text = $"Perbandingan dengan HPP (Rp {hppTotal:N0}): {(hppDiff >= 0 ? "+" : "-")}Rp {Math.Abs(hppDiff):N0} ({(hppDiff >= 0 ? "+" : "-")}{Math.Abs(hppDiffPercent):N1}%)",
                AutoSize = true,
                Location = new Point(250, 56),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = hppDiff >= 0 ? Color.Green : Color.Red
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
            panel.Controls.Add(labelBarcodeSuffix);
            panel.Controls.Add(labelSellVariant);
            panel.Controls.Add(labelCompareBase);
            panel.Controls.Add(labelCompareHpp);
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
