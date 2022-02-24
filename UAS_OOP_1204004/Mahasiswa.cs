using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UAS_OOP_1204004
{
    public partial class Mahasiswa : Form
    {

        private MySqlCommand cmd;
        private DataSet ds;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;

        private string random()
        {
            long npm;
            string urutan;
            db database = new db();
            MySqlConnection conn = database.GetConn();
            conn.Open();
            cmd = new MySqlCommand("select npm from ms_mhs where npm in(select max(npm) from ms_mhs) order by npm desc", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                npm = Convert.ToInt64(rd[0].ToString().Substring(rd["npm"].ToString().Length - 3, 3)) + 1;
                string joinstr = "000" + npm;
                urutan = "120" + joinstr.Substring(joinstr.Length - 3, 3);

            }
            else
            {
                urutan = "120001";
            }
            rd.Close();
            conn.Close();

            return urutan;
        }

        private void get_prodi_list()
        {
            db database = new db();
            MySqlDataReader rd;
            MySqlConnection conn = database.GetConn();
            conn.Open();

            cmd = new MySqlCommand("select * from ms_prodi", conn);
            cmd.CommandType = CommandType.Text;
            rd = cmd.ExecuteReader();

            while (rd.Read())
            {

                comboBox1.Items.Add(rd[0].ToString());

            }
            rd.Close();
            conn.Close();

        }

        private void clear_textbox()
        {
            textBox1.Text = "";
            textBox3.Text = "";
            comboBox1.ResetText();
        }

        private void set_dataset()
        {
            string url = "Server=localhost;Database=UAS;Uid=root;Pwd=;";
            MySqlConnection conn = new MySqlConnection(url);
            conn.Open();
            cmd = new MySqlCommand("SELECT * FROM ms_mhs", conn);
            ds = new DataSet();
            da = new MySqlDataAdapter(cmd);

            da.Fill(ds, "ms_mhs");
            ds_mhs.DataSource = ds;
            ds_mhs.DataMember = "ms_mhs";
            ds_mhs.AllowUserToAddRows = false;
            ds_mhs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ds_mhs.Refresh();
        }

        public Mahasiswa()
        {
            InitializeComponent();
            set_dataset();
            get_prodi_list();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            set_dataset();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Pastikan semua terisi");
            }
            else
            {
                db database = new db();
                MySqlConnection conn = database.GetConn();
                cmd = new MySqlCommand("insert into ms_mhs values('" + random() + "','" + textBox1.Text + "','" + comboBox1.Text + "')", conn);
                conn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Data berhasil ditambah");
                clear_textbox();
                set_dataset();
            }
        }

        private void ds_mhs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.ds_mhs.Rows[e.RowIndex];
                textBox1.Text = row.Cells["nama_mhs"].Value.ToString();
                comboBox1.SelectedText = row.Cells["kode_prodi"].Value.ToString();
                //comboBox1.Text = row.Cells["kode_prodi"].Value.ToString();
                textBox3.Text = row.Cells["npm"].Value.ToString();
            }
            catch (Exception X)
            {
                MessageBox.Show(X.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || comboBox1.Text.Trim() == "")
            {
                MessageBox.Show("Pastikan semua terisi");
            }
            else
            {
                db database = new db();
                MySqlConnection conn = database.GetConn();

                cmd = new MySqlCommand("Update ms_mhs set nama_mhs='" + textBox1.Text + "',kode_prodi='" + comboBox1.Text + "' where npm='" + textBox3.Text + "'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil di Ganti");
                clear_textbox();
                set_dataset();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("APAKAH YAKIN AKAN MENGHAPUS DATA INI " + "??", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db database = new db();
                MySqlConnection conn = database.GetConn();

                cmd = new MySqlCommand("DELETE from ms_mhs where npm='" + textBox3.Text + "'", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data berhasil di Hapus");
                set_dataset();
            }
        }
    }
}
