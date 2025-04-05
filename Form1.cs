
using Npgsql;
using System.Data;

namespace POS_qu
{
    public partial class Form1 : Form
    {

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

        public Form1()
        {
            InitializeComponent();
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
        private void Form1_Load(object sender, EventArgs e)
        {
            connection();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            DataTable dtgetdata = new DataTable();

            dtgetdata = getData("select * from items");

            dataGridView1.DataSource = dtgetdata;
        }
    }
}
