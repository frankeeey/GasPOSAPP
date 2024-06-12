using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gas
{
    public partial class SalesMain : Form
    {
        
        readonly Random r = new Random();
        readonly Random v = new Random();

        public SalesMain()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            salesHome1.button1.PerformClick();
        }

        private void SalesMain_Load(object sender, EventArgs e)
        {
            salesHome1.button1.PerformClick();
        }

        private void SalesMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                Application.Exit();
            }
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            CSales SS = new CSales();
            SS.FindForm();
            if(Bnum.Text == "")
            {
                MessageBox.Show("PLEASE PUT A VALID GAS PRICE", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                SS.price = int.Parse(Bnum.Text);
                SS.Cashier = lblname.Text;                
                SS.ShowDialog();
               
            }
           
        }

        private void salesHome1_Load(object sender, EventArgs e)
        {

        }

        private void Bnum_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Bnum.Text, "[^0-9]"))
            {
                Bnum.Text = Bnum.Text.Remove(Bnum.Text.Length - 1);

            }
        }

        private void Bnum_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                CSales SS = new CSales();
                SS.FindForm();
                if (Bnum.Text == "")
                {
                    MessageBox.Show("PLEASE PUT A VALID GAS PRICE", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    SS.price = int.Parse(Bnum.Text);
                    SS.Cashier = lblname.Text;
                    SS.ShowDialog();

                }

            }
        }

        
       




    }
}
