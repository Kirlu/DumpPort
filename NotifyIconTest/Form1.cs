using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NotifyIconTest
{
    public unsafe partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String DllName);

        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr hModule, String ProcName);

        [DllImport("kernel32")]
        private extern static bool FreeLibrary(IntPtr hModule);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool InitializeWinIoType();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool GetPortValType(UInt16 PortAddr, UInt32* pPortVal, UInt16 Size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool SetPortValType(UInt16 PortAddr, UInt32 PortVal, UInt16 Size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool ShutdownWinIoType();

        IntPtr hMod;
        private const string keyAdress = "58B";
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Check if this is a 32 bit or 64 bit system
            if (IntPtr.Size == 4)
            {
                hMod = LoadLibrary("WinIo32.dll");

            }
            else if (IntPtr.Size == 8)
            {
                hMod = LoadLibrary("WinIo64.dll");
            }

            if (hMod == IntPtr.Zero)
            {
                MessageBox.Show("Can't find WinIo dll.\nMake sure the WinIo library files are located in the same directory as your executable file.", "DumpPort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            IntPtr pFunc = GetProcAddress(hMod, "InitializeWinIo");

            if (pFunc != IntPtr.Zero)
            {
                InitializeWinIoType InitializeWinIo = (InitializeWinIoType)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(InitializeWinIoType));
                bool Result = InitializeWinIo();

                if (!Result)
                {
                    MessageBox.Show("Error returned from InitializeWinIo.\nMake sure you are running with administrative privileges and that the WinIo library files are located in the same directory as your executable file.", "DumpPort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FreeLibrary(hMod);
                    this.Close();
                }
            }

            //btnGetValue_Click(this, null);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        string getValue(string ioAddress)
        {
            IntPtr pFunc = GetProcAddress(hMod, "GetPortVal");

            if (pFunc != IntPtr.Zero)
            {
                UInt16 PortAddr;
                UInt32 PortVal;

                PortAddr = UInt16.Parse(ioAddress, System.Globalization.NumberStyles.HexNumber);

                GetPortValType GetPortVal = (GetPortValType)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(GetPortValType));

                // Call WinIo to get value
                bool Result = GetPortVal(PortAddr, &PortVal, 1);
                //bool Result = GetPortVal(PortAddr, &PortVal, 4);
                if (Result)
                {
                    //txtValue.Text = PortVal.ToString("X");
                    //txtValue.Text = PortVal.ToString("X8");
                    return PortVal.ToString();
                }
                else
                {
                    MessageBox.Show("Error returned from GetPortVal", "DumpPort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "fail";
                }
            }
            else
            {
                return "fail";
            }
        }
    }
}
