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

            LoginController loginController = new LoginController();
            var user = loginController.AuthenticateUser(username, password);

            if (user != null)
            {
                // Buat sesi user setelah login sukses
                SessionUser.CreateSession(user.Id, user.Username, user.RoleId, user.ShiftId,user.TerminalId );

                MessageBox.Show($"Login successful! Welcome {user.Username}");

                // Pindah ke form utama
                this.Hide();
                MenuNative posForm = new MenuNative();
                posForm.FormClosed += (s, args) => Application.Exit(); // Kalau POS ditutup, aplikasi juga close
                posForm.Show();
                //Casher_POS posForm = new Casher_POS();
                //posForm.FormClosed += (s, args) => Application.Exit(); // Kalau POS ditutup, aplikasi juga close
                //posForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

    }
}
