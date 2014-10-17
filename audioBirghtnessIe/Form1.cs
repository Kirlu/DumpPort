using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace audioBirghtnessIe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox5.Enabled = checkBox9.Enabled = checkBox13.Enabled = checkBox17.Enabled = false;
            }
            else
                checkBox5.Enabled = checkBox9.Enabled = checkBox13.Enabled = checkBox17.Enabled = true;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox1.Enabled = checkBox9.Enabled = checkBox13.Enabled = checkBox17.Enabled = false;
            }
            else
                checkBox1.Enabled = checkBox9.Enabled = checkBox13.Enabled = checkBox17.Enabled = true;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox1.Enabled = checkBox5.Enabled = checkBox13.Enabled = checkBox17.Enabled = false;
            }
            else
                checkBox1.Enabled = checkBox5.Enabled = checkBox13.Enabled = checkBox17.Enabled = true;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox1.Enabled = checkBox5.Enabled = checkBox9.Enabled = checkBox17.Enabled = false;
            }
            else
                checkBox1.Enabled = checkBox5.Enabled = checkBox9.Enabled = checkBox17.Enabled = true;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox1.Enabled = checkBox5.Enabled = checkBox9.Enabled = checkBox13.Enabled = false;
            }
            else
                checkBox1.Enabled = checkBox5.Enabled = checkBox9.Enabled = checkBox13.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox6.Enabled = checkBox10.Enabled = checkBox14.Enabled = checkBox18.Enabled = false;
            }
            else
                checkBox6.Enabled = checkBox10.Enabled = checkBox14.Enabled = checkBox18.Enabled = true;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox2.Enabled = checkBox10.Enabled = checkBox14.Enabled = checkBox18.Enabled = false;
            }
            else
                checkBox2.Enabled = checkBox10.Enabled = checkBox14.Enabled = checkBox18.Enabled = true;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox2.Enabled = checkBox6.Enabled = checkBox14.Enabled = checkBox18.Enabled = false;
            }
            else
                checkBox2.Enabled = checkBox6.Enabled = checkBox14.Enabled = checkBox18.Enabled = true;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox2.Enabled = checkBox6.Enabled = checkBox10.Enabled = checkBox18.Enabled = false;
            }
            else
                checkBox2.Enabled = checkBox6.Enabled = checkBox10.Enabled = checkBox18.Enabled = true;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox2.Enabled = checkBox6.Enabled = checkBox10.Enabled = checkBox14.Enabled = false;
            }
            else
                checkBox2.Enabled = checkBox6.Enabled = checkBox10.Enabled = checkBox14.Enabled = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox7.Enabled = checkBox11.Enabled = checkBox15.Enabled = checkBox19.Enabled = false;
            }
            else
                checkBox7.Enabled = checkBox11.Enabled = checkBox15.Enabled = checkBox19.Enabled = true;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox3.Enabled = checkBox11.Enabled = checkBox15.Enabled = checkBox19.Enabled = false;
            }
            else
                checkBox3.Enabled = checkBox11.Enabled = checkBox15.Enabled = checkBox19.Enabled = true;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox3.Enabled = checkBox7.Enabled = checkBox15.Enabled = checkBox19.Enabled = false;
            }
            else
                checkBox3.Enabled = checkBox7.Enabled = checkBox15.Enabled = checkBox19.Enabled = true;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox3.Enabled = checkBox7.Enabled = checkBox11.Enabled = checkBox19.Enabled = false;
            }
            else
                checkBox3.Enabled = checkBox7.Enabled = checkBox11.Enabled = checkBox19.Enabled = true;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox3.Enabled = checkBox7.Enabled = checkBox11.Enabled = checkBox15.Enabled = false;
            }
            else
                checkBox3.Enabled = checkBox7.Enabled = checkBox11.Enabled = checkBox15.Enabled = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox8.Enabled = checkBox12.Enabled = checkBox16.Enabled = checkBox20.Enabled = false;
            }
            else
                checkBox8.Enabled = checkBox12.Enabled = checkBox16.Enabled = checkBox20.Enabled = true;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox4.Enabled = checkBox12.Enabled = checkBox16.Enabled = checkBox20.Enabled = false;
            }
            else
                checkBox4.Enabled = checkBox12.Enabled = checkBox16.Enabled = checkBox20.Enabled = true;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox4.Enabled = checkBox8.Enabled = checkBox16.Enabled = checkBox20.Enabled = false;
            }
            else
                checkBox4.Enabled = checkBox8.Enabled = checkBox16.Enabled = checkBox20.Enabled = true;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox4.Enabled = checkBox8.Enabled = checkBox12.Enabled = checkBox20.Enabled = false;
            }
            else
                checkBox4.Enabled = checkBox8.Enabled = checkBox12.Enabled = checkBox20.Enabled = true;
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                checkBox4.Enabled = checkBox8.Enabled = checkBox12.Enabled = checkBox16.Enabled = false;
            }
            else
                checkBox4.Enabled = checkBox8.Enabled = checkBox12.Enabled = checkBox16.Enabled = true;
        }


    }
}
