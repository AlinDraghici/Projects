using System;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;

namespace MonitorizareMicrofon
{
    public partial class Form1 : Form
    {
        private Timer timer;
        public string lastcheck = "";

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // Interval de 1 secundă
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckMicrophoneUsage();
        }

        private void CheckMicrophoneUsage()
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            foreach (var device in devices)
            {
                var sessions = device.AudioSessionManager.Sessions;

                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];

                    if (session.State == AudioSessionState.AudioSessionStateActive)
                    {
                        var processID = session.GetProcessID;
                        var process = System.Diagnostics.Process.GetProcessById((int)processID);

                        if (!lastcheck.Equals(process.ProcessName))
                        {
                            lastcheck = process.ProcessName;
                            MessageBox.Show($"Microfonul este utilizat de către aplicația: {process.ProcessName}");
                        }
                    }
                }
            }
        }


    }
}
