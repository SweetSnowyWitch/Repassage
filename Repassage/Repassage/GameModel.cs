using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    class GameModel
    {
        private Form gameForm = new Form();
        private AriaPower ariaPower = new AriaPower();
        private Money money = new Money();
        private Loyalty loyalty = new Loyalty();
        private Equipment equipment = new Equipment();
        private Medicine medicine = new Medicine();
        private Army.Riflemen riflemen = new Army.Riflemen();
        private Army.Horsemen horsemen = new Army.Horsemen();
        private Army.Infantrymen infantrymen = new Army.Infantrymen();
        private Army.Servicemen servicemen = new Army.Servicemen();

        public GameModel(Form newForm)
        {
            gameForm = newForm;
            ariaPower.Amount = 10;
            loyalty.Amount = 10;
            money.Amount = 100;
        }

        public void Start()
        {
            DrawGame(riflemen.Amount + horsemen.Amount + infantrymen.Amount + servicemen.Amount);
        }

        private void AddArchiveButton(PictureBox mainArchive, PictureBox localArchive, Point buttonLocation, string buttonText)
        {
            var archiveButton = AddButton(mainArchive, buttonLocation, buttonText);
            localArchive.Parent = mainArchive;
            localArchive.BringToFront();
            archiveButton.Click += (sender, args) => localArchive.Visible = true;
            localArchive.Click += (sender, args) => localArchive.Visible = false;
        }

        private void AddLetter(PictureBox parent, Point buttonLocation, int number, string letterText)
        {
            var letter = AddTextBox(new Point(0, 0), new Size(1000, 840), false, letterText);
            letter.Parent = parent;
            letter.BringToFront();
            letter.Click += (sender, args) => letter.Visible = false;

            var letterButton = AddButton(parent, buttonLocation, String.Format("Письмо {0}", number));
            letterButton.Click += (sender, args) => letter.Visible = true;
        }

        private void AddResource(Point iconLocation, Point amountLocation, Image imageIcon, int amount)
        {
            var resourceAmount = new TextBox();

            resourceAmount.ReadOnly = true;
            resourceAmount.BackColor = Color.AntiqueWhite;
            resourceAmount.Location = amountLocation;
            resourceAmount.Text = amount.ToString();
            resourceAmount.Font = new Font("Arial", 32);
            AddImage(iconLocation, imageIcon, true, Color.Transparent, imageIcon.Size);
            gameForm.Controls.Add(resourceAmount);
        }

        private void AddTrackBar(Control parent, Point labelLocation, Point trackBarLocation, string phrase, int maxAmount)
        {
            var trackLabel = new Label();

            trackLabel.Parent = parent;
            trackLabel.Location = labelLocation;
            trackLabel.ClientSize = new Size(900, 50);
            trackLabel.Font = new Font("Arial", 18);
            trackLabel.Text = String.Format(phrase, 0);

            var trackBar = new TrackBar();

            trackBar.Parent = parent;
            trackBar.Location = trackBarLocation;
            trackBar.ClientSize = new Size(900, 100);
            trackBar.Maximum = maxAmount;
            trackBar.Scroll += (sender, args) =>
            trackLabel.Text = String.Format(phrase, trackBar.Value);
        }

        private TextBox AddTextBox(Point location, Size size, bool isVisible, string text)
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

        private Button AddButton(PictureBox parent, Point location, string title)
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

        private PictureBox AddImage(Point location, Image image, bool isVisible, Color backColor, Size imageSize)
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

        private void DrawGame(int armyTotalAmount)
        {
            var characterText = AddImage(new Point(500, 820), Resources.Textbox, false, Color.Transparent, Resources.Textbox.Size);
            var archiveIcon = AddImage(new Point(300, 960), Resources.ArchiveIcon, true, Color.Transparent, Resources.ArchiveIcon.Size);
            var orderIcon = AddImage(new Point(1450, 960), Resources.OrderIcon, true, Color.Transparent, Resources.OrderIcon.Size);
            var archive = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, new Size(1000, 840));
            
            var ariaArchive = AddImage(new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            AddArchiveButton(archive, ariaArchive, new Point(50, 50), "Письма Арии");
            var ariaLettersCounter = 1;

            var warArchive = AddImage(new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            AddArchiveButton(archive, warArchive, new Point(50, 175), "Приказы Военного Совета");
            var warLettersCounter = 1;

            var scoutArchive = AddImage(new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            AddArchiveButton(archive, scoutArchive, new Point(50, 300), "Информация от разведки");
            var scoutLettersCounter = 1;

            var friendsArchive = AddImage(new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            AddArchiveButton(archive, friendsArchive, new Point(50, 425), "Личные сообщения");
            var friendsLettersCounter = 1;

            var infoArchive = AddImage(new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            AddArchiveButton(archive, infoArchive, new Point(50, 550), "Доклады о состоянии роты");
            var infoLettersCounter = 1;

            var order = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, archive.Size);

            AddResource(new Point(20, -1), new Point(180, 10), Resources.AriaIcon, ariaPower.Amount);
            AddResource(new Point(300, -1), new Point(450, 10), Resources.LoyaltyIcon, loyalty.Amount);
            AddResource(new Point(580, -1), new Point(730, 10), Resources.MoneyIcon, money.Amount);
            AddResource(new Point(1020, -1), new Point(1170, 10), Resources.EquipmentIcon, equipment.Amount);
            AddResource(new Point(1320, -1), new Point(1470, 10), Resources.MedicineIcon, medicine.Amount);
            AddResource(new Point(1580, -1), new Point(1730, 10), Resources.ArmyIcon, armyTotalAmount);
            AddImage(new Point(0, 0), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);
            AddImage(new Point(0, 960), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);

            var rawLetters = GetLetters(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\FirstWeek.txt");
            var currentLetter = AddTextBox(new Point(450, 100), archive.Size, false, rawLetters.First().Value);
            AddLetterToArchive(rawLetters);
            var letters = rawLetters.Skip(1);
            gameForm.Controls.Add(currentLetter);

            var shop = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, archive.Size);
            AddTrackBar(shop, new Point(50, 50), new Point(50, 100), "Количество покупаемого снаряжения: {0}", 100);
            AddTrackBar(shop, new Point(50, 150), new Point(50, 200), "Количество покупаемых лекарств: {0}", 100);
            AddTrackBar(shop, new Point(50, 250), new Point(50, 300), "Количество нанимаемых людей: {0}", 100);
            AddTrackBar(shop, new Point(50, 350), new Point(50, 400), "Выделяемая Арии сумма: {0}", money.Amount);

            var lettersTable = AddImage(new Point(0, 550), Resources.Table, true, Color.Transparent, Resources.Table.Size);
            var shopMenu = AddImage(new Point(900, 400), Resources.Boxes, true, Color.Transparent, Resources.Boxes.Size);

            archive.Click += (sender, args) => archive.Visible = false;
            order.Click += (sender, args) => order.Visible = false;
            shop.Click += (sender, args) => shop.Visible = false;
            currentLetter.Click += (sender, args) =>
            {
                currentLetter.Visible = false;
                currentLetter.Text = letters.FirstOrDefault().Value;
                if (letters.Count() > 0) AddLetterToArchive(letters);
                letters = letters.Skip(1);
            };

            archiveIcon.Click += (sender, args) => OtherBoxesUnvisible(archive, default);
            orderIcon.Click += (sender, args) => OtherBoxesUnvisible(order, default);
            shopMenu.Click += (sender, args) => OtherBoxesUnvisible(shop, default);
            lettersTable.Click += (sender, args) =>
            {
                if (currentLetter.Text.Length > 0) OtherBoxesUnvisible(default, currentLetter);
            };

            void AddLetterToArchive(IEnumerable<KeyValuePair<string, string>> letters)
            {
                switch (letters.First().Key)
                {
                    case "(Ария)":
                        AddLetter(ariaArchive, new Point(50, 50 + 125 * (ariaLettersCounter - 1)), 
                            ariaLettersCounter++, currentLetter.Text);
                        break;
                    case "(Военсовет)":
                        AddLetter(warArchive, new Point(50, 50 + 125 * (warLettersCounter - 1)), 
                            warLettersCounter++, currentLetter.Text);
                        break;
                    case "(Разведка)":
                        AddLetter(scoutArchive, new Point(50, 50 + 125 * (scoutLettersCounter - 1)), 
                            scoutLettersCounter++, currentLetter.Text);
                        break;                       
                    case "(Друг)":
                        AddLetter(friendsArchive, new Point(50, 50 + 125 * (friendsLettersCounter - 1)), 
                            friendsLettersCounter++, currentLetter.Text);
                        break;
                    case "(Доклад)":
                        AddLetter(infoArchive, new Point(50, 50 + 125 * (infoLettersCounter - 1)), 
                            infoLettersCounter++, currentLetter.Text);
                        break;
                }
            }

            void OtherBoxesUnvisible(PictureBox exceptPictureBox, TextBox exceptTextBox)
            {
                archive.Visible = false;
                order.Visible = false;
                currentLetter.Visible = false;
                shop.Visible = false;
                if (exceptPictureBox != null) exceptPictureBox.Visible = true;
                if (exceptTextBox != null) exceptTextBox.Visible = true;
            }
        }

        private Dictionary<string, string> GetLetters(string scenarioPath)
        {
            var text = File.ReadAllText(scenarioPath).Split(' ');
            var letters = new Dictionary<string, string>();
            var isLetterFirst = true;
            var newSender = "";
            var newLetter = new StringBuilder();

            foreach (var word in text)
            {
                if (word[0].Equals('('))
                {
                    if (isLetterFirst)
                    {
                        newSender = word;
                        isLetterFirst = false;
                        continue;
                    }

                    letters.Add(newSender, newLetter.ToString());
                    newLetter = new StringBuilder();
                    newSender = word;
                }

                else newLetter.Append(word + " ");
            }

            return letters;
        }
    }
}