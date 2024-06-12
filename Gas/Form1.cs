using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace Gas
{
    public partial class Form1 : Form
    {
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        ToolTip T = new ToolTip();
        Refills Rf = new Refills();
        double Sn,Tot;
        string tnk, KG;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            home1.BringToFront();
            home1.button1.PerformClick();

            guna2Button1.FillColor = Color.FromArgb(116, 185, 255);
            guna2Button1.ForeColor = Color.White;
            guna2Button3.ForeColor = Color.FromArgb(99, 110, 114);
            guna2Button3.FillColor = Color.Transparent;
            guna2Button4.ForeColor = Color.FromArgb(99, 110, 114);
            guna2Button4.FillColor = Color.Transparent;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            report2.BringToFront();
            report2.button1.PerformClick();
            guna2Button3.FillColor = Color.FromArgb(116, 185, 255);
            guna2Button3.ForeColor = Color.White;
            guna2Button1.ForeColor = Color.FromArgb(99, 110, 114);
            guna2Button1.FillColor = Color.Transparent;
            guna2Button4.ForeColor = Color.FromArgb(99, 110, 114);
            guna2Button4.FillColor = Color.Transparent;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            staff2.BringToFront();
            staff2.button1.PerformClick();

            guna2Button4.FillColor = Color.FromArgb(116, 185, 255);
            guna2Button4.ForeColor = Color.White;
            guna2Button1.ForeColor = Color.FromArgb(99, 110, 114);
            guna2Button1.FillColor = Color.Transparent;
            guna2Button3.ForeColor = Color.FromArgb(99, 110, 114);
            guna2Button3.FillColor = Color.Transparent;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            home1.button1.PerformClick();
        }

        private void Bnum_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Bnum_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void Bnum_KeyDown(object sender, KeyEventArgs e)
        {
            
            DataTable dt = new DataTable();
            MySqlDataAdapter da;

            if (e.KeyCode == Keys.Enter)
            { 
                if(Bnum.Text == "")
                {
                    MessageBox.Show("InvoiceNo Cannot be Blanc", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        Sales SS = new Sales();
                        SS.FindForm();

                        MySqlConnection CON = new MySqlConnection(mysqlcon);

                        string selectQuery = "SELECT * FROM sales WHERE invoice = @user";

                        CON.Open();
                        MySqlCommand command = new MySqlCommand(selectQuery, CON);
                        command.Parameters.AddWithValue("@user", Bnum.Text);
                        da = new MySqlDataAdapter(command);
                        da.Fill(dt);


                        SS.Bnum.Text = dt.Rows[0][0].ToString();
                        SS.Fullname.Text = dt.Rows[0][1].ToString();
                        SS.guna2DateTimePicker1.Text = dt.Rows[0][3].ToString();
                        SS.bunifuTextBox1.Text = string.Format("{0:0.00}", double.Parse(dt.Rows[0][4].ToString()));
                        SS.gunaComboBox1.Text = dt.Rows[0][5].ToString();
                        SS.txtsubtotal.Text = string.Format(CultureInfo.CreateSpecificCulture("en-NG"), "{0:C}", double.Parse(dt.Rows[0][7].ToString()));
                        SS.txtcash.Text = string.Format(CultureInfo.CreateSpecificCulture("en-NG"), "{0:C}", double.Parse(dt.Rows[0][9].ToString()));
                        SS.txtbal.Text = string.Format(CultureInfo.CreateSpecificCulture("en-NG"), "{0:C}", double.Parse(dt.Rows[0][11].ToString()));
                        SS.gunaButton1.Text = string.Format(CultureInfo.CreateSpecificCulture("en-NG"), "{0:C}", double.Parse(dt.Rows[0][8].ToString()));
                        SS.txtdisc.Text = string.Format(CultureInfo.CreateSpecificCulture("en-NG"), "{0:C}", double.Parse(dt.Rows[0][10].ToString()));


                        SS.Bnum.ReadOnly = true;
                        SS.Fullname.ReadOnly = true;
                        SS.bunifuTextBox1.ReadOnly = true;
                        SS.txtsubtotal.ReadOnly = true;
                        SS.txtcash.ReadOnly = true;
                        SS.txtdisc.ReadOnly = true;
                        SS.txtbal.ReadOnly = true;
                        SS.gunaComboBox1.Enabled = false;
                        SS.gunaButton1.Enabled = false;

                        SS.ShowDialog();
                        Bnum.Clear();




                        CON.Close();
                    }
                    catch
                    {
                        MessageBox.Show("InvoiceNo " + Bnum.Text + " Cannot be Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Bnum.Clear();
                    }
                }
                

            }
        }

        private void home1_Load(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton3_MouseHover(object sender, EventArgs e)
        {
            T.Show("Refills", guna2ImageButton3); 
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
           
            Rf.Name = lblname.Text;
            Details();
            Rf.ShowDialog();
            

        }

        private void Details()
        {
            Rf.guna2DataGridView1.Rows.Clear();
            Rf.guna2DataGridView1.RowTemplate.Height = 30;

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

                            Rf.guna2DataGridView1.Rows.Add();

                            Rf.guna2DataGridView1.Rows[Rf.guna2DataGridView1.Rows.Count - 1].Cells["dt"].Value = dt.Rows[i]["Date"].ToString();
                            Rf.guna2DataGridView1.Rows[Rf.guna2DataGridView1.Rows.Count - 1].Cells["dr"].Value = dt.Rows[i]["KG"].ToString();
                            Rf.guna2DataGridView1.Rows[Rf.guna2DataGridView1.Rows.Count - 1].Cells["comment"].Value = dt.Rows[i]["Comment"].ToString();
                            Rf.guna2DataGridView1.Rows[Rf.guna2DataGridView1.Rows.Count - 1].Cells["mn"].Value = dt.Rows[i]["Name"].ToString();




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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int Ct;

                using (var CON = new MySqlConnection(mysqlcon))
                {

                    CON.Open();

                    using (var command2 = new MySqlCommand("SELECT COUNT(SN) FROM Refills", CON))
                    {

                        Ct = Convert.ToInt32(command2.ExecuteScalar());

                        if (Ct == 0)
                        {
                            gunaLabel3.Text = "0.00KG";
                        }
                        else
                        {
                            using (var command1 = new MySqlCommand("SELECT* FROM Refills ORDER BY SN DESC LIMIT 1", CON))
                            {
                                MySqlDataAdapter adapter = new MySqlDataAdapter(command1);

                                DataTable dt = new DataTable();
                                adapter.Fill(dt);
                                string rw = dt.Rows[0][6].ToString();
                                tnk = dt.Rows[0][0].ToString();
                                KG = dt.Rows[0][1].ToString();


                                if (rw == "0")
                                {
                                    gunaLabel3.Text = KG + "KG";

                                    if ((double.Parse(KG)) <= 100)
                                    {
                                        gunaLabel3.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        gunaLabel3.ForeColor = Color.SeaGreen;
                                    }

                                }
                                else
                                {
                                    using (var command = new MySqlCommand("SELECT IFNULL(SUM(Kgpurchased),0) FROM Sales WHERE tank = @tank", CON))
                                    {
                                        command.Parameters.AddWithValue("@tank", tnk);
                                        Tot = Convert.ToDouble(command.ExecuteScalar());
                                        gunaLabel3.Text = (double.Parse(KG) - Tot).ToString() + "KG";


                                        if ((double.Parse(KG) - Tot) <= 100)
                                        {
                                            gunaLabel3.ForeColor = Color.Red;
                                        }
                                        else
                                        {
                                            gunaLabel3.ForeColor = Color.SeaGreen;
                                        }


                                    }
                                }



                            }

                        }


                    }



                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
