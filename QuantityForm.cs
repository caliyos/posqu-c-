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

        private string mainunit;

        public QuantityForm(int availableStock, List<UnitVariant> variants,string prmmainunit)
        {
            InitializeComponent();
            lblStockAvailable.Text = $"Stock Available: {availableStock}";
            unitVariants = variants ?? new List<UnitVariant>();
            mainunit = prmmainunit;
            if (unitVariants.Count == 0)
            {
                lblUnitVariant.Visible = false;
                cbUnitVariant.Visible = false;
                ShowNoUnitVariantMessage();
            }
            else
            {
                // Add a dummy 'No Variant' option
                UnitVariant noVariantOption = new UnitVariant
                {
                    UnitId = 0, // assuming 0 means "No Variant"
                    UnitName = "-- No Variant (Base Unit) --",
                    Conversion = 1,
                    SellPrice = 0 // let it indicate base price is used elsewhere
                };

                unitVariants.Insert(0, noVariantOption);

                cbUnitVariant.DataSource = unitVariants;
                cbUnitVariant.DisplayMember = "UnitName";
                cbUnitVariant.ValueMember = "UnitId";

                cbUnitVariant.SelectedIndex = 0;

                UpdateUnitVariantInfo();
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
                    if (selectedUnit.UnitId == 0) // No Variant Selected
                    {
                        lblConversionRate.Text = "Using base unit.";
                        lblSellPrice.Text = ""; // or your base unit price if you have it
                    }
                    else
                    {
                        lblConversionRate.Text = $"Conversion Rate: 1 {selectedUnit.UnitName} = {selectedUnit.Conversion} {mainunit}";
                        lblSellPrice.Text = $"Sell Price: {selectedUnit.SellPrice:C} per {selectedUnit.UnitName}";
                    }
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
                        var selected = cbUnitVariant.SelectedItem as UnitVariant;

                        if (selected != null && selected.UnitId != 0) // Real variant selected
                        {
                            SelectedUnitVariant = selected;
                            TotalSellPrice = SelectedUnitVariant.SellPrice * enteredQuantity;
                        }
                        else
                        {
                            // No Variant chosen (base unit)
                            SelectedUnitVariant = null;

                            // Assume you want to use base unit price here, 
                            // for example if you passed a decimal `BaseSellPrice` in constructor
                            // TotalSellPrice = BaseSellPrice * enteredQuantity;

                            TotalSellPrice = 0; // Set 0 or handle base price as needed
                        }
                    }
                    else
                    {
                        // Tidak ada unit variants, jadi base unit digunakan
                        SelectedUnitVariant = null;

                        // Assume you want to use base unit price here
                        // TotalSellPrice = BaseSellPrice * enteredQuantity;

                        TotalSellPrice = 0; // or your calculation
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
