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

namespace POS_qu
{
    public partial class CategoryForm : Form
    {
        private CategoryController _controller;
        private List<Category> _categories;

        public CategoryForm()
        {
            InitializeComponent();
            _controller = new CategoryController();
            LoadCategoryTree();
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            // **Bind TreeView event**
            treeViewCategories.AfterSelect += treeViewCategories_AfterSelect;
        }


        // Helper class untuk ComboBoxItem
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int? Value { get; set; }
            public ComboBoxItem(string text, int? value)
            {
                Text = text;
                Value = value;
            }
            public override string ToString()
            {
                return Text;
            }
        }
        private void LoadParentCategories()
        {
            cmbParentCategory.Items.Clear();

            // Tambahkan opsi Root / tanpa parent
            cmbParentCategory.Items.Add(new ComboBoxItem("(Root)", null));

            foreach (var cat in _categories)
            {
                cmbParentCategory.Items.Add(new ComboBoxItem(cat.Name + " (" + cat.Kode + ")", cat.Id));
            }

            cmbParentCategory.SelectedIndex = 0; // default Root
        }

        private void LoadCategoryTree()
        {
            treeViewCategories.Nodes.Clear();

            // Ambil semua kategori flat list
            _categories = _controller.GetCategories();

            // Load ComboBox parent
            LoadParentCategories();

            // Build TreeView hierarchy
            var roots = _controller.GetCategoryHierarchy();
            foreach (var cat in roots)
            {
                treeViewCategories.Nodes.Add(BuildTreeNode(cat));
            }

            treeViewCategories.ExpandAll();
        }


        private TreeNode BuildTreeNode(Category cat)
        {
            var node = new TreeNode($"{cat.Name} ({cat.Kode})") { Tag = cat };
            foreach (var child in cat.Children)
            {
                node.Nodes.Add(BuildTreeNode(child));
            }
            return node;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Clear input fields
            txtName.Text = "";
            txtKode.Text = "";
            txtDescription.Text = "";

            // Optional: reset TreeView selection
            treeViewCategories.SelectedNode = null;

            // Reload tree
            LoadCategoryTree();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            int? parentId = null;
            if (treeViewCategories.SelectedNode != null)
            {
                var parentCat = (Category)treeViewCategories.SelectedNode.Tag;
                parentId = parentCat.Id;
            }

            var newCat = new Category
            {
                Name = txtName.Text,
                Kode = txtKode.Text,
                Description = txtDescription.Text,
                ParentId = parentId
            };

            if (_controller.AddCategory(newCat))
            {
                MessageBox.Show("Kategori berhasil ditambahkan.");
                LoadCategoryTree();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (treeViewCategories.SelectedNode == null) return;

            var cat = (Category)treeViewCategories.SelectedNode.Tag;
            cat.Name = txtName.Text;
            cat.Kode = txtKode.Text;
            cat.Description = txtDescription.Text;

            if (_controller.UpdateCategory(cat))
            {
                MessageBox.Show("Kategori berhasil diupdate.");
                LoadCategoryTree();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeViewCategories.SelectedNode == null) return;

            var cat = (Category)treeViewCategories.SelectedNode.Tag;

            if (_controller.DeleteCategory(cat.Id))
            {
                MessageBox.Show("Kategori berhasil dihapus.");
                LoadCategoryTree();
            }
        }

        private void treeViewCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            var cat = e.Node.Tag as Category;
            if (cat == null) return;

            // Isi TextBox sesuai node
            txtName.Text = cat.Name;
            txtKode.Text = cat.Kode;
            txtDescription.Text = cat.Description;

            // Set ComboBox parent
            if (cat.ParentId == null)
            {
                cmbParentCategory.SelectedIndex = 0; // Root
            }
            else
            {
                for (int i = 0; i < cmbParentCategory.Items.Count; i++)
                {
                    var item = cmbParentCategory.Items[i] as ComboBoxItem;
                    if (item.Value == cat.ParentId)
                    {
                        cmbParentCategory.SelectedIndex = i;
                        break;
                    }
                }
            }
        }




    }
}
