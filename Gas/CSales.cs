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
using System.Globalization;
using BarcodeLib;

namespace Gas
{
    public partial class CSales : Form
    {
        private double I, B, disctot,fin,count1,count2;
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        string sn;
        
        int no;        
        new string Name;
        public int price;
        public string Cashier;
        readonly Random r = new Random();
        readonly Random v = new Random();
        string mgr;




        private void txtsubtotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                I = float.Parse(txtsubtotal.Text);

                B = I / price;
                B = Math.Round(B, 2);
                kg.Text = B.ToString();
                Calculate();
                txtdisc.Text = disctot.ToString();
            }
            catch
            {

            }

            

        }

        private void kg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    I = float.Parse(kg.Text);
                    B = I * price;
                    txtsubtotal.Text = B.ToString();


                }
                catch
                {

                }
            }
            
        }

        private void kg_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Calculate();
               
                
            }
            catch
            {

            }
            
        }

        private void txtdisc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Calculate();

            }
            catch
            {

            }
           

        }

        double dis, Sub, td;

        private void CSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Calculate()

        {
            if (txtcash.Text == "")
            {
                txtcash.Text = "0.00";
            }
            
            else if (txtdisc.Text == "")
            {
                txtdisc.Text = "0.00";
            }
           
           

            dis = double.Parse(txtcash.Text);
            Sub = double.Parse(txtsubtotal.Text);
            td = double.Parse(txtdisc.Text);

            if (txtsubtotal.Text == "")
            {
                Sub = 0;
            }



            disctot = Sub - dis;
            
            gunaButton1.Text = disctot.ToString("C2");
            fin = (td - disctot);
            txtbal.Text = fin.ToString("N2");
           


        }


        public CSales()
        {
            InitializeComponent();
            Invoice();
            
            
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            Mgr();
                DialogResult D = MessageBox.Show("Are Sure All Data Is Correct?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (D == DialogResult.Yes)
                {

                    Submit();
                }

            

           
        }

        public void Mgr()
        {
            List<string> lst = new List<string>();
            try
            {
                DataTable dt = new DataTable();
                MySqlDataAdapter da;
                string A = "Manager";
                using (var CON = new MySqlConnection(mysqlcon))
                {
                    string Log = "select * from login where position = @man ";
                    CON.Open();
                    MySqlCommand cmd = new MySqlCommand(Log, CON);
                    cmd.Parameters.AddWithValue("@man", A);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lst.Add(reader["Mobile"].ToString());
                    }
                    string[] num = lst.ToArray();
                    mgr = string.Join(", ", num);
                    


                    CON.Close();
                }

           
            }
            catch
            {
               
            }
        }

        public void Invoice()
        {
            no = r.Next(10000, 5000000);
            String[] random = new String[7] { "ED", "BS", "EG", "MM", "MB","IO","GC" };
            string Sn = random[v.Next(7)] + no;
            Bnum.Text = Sn;

            try
            {
                using (var CON = new MySqlConnection(mysqlcon))
                {
                    CON.Open();

                    using (var command1 = new MySqlCommand("SELECT* FROM Sales where Invoice = @batch", CON))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command1);
                        command1.Parameters.AddWithValue("@batch", Bnum.Text);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Invoice();

                        }


                    }
                }
            }
            catch
            {

            }

        }


        private void Submit()
        {
            string datenow = DateTime.Now.ToString("dddd,dd MMMM yyyy");
            string Tm = DateTime.Now.ToString("hh:mm tt");
            string dy = DateTime.Now.ToString("dd");
            string mnt = DateTime.Now.ToString("MMMM");
            string yr = DateTime.Now.ToString("yyyy");
           

            Name = Fullname.Text;

            if (Fullname.Text == "")
            {
                Name = "Customer";
            }
            else
            {
                Name = Fullname.Text;
            }
                       
                                      
           

            try
            {
                if (kg.Text == "" || gunaComboBox1.Text == "" || txtdisc.Text == "" )

                {
                    MessageBox.Show("Please Fill the Contents Correctly ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    

                }
               
                else
                {                  

                    // report generation
                    Breports.Clear();

                    Image img = Image.FromFile(@"images\ng.jpg");
                    Breports.AddImage(img, "width = 80px  style = 'float:center'");
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();

                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:15px;'> Invoice No: " + Bnum.Text + " </div>");
                    Barcode BAR = new Barcode();
                    Image being = BAR.Encode(TYPE.CODE93, Bnum.Text);
                    Breports.AddImage(being, "width = 120px  style = 'float:right'");
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;'><b style= 'font-size:14px;'> FRANKGAS LPG PLANT... </div>");
                    Breports.AddLineBreak();                    
                    Breports.AddString("<p><div  style = 'float: left;'><b style= 'color:#2E8B57;font-size:14px;'>PHONE: " + mgr + "</div></p>");
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'>Date Entered  ; " + guna2DateTimePicker1.Text + " "+ Tm + " </div>");
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'> Cashier   : " + Cashier.ToUpper() + " </div>");
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'> Customer Name  : " + Name.ToUpper() + " </div>");
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();

                    DataTable header = new DataTable();

                    header.Columns.Add("Product Name:");
                    header.Columns.Add("QTY");
                    header.Columns.Add("Price Per Kg");
                    header.Columns.Add("Medium");

                    header.Rows.Add(new object[] { "Cooking Gas",kg.Text +"Kg",price.ToString("N2"),gunaComboBox1.SelectedItem.ToString()});
                    

                    Breports.AddDataTable(header, "width = 800px border = 1");
                   
                    Breports.AddHorizontalRule("border = 1");
                    Breports.AddLineBreak();
                   
                    DataTable header1 = new DataTable();

                    header1.Columns.Add("SUB TOTAL");
                    header1.Columns.Add(txtsubtotal.Text);
                    header1.Rows.Add(new object[] { "DISCOUNT",double.Parse(txtcash.Text)});
                    header1.Rows.Add(new object[] { "TENDERED", txtdisc.Text });
                    header1.Rows.Add(new object[] { "BALANCE", txtbal.Text });
                    header1.Rows.Add(new object[] { "TOTAL", gunaButton1.Text});
                    Breports.AddDataTable(header1, "width = 250px border = 1");
                    Breports.AddLineBreak();
                    Breports.AddHorizontalRule("border = 1");
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-15px;'><b style= 'color:#07629B;font-size:13px;'>THANK YOU AND PLEASE COME AGAIN!!! </div>");
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'> Powered by Frankcorp</div>");
                    Breports.AddLineBreak();

                    Breports.ShowPrintPreviewDialog();

                    using (var CON = new MySqlConnection(mysqlcon))
                    {
                        
                        CON.Open();


                        using (var command1 = new MySqlCommand("SELECT* FROM Refills ORDER BY SN DESC LIMIT 1", CON))
                        {
                            MySqlDataAdapter adapter = new MySqlDataAdapter(command1);

                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            string rw = dt.Rows[0][6].ToString();
                            

                            if (rw == "0")
                            {                               
                                MySqlCommand command2;
                                string updatequery = "UPDATE Refills SET point = @pt where point = @rw";
                                command2 = new MySqlCommand(updatequery, CON);
                                command2.Parameters.AddWithValue("@pt", "1");
                                command2.Parameters.AddWithValue("@rw", rw);
                                command2.ExecuteNonQuery();
                                sn = dt.Rows[0][0].ToString();

                            }
                            else
                            {
                                using (var command3 = new MySqlCommand("SELECT  IFNULL ((Tank),0) FROM Sales ORDER BY SN DESC LIMIT 1", CON))
                                {

                                    sn = dt.Rows[0][0].ToString();


                                }
                            }
                                                  


                        }

                       

                        string insert = "INSERT INTO sales(invoice,fullname,attendant,date,kgpurchased,medium,time,Amount,total,discount,tendered,balance,APKG,day,month,year,tank)" +
                       "VALUES(@invoice,@fullname,@attendant,@date,@kg,@medium,@time,@amount,@total,@discount,@tend,@bal,@apkg,@day,@month,@year,@tank)";

                        using (var command = new MySqlCommand(insert, CON))
                        {
                            command.Parameters.AddWithValue("@invoice", Bnum.Text);
                            command.Parameters.AddWithValue("@fullname", Name);
                            command.Parameters.AddWithValue("@attendant", Cashier);
                            command.Parameters.AddWithValue("@date", guna2DateTimePicker1.Text);
                            command.Parameters.AddWithValue("@kg", kg.Text);
                            command.Parameters.AddWithValue("@medium", gunaComboBox1.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@time", Tm);
                            command.Parameters.AddWithValue("@amount", Sub);
                            command.Parameters.AddWithValue("@total", disctot);
                            command.Parameters.AddWithValue("@discount", dis);
                            command.Parameters.AddWithValue("@tend", td);
                            command.Parameters.AddWithValue("@bal", fin);
                            command.Parameters.AddWithValue("@apkg", price);
                            command.Parameters.AddWithValue("@day", dy);
                            command.Parameters.AddWithValue("@month", mnt);
                            command.Parameters.AddWithValue("@year", yr);
                            command.Parameters.AddWithValue("@tank", sn);


                            if (command.ExecuteNonQuery() == 1)
                            {
                                Breports.ShowPrintPreviewDialog();
                                Invoice();
                                Fullname.Clear();
                                gunaComboBox1.Text = null;
                                kg.Text = "";
                                guna2DateTimePicker1.ResetText();
                                gunaButton1.Text = "₦0.00";
                                txtbal.Clear();
                                txtsubtotal.Clear();
                                txtdisc.Clear();
                                txtcash.Clear();




                            }

                            else
                            {
                                MessageBox.Show("Please Fill the Contents Correctly  ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }
                        
                            CON.Close();
                    }
                                  

                                   
                    

                    

                }



            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);            

            }
        }









    }
}
