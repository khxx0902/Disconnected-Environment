using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Environment
{
    public partial class Form4 : Form
    {
        private string stringConnection = "data source=LAPTOP-714CJH2U\\KHAIDAR;" + "database=Activity6;User ID=sa;Password=12345";
        private SqlConnection koneksi;

        private void refreshform()
        {
            cbxNama.Enabled = false;
            cbxStatusMahasiswa.Enabled = false;
            cbxTahunMasuk.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxStatusMahasiswa.SelectedIndex = -1;
            cbxTahunMasuk.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = true;
        }
        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        private void dataGridView()
        {
            koneksi.Open();
            string query = "SELECT id_status, nim, status_mahasiswa, tahun_masuk FROM dbo.StatusMahasiswa";
            SqlDataAdapter adapter = new SqlDataAdapter(query, koneksi);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            koneksi.Close();
        } 

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cbNama()
        {
            koneksi.Open();
            string query = "SELECT nama, nim FROM dbo.Mahasiswa WHERE NOT EXISTS(SELECT id_status FROM dbo.StatusMahasiswa WHERE StatusMahasiswa.nim = mahasiswa.nim)";
            SqlCommand command = new SqlCommand(query, koneksi);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string namaMahasiswa = reader["nama"].ToString();
                string nim = reader["nim"].ToString();
                cbxNama.Items.Add(namaMahasiswa);
                cbxNama.ValueMember = nim;
            }
            reader.Close();
            koneksi.Close();
        }

        private void cbTahunMasuk()
        {
            int currentYear = DateTime.Now.Year;
            int startYear = 2010;
            for (int year = startYear; year <= currentYear; year++)
            {
                cbxTahunMasuk.Items.Add(year.ToString());
            }
        }

        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from dbo.Mahasiswa where nama = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxTahunMasuk.Enabled = true;
            cbxNama.Enabled = true;
            cbxStatusMahasiswa.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            btnAdd.Enabled = true;
            btnClear.Enabled = true;
            btnSave.Enabled = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatusMahasiswa.Text;
            string tahunMasuk = cbxTahunMasuk.Text;

            koneksi.Open();

            // Get the maximum id_status value from the table
            string getMaxIdQuery = "SELECT MAX(id_status) FROM dbo.StatusMahasiswa";
            SqlCommand getMaxIdCommand = new SqlCommand(getMaxIdQuery, koneksi);
            object maxIdResult = getMaxIdCommand.ExecuteScalar();
            int newId = (maxIdResult != DBNull.Value) ? Convert.ToInt32(maxIdResult) + 1 : 1;

            string insertQuery = "INSERT INTO dbo.StatusMahasiswa (id_status, nim, status_mahasiswa, tahun_masuk) VALUES (@idStatus, @nim, @statusMahasiswa, @tahunMasuk)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, koneksi);
            insertCommand.Parameters.AddWithValue("@idStatus", newId);
            insertCommand.Parameters.AddWithValue("@nim", nim);
            insertCommand.Parameters.AddWithValue("@statusMahasiswa", statusMahasiswa);
            insertCommand.Parameters.AddWithValue("@tahunMasuk", tahunMasuk);
            insertCommand.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
            dataGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void FormDataStatusMahasiswa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }

        private void cbxNama_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void cbxStatusMahasiswa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxTahunMasuk_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtNIM_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
