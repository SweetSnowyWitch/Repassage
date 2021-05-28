using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repassage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Icon = Resources.Icon;
            Text = "Repassage";
            MaximizeBox = false;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackgroundImage = Resources.Background;

            var begin = new Beginning();
            var game = new GameModel(this);
            
            begin.Start(this);
            game.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
