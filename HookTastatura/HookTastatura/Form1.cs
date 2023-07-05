using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HookTastatura
{
    public partial class Form1 : Form
    {
        private TextBox textBox;

        public Form1()
        {
            InitializeComponents();
        }
        private void InitializeComponents()
        {
            textBox = new TextBox();
            textBox.Dock = DockStyle.Fill;
            textBox.Enabled = false;

            Controls.Add(textBox);

            Width = 300;
            Height = 200;
        }

        public static void WriteToTextBox(string text)
        {
            if (Application.OpenForms.Count > 0)
            {
                Form1 form1 = Application.OpenForms[0] as Form1;
                form1.Invoke(new Action(() =>
                {
                    form1.textBox.AppendText(text);
                }));
            }
        }
    }
    
}
