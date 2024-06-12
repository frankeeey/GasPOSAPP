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
using System.Threading;

namespace Gas
{
    public partial class Staff : UserControl
    {
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        readonly ToolTip t = new ToolTip();
        public Staff()
        {
            InitializeComponent();
            Details();
        }

        private void guna2DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.guna2DataGridView1.Rows[e.RowIndex].Cells["sn"].Value = (e.RowIndex + 1).ToString();
        }

      

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button2.Text = "Adding...";



            DialogResult D = MessageBox.Show("Are Sure All Data Is Correct?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (D == DialogResult.Yes)
            {
                Thread sub = new Thread(Submit);
                sub.Start();

            }
            else
            {

                guna2Button2.Text = "Add User";
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

                    using (var command = new MySqlCommand("SELECT* FROM login", CON))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            guna2DataGridView1.Rows.Add();

                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["fname"].Value = dt.Rows[i]["Fullname"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["DOR"].Value = dt.Rows[i]["Dor"].ToString();                         
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Role"].Value = dt.Rows[i]["Position"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Lseen"].Value = dt.Rows[i]["lastseen"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["User"].Value = dt.Rows[i]["username"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Pass"].Value = dt.Rows[i]["password"].ToString();





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

















        private void username_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (var CON = new MySqlConnection(mysqlcon))
                {
                    CON.Open();

                    using (var command1 = new MySqlCommand("SELECT* FROM login where username = @username", CON))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command1);
                        command1.Parameters.AddWithValue("@username", username.Text);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            pictureBox1.Visible = true;
                        }
                        else
                        {
                            pictureBox1.Visible = false;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Details();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string a = guna2DataGridView1.CurrentRow.Cells[1].Value.ToString();
            string b = guna2DataGridView1.CurrentRow.Cells[5].Value.ToString();

            if (e.ColumnIndex == guna2DataGridView1.Columns["Del"].Index)
            {
                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("Are You Sure You Want To Fire " + a + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        MySqlConnection CON = new MySqlConnection(mysqlcon);

                        string Delete = "Delete From login where Username = @username";

                        CON.Open();                        
                        MySqlCommand command = new MySqlCommand(Delete, CON);
                        command.Parameters.AddWithValue("@username", b);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show(a + " Has Successfully Been Fired!!!");
                            button1.PerformClick();



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


        private void Submit()
        {
            try
            {
                if (fullname.Text == "" || gunaComboBox1.Text == "" || username.Text == "" || password.Text == "" || mobile.Text == "")

                {
                    MessageBox.Show("Please Fill the Contents Correctly ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2Button2.Text = "Add User";

                }
                else if (pictureBox1.Visible == true)
                {
                    MessageBox.Show("Invalid Username ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2Button2.Text = "Add User";
                }

                else
                {
                    MySqlConnection CON = new MySqlConnection(mysqlcon);


                    string insert = "INSERT INTO login(fullname,position,mobile,dor,username,password)" +
                        "VALUES('" + fullname.Text + "','" + gunaComboBox1.SelectedItem.ToString() + "','" + mobile.Text + "','" + guna2DateTimePicker1.Value + "','" + username.Text + "','" + password.Text + "')";

                    CON.Open();
                    MySqlCommand command = new MySqlCommand(insert, CON);

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("STAFF " + fullname.Text.ToUpper() + " HAS SUCCESSFULLY BEEN REGISTERED ", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        username.Text = "";
                        password.Text = "";
                        fullname.Text = "";
                        gunaComboBox1.Text = null;
                        mobile.Text = "";
                        guna2Button2.Text = "Add User";
                        button1.PerformClick();


                    }

                    else
                    {
                        MessageBox.Show("Please Fill the Contents Correctly  ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        guna2Button2.Text = "Add User";
                    }


                    CON.Close();

                }



            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                guna2Button2.Text = "Add User";



            }

        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            t.Show("Refresh", button1);
        }

        private void mobile_TextChanged_1(object sender, EventArgs e)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(mobile.Text, "[^0-9]"))
            {
                mobile.Text = mobile.Text.Remove(mobile.Text.Length - 1);

            }
            mobile.MaxLength = 11;

        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(guna2DataGridView1.Columns[e.ColumnIndex].Index == 6 && e.Value != null)
            {
                guna2DataGridView1.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new string('*', e.Value.ToString().Length);
            }
        }
    }
}
