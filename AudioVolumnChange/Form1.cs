using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//using CoreAudioApi;
using NAudio;

namespace AudioVolumnChange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trackBar1.ValueChanged += trackBar1_ValueChanged;
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
                        dev.AudioEndpointVolume.MasterVolumeLevel = 0;
                        Trace.WriteLine("MasterVolumeLevel:"+dev.AudioEndpointVolume.StepInformation.StepCount);
                        trackBar1.Minimum = (int) dev.AudioEndpointVolume.VolumeRange.MinDecibels;
                        trackBar1.Maximum = (int) dev.AudioEndpointVolume.VolumeRange.MaxDecibels;
                        trackBar1.Scale(dev.AudioEndpointVolume.MasterVolumeLevelScalar);
                        trackBar1.Value = (int) dev.AudioEndpointVolume.MasterVolumeLevel;
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

        void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            /*
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
                        dev.AudioEndpointVolume.MasterVolumeLevel=trackBar1.Value;
                        Trace.WriteLine(trackBar1.Value);
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
            */
            
        }

        private void buttonUp_Click(object sender, EventArgs e)
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
                        trackBar1.Value=(int) dev.AudioEndpointVolume.MasterVolumeLevel;
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

        private void buttonDown_Click(object sender, EventArgs e)
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
                        trackBar1.Value = (int)dev.AudioEndpointVolume.MasterVolumeLevel;
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
    }
    /*
    static class NativeMethods
    {

        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);


        [DllImport("winmm.dll", SetLastError = true)]
        public static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
    }

    public static class MSWindowsFriendlyNames
    {
        public static string WindowsXP { get { return "Windows XP"; } }
        public static string WindowsVista { get { return "Windows Vista"; } }
        public static string Windows7 { get { return "Windows 7"; } }
        public static string Windows8 { get { return "Windows 8"; } }
    }

    public static class SistemVolumChanger
    {
        public static void SetVolume(int value)
        {
            if (value < 0)
                value = 0;

            if (value > 100)
                value = 100;

            var osFriendlyName = GetOSFriendlyName();

            if (osFriendlyName.Contains(MSWindowsFriendlyNames.WindowsXP))
            {
                SetVolumeForWIndowsXP(value);
            }
            else if (osFriendlyName.Contains(MSWindowsFriendlyNames.WindowsVista) || osFriendlyName.Contains(MSWindowsFriendlyNames.Windows7) || osFriendlyName.Contains(MSWindowsFriendlyNames.Windows8))
            {
                SetVolumeForWIndowsVista78(value);
            }
            else
            {
                SetVolumeForWIndowsVista78(value);
            }
        }

        public static int GetVolume()
        {
            int result = 100;
            try
            {
                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                result = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            }
            catch (Exception)
            {
            }

            return result;
        }

        private static void SetVolumeForWIndowsVista78(int value)
        {
            try
            {
                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

                device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)value / 100.0f;
            }
            catch (Exception)
            {
            }
        }

        private static void SetVolumeForWIndowsXP(int value)
        {
            try
            {
                // Calculate the volume that's being set
                double newVolume = ushort.MaxValue * value / 10.0;

                uint v = ((uint)newVolume) & 0xffff;
                uint vAll = v | (v << 16);

                // Set the volume
                int retVal = NativeMethods.WaveOutSetVolume(IntPtr.Zero, vAll);
            }
            catch (Exception)
            {
            }
        }

        private static string GetOSFriendlyName()
        {
            string result = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }
            return result;
        }
    }
    */
}
