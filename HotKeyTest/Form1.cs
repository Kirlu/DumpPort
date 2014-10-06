using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hotkeys;

namespace HotKeyTest
{
    public partial class Form1 : Form
    {
        private Hotkeys.GlobalHotkey ghk, abk;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing+=Form1_FormClosing;
            ghk = new Hotkeys.GlobalHotkey(Constants.ALT + Constants.SHIFT, Keys.O, this);
            abk = new GlobalHotkey(Constants.ALT, Keys.A, this);
        }

        private void HandleHotkey(string result)
        {
            WriteLine(result + " Hotkey pressed!");
        }
        private Keys GetKey(IntPtr LParam)
        {
            return (Keys)((LParam.ToInt32()) >> 16); // not all of the parenthesis are needed, I just found it easier to see what's happening
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Hotkeys.Constants.WM_HOTKEY_MSG_ID)
            {
                switch (GetKey(m.LParam))
                {

                    case Keys.A:
                        HandleHotkey("A");
                        break;
                    case Keys.O:
                        HandleHotkey("O");
                        break;
                }
            }

            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WriteLine("Trying to register SHIFT+ALT+O");
            if (ghk.Register() && abk.Register())
                WriteLine("Hotkey registered.");
            else
                WriteLine("Hotkey failed to register");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ghk.Unregiser() && !abk.Unregiser())
                MessageBox.Show("Hotkey failed to unregister!");
        }

        private void WriteLine(string text)
        {
            textBox1.Text += text + Environment.NewLine;
        }
    }
}
