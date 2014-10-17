using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using NAudio;

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
        private Monitor _currentMonitor;
        private readonly MonitorCollection _monitorCollection = new MonitorCollection();
        byte[] bLevels;
        int? iBrightness=50;
        string OSName = null;
        enum ABI_function
        {
            AudioUp,
            AudioDown,
            BrightnessUp,
            BrightnessDown,
            OpenIE
        };
        ABI_function bit27, bit28, bit29, bit30;
        public Form1()
        {
            InitializeComponent();
            OSName = getOSName();
            if (OSName.Contains("Windows 7"))
            {
                OSName = "win7";
            }
            else
                OSName = "win8";
            this.FormClosing += Form1_FormClosing;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            if(OSName.Equals("win8"))
            {
                var @delegate = new NativeMethods.MonitorEnumDelegate(MonitorEnum);
                NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, @delegate, IntPtr.Zero);
                bLevels = GetBrightnessLevels(); //get the level array for this system
                if (bLevels.Count() == 0) //"WmiMonitorBrightness" is not supported by the system
                {
                    Application.Exit();
                }
                else
                {
                    /*
                    trackBar1.TickFrequency = bLevels.Count(); //adjust the trackbar ticks according the number of possible brightness levels
                    trackBar1.Maximum = bLevels.Count() - 1;
                    trackBar1.Update();
                    trackBar1.Refresh();
                    */
                    check_brightness();
                }
            }
        }

        private void UpdateScreenWithBarValue(uint value)
        {
            _currentMonitor = _monitorCollection[0];
            NativeMethods.SetMonitorBrightness(_currentMonitor.HPhysicalMonitor, value);
        }
        // To be called by a delegate
        private bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData)
        {
            _monitorCollection.Add(hMonitor);
            return true;
        }

        static void SetBrightness(byte targetBrightness)
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightnessMethods");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            foreach (System.Management.ManagementObject o in moc)
            {
                o.InvokeMethod("WmiSetBrightness", new Object[] { UInt32.MaxValue, targetBrightness }); //note the reversed order - won't work otherwise!
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();
        }
        //array of valid brightness values in percent
        static byte[] GetBrightnessLevels()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);
            byte[] BrightnessLevels = new byte[0];

            try
            {
                System.Management.ManagementObjectCollection moc = mos.Get();

                //store result


                foreach (System.Management.ManagementObject o in moc)
                {
                    BrightnessLevels = (byte[])o.GetPropertyValue("Level");
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();

            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, Your System does not support this brightness control...");

            }

            return BrightnessLevels;
        }
        private void check_brightness()
        {
            iBrightness = GetBrightness(); //get the actual value of brightness
            int i = Array.IndexOf(bLevels, (byte)iBrightness);
            if (i < 0) i = 1;
            //trackBar1.Value = i;
        }
        //get the actual percentage of brightness
        static int GetBrightness()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            //store result
            byte curBrightness = 0;
            foreach (System.Management.ManagementObject o in moc)
            {
                curBrightness = (byte)o.GetPropertyValue("CurrentBrightness");
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();

            return (int)curBrightness;
        }
        void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        /*
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
            ThreadPool.QueueUserWorkItem(callback =>
            {
                while (true)
                {
                    //bit27
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[28].Equals(false))
                    {
                        //MessageBox.Show("Volumn up");
                        VolumnUp();
                    }
                    Thread.Sleep(1);
                }
            });
            ThreadPool.QueueUserWorkItem(callback =>
            {
                while (true)
                {
                    //bit28
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[27].Equals(false))
                    {
                        //MessageBox.Show("Volumn down");
                        VolumnDown();
                    }
                    Thread.Sleep(1);
                }
            });
            ThreadPool.QueueUserWorkItem(callback =>
            {
                while (true)
                {
                    //bit29
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[26].Equals(false))
                    {
                        //MessageBox.Show("Brightness up");
                        if (iBrightness < 100)
                        {
                            iBrightness += 1;
                            SetBrightness((byte)iBrightness);
                        }
                    }
                    Thread.Sleep(1);
                }
            });
            ThreadPool.QueueUserWorkItem(callback =>
            {
                while (true)
                {
                    //bit30
                    bool[] result = getBinValue(getValue(keyAdress));
                    if (result[25].Equals(false))
                    {
                        //MessageBox.Show("Brightness down");
                        if (iBrightness > 0)
                        {
                            iBrightness -= 1;
                            SetBrightness((byte)iBrightness);
                        }
                    }
                    Thread.Sleep(1);
                }
            });
        }
        */
        #region new io library
        [DllImport("inpout32.dll")]
        private static extern UInt32 IsInpOutDriverOpen();
        [DllImport("inpout32.dll")]
        private static extern void Out32(short PortAddress, short Data);
        [DllImport("inpout32.dll")]
        private static extern char Inp32(short PortAddress);

        [DllImport("inpout32.dll")]
        private static extern void DlPortWritePortUshort(short PortAddress, ushort Data);
        [DllImport("inpout32.dll")]
        private static extern ushort DlPortReadPortUshort(short PortAddress);

        [DllImport("inpout32.dll")]
        private static extern void DlPortWritePortUlong(int PortAddress, uint Data);
        [DllImport("inpout32.dll")]
        private static extern uint DlPortReadPortUlong(int PortAddress);

        [DllImport("inpoutx64.dll")]
        private static extern bool GetPhysLong(ref int PortAddress, ref uint Data);
        [DllImport("inpoutx64.dll")]
        private static extern bool SetPhysLong(ref int PortAddress, ref uint Data);


        [DllImport("inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
        private static extern UInt32 IsInpOutDriverOpen_x64();
        [DllImport("inpoutx64.dll", EntryPoint = "Out32")]
        private static extern void Out32_x64(short PortAddress, short Data);
        [DllImport("inpoutx64.dll", EntryPoint = "Inp32")]
        private static extern char Inp32_x64(short PortAddress);

        [DllImport("inpoutx64.dll", EntryPoint = "DlPortWritePortUshort")]
        private static extern void DlPortWritePortUshort_x64(short PortAddress, ushort Data);
        [DllImport("inpoutx64.dll", EntryPoint = "DlPortReadPortUshort")]
        private static extern ushort DlPortReadPortUshort_x64(short PortAddress);

        [DllImport("inpoutx64.dll", EntryPoint = "DlPortWritePortUlong")]
        private static extern void DlPortWritePortUlong_x64(int PortAddress, uint Data);
        [DllImport("inpoutx64.dll", EntryPoint = "DlPortReadPortUlong")]
        private static extern uint DlPortReadPortUlong_x64(int PortAddress);

        [DllImport("inpoutx64.dll", EntryPoint = "GetPhysLong")]
        private static extern bool GetPhysLong_x64(ref int PortAddress, ref uint Data);
        [DllImport("inpoutx64.dll", EntryPoint = "SetPhysLong")]
        private static extern bool SetPhysLong_x64(ref int PortAddress, ref uint Data);


        bool m_bX64 = false;
        Thread t27=null, t28=null, t29=null, t30=null;
        //void test()//Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                uint nResult = 0;
                try
                {
                    nResult = IsInpOutDriverOpen();
                }
                catch (BadImageFormatException)
                {
                    nResult = IsInpOutDriverOpen_x64();
                    if (nResult != 0)
                        m_bX64 = true;

                }

                if (nResult == 0)
                {
                    /*
                    lblMessage.Text = "Unable to open InpOut32 driver";
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Enabled = false;
                    button7.Enabled = false;
                     * */
                }
            }
            catch (DllNotFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                /*
                lblMessage.Text = "Unable to find InpOut32.dll";
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                 * */
            }
            #region 27bit
            t27 = new System.Threading.Thread(delegate()
            {
                while (true)
                {
                    //bit27
                    try
                    {
                        int nPort = int.Parse(keyAdress, System.Globalization.NumberStyles.HexNumber);

                        uint l;
                        if (m_bX64)
                            l = DlPortReadPortUlong_x64(nPort);
                        else
                            l = DlPortReadPortUlong(nPort);

                        byte[] bytes = BitConverter.GetBytes(l);
                        //Console.WriteLine(BitConverter.ToString(bytes));
                        Array.Reverse(bytes);
                        string result2 = BitConverter.ToString(bytes).Replace("-", "");
                        //Console.WriteLine(result2);
                        BitArray ba = ConvertHexToBitArray(result2);
                        //foreach (var cccd in ba)
                        {
                            //Console.Write(cccd);
                        }
                        //Console.WriteLine();
                        int bit = 27;
                        //Console.WriteLine("bit" + bit + ":" + ba[ba.Length - 1 - bit]);
                        if (ba[ba.Length - 1 - bit].Equals(false))
                        {
                            //MessageBox.Show("Volumn up");
                            VolumnUp();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured:\n" + ex.Message);
                    }
                    Thread.Sleep(1);
                }
            });
#endregion 27bit
            #region 28bit
            t28 = new System.Threading.Thread(delegate()
            {
                while (true)
                {
                    //bit28
                    try
                    {
                        int nPort = int.Parse(keyAdress, System.Globalization.NumberStyles.HexNumber);

                        uint l;
                        if (m_bX64)
                            l = DlPortReadPortUlong_x64(nPort);
                        else
                            l = DlPortReadPortUlong(nPort);

                        byte[] bytes = BitConverter.GetBytes(l);
                        //Console.WriteLine(BitConverter.ToString(bytes));
                        Array.Reverse(bytes);
                        string result2 = BitConverter.ToString(bytes).Replace("-", "");
                        //Console.WriteLine(result2);
                        BitArray ba = ConvertHexToBitArray(result2);
                        //foreach (var cccd in ba)
                        {
                            //Console.Write(cccd);
                        }
                        //Console.WriteLine();
                        int bit = 28;
                        //Console.WriteLine("bit" + bit + ":" + ba[ba.Length - 1 - bit]);
                        if (ba[ba.Length - 1 - bit].Equals(false))
                        {
                            //MessageBox.Show("Volumn down");
                            VolumnDown();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured:\n" + ex.Message);
                    }
                    Thread.Sleep(1);
                }
            });
            #endregion 28bit
            #region 29bit
            t29 = new System.Threading.Thread(delegate()
            {
                while (true)
                {
                    //bit29
                    try
                    {
                        int nPort = int.Parse(keyAdress, System.Globalization.NumberStyles.HexNumber);

                        uint l;
                        if (m_bX64)
                            l = DlPortReadPortUlong_x64(nPort);
                        else
                            l = DlPortReadPortUlong(nPort);

                        byte[] bytes = BitConverter.GetBytes(l);
                        //Console.WriteLine(BitConverter.ToString(bytes));
                        Array.Reverse(bytes);
                        string result2 = BitConverter.ToString(bytes).Replace("-", "");
                        //Console.WriteLine(result2);
                        BitArray ba = ConvertHexToBitArray(result2);
                        //foreach (var cccd in ba)
                        {
                            //Console.Write(cccd);
                        }
                        //Console.WriteLine();
                        int bit = 29;
                        //Console.WriteLine("bit" + bit + ":" + ba[ba.Length - 1 - bit]);
                        if (ba[ba.Length - 1 - bit].Equals(false))
                        {
                            //MessageBox.Show("iBrightness up");
                            //if (iBrightness < 100)
                            {
                                iBrightness += 1;
                                //SetBrightness((byte)iBrightness);
                                switch (OSName)
                                {
                                    case "win7":
                                        if (iBrightness > 255)
                                            iBrightness = 255;
                                        Brightness.SetBrightness((short)iBrightness);
                                        break;
                                    case "win8":
                                        if (iBrightness > 100)
                                            iBrightness = 100;
                                        SetBrightness((byte)iBrightness);
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured:\n" + ex.Message);
                    }
                    Thread.Sleep(1);
                }
            });
            #endregion 29bit
            #region 30bit
            t30 = new System.Threading.Thread(delegate()
            {
                while (true)
                {
                    //bit30
                    try
                    {
                        int nPort = int.Parse(keyAdress, System.Globalization.NumberStyles.HexNumber);

                        uint l;
                        if (m_bX64)
                            l = DlPortReadPortUlong_x64(nPort);
                        else
                            l = DlPortReadPortUlong(nPort);

                        byte[] bytes = BitConverter.GetBytes(l);
                        //Console.WriteLine(BitConverter.ToString(bytes));
                        Array.Reverse(bytes);
                        string result2 = BitConverter.ToString(bytes).Replace("-", "");
                        //Console.WriteLine(result2);
                        BitArray ba = ConvertHexToBitArray(result2);
                        //foreach (var cccd in ba)
                        {
                            //Console.Write(cccd);
                        }
                        //Console.WriteLine();
                        int bit = 30;
                        //Console.WriteLine("bit" + bit + ":" + ba[ba.Length - 1 - bit]);
                        if (ba[ba.Length - 1 - bit].Equals(false))
                        {
                            //MessageBox.Show("iBrightness down");
                            if (iBrightness > 0)
                            {
                                iBrightness -= 1;
                                //SetBrightness((byte)iBrightness);
                                switch (OSName)
                                {
                                    case "win7":
                                        Brightness.SetBrightness((short)iBrightness);
                                        break;
                                    case "win8":
                                        SetBrightness((byte)iBrightness);
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured:\n" + ex.Message);
                    }
                    Thread.Sleep(1);
                }
            });
            #endregion 30bit
            t27.Start();
            t28.Start();
            t29.Start();
            t30.Start();
        }
        string getOSName()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }
        static BitArray ConvertHexToBitArray(string hexData)
        {
            if (hexData == null)
                return null; // or do something else, throw, ...

            BitArray ba = new BitArray(4 * hexData.Length);
            for (int i = 0; i < hexData.Length; i++)
            {
                byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
        }
        #endregion new io library
        private void VolumnDown()
        {
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    try
                    {
                        dev.AudioEndpointVolume.VolumeStepDown();
                        Trace.WriteLine(dev.AudioEndpointVolume.MasterVolumeLevel);
                        //trackBar1.Value = (int)dev.AudioEndpointVolume.MasterVolumeLevel;
                        //Trace.WriteLine(trackBar1.Value);
                        //Set at maximum volume
                        //dev.AudioEndpointVolume.MasterVolumeLevel = 0;

                        //Get its audio volume
                        System.Diagnostics.Debug.Print("Volume of " + dev.FriendlyName + " is " + dev.AudioEndpointVolume.MasterVolumeLevel.ToString());

                        //Mute it
                        //dev.AudioEndpointVolume.Mute = true;
                        System.Diagnostics.Debug.Print(dev.FriendlyName + " is muted");
                    }
                    catch (Exception ex)
                    {
                        //Do something with exception when an audio endpoint could not be muted
                        System.Diagnostics.Debug.Print(dev.FriendlyName + " could not be muted");
                    }
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
                System.Diagnostics.Debug.Print("Could not enumerate devices due to an excepion: " + ex.Message);
            }
        }

        private void VolumnUp()
        {
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    try
                    {
                        dev.AudioEndpointVolume.VolumeStepUp();
                        Trace.WriteLine(dev.AudioEndpointVolume.MasterVolumeLevel);
                        //trackBar1.Value = (int)dev.AudioEndpointVolume.MasterVolumeLevel;
                        //Trace.WriteLine(trackBar1.Value);
                        //Set at maximum volume
                        //dev.AudioEndpointVolume.MasterVolumeLevel = 0;

                        //Get its audio volume
                        System.Diagnostics.Debug.Print("Volume of " + dev.FriendlyName + " is " + dev.AudioEndpointVolume.MasterVolumeLevel.ToString());

                        //Mute it
                        //dev.AudioEndpointVolume.Mute = true;
                        System.Diagnostics.Debug.Print(dev.FriendlyName + " is muted");
                    }
                    catch (Exception ex)
                    {
                        //Do something with exception when an audio endpoint could not be muted
                        System.Diagnostics.Debug.Print(dev.FriendlyName + " could not be muted");
                    }
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
                System.Diagnostics.Debug.Print("Could not enumerate devices due to an excepion: " + ex.Message);
            }
        }



        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.Hide();
            if(t27!=null)
                t27.IsBackground=true;
            if(t28!=null)
                t28.IsBackground = true;
            if (t29 != null)
                t29.IsBackground = true;
            if (t30 != null)
                t30.IsBackground = true;
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

    public static class Brightness
    {
        [DllImport("gdi32.dll")]
        private unsafe static extern bool SetDeviceGammaRamp(Int32 hdc, void* ramp);

        private static bool initialized = false;
        private static Int32 hdc;


        private static void InitializeClass()
        {
            if (initialized)
                return;

            //Get the hardware device context of the screen, we can do
            //this by getting the graphics object of null (IntPtr.Zero)
            //then getting the HDC and converting that to an Int32.
            hdc = Graphics.FromHwnd(IntPtr.Zero).GetHdc().ToInt32();

            //initialized = true;
        }

        public static unsafe bool SetBrightness(short brightness)
        {
            InitializeClass();

            if (brightness > 255)
                brightness = 255;

            if (brightness < 0)
                brightness = 0;

            short* gArray = stackalloc short[3 * 256];
            short* idx = gArray;

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 256; i++)
                {
                    int arrayVal = i * (brightness + 128);

                    if (arrayVal > 65535)
                        arrayVal = 65535;

                    *idx = (short)arrayVal;
                    idx++;
                }
            }

            //For some reason, this always returns false?
            bool retVal = SetDeviceGammaRamp(hdc, gArray);

            //Memory allocated through stackalloc is automatically free'd
            //by the CLR.

            return retVal;

        }
    }
}
