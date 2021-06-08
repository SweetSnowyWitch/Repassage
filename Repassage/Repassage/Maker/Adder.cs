using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    public class Adder
    {
        public TextBox AddPrompt(Form gameForm, Control parent, Point location, string promptText)
        {
            var prompt = AddTextBox(gameForm, location, new Size(300, 250), false, promptText);
            prompt.ScrollBars = ScrollBars.None;
            prompt.Font = new Font("Arial", 14);
            parent.MouseEnter += (sender, args) => prompt.Visible = true;
            prompt.MouseLeave += (sender, args) => prompt.Visible = false;

            return prompt;
        }

        public TextBox AddResource(Form gameForm, Point iconLocation, Point amountLocation, Image imageIcon, int amount, string prompt)
        {
            var resourceAmount = AddTextBox(gameForm, amountLocation, new Size(amount.ToString().Length * 35, 50), true, amount.ToString());
            var resourceImage = AddImage(gameForm, iconLocation, imageIcon, true, Color.Transparent, imageIcon.Size);
            var resourcePrompt = AddPrompt(gameForm, resourceImage, iconLocation, prompt);
            
            resourcePrompt.BringToFront();
            resourceAmount.Multiline = false;
            resourceAmount.Font = new Font("Arial", 32);

            return resourceAmount;
        }

        public Label AddLabel(Control parent, Point labelLocation, string phrase)
        {
            var newLabel = new Label();
            newLabel.Parent = parent;
            newLabel.Location = labelLocation;
            newLabel.ClientSize = new Size(900, 30);
            newLabel.Font = new Font("Arial", 14);
            newLabel.Text = String.Format(phrase, 0);

            return newLabel;
        }

        public TrackBar AddTrackBar(Control parent, Point labelLocation,
            Point trackBarLocation, string phrase, int maxAmount)
        {           
            var trackValueLabel = AddLabel(parent, labelLocation, phrase);
            var trackBar = new TrackBar();

            trackBar.Parent = parent;
            trackBar.Location = trackBarLocation;
            trackBar.ClientSize = new Size(900, 100);
            trackBar.Maximum = maxAmount;
            trackBar.Scroll += (sender, args) =>
            {
                trackValueLabel.Text = String.Format(phrase, trackBar.Value);
            };

            return trackBar;
        }

        public TextBox AddTextBox(Form gameForm, Point location, Size size, bool isVisible, string text)
        {
            var newTextBox = new TextBox();
            newTextBox.Visible = isVisible;
            newTextBox.WordWrap = true;
            newTextBox.Multiline = true;
            newTextBox.ReadOnly = true;
            newTextBox.Font = new Font("Bad Script", 26);
            newTextBox.BackColor = Color.AntiqueWhite;
            newTextBox.Location = location;
            newTextBox.ScrollBars = ScrollBars.Vertical;
            newTextBox.ClientSize = size;
            newTextBox.Text = text;
            gameForm.Controls.Add(newTextBox);

            return newTextBox;
        }

        public Button AddButton(PictureBox parent, Point location, string title)
        {
            var newButton = new Button();
            newButton.Parent = parent;
            newButton.Text = title;
            newButton.Font = new Font("Arial", 20);
            newButton.BackColor = Color.Bisque;
            newButton.ClientSize = new Size(900, 100);
            newButton.Location = location;

            return newButton;
        }

        public PictureBox AddImage(Form gameForm, Point location, Image image, bool isVisible, Color backColor, Size imageSize)
        {
            var gameImage = new PictureBox();
            gameImage.Visible = isVisible;
            gameImage.Image = image;
            gameImage.BackColor = backColor;
            gameImage.Location = location;
            if (!imageSize.IsEmpty) gameImage.ClientSize = imageSize;
            gameForm.Controls.Add(gameImage);
            return gameImage;
        }

        public void AddArchiveButton(PictureBox mainArchive, PictureBox localArchive, Point buttonLocation, string buttonText)
        {
            var archiveButton = AddButton(mainArchive, buttonLocation, buttonText);
            localArchive.Parent = mainArchive;
            localArchive.BringToFront();
            archiveButton.Click += (sender, args) => localArchive.Visible = true;
            localArchive.Click += (sender, args) => localArchive.Visible = false;
        }

        public void AddLetter(Form gameForm, PictureBox parent, Point buttonLocation, int number, string letterText)
        {
            var letter = AddTextBox(gameForm, new Point(0, 0), new Size(1000, 840), false, letterText);
            letter.Parent = parent;
            letter.BringToFront();
            letter.Click += (sender, args) => letter.Visible = false;

            var letterButton = AddButton(parent, buttonLocation, String.Format("Письмо {0}", number));
            letterButton.Click += (sender, args) => letter.Visible = true;
        }

        public void AddLetterToArchive(ref PictureBox ariaArchive, ref PictureBox warArchive, ref PictureBox scoutArchive,
            ref PictureBox friendsArchive, ref PictureBox infoArchive, ref int ariaLettersCounter, ref int warLettersCounter, 
            ref int scoutLettersCounter, ref int friendsLettersCounter, ref int infoLettersCounter, Form gameForm, 
            string letterKey, string letterText)
        {
            switch (letterKey)
            {
                case "(Ария)":
                    AddLetter(gameForm, ariaArchive, new Point(50, 50 + 125 * (ariaLettersCounter - 1)),
                        ariaLettersCounter++, letterText);
                    break;
                case "(Военсовет)":
                    AddLetter(gameForm, warArchive, new Point(50, 50 + 125 * (warLettersCounter - 1)),
                        warLettersCounter++, letterText);
                    break;
                case "(Разведка)":
                    AddLetter(gameForm, scoutArchive, new Point(50, 50 + 125 * (scoutLettersCounter - 1)),
                        scoutLettersCounter++, letterText);
                    break;
                case "(Друг)":
                    AddLetter(gameForm, friendsArchive, new Point(50, 50 + 125 * (friendsLettersCounter - 1)),
                        friendsLettersCounter++, letterText);
                    break;
                case "(Доклад)":
                    AddLetter(gameForm, infoArchive, new Point(50, 50 + 125 * (infoLettersCounter - 1)),
                        infoLettersCounter++, letterText);
                    break;
            }
        }
    }
}
