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
using System.Globalization;


namespace Gas
{
    public partial class SalesHome : UserControl
    {
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        double TT, TTP;
        int total;        
        readonly string yr = DateTime.Now.ToString("yyyy");
        double count3;
        int count, count1, count2;
        readonly ToolTip t = new ToolTip();
        


        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            SalesMain frm = new SalesMain();
            frm = (SalesMain)this.FindForm();

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

                Breports.AddString("<div style = 'float: left;margin-top:-50px;'><b style= 'color:#07629B;font-size:30px;'>" + dpicker.Value.ToLongDateString().ToUpper()  + "  REPORT</div>");

                DataTable header = new DataTable();

                header.Columns.Add("COMPANY");
                header.Columns.Add("FRANKGAS");

                header.Rows.Add(new object[] { "DATE", dpicker.Value.ToLongDateString().ToUpper() });
                header.Rows.Add(new object[] { "YEAR", yr});               
                header.Rows.Add(new object[] { "CASHIER", frm.lblname.Text.ToUpper() });

                Breports.AddDataTable(header, "width = 400px border = 2 style = 'float: right '");
                Breports.AddLineBreak();
                Breports.AddLineBreak();
                Breports.AddLineBreak();
                Breports.AddHorizontalRule("border = 2");
                Breports.AddLineBreak();

                Breports.AddLineBreak();


                DataTable header1 = new DataTable();

                header1.Columns.Add("TOTAL SALES");
                header1.Columns.Add(lbts.Text);

                header1.Rows.Add(new object[] { "AVERAGE GAS PRICE", gunaLabel16.Text.ToUpper() });
                header1.Rows.Add(new object[] { "TOTAL GAS SOLD", lbtkg.Text.ToUpper()});
                header1.Rows.Add(new object[] { "TOTAL INCOME", lbte.Text });
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

        private void button1_Click(object sender, EventArgs e)
        {
            dpicker.ResetText();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string A = guna2DataGridView1.CurrentRow.Cells[2].Value.ToString();
            DataTable dt = new DataTable();
            MySqlDataAdapter da;
           


            if (e.ColumnIndex == guna2DataGridView1.Columns["Edit"].Index)
            {
                
                    try
                    {
                    Sales SS = new Sales();
                    SS.FindForm();

                    MySqlConnection CON = new MySqlConnection(mysqlcon);

                    string selectQuery = "SELECT * FROM sales WHERE invoice = @user";

                    CON.Open();
                        MySqlCommand command = new MySqlCommand(selectQuery, CON);
                        command.Parameters.AddWithValue("@user", A);
                    da = new MySqlDataAdapter(command);
                    da.Fill(dt);
                    SS.Bnum.Text = dt.Rows[0][0].ToString();
                    SS.Tm = dt.Rows[0][6].ToString();
                    SS.Cashier = dt.Rows[0][2].ToString();
                    SS.price = double.Parse(dt.Rows[0][12].ToString());                   
                    SS.Fullname.Text = dt.Rows[0][1].ToString();
                    SS.guna2DateTimePicker1.Text = dt.Rows[0][3].ToString();                   
                    SS.bunifuTextBox1.Text = string.Format("{0:0.00}",double.Parse(dt.Rows[0][4].ToString()));
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
                    SS.ShowDialog();



                    CON.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                

            }           

        
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            t.Show("Refresh", button1);
        }

        private void bunifuImageButton7_MouseHover(object sender, EventArgs e)
        {
            t.Show("Print Report", bunifuImageButton7);
        }

        private void dpicker_ValueChanged(object sender, EventArgs e)
        {
            Loadata();
        }

        public SalesHome()
        {
            InitializeComponent();
        }

        private void guna2DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.guna2DataGridView1.Rows[e.RowIndex].Cells["Sn"].Value = (e.RowIndex + 1).ToString();
        }


