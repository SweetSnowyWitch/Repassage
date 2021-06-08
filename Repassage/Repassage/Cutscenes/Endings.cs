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
            Resources.Riot_Ending, "Riot Ending.txt", "Оплата труда солдат объязательна, не стоит забывать о ней");

        public void Criminal(ref bool isNotEnded, Form gameForm, string message) => CreateEnding(ref isNotEnded, gameForm, 
            Color.DarkSalmon, Resources.Criminal_Ending, "Criminal Ending.txt", message);

        public void DeathInBatte(ref bool isNotEnded, Form gameForm, string message) => CreateEnding(ref isNotEnded, gameForm, 
            Color.RosyBrown, Resources.DeathInBattle_Ending, "DeathInBattle Ending.txt", message);

        public void Triumph(ref bool isNotEnded, Form gameForm) => CreateEnding(ref isNotEnded, gameForm, Color.BlanchedAlmond,
            Resources.Triumph_Ending, "Triumph Ending.txt", "Поздравляем, Вы победили!");

        private void CreateEnding(ref bool isNotEnded, Form gameForm, Color backColor, 
            Bitmap backImage, string endingTextName, string message)
        {
            var adder = new Adder();
            var endingMessage = adder.AddTextBox(gameForm, new Point(450, 500), new Size(1000, 100), false, message);
            endingMessage.ScrollBars = ScrollBars.None;
            endingMessage.Font = new Font("Arial", 20);
            endingMessage.BackColor = backColor;
            endingMessage.Click += (sender, args) => gameForm.Close();

            var fileReader = new FileReader();
            var endingText = adder.AddTextBox(gameForm, new Point(450, 0), new Size(1000, 1080),
                false, File.ReadAllText(fileReader.GetFilePath(@"Cutscenes\Texts\" + endingTextName)));
            endingText.Font = new Font("Arial", 20);
            endingText.BackColor = backColor;
            endingText.Click += (sender, args) =>
            {
                endingMessage.Visible = true;
                endingText.Visible = false;
            };

            var endingBackground = adder.AddImage(gameForm, new Point(0, 0), backImage, 
                true, Color.Transparent, backImage.Size);

            isNotEnded = false;
            for (var i = 0; i < 10; i++)
            {
                endingBackground.BringToFront();
                endingText.BringToFront();
                endingMessage.BringToFront();
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