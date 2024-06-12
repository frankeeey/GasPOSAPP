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
    public partial class Report : UserControl
    {

        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        
        readonly string mnt = DateTime.Now.ToString("MMMM");
        readonly string yr = DateTime.Now.ToString("yyyy");
        readonly string date1 = DateTime.Now.ToLongDateString();



        double count3;
        int count, count1, count2;


        private void button1_Click(object sender, EventArgs e)
        {
            gunaComboBox1.SelectedItem = mnt;
            guna2TextBox2.Text = yr;
            Loadata();

        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            t.Show("Refresh", button1);
        }

        private void guna2DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.guna2DataGridView1.Rows[e.RowIndex].Cells["Sn"].Value = (e.RowIndex + 1).ToString();  
        }

        readonly ToolTip t = new ToolTip();

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public Report()
        {
            InitializeComponent();
            button1.PerformClick();
        }

        private void gunaComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loadata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Thread trn = new Thread(Truncate);
            trn.Start();
        }

        private void bunifuImageButton7_MouseHover(object sender, EventArgs e)
        {
            t.Show("Print Report", bunifuImageButton7);
        }

        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm = (Form1)this.FindForm();

            try
            {

                // clear previous reports
                Breports.Clear();

                //add logo
                Image img = Image.FromFile(@"images\ng.jpg");
                Breports.AddImage(img, "width = 120px  style = 'float:center'");
                Breports.AddLineBreak();
                Breports.AddLineBreak();
                Breports.AddLineBreak();
                Breports.AddLineBreak();

                Breports.AddString("<div style = 'float: left;margin-top:-50px;'><b style= 'color:#07629B;font-size:30px;'>" + gunaComboBox1.SelectedItem.ToString().ToUpper() + " " + guna2TextBox2.Text.ToUpper() + "  REPORT</div>");

                DataTable header = new DataTable();

                header.Columns.Add("COMPANY");
                header.Columns.Add("FRANKGAS");

                header.Rows.Add(new object[] { "DATE", date1.ToUpper() });
                header.Rows.Add(new object[] { "YEAR", guna2TextBox2.Text.ToUpper()});
                header.Rows.Add(new object[] { "EMAIL", "frankgas@gmail.com" });
                header.Rows.Add(new object[] { "MANAGER", frm.lblname.Text.ToUpper() });

                Breports.AddDataTable(header, "width = 400px border = 2 style = 'float: right '");
                Breports.AddLineBreak();
                Breports.AddLineBreak();
                Breports.AddLineBreak();
                Breports.AddHorizontalRule("border = 2");
                Breports.AddLineBreak();
               
                Breports.AddLineBreak();


                DataTable header1 = new DataTable();

                header1.Columns.Add("TOTAL SALES");
                header1.Columns.Add(lbtt.Text);

                header1.Rows.Add(new object[] { "AVERAGE GAS PRICE", gunaLabel8.Text.ToUpper() });
                header1.Rows.Add(new object[] { "TOTAL GAS SOLD", lbtqs.Text.ToUpper()});
                header1.Rows.Add(new object[] { "TOTAL INCOME", lbta.Text });
                header1.Rows.Add(new object[] { "TOTAL DISCOUNT", lbdisc.Text.ToUpper() });

                Breports.AddDataTable(header1, "width = 400px border = 2   ");
                Breports.AddLineBreak();

                DataTable header3 = new DataTable();
                header3.Columns.Add("CASH: ");
                header3.Columns.Add(lblcash.Text);

                header3.Rows.Add(new object[] { "POS ", lblpos.Text });


                Breports.AddDataTable(header3, "width = 250px border = 2");
                Breports.AddLineBreak();
                double Net;
                Net = (count1 - count2);
                Breports.AddString("<div style = 'float: left;'><b style= 'color:#2E8B57;font-size:15px;'> NET INCOME = " + Net.ToString("C2") + "</div>");
                Breports.AddLineBreak();
                Breports.AddHorizontalRule("border = 2");
                Breports.AddLineBreak();
                Breports.AddLineBreak();


                Breports.ShowPrintPreviewDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Loadata()
        {
           
            string C = "Cash";            
            string P = "POS";
            int Pos, cash;

            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.RowTemplate.Height = 30;





            try
            {

                using (var CON = new MySqlConnection(mysqlcon))
                {
                    CON.Open();

                    using (var command = new MySqlCommand("SELECT COUNT(Medium) FROM Sales WHERE month = @month and year = @year", CON))
                    {
                        command.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command.Parameters.AddWithValue("@year", guna2TextBox2.Text);
                        count = Convert.ToInt32(command.ExecuteScalar());
                        lbtt.Text = count.ToString("N2");

                    }



                    using (var command1 = new MySqlCommand("SELECT IFNULL(SUM(Amount),0) FROM Sales WHERE month = @month and year = @year", CON))
                    {
                        command1.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command1.Parameters.AddWithValue("@year", guna2TextBox2.Text);
                        count1 = Convert.ToInt32(command1.ExecuteScalar());
                        lbta.Text = count1.ToString("C2");


                    }

                    using (var command2 = new MySqlCommand("SELECT IFNULL(SUM(kgpurchased),0) FROM Sales WHERE month = @month and year = @year", CON))
                    {
                        command2.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command2.Parameters.AddWithValue("@year", guna2TextBox2.Text);
                        count3 = Convert.ToDouble(command2.ExecuteScalar());
                        double bottle = count3 / 12.5;
                        lbtqs.Text = count3.ToString("N2")+ "Kg";


                    }

                    using (var command3 = new MySqlCommand("SELECT IFNULL(SUM(discount),0) FROM Sales WHERE Month = @month and Year = @year", CON))
                    {
                        command3.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command3.Parameters.AddWithValue("@year", guna2TextBox2.Text);
                        count2 = Convert.ToInt32(command3.ExecuteScalar());
                        lbdisc.Text = count2.ToString("C2");


                    }


                    //  POS
                    using (var command4 = new MySqlCommand("SELECT IFNULL(SUM(amount),0) FROM Sales WHERE Month = @month and Year = @year and  Medium = @medium", CON))
                    {
                        command4.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command4.Parameters.AddWithValue("@year", guna2TextBox2.Text);
                        command4.Parameters.AddWithValue("@medium", C);
                        cash = Convert.ToInt32(command4.ExecuteScalar());
                        lblcash.Text = cash.ToString("C2");


                    }


                    using (var command5 = new MySqlCommand("SELECT IFNULL(SUM(amount),0) FROM Sales WHERE Month = @month and Year = @year and Medium = @med", CON))
                    {
                        command5.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command5.Parameters.AddWithValue("@year", guna2TextBox2.Text);
                        command5.Parameters.AddWithValue("@med", P);
                        Pos = Convert.ToInt32(command5.ExecuteScalar());
                        lblpos.Text = Pos.ToString("C2");


                    }
                    double all = count1 / count3;
                    all = Math.Round(all, 2);
                    gunaLabel8.Text = all.ToString("C2");


                    using (var command6 = new MySqlCommand("SELECT* FROM Sales WHERE Month = @month and Year = @year ORDER BY Date DESC" , CON))                   {
                      
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command6);
                        command6.Parameters.AddWithValue("@month", gunaComboBox1.SelectedItem);
                        command6.Parameters.AddWithValue("@year", guna2TextBox2.Text);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            guna2DataGridView1.Rows.Add();

                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Recep"].Value = dt.Rows[i]["invoice"].ToString();                           
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["KG"].Value = dt.Rows[i]["Kgpurchased"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Apkg"].Value = dt.Rows[i]["APKG"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["amt"].Value = dt.Rows[i]["Amount"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["disc"].Value = dt.Rows[i]["Discount"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Medium"].Value = dt.Rows[i]["Medium"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["dt"].Value = dt.Rows[i]["date"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Cashier"].Value = dt.Rows[i]["Attendant"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["tm"].Value = dt.Rows[i]["Time"].ToString();






                        }

                    }


                    CON.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }



        private void Truncate()
        {
            DialogResult D = MessageBox.Show("ARE YOU SURE YOU WANT TO DELETE ALL DATA?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (D == DialogResult.Yes)
            {

                try
                {

                    using (var CON = new MySqlConnection(mysqlcon))
                    {
                        CON.Open();



                        using (var command = new MySqlCommand("Truncate Table refills", CON))
                        {
                            command.ExecuteNonQuery();

                            if (command.ExecuteNonQuery() <= 1)
                            {
                                MessageBox.Show("ERROR", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                using (var command1 = new MySqlCommand("Truncate Table Sales", CON))
                                {
                                    command1.ExecuteNonQuery();
                                    MessageBox.Show("DELETED ALL DATA SUCCESSFULL", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    button1.PerformClick();
                                    button2.Enabled = true;
                                }



                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button2.Enabled = true;


                }
            }
            else
            {
                button2.Enabled = true;
            }

        }













    }
}
