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

namespace Gas
{
    public partial class Login : Form
    {
        readonly string mysqlcon = "datasource = localhost; port = 3306; username = root; password =; database = gas; SslMode = none";
        int i;
        readonly string date = DateTime.Now.ToLongDateString();
        readonly string time = DateTime.Now.ToShortTimeString();

        public Login()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            

            lblerror.Visible = false;
            try
            {
                guna2Button1.Text = "Logging...";

                MySqlConnection CON = new MySqlConnection(mysqlcon);
                MySqlCommand cmd1;
                CON.Open();
                string Log = "select * from login where username = @Username and password = @Password ";
                MySqlCommand cmd = new MySqlCommand(Log, CON);
                cmd.Parameters.AddWithValue("@Username", Usertext.Text);
                cmd.Parameters.AddWithValue("@Password", Passtext.Text);

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                cmd.ExecuteNonQuery();
                i = Convert.ToInt32(dt.Rows.Count.ToString());


                try
                {
                    //Manager's login

                    if (dt.Rows[0][4].ToString() == "Manager")
                    {
                       Form1 man = new Form1();
                        man.FindForm();


                        if (i == 0)
                        {
                            lblerror.Visible = true;
                            guna2Button1.Text = "Login";


                        }
                        else
                        {
                            lblerror.Visible = false;
                            string selectQuery = "SELECT * FROM login WHERE Username = @user";
                            cmd = new MySqlCommand(selectQuery, CON);
                            cmd.Parameters.AddWithValue("@user", Usertext.Text);
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dt);
                            man.lblname.Text = dt.Rows[0][1].ToString();                           

                            man.Show();

                            //Update Lastseen
                            string updatequery = "UPDATE Login SET lastseen = @lseen  where username  = @use";
                            cmd = new MySqlCommand(updatequery, CON);
                            cmd.Parameters.AddWithValue("@use", Usertext.Text);
                            cmd.Parameters.AddWithValue("@lseen", date + " " + time);
                            cmd.ExecuteNonQuery();

                            this.Hide();
                            

                        }
                    }
                    // Others Login

                    else
                    {
                        SalesMain hm = new SalesMain();
                        hm.FindForm();


                        if (i == 0)
                        {
                            lblerror.Visible = true;
                            guna2Button1.Text = "Login Now";


                        }
                        else
                        {
                            
                            lblerror.Visible = false;
                            string selectQuery = "SELECT * FROM login WHERE Username = @user";
                            cmd = new MySqlCommand(selectQuery, CON);
                            cmd.Parameters.AddWithValue("@user", Usertext.Text);
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dt);
                            hm.lblname.Text = dt.Rows[0][1].ToString();
                            hm.Show();

                            //Update Lastseen
                            string updatequery = "UPDATE Login SET lastseen = @lseen  where Username  = @user";
                            cmd1 = new MySqlCommand(updatequery, CON);
                            cmd1.Parameters.AddWithValue("@lseen", date + " " + time);
                            cmd1.Parameters.AddWithValue("@user", Usertext.Text);
                            cmd1.ExecuteNonQuery();

                            this.Hide();



                        }
                    }
                }
                catch
                {

                    guna2Button1.Text = "Login";

                    lblerror.Visible = true;
                }
                CON.Close();

            }
            catch
            {
                MessageBox.Show("No or Slow Network Connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2Button1.Text = "Login";

            }
        }
    }
}
