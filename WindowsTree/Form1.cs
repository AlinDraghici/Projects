using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsTree
{
    public partial class Form1 : Form
    {
        private ListBox listBox;
        private int maxDepth = 3;

        public Form1()
        {
            InitializeComponent();
            DirectoryTreeForm();
        }

        public void DirectoryTreeForm()
        {
            listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            Controls.Add(listBox);

            string rootPath = $"C:\\Users\\{System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
            DisplayDirectory(rootPath, 0);
        }

        private void DisplayDirectory(string directoryPath, int depth)
        {
            if (depth >= maxDepth)
            {
                return;
            }

            string indent = new string(' ', depth * 2);
            listBox.Items.Add(indent + directoryPath);

            try
            {
                string[] files = Directory.GetFiles(directoryPath);
                foreach (string file in files)
                {
                    listBox.Items.Add(indent + "  " + Path.GetFileName(file));
                }

                string[] subdirectories = Directory.GetDirectories(directoryPath);
                foreach (string subdirectory in subdirectories)
                {
                    DisplayDirectory(subdirectory, depth + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                listBox.Items.Add(indent + "  Acces neautorizat");
            }
            catch (Exception)
            {
                listBox.Items.Add(indent + "  Eroare la accesarea directorului");
            }
        }
    }
}
