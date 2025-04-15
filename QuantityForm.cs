using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class QuantityForm : Form
    {
        public int Quantity { get; private set; }
        public UnitVariant SelectedUnitVariant { get; private set; }
        public decimal TotalSellPrice { get; private set; }

        private List<UnitVariant> unitVariants;

        public QuantityForm(int availableStock, List<UnitVariant> variants)
        {
            InitializeComponent();
            lblStockAvailable.Text = $"Stock Available: {availableStock}";
            unitVariants = variants;

            if (unitVariants == null || unitVariants.Count == 0)
            {
                // Sembunyikan ComboBox dan Label untuk unit variant
                lblUnitVariant.Visible = false;
                cbUnitVariant.Visible = false;
                ShowNoUnitVariantMessage();  // Tampilkan pesan jika tidak ada unit variant
            }
            else
            {
                // Tampilkan unit variant jika ada
                cbUnitVariant.DataSource = unitVariants;
                cbUnitVariant.DisplayMember = "UnitName";
                cbUnitVariant.ValueMember = "UnitId";

                if (unitVariants.Count > 0)
                    cbUnitVariant.SelectedIndex = 0;

                UpdateUnitVariantInfo(); // Update informasi konversi dan harga jual
            }
        }

        private void cbUnitVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUnitVariantInfo(); // Update informasi ketika unit variant dipilih
        }

        private void UpdateUnitVariantInfo()
        {
            if (cbUnitVariant.SelectedItem != null)
            {
                var selectedUnit = cbUnitVariant.SelectedItem as UnitVariant;
                if (selectedUnit != null)
                {
                    // Tampilkan conversion rate dan harga jual
                    lblConversionRate.Text = $"Conversion Rate: 1 {selectedUnit.UnitName} = {selectedUnit.Conversion} pcs";
                    lblSellPrice.Text = $"Sell Price: {selectedUnit.SellPrice:C} per {selectedUnit.UnitName}";
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantity.Text, out int enteredQuantity))
            {
                if (enteredQuantity > 0)
                {
                    // Validasi jika quantity melebihi stok yang tersedia
                    int availableStock = int.Parse(lblStockAvailable.Text.Split(':')[1].Trim());
                    if (enteredQuantity > availableStock)
                    {
                        MessageBox.Show("Quantity cannot exceed available stock.");
                        return;
                    }

                    Quantity = enteredQuantity;

                    if (unitVariants != null && unitVariants.Count > 0)
                    {
                        SelectedUnitVariant = cbUnitVariant.SelectedItem as UnitVariant;
                    }
                    else
                    {
                        // Jika tidak ada unit variant, set null
                        SelectedUnitVariant = null;
                    }

                    // Menghitung total harga jual berdasarkan unit yang dipilih
                    if (SelectedUnitVariant != null)
                    {
                        TotalSellPrice = SelectedUnitVariant.SellPrice * enteredQuantity;
                    }

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid quantity.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        public void ShowNoUnitVariantMessage()
        {
            lblNoUnitVariant.Visible = true;
            lblNoUnitVariant.Text = "No unit variants available for this product.";
            lblUnitVariant.Visible = false;
            cbUnitVariant.Visible = false;
        }

        public void ShowUnitVariants(List<UnitVariant> unitVariants)
        {
            lblNoUnitVariant.Visible = false;
            lblUnitVariant.Visible = true;
            cbUnitVariant.Visible = true;

            // Isi ComboBox dengan unit variants
            cbUnitVariant.DataSource = unitVariants;
            cbUnitVariant.DisplayMember = "UnitName";
            cbUnitVariant.ValueMember = "UnitId";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
