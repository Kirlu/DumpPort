using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private const string keyAdress = "588";
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
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //bit27
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[27].Equals(false))
                    {
                        MessageBox.Show("Volumn up");
                    }
                    Thread.SpinWait(100);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //bit28
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[28].Equals(false))
                    {
                        MessageBox.Show("Volumn down");
                    }
                    Thread.SpinWait(100);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //bit29
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[29].Equals(false))
                    {
                        MessageBox.Show("Brightness up");
                    }
                    Thread.SpinWait(100);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //bit30
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[30].Equals(false))
                    {
                        MessageBox.Show("Brightness down");
                    }
                    Thread.SpinWait(100);
                }
            });
        }



        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        bool[] getBinValue(string UintValue)
        {
            /*
            byte cc = byte.Parse(UintValue);
            //Console.WriteLine(cc.ToString("X"));
            string bb = Convert.ToString(cc, 2).PadLeft(8, '0');
            //Console.WriteLine(bb);
            bool[] ccc = bb.Select(s => s.Equals('1')).ToArray();
            return ccc;
            */
            //UInt32 a = 78;
            byte[] bytes = BitConverter.GetBytes(UInt32.Parse(UintValue));
            Console.WriteLine(BitConverter.ToString(bytes));
            Array.Reverse(bytes);
            Console.WriteLine(BitConverter.ToString(bytes));
            UInt32 result = BitConverter.ToUInt32(bytes, 0);
            Console.WriteLine(result);
            Console.WriteLine(BitConverter.ToString(BitConverter.GetBytes(result)));
            Console.WriteLine(result.ToString("X8"));
            string b = Convert.ToString(result, 2).PadLeft(32, '0');
            bool[] c = b.Select(s => s.Equals('1')).ToArray();
            return c;
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
                //bool Result = GetPortVal(PortAddr, &PortVal, 1);
                bool Result = GetPortVal(PortAddr, &PortVal, 4);
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
