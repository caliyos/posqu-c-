using Npgsql;
using POS_qu.Helpers;
using System;
using System.Windows.Forms;

namespace POS_qu
{
    public partial class HppMethodSettingForm : Form
    {
        public HppMethodSettingForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;

            Load += HppMethodSettingForm_Load;
            btnSave.Click += btnSave_Click;
            btnClose.Click += (s, e) => Close();
        }

        private void HppMethodSettingForm_Load(object sender, EventArgs e)
        {
            string current = GetCurrentDefaultMethod();
            SelectRadio(current);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string method = GetSelectedMethod();
            if (string.IsNullOrWhiteSpace(method)) method = "FIFO";

            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand("UPDATE settingtoko SET default_hpp_method = @m WHERE id = 1", con);
            cmd.Parameters.AddWithValue("@m", method);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Default HPP method berhasil disimpan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private static string GetCurrentDefaultMethod()
        {
            using var con = new NpgsqlConnection(DbConfig.ConnectionString);
            con.Open();
            using var cmd = new NpgsqlCommand("SELECT COALESCE(NULLIF(default_hpp_method,''),'FIFO') FROM settingtoko WHERE id = 1 LIMIT 1", con);
            var res = cmd.ExecuteScalar();
            return res != null && res != DBNull.Value ? res.ToString() : "FIFO";
        }

        private void SelectRadio(string method)
        {
            method = (method ?? "FIFO").Trim().ToUpperInvariant();
            rbFifo.Checked = method == "FIFO";
            rbAvg.Checked = method == "AVG";
            rbLifo.Checked = method == "LIFO";
            rbFefo.Checked = method == "FEFO";

            if (!rbFifo.Checked && !rbAvg.Checked && !rbLifo.Checked && !rbFefo.Checked)
                rbFifo.Checked = true;
        }

        private string GetSelectedMethod()
        {
            if (rbAvg.Checked) return "AVG";
            if (rbLifo.Checked) return "LIFO";
            if (rbFefo.Checked) return "FEFO";
            return "FIFO";
        }
    }
}

