using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotifyIconTest
{
    public class Monitor
    {
        #region Declarations

        public IntPtr HPhysicalMonitor { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }

        public bool SupportsDDC { get; set; }
        public MonitorFeature Brightness;
        public MonitorFeature Contrast;
        public MonitorFeature RgbDrive;
        public MonitorFeature RgbGain;

        private uint _monitorCapabilities = 0u;
        private uint _supportedColorTemperatures = 0u;

        #endregion


        public Monitor(NativeStructures.PHYSICAL_MONITOR physicalMonitor)
        {
            HPhysicalMonitor = physicalMonitor.hPhysicalMonitor;
            Name = physicalMonitor.szPhysicalMonitorDescription;
            CheckCapabilities();
        }

        private void CheckCapabilities()
        {
            NativeMethods.GetMonitorCapabilities(HPhysicalMonitor, ref _monitorCapabilities, ref _supportedColorTemperatures);

            SupportsDDC = (_monitorCapabilities > 0);
            if (SupportsDDC)
            {
                CheckBrightness();
                CheckContrast();
                CheckRgbDrive();
                CheckRgbGain();
            }
        }

        private void CheckBrightness()
        {
            Brightness.Supported = ((int)NativeStructures.MC_MONITOR_CAPABILITIES.MC_CAPS_BRIGHTNESS & _monitorCapabilities) > 0;
            if (Brightness.Supported)
            {
                NativeMethods.GetMonitorBrightness(HPhysicalMonitor, ref Brightness.Min, ref Brightness.Current, ref Brightness.Max);
                Brightness.Original = Brightness.Current;
            }
        }
        private void CheckContrast()
        {
            Contrast.Supported = ((int)NativeStructures.MC_MONITOR_CAPABILITIES.MC_CAPS_CONTRAST & _monitorCapabilities) > 0;
            if (Contrast.Supported)
            {
                NativeMethods.GetMonitorContrast(HPhysicalMonitor, ref Contrast.Min, ref Contrast.Current, ref Contrast.Max);
                Contrast.Original = Contrast.Current;
            }
        }

        private void CheckRgbDrive()
        {
            RgbDrive.Supported = ((int)NativeStructures.MC_MONITOR_CAPABILITIES.MC_CAPS_RED_GREEN_BLUE_DRIVE) > 0;
            if (RgbDrive.Supported)
            {
                NativeMethods.GetMonitorRedGreenOrBlueDrive(HPhysicalMonitor,
                                                            NativeStructures.MC_DRIVE_TYPE.MC_BLUE_DRIVE,
                                                            ref RgbDrive.Min, ref RgbDrive.Current, ref RgbDrive.Max);
            }
        }

        private void CheckRgbGain()
        {
            RgbGain.Supported = ((int)NativeStructures.MC_MONITOR_CAPABILITIES.MC_CAPS_RED_GREEN_BLUE_GAIN) > 0;
            if (RgbGain.Supported)
            {
                NativeMethods.GetMonitorRedGreenOrBlueGain(HPhysicalMonitor,
                                                            NativeStructures.MC_GAIN_TYPE.MC_BLUE_GAIN,
                                                            ref RgbGain.Min, ref RgbGain.Current, ref RgbGain.Max);
            }
        }

    }
}
