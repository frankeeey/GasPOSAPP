using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarcodeLib;
using MySql.Data.MySqlClient;


namespace Gas
{
    public partial class Sales : Form
    {
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";

        public string Tm, Cashier;
        public double price;
        public bool Start;

        public Sales()
        {
            InitializeComponent();
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        new void Update()
        {
            try
            {
                if (bunifuTextBox1.Text == "" || gunaComboBox1.Text == "" || txtdisc.Text == "" ||Fullname.Text == "" || txtsubtotal.Text == "")

                {
                    MessageBox.Show("Please Fill the Contents Correctly ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);


                }
                else
                {
                    
                    MySqlConnection CON = new MySqlConnection(mysqlcon);
                    MySqlCommand command = new MySqlCommand();

                    string updatequery = "UPDATE sales SET Fullname = @full ,kgpurchased = @kg, medium = @medium, amount = @amt, Total = @total, discount = @disc, Tendered = @tend,balance = @bal  where invoice  = @inv";

                    CON.Open();

                    command = new MySqlCommand(updatequery, CON);
                    command.Parameters.AddWithValue("@full", Fullname.Text);
                    command.Parameters.AddWithValue("@kg", bunifuTextBox1.Text);
                    command.Parameters.AddWithValue("@medium", gunaComboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@amt", txtsubtotal.Text);
                    command.Parameters.AddWithValue("@total", gunaButton1.Text);
                    command.Parameters.AddWithValue("@disc", txtcash.Text);
                    command.Parameters.AddWithValue("@tend", txtdisc.Text);
                    command.Parameters.AddWithValue("@bal", txtbal.Text);
                    command.Parameters.AddWithValue("@inv", Bnum.Text);
                    command.ExecuteNonQuery();

                    if (command.ExecuteNonQuery() == 1)
                    {


                        MessageBox.Show("DATABASE UPDATED SUCCESSFULLY", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                       
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            if(Start == true)
            {

                DialogResult D = MessageBox.Show("Are Sure All Data Is Correct To Be Updated?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (D == DialogResult.Yes)
                {

                    Update();
                }

            }
            else
            {
                DialogResult D = MessageBox.Show("Are Sure You Want To Reprint ?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (D == DialogResult.Yes)
                {

                    Submit();
                }
            }
           
        }

        private void txtsubtotal_TextChanged(object sender, EventArgs e)
        {
            gunaButton1.Text = txtsubtotal.Text;
        }

        private void Submit()
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
                    Breports.AddString("<p><div  style = 'float: left;'><b style= 'color:#2E8B57;font-size:14px;'>PHONE: 08115544044, 08115544044</div></p>");
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'>Date Entered : " + guna2DateTimePicker1.Text + " " + Tm + " </div>");
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'> Cashier  : " + Cashier.ToUpper() + " </div>");
                    Breports.AddLineBreak();
                    Breports.AddString("<div style = 'float: left;margin-top:-10px;'><b style= 'color:#07629B;font-size:13px;'> Customer Name  : " + Fullname.Text.ToUpper() + " </div>");
                    Breports.AddLineBreak();
                    Breports.AddLineBreak();

                    DataTable header = new DataTable();

                    header.Columns.Add("Product Name:");
                    header.Columns.Add("QTY");
                    header.Columns.Add("Price Per Kg");
                    header.Columns.Add("Medium");

                    header.Rows.Add(new object[] { "Cooking Gas", bunifuTextBox1.Text + "Kg", price.ToString("N2"), gunaComboBox1.SelectedItem.ToString() });


                    Breports.AddDataTable(header, "width = 800px border = 1");

                    Breports.AddHorizontalRule("border = 1");
                    Breports.AddLineBreak();

                    DataTable header1 = new DataTable();

                    header1.Columns.Add("SUB TOTAL");
                    header1.Columns.Add(txtsubtotal.Text);
                    header1.Rows.Add(new object[] { "DISCOUNT", txtcash.Text });
                    header1.Rows.Add(new object[] { "TENDERED", txtdisc.Text });
                    header1.Rows.Add(new object[] { "BALANCE", txtbal.Text });
                    header1.Rows.Add(new object[] { "TOTAL", gunaButton1.Text });
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


                   
        }






















    }
}
