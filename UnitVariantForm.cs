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

namespace POS_qu
{
    public partial class UnitVariantForm : Form
    {
        private ItemController itemController;
      
        private string baseUnitName;
        public Item _item;

        public UnitVariantForm(Item item, List<UnitVariant> existingVariants = null)
        {
            InitializeComponent();
            _item = item;
            baseUnitName = _item.unit;

            if (existingVariants != null && existingVariants.Count > 0)
            {
                _item.UnitVariants = existingVariants; // hanya assign kalau memang ada
            }

            StartPosition = FormStartPosition.CenterScreen;
        }


        private void UnitVariantForm_Load(object sender, EventArgs e)
        {
            itemController = new ItemController();
            DataTable unitTable = itemController.GetUnits();
            cmbUnitVariant.DataSource = unitTable;
            cmbUnitVariant.DisplayMember = "display";
            cmbUnitVariant.ValueMember = "id";
            cmbUnitVariant.SelectedIndex = -1;

            lblConvertionRate.Text = baseUnitName;

            foreach (var variant in _item.UnitVariants)
            {
                AddVariantToPanel(variant);
            }
            
        }

        private void BtnSaveVariant_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtConvertionRate.Text, out int conversion) || conversion <= 0)
            {
                MessageBox.Show("Conversion must be a positive number.");
                return;
            }

            if (!decimal.TryParse(txtSellingPrice.Text, out decimal sellingPrice))
            {
                MessageBox.Show("Selling Price must be a valid number.");
                return;
            }

            //decimal.TryParse(txtProfit.Text, out decimal profit);

            //if (cmbUnitVariant.SelectedValue == null)
            //{
            //    MessageBox.Show("Please select a unit.");
            //    return;
            //}
            decimal actualSellPrice = _item.sell_price * conversion;
            decimal profit = sellingPrice - actualSellPrice;

            int unitId = Convert.ToInt32(cmbUnitVariant.SelectedValue);
            string unitName = cmbUnitVariant.Text;

            decimal MinQty;
            if (!decimal.TryParse(txtMinQty.Text, out MinQty))
            {
                // Input bukan angka, beri default atau tampilkan pesan error
                MinQty = 0; // contoh default
                MessageBox.Show("Masukkan angka yang valid untuk MinQty.");
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

            _item.UnitVariants.Add(variant);
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
                AutoSize = true,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(0, 5, 0, 5)
            };

            var labelEquation = new Label
            {
                Text = $"1 {variant.UnitName} = {variant.Conversion} {lblConvertionRate.Text}",
                AutoSize = true
            };


            var labelMinQty = new Label
            {
                Text = $"Minimal Qty: {variant.MinQty}",
                AutoSize = true

            };
            var labelConversion = new Label
            {
                Text = $"Konversi: {variant.Conversion}",
                AutoSize = true
            };

            var labelhargabeli = new Label
            {
                Text = $"Harga Beli Produk: {_item.sell_price}",
                AutoSize = true
            };

            var labelActualSelling = new Label
            {
                Text = $"Harga Jual Normal: {variant.actualSellPrice}",
                AutoSize = true
            };

            var labelSelling = new Label
            {
                Text = $"Harga Jual Variant: {variant.SellPrice}",
                AutoSize = true
            };

            var labelProfit = new Label
            {
                Text = $"Profit: {variant.Profit}",
                AutoSize = true
            };

            var btnDelete = new Button
            {
                Text = "Delete",
                AutoSize = true,
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                Tag = variant,
                Margin = new Padding(5)
            };

            btnDelete.Click += (s, ev) =>
            {
                var result = MessageBox.Show("Are you sure you want to delete this unit variant?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    UnitVariant toRemove = (UnitVariant)btnDelete.Tag;
                    _item.UnitVariants.Remove(toRemove);
                    flpVariantLog.Controls.Remove(panel);
                }
            };

            var contentLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false
            };

            contentLayout.Controls.Add(labelEquation);
            contentLayout.Controls.Add(labelMinQty); 
            contentLayout.Controls.Add(labelConversion);
            contentLayout.Controls.Add(labelActualSelling); 
            contentLayout.Controls.Add(labelhargabeli);
            contentLayout.Controls.Add(labelSelling);
            contentLayout.Controls.Add(labelProfit);

            panel.Controls.Add(contentLayout);
            panel.Controls.Add(btnDelete);

            btnDelete.Anchor = AnchorStyles.Right;
            btnDelete.Location = new Point(panel.Width - btnDelete.Width - 10, 10);

            var separator = new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Height = 2,
                Width = panel.Width,
                Margin = new Padding(3, 10, 3, 10)
            };

            flpVariantLog.Controls.Add(panel);
            flpVariantLog.Controls.Add(separator);
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
