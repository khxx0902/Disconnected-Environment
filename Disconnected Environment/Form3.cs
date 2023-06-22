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
    public partial class Form3 : Form
    {
        private string stringConnection = "data source=LAPTOP-714CJH2U\\KHAIDAR;" + "database=Activity6;User ID=sa;Password=12345";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customerBindingSource = new BindingSource();

        public Form3()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            this.bindingNavigator1.BindingSource = this.customerBindingSource;
            refreshform();
        }

        private void FormDataMahasiswa_Load()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter1 = new SqlDataAdapter(new SqlCommand("Select m.nim, m.nama, m.alamat, m.jenis_kel, m.tgl_lahir, p.id_prodi From dbo.Mahasiswa m join dbo.Prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter1.Fill(ds);

            this.customerBindingSource.DataSource = ds.Tables[0];
            this.txtNim.DataBindings.Add(new Binding("Text", this.customerBindingSource, "NIM", true));
            this.txtNama.DataBindings.Add(new Binding("Text", this.customerBindingSource, "nama", true));
            this.txtAlamat.DataBindings.Add(new Binding("Text", this.customerBindingSource, "alamat", true));
            this.cbxJeniskelamin.DataBindings.Add(new Binding("Text", this.customerBindingSource, "jenis_kel", true));
            this.dtTanggallahir.DataBindings.Add(new Binding("Text", this.customerBindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(new Binding("Text", this.customerBindingSource, "nama", true));
            koneksi.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNim.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTanggallahir.Value = DateTime.Today;
            txtNim.Enabled = true;
            txtNama.Enabled = true;
            cbxJeniskelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggallahir.Enabled = true;
            cbxProdi.Enabled = true;
            Prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNim.Text;
            nama = txtNama.Text;
            jk = cbxJeniskelamin.Text;
            alamat = txtAlamat.Text;
            tgl = dtTanggallahir.Value;
            prodi = cbxProdi.Text;
            string hs = "";
            koneksi.Open();
            string strs = "select id_prodi from dbo.Prodi where nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@dd", prodi));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = dr["id_prodi"].ToString();
            }
            dr.Close();
            string str = "insert into dbo.Mahasiswa (nim, nama, jenis_kel, alamat, tgl_lahir, id_prodi)" + "values(@NIM, @Nm, @Jk, @Al, @Tgll, @Idp)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("Nim", nim));
            cmd.Parameters.Add(new SqlParameter("Nm", nama));
            cmd.Parameters.Add(new SqlParameter("Jk", jk));
            cmd.Parameters.Add(new SqlParameter("AL", alamat));
            cmd.Parameters.Add(new SqlParameter("Tgll", tgl));
            cmd.Parameters.Add(new SqlParameter("Idp", hs));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            refreshform();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void ClearBinding()
        {
            this.txtNim.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJeniskelamin.DataBindings.Clear();
            this.dtTanggallahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
        }

        private void refreshform()
        {
            txtNim.Enabled = false;
            txtNama.Enabled = false;
            cbxJeniskelamin.Enabled = false;
            txtAlamat.Enabled = false;
            dtTanggallahir.Enabled = false;
            cbxProdi.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            ClearBinding();
            FormDataMahasiswa_Load();
        }
       
        private void Form3_Load(object sender, EventArgs e)
        {

        }
        private void Prodicbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }

        private void FormDataMahasiswa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
