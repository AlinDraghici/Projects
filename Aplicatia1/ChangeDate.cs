using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicatia1
{
    public partial class ChangeDate : Form
    {
        public ChangeDate()
        {
            InitializeComponent();
        }

        DateTime data = DateTime.Now.Date;


        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
            regkey.SetValue("sShortDate", "dd/MM/yy");
            regkey.SetValue("sLongDate", "dd/MM/yyyy");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
            regkey.SetValue("sShortDate", "MM/yy/dd");
            regkey.SetValue("sLongDate", "MM/yyyy/dd");
        }
    }
}
