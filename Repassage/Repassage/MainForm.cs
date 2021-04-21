using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repassage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Text = "Repassage";
            Size = new Size(1920, 1080);
            MaximizeBox = false;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
