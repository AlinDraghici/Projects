using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Aplicatia4
{
    public partial class Form1 : Form
    {
        private string appPath;
        private DateTime lastModificationTime = DateTime.Now;
        private FileSystemWatcher watcher;
        int counter = 0;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            appPath = AppDomain.CurrentDomain.BaseDirectory;
            watcher = new FileSystemWatcher(); //a creat obiect
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            watcher.Filter = "*.txt";
            watcher.Path = appPath;
            watcher.EnableRaisingEvents = true;

            CheckTextFilesCount();
            UpdateListBox();
            RegisterTaskInScheduler();

            timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckTextFilesCount();
            UpdateListBox();
            CheckLastModificationTime();
        }

        private void CheckTextFilesCount()
        {
            string[] files = Directory.GetFiles(appPath);
            int txtFilesCount = files.Count(file => Path.GetExtension(file) == ".txt");

            if (txtFilesCount >= 3)
            {
                MessageBox.Show("Sunt 3 sau mai multe fisiere .txt!");
            }
            else
            {
                MessageBox.Show("Sunt mai puțin de 3 fișiere .txt!");
            }
        }

        private void UpdateListBox()
        {
            string[] files = Directory.GetFiles(appPath);

            listBox1.Items.Clear();
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                listBox1.Items.Add(fileName);
            }
        }

        private void RegisterTaskInScheduler()
        {
            using (TaskService taskService = new TaskService())
            {

                string taskName = "Aplicatia4"; // Numele sarcinii în Task Scheduler

                if (taskService.GetTask(taskName) != null)
                {
                    taskService.RootFolder.DeleteTask(taskName, false);
                }

                TaskDefinition taskDefinition = taskService.NewTask();

                taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;

                taskDefinition.RegistrationInfo.Description = "Test";

                LogonTrigger logonTrigger = new LogonTrigger();
                logonTrigger.UserId = Environment.UserName;

                //logonTrigger.Delay = TimeSpan.FromMinutes(1);

                if (taskService.GetTask(taskName) != null)
                {
                    taskService.RootFolder.DeleteTask(taskName);
                }

                taskDefinition.Triggers.Add(logonTrigger);

                taskDefinition.Actions.Add(new ExecAction(Application.ExecutablePath));

                taskService.RootFolder.RegisterTaskDefinition(taskName, taskDefinition);

                taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;

            }

        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                counter++;
                string message = "A mai fost creat un file, iar acum avem cu " + counter + " fisier de tip .txt mai mult";
                MessageBox.Show(message, "File Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lastModificationTime = DateTime.Now;
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                counter--;
                string message = "S-a șters un fișier txt. Acum avem cu " + counter + " fișier cu terminația .txt mai putin";
                MessageBox.Show(message, "Fișier șters", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lastModificationTime = DateTime.Now;
            }
        }

        private void CheckLastModificationTime()
        {
            string registryKeyPath = @"SOFTWARE\Aplicatia4";
            string lastModificationValueName = "LastModificationTime";

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(registryKeyPath, true);

            if (registryKey == null)
            {
                registryKey = Registry.CurrentUser.CreateSubKey(registryKeyPath);
                registryKey.SetValue(lastModificationValueName, DateTime.Now.ToString());
                lastModificationTime = DateTime.Now;
            }
            else
            {
                string time = registryKey.GetValue(lastModificationValueName) as string;

                if (time != null)
                {
                    DateTime storedTime = DateTime.Parse(time);
                    if (DateTime.Equals(lastModificationTime, storedTime))
                    {
                        registryKey.SetValue(lastModificationValueName, lastModificationTime.ToString());
                    }
                    else
                    {
                        TimeSpan timeSinceLastModification = DateTime.Now - lastModificationTime;
                        if (timeSinceLastModification.TotalHours >= 12)
                        {
                            MessageBox.Show("Nu a fost efectuată nicio modificare în fișier!");
                            lastModificationTime = DateTime.Now;
                            registryKey.SetValue(lastModificationValueName, DateTime.Now.ToString());
                        }
                    }
                }
                else
                {
                    lastModificationTime = DateTime.Now;
                    registryKey.SetValue(lastModificationValueName, DateTime.Now.ToString());
                }
            }
        }
    }
}