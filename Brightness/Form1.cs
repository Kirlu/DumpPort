using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace Brightness
{
    public partial class Form1 : Form
    {
        private Monitor _currentMonitor;
        private readonly MonitorCollection _monitorCollection = new MonitorCollection();
        public Form1()
        {
            InitializeComponent();
            trackBar1.ValueChanged += trackBar1_ValueChanged;
            var @delegate = new NativeMethods.MonitorEnumDelegate(MonitorEnum);
            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, @delegate, IntPtr.Zero);
        }

        void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            brightValue.Text = trackBar1.Value.ToString();
            UpdateScreenWithBarValue((uint) trackBar1.Value);
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
    }
}