        private void Loadata()
        {
            SalesMain mn = new SalesMain();
            mn = (SalesMain)this.FindForm();
            string user = mn.lblname.Text;

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

                    using (var command = new MySqlCommand("SELECT COUNT(Medium) FROM Sales WHERE Date = @date and Attendant = @atd ", CON))
                    {
                        command.Parameters.AddWithValue("@date", dpicker.Text);
                        command.Parameters.AddWithValue("@atd", user);
                        count = Convert.ToInt32(command.ExecuteScalar());
                        lbts.Text = count.ToString("N2");

                    }



                    using (var command1 = new MySqlCommand("SELECT IFNULL(SUM(Amount),0) FROM Sales WHERE Date = @date and Attendant = @atd ", CON))
                    {
                        command1.Parameters.AddWithValue("@date", dpicker.Text);
                        command1.Parameters.AddWithValue("@atd", user);
                        count1 = Convert.ToInt32(command1.ExecuteScalar());
                        lbte.Text = count1.ToString("C2");


                    }

                    using (var command2 = new MySqlCommand("SELECT IFNULL(SUM(kgpurchased),0) FROM Sales WHERE Date = @date and Attendant = @atd ", CON))
                    {
                        command2.Parameters.AddWithValue("@date", dpicker.Text);
                        command2.Parameters.AddWithValue("@atd", user);
                        count3 = Convert.ToDouble(command2.ExecuteScalar());
                        double bottle = count3 / 12.5;
                        lbtkg.Text = count3.ToString("N2") + "Kg";


                    }

                    using (var command3 = new MySqlCommand("SELECT IFNULL(SUM(discount),0) FROM Sales WHERE Date = @date and Attendant = @atd ", CON))
                    {
                        command3.Parameters.AddWithValue("@date", dpicker.Text);
                        command3.Parameters.AddWithValue("@atd", user);
                        count2 = Convert.ToInt32(command3.ExecuteScalar());
                        lbdisc.Text = count2.ToString("C2");


                    }

                    double all = count1 / count3;
                    all = Math.Round(all, 2);
                    gunaLabel16.Text = all.ToString("C2");




                    //  POS
                    using (var command4 = new MySqlCommand("SELECT IFNULL(SUM(amount),0) FROM Sales WHERE Date = @date  and  Medium = @medium and Attendant = @atd", CON))
                    {
                        command4.Parameters.AddWithValue("@date", dpicker.Text);
                        command4.Parameters.AddWithValue("@medium", C);
                        command4.Parameters.AddWithValue("@atd", user);
                        cash = Convert.ToInt32(command4.ExecuteScalar());
                        lblcash.Text = cash.ToString("C2");


                    }


                    using (var command5 = new MySqlCommand("SELECT IFNULL(SUM(amount),0) FROM Sales WHERE Date = @date  and  Medium = @medium  and Attendant = @atd", CON))
                    {
                        command5.Parameters.AddWithValue("@date", dpicker.Text);
                        command5.Parameters.AddWithValue("@medium", P);
                        command5.Parameters.AddWithValue("@atd", user);
                        Pos = Convert.ToInt32(command5.ExecuteScalar());
                        lblpos.Text = Pos.ToString("C2");


                    }

                    total = cash + Pos;

                    if (total == 0)
                    {
                        TT = 0;
                        cashprogress.Value = Convert.ToInt32(TT);
                        posprogress.Value = Convert.ToInt32(TT);
                        Circleguna.Value = Convert.ToInt32(TT);



                    }
                    else
                    {
                        TT = (cash * 100) / total;
                        cashprogress.Value = Convert.ToInt32(TT);
                        Circleguna.Value = Convert.ToInt32(TT);

                        TTP = (Pos * 100) / total;
                        posprogress.Value = Convert.ToInt32(TTP);



                    }


                    using (var command6 = new MySqlCommand("SELECT* FROM Sales WHERE Date = @date  and Attendant = @atd", CON))
                    {

                        MySqlDataAdapter adapter = new MySqlDataAdapter(command6);
                        command6.Parameters.AddWithValue("@date", dpicker.Text);
                        command6.Parameters.AddWithValue("@atd", user);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            guna2DataGridView1.Rows.Add();

                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Recep"].Value = dt.Rows[i]["invoice"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["customer"].Value = dt.Rows[i]["Fullname"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["KG"].Value = dt.Rows[i]["Kgpurchased"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Apkg"].Value = dt.Rows[i]["APKG"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["amt"].Value = dt.Rows[i]["Amount"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["disc"].Value = dt.Rows[i]["Discount"].ToString();
                            guna2DataGridView1.Rows[guna2DataGridView1.Rows.Count - 1].Cells["Medium"].Value = dt.Rows[i]["Medium"].ToString();
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















    }
}
