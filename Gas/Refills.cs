using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;

namespace Gas
{
    public partial class Refills : Form
    {
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        public string Nmae;

        public Refills()
        {
            InitializeComponent();
        }

        private void Refills_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fullname_TextChanged(object sender, EventArgs e)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(fullname.Text, "[^0-9]"))
            {
                fullname.Text = fullname.Text.Remove(fullname.Text.Length - 1);

            }
            fullname.MaxLength = 11;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button2.Text = "Submitting...";
            DialogResult D = MessageBox.Show("Are Sure All Data Is Correct?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (D == DialogResult.Yes)
            {
                Submit();

            }
            else
            {

                guna2Button2.Text = "Submit";
            }
        }

        private void Submit()
        {
            string yr = DateTime.Now.ToString("yyyy");

            try
            {
                if (fullname.Text == "" || gunaComboBox1.Text == "" )

                {
                    MessageBox.Show("Please Fill the Contents Correctly ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2Button2.Text = "Submit";

                }
                

                else
                {
                    MySqlConnection CON = new MySqlConnection(mysqlcon);


                    string insert = "INSERT INTO refills(Kg,Comment,Date,Name,Year)" +
                        "VALUES(@kg,@cmmnt,@dt,@nm,@yr)";

                    CON.Open();
                    MySqlCommand command = new MySqlCommand(insert, CON);
                    command.Parameters.AddWithValue("@kg", fullname.Text);
                    command.Parameters.AddWithValue("@cmmnt", gunaComboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@dt", guna2DateTimePicker1.Text);
                    command.Parameters.AddWithValue("@nm", Name);
                    command.Parameters.AddWithValue("@yr", yr);
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("DATABASE UPDATED SUCCESSFULLY", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Details();
                        fullname.Text = "";
                        gunaComboBox1.Text = null;
                        guna2Button2.Text = "Submit";
                       


                    }

                    else
                    {
                        MessageBox.Show("Please Fill the Contents Correctly  ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        guna2Button2.Text = "Submit";
                    }


                    CON.Close();

                }



            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                guna2Button2.Text = "Submit";



            }

        }


        private void Details()
        {
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.RowTemplate.Height = 30;

            try
            {
                using (var CON = new MySqlConnection(mysqlcon))
                {
                    CON.Open();

                    using (var command = new MySqlCommand("SELECT* FROM refills", CON))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            guna2DataGridView1.Rows.Add();

                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["dt"].Value = dt.Rows[i]["Date"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["dr"].Value = dt.Rows[i]["KG"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["comment"].Value = dt.Rows[i]["Comment"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["mn"].Value = dt.Rows[i]["Name"].ToString();
                           



                        }

                    }
                    CON.Close();
                }

            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }
        }

        private void guna2DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.guna2DataGridView1.Rows[e.RowIndex].Cells["Sn"].Value = (e.RowIndex + 1).ToString();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string a = guna2DataGridView1.CurrentRow.Cells[1].Value.ToString();
            string b = guna2DataGridView1.CurrentRow.Cells[5].Value.ToString();

            if (e.ColumnIndex == guna2DataGridView1.Columns["Del"].Index)
            {
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("Are You Sure You Want To Delete Record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        MySqlConnection CON = new MySqlConnection(mysqlcon);

                        string Delete = "Delete From refills where Date = @Dt";

                        CON.Open();
                        MySqlCommand command = new MySqlCommand(Delete, CON);
                        command.Parameters.AddWithValue("@Dt", a);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show(" Record Has Been Successfully Deleted", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Details();



                        }
                        CON.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }

            }
        }
    }
}
