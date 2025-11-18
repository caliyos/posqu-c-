using POS_qu.Controllers;
using POS_qu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class SupplierForm : Form
    {
        private SupplierController _controller;
        private List<Supplier> _suppliers;

        public SupplierForm()
        {
            InitializeComponent();
            _controller = new SupplierController();

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;

            dataGridViewSuppliers.SelectionChanged += DataGridViewSuppliers_SelectionChanged;

            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            _suppliers = _controller.GetSuppliers();
            dataGridViewSuppliers.DataSource = null;
            dataGridViewSuppliers.DataSource = _suppliers;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var supplier = new Supplier
            {
                Name = txtName.Text,
                Kode = txtKode.Text,
                ContactName = txtContact.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text,
                Note = txtNote.Text
            };

            if (_controller.AddSupplier(supplier))
            {
                MessageBox.Show("Supplier berhasil ditambahkan.");
                LoadSuppliers();
                ClearInputs();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.CurrentRow == null) return;

            var supplier = (Supplier)dataGridViewSuppliers.CurrentRow.DataBoundItem;
            supplier.Name = txtName.Text;
            supplier.Kode = txtKode.Text;
            supplier.ContactName = txtContact.Text;
            supplier.Phone = txtPhone.Text;
            supplier.Email = txtEmail.Text;
            supplier.Address = txtAddress.Text;
            supplier.Note = txtNote.Text;

            if (_controller.UpdateSupplier(supplier))
            {
                MessageBox.Show("Supplier berhasil diupdate.");
                LoadSuppliers();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.CurrentRow == null) return;

            var supplier = (Supplier)dataGridViewSuppliers.CurrentRow.DataBoundItem;

            if (_controller.DeleteSupplier(supplier.Id))
            {
                MessageBox.Show("Supplier berhasil dihapus.");
                LoadSuppliers();
                ClearInputs();
            }
        }

        private void DataGridViewSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.CurrentRow == null) return;

            var supplier = (Supplier)dataGridViewSuppliers.CurrentRow.DataBoundItem;

            txtName.Text = supplier.Name;
            txtKode.Text = supplier.Kode;
            txtContact.Text = supplier.ContactName;
            txtPhone.Text = supplier.Phone;
            txtEmail.Text = supplier.Email;
            txtAddress.Text = supplier.Address;
            txtNote.Text = supplier.Note;
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtKode.Clear();
            txtContact.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtNote.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoadSuppliers();
        }
    }
}
