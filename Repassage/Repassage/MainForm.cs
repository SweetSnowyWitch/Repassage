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
            MaximizeBox = false;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackgroundImage = Resources.Background;

            var game = new GameModel();
            game.Start(this);
        }
    }
}
