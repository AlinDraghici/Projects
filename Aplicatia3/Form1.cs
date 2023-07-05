using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Data.Entity.Core.Objects;

namespace Aplicatie3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadInstalledApplications();
        }

        private void LoadInstalledApplications()
        {
            try
            {
                // Creați o interogare WMI pentru a obține informații despre aplicații instalate
                var query = new ObjectQuery("SELECT * FROM Win32_Product");
                var searcher = new ManagementObjectSearcher(query);
                var results = searcher.Get();

                // Parcurgeți rezultatele și afișați numele aplicațiilor în panel
                foreach (ManagementObject obj in results)
                {
                    string name = obj["Name"]?.ToString();
                    appPanel.Controls.Add(new Label() { Text = name });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("A apărut o eroare la încărcarea aplicațiilor instalate: " + ex.Message);
            }
        }

    }
}
