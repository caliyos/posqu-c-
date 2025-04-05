using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Data;

using POS_qu.Controllers;
using POS_qu.Models;
using System.Diagnostics;
using System.Xml.Linq;


namespace POS_qu
{
    public partial class Form2_crud : Form
    {

        private const string TABLE_NAME = "itemcrud";
        private ItemController itemController;
        //public async Task Add(BoardGame game)
        //{
        //    string commandText = $"INSERT INTO {TABLE_NAME} (id, Name, MinPlayers, MaxPlayers, AverageDuration) VALUES (@id, @name, @minPl, @maxPl, @avgDur)";
        //    await using (var cmd = new NpgsqlCommand(commandText, connection))
        //    {
        //        cmd.Parameters.AddWithValue("id", game.Id);
        //        cmd.Parameters.AddWithValue("name", game.Name);
        //        cmd.Parameters.AddWithValue("minPl", game.MinPlayers);
        //        cmd.Parameters.AddWithValue("maxPl", game.MaxPlayers);
        //        cmd.Parameters.AddWithValue("avgDur", game.AverageDuration);

        //        await cmd.ExecuteNonQueryAsync();
        //    }
        //}

        //public async Task<BoardGame> Get(int id)
        //{
        //    string commandText = $"SELECT * FROM {TABLE_NAME} WHERE ID = @id";
        //    await using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
        //    {
        //        cmd.Parameters.AddWithValue("id", id);

        //        await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            while (await reader.ReadAsync())
        //            {
        //                BoardGame game = ReadBoardGame(reader);
        //                return game;
        //            }
        //    }
        //    return null;
        //}

        // POSTGRES /////////////////////////////////
        string vStrConnection = "Host=localhost;Port=5433;Username=postgres;Password=postgres11;Database=posqu";
        NpgsqlConnection vCon;
        NpgsqlCommand vCmd;


        private void connection()
        {
            vCon = new NpgsqlConnection();
            vCon.ConnectionString = vStrConnection;

            if (vCon.State == ConnectionState.Closed)
            {
                vCon.Open();
            }

        }


        public DataTable getData(string sql)
        {
            DataTable dt = new DataTable();
            connection();
            vCmd = new NpgsqlCommand();
            vCmd.Connection = vCon;
            vCmd.CommandText = sql;

            NpgsqlDataReader dr = vCmd.ExecuteReader();
            dt.Load(dr);

            return dt;

        }

        private void Form2_crud_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Delete key is pressed (KeyCode is 'Delete' key)
            if (e.KeyCode == Keys.Delete)
            {
                // Call the button3_Click method to perform the delete action
                button3_Click(sender, e);
                //MessageBox.Show("pressed delete ");
            }
        }


        private void Form2_crud_Load(object sender, EventArgs e)
        {
            connection();
            // Set TabIndex for controls (example)
            textBox1.TabIndex = 0;
            textBox2.TabIndex = 1;
            button1.TabIndex = 2;
            button2.TabIndex = 3;
            button3.TabIndex = 4;
            button4.TabIndex = 5;
            button5.TabIndex = 6;
            button6.TabIndex = 7;

            //////////////////// NOT WORKING HERE ////////////////////////////
            //// Ensure the form listens for key events NOT WORKING HERE
            //this.KeyPreview = true;
            //this.KeyDown += new KeyEventHandler(Form2_crud_KeyDown);
            //////////////////// END NOT WORKING HERE ////////////////////////////
        }
        // END POSTGRES /////////////////////////////////
        public Form2_crud()
        {
            InitializeComponent();
            itemController = new ItemController();
            LoadItems();

            //////////////////// WORKING HERE ////////////////////////////
            this.KeyPreview = true; // Set KeyPreview here
            this.KeyDown += new KeyEventHandler(Form2_crud_KeyDown);
            ////////////////////END WORKING HERE ////////////////////////////
        }

        private void LoadItems()
        {
            DataTable dt = itemController.GetItems();
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadItems(); // Refresh the DataGridView
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearForm();
        }


        private void ClearForm()
        {
            textBox1.Text = textBox2.Text = "";
            button3.Enabled = false;
            button1.Text = "Save";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?",
                                                      "Confirmation",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int itemId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                    MessageBox.Show("itemid : " + itemId);
                    itemController.DeleteItem(itemId);
                    LoadItems(); // Refresh the DataGridView
                    ClearForm();
                }
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["name"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["buy_price"].Value.ToString();
                button1.Text = "Update";
                button3.Enabled = true;

                //////////////////////// DEBUGGING //////////////////
                string name = dataGridView1.SelectedRows[0].Cells["name"].Value.ToString();
                string price = dataGridView1.SelectedRows[0].Cells["buy_price"].Value.ToString();
                Console.WriteLine("Selected Name: " + name); // Log to the Output window
                Console.WriteLine("Selected Price: " + price); // Log to the Output window

                MessageBox.Show("Selected Name: " + name + "\nSelected Price: " + price, "Selection Info");
                // Log output to the Output window
                Debug.WriteLine($"Selected Name: {name}");
                Debug.WriteLine($"Selected Price: {price}");
                //////////////////////// END DEBUGGING //////////////////


            }
        }

        private void Form2_crud_Load_1(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Item newItem = new Item
        //    {
        //        Name = textBox1.Text,
        //        Price = decimal.Parse(textBox2.Text)
        //    };

        //    itemController.InsertItem(newItem);
        //    LoadItems(); // Refresh the DataGridView
        //    ClearForm();
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView1.SelectedRows.Count > 0)
        //    {
        //        int itemId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
        //        Item updatedItem = new Item
        //        {
        //            Id = itemId,
        //            Name = textBox1.Text,
        //            Price = decimal.Parse(textBox2.Text)
        //        };

        //        itemController.UpdateItem(updatedItem);
        //        LoadItems(); // Refresh the DataGridView
        //        ClearForm();
        //    }
        //}

        private void button6_Click(object sender, EventArgs e)
        {
            string keyword = textBox3search.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                DataTable dt = itemController.SearchItems(keyword);
                dataGridView1.DataSource = dt;
            }
            else
            {
                LoadItems(); // If the search box is empty, reload all items
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
