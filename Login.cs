using POS_qu.Controllers;
using POS_qu.Models;
using POSqu_menu;
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
    public partial class Login: Form
    {
        public Login()
        {
            InitializeComponent();
        }
        // Click event for Login button
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoginController loginController = new LoginController();
            var user = loginController.AuthenticateUser(username, password);

            if (user != null)
            {
               
                MessageBox.Show(
                    $"Login successful!\n\nUsername: {user.Username}\nRole: {user.RoleName}\nTerminal: {user.TerminalName}",
                    "Welcome",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Pindah ke form utama
                this.Hide();
                MenuNative menu = new MenuNative();
                menu.FormClosed += (s, args) => Application.Exit();
                menu.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
