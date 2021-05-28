using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    public class Endings
    {
        public void Riot(ref bool isNotEnded, Form gameForm) => CreateEnding(ref isNotEnded, gameForm, Color.Silver,
            Resources.Riot_Ending, @"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Cutscenes\Texts\Riot Ending.txt");

        public void Criminal(ref bool isNotEnded, Form gameForm) => CreateEnding(ref isNotEnded, gameForm, Color.DarkSalmon,
            Resources.Criminal_Ending, @"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Cutscenes\Texts\Criminal Ending.txt");

        public void DeathInBatte(ref bool isNotEnded, Form gameForm) => CreateEnding(ref isNotEnded, gameForm, Color.RosyBrown,
            Resources.DeathInBattle_Ending, @"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Cutscenes\Texts\DeathInBattle Ending.txt");

        public void Triumph(ref bool isNotEnded, Form gameForm) => CreateEnding(ref isNotEnded, gameForm, Color.BlanchedAlmond,
            Resources.Triumph_Ending, @"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Cutscenes\Texts\Triumph Ending.txt");

        private void CreateEnding(ref bool isNotEnded, Form gameForm, Color backColor, Bitmap backImage, string endingPath)
        {
            var endingText = new TextBox();
            endingText.Visible = false;
            endingText.WordWrap = true;
            endingText.Multiline = true;
            endingText.ReadOnly = true;
            endingText.Font = new Font("Arial", 20);
            endingText.BackColor = backColor;
            endingText.Location = new Point(450, 0);
            endingText.ClientSize = new Size(1000, 1080);
            endingText.Text = File.ReadAllText(endingPath);
            endingText.Click += (sender, args) => gameForm.Close();
            gameForm.Controls.Add(endingText);

            var endingBackground = new PictureBox();
            endingBackground.Image = backImage;
            endingBackground.Size = backImage.Size;
            endingBackground.Visible = true;
            gameForm.Controls.Add(endingBackground);

            isNotEnded = false;
            for (var i = 0; i < 10; i++)
            {
                endingBackground.BringToFront();
                endingText.BringToFront();
            }

            var timer = new Timer();
            var timerValue = 0;
            timer.Interval = 20;
            timer.Enabled = true;

            timer.Tick += (sender, args) =>
            {
                timerValue++;

                if (timerValue >= timer.Interval)
                {
                    endingText.Visible = true;
                    timer.Stop();
                }
            };            
        }
    }
}
