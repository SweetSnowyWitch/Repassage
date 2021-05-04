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
            var order = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, new Size(1000, 840));
            
            AddResource(new Point(20, -1), new Point(180, 10), Resources.AriaIcon, ariaPower.Amount);
            AddResource(new Point(300, -1), new Point(450, 10), Resources.LoyaltyIcon, loyalty.Amount);
            AddResource(new Point(580, -1), new Point(730, 10), Resources.MoneyIcon, money.Amount);
            AddResource(new Point(1020, -1), new Point(1170, 10), Resources.EquipmentIcon, equipment.Amount);
            AddResource(new Point(1320, -1), new Point(1470, 10), Resources.MedicineIcon, medicine.Amount);
            AddResource(new Point(1580, -1), new Point(1730, 10), Resources.ArmyIcon, armyTotalAmount);
            AddImage(new Point(0, 0), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);
            AddImage(new Point(0, 960), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);

            var letters = GetLetters(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\FirstWeek.txt");
            var currentLetter = new TextBox();
            currentLetter.Visible = false;
            currentLetter.WordWrap = true;
            currentLetter.Multiline = true;
            currentLetter.ReadOnly = true;
            currentLetter.Font = new Font("Bad Script", 26);
            currentLetter.BackColor = Color.AntiqueWhite;
            currentLetter.Location = new Point(450, 100);
            currentLetter.ScrollBars = ScrollBars.Vertical;
            currentLetter.ClientSize = new Size(1000, 840);
            currentLetter.Text = letters.First();
            letters = letters.Skip(1);         
            gameForm.Controls.Add(currentLetter);

            var shop = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, new Size(1000, 840));
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
                currentLetter.Text = letters.FirstOrDefault();
                letters = letters.Skip(1);
            };

            archiveIcon.Click += (sender, args) => OtherBoxesUnvisible(archive, default);
            orderIcon.Click += (sender, args) => OtherBoxesUnvisible(order, default);
            shopMenu.Click += (sender, args) => OtherBoxesUnvisible(shop, default);
            lettersTable.Click += (sender, args) =>
            {
                if (currentLetter.Text.Length > 0) OtherBoxesUnvisible(default, currentLetter);
            };

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

        private IEnumerable<string> GetLetters(string scenarioPath)
        {
            var text = File.ReadAllText(scenarioPath).Split(' ');
            var isLetterFirst = true;
            var newLetter = new StringBuilder();

            foreach (var word in text)
            {
                //if (word[0].Equals('(')) - проверка на адресата письма

                if (word[0].Equals('(') && isLetterFirst)
                {
                    isLetterFirst = false;
                    continue;
                }

                if (!word[0].Equals('('))
                    newLetter.Append(word + " ");

                else
                {
                    yield return newLetter.ToString();
                    newLetter = new StringBuilder();
                }
            }
        }

        private IEnumerable<string> GetPhrases(string phrasesPath)
        {

            return default;
        }
    }
}
