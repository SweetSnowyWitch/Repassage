using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    public class Beginning
    {
        public void Start(Form gameForm)
        {
            var beginText = new TextBox();
            var fileReader = new FileReader();
            beginText.Visible = false;
            beginText.WordWrap = true;
            beginText.Multiline = true;
            beginText.ReadOnly = true;
            beginText.Font = new Font("Bad Script", 26);
            beginText.BackColor = Color.Gray;
            beginText.Location = new Point(450, 0);
            beginText.ScrollBars = ScrollBars.Vertical;
            beginText.ClientSize = new Size(1000, 1080);
            beginText.Text = File.ReadAllText(fileReader.GetFilePath(@"Cutscenes\Texts\Beginning.txt"));
            gameForm.Controls.Add(beginText);

            var beginBackground = new PictureBox();
            beginBackground.Image = Resources.Beginning;
            beginBackground.Size = Resources.Beginning.Size;
            beginBackground.Visible = true;
            gameForm.Controls.Add(beginBackground);

            var timer = new Timer();
            var timerValue = 0;
            timer.Interval = 30;
            timer.Enabled = true;

            timer.Tick += (sender, args) =>
            {
                timerValue++;

                if (timerValue >= timer.Interval)
                {
                    beginText.Visible = true;
                    timer.Stop();
                }
            };

            beginText.Click += (sender, args) =>
            {
                gameForm.Controls.Remove(beginText);
                gameForm.Controls.Remove(beginBackground);
            };
        }
    }
}