using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace comboboxTest
{
    public partial class Form1 : Form
    {
        private List<string> funcList; 
        public Form1()
        {
            InitializeComponent();
            funcList=new List<string>();
            funcList.Add("Audio_Up");
            funcList.Add("Audio_Down");
            funcList.Add("Brightness_Up");
            funcList.Add("Brightness_Down");
            funcList.Add("Open_IE");
            funcList.Add("");
            this.Load += Form1_Load;
            comboBox1.SelectionChangeCommitted+=comboBox1_SelectionChangeCommitted;
            comboBox2.SelectionChangeCommitted += comboBox2_SelectionChangeCommitted;
            comboBox3.SelectionChangeCommitted += comboBox3_SelectionChangeCommitted;
            comboBox4.SelectionChangeCommitted += comboBox4_SelectionChangeCommitted;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
        }

        void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Trace.WriteLine(((ComboBox)sender).SelectedValue);
            comboBox4.Enabled = false;
        }

        void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Trace.WriteLine(((ComboBox)sender).SelectedValue);
            funcList.Remove(((ComboBox)sender).SelectedValue.ToString());
            comboBox4.DataSource = new BindingList<string>(funcList);
            comboBox3.Enabled = false;
            comboBox4.Enabled = true;
        }

        void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Trace.WriteLine(((ComboBox)sender).SelectedValue);
            funcList.Remove(((ComboBox)sender).SelectedValue.ToString());
            comboBox3.DataSource = new BindingList<string>(funcList);
            comboBox2.Enabled = false;
            comboBox3.Enabled = true;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Trace.WriteLine(((ComboBox)sender).SelectedValue);
            funcList.Remove(((ComboBox)sender).SelectedValue.ToString());
            comboBox2.DataSource = new BindingList<string>(funcList);
            comboBox2.Enabled = true;
            comboBox1.Enabled = false;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new BindingList<string>(funcList);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            funcList.Clear();
            funcList.Add("Audio Up");
            funcList.Add("Audio Down");
            funcList.Add("Brightness Up");
            funcList.Add("Brightness Down");
            funcList.Add("Open IE");
            funcList.Add("");
            comboBox1.Enabled = true;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox2.Text = comboBox3.Text = comboBox4.Text = "";

            comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = -1;

        }

        
    }
}
