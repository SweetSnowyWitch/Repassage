using System;
using System.Collections.Generic;
using System.Drawing;
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
            ariaPower.Amount = 100;
            loyalty.Amount = 100;
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
            AddImage(iconLocation, imageIcon);
            gameForm.Controls.Add(resourceAmount);
        }

        private PictureBox AddImage(Point location, Image image)
        {
            var gameImage = new PictureBox();
            gameImage.Image = image;
            gameImage.BackColor = Color.Transparent;
            gameImage.Location = location;
            if (image != null) gameImage.ClientSize = image.Size;
            gameForm.Controls.Add(gameImage);
            return gameImage;
        }

        private void DrawGame(int armyTotalAmount)
        {
            AddResource(new Point(20, -1), new Point(180, 10), Resources.AriaIcon, ariaPower.Amount);
            AddResource(new Point(300, -1), new Point(450, 10), Resources.LoyaltyIcon, loyalty.Amount);
            AddResource(new Point(580, -1), new Point(730, 10), Resources.MoneyIcon, money.Amount);
            AddResource(new Point(1020, -1), new Point(1170, 10), Resources.EquipmentIcon, equipment.Amount);
            AddResource(new Point(1320, -1), new Point(1470, 10), Resources.MedicineIcon, medicine.Amount);
            AddResource(new Point(1580, -1), new Point(1730, 10), Resources.ArmyIcon, armyTotalAmount);

            var characterText = AddImage(new Point(500, 820), Resources.Textbox);
            characterText.Visible = false;

            var archive = AddImage(new Point(300, 960), Resources.ArchiveIcon);
            var order = AddImage(new Point(1450, 960), Resources.OrderIcon);

            AddImage(new Point(0,0), Resources.MainFrame);
            AddImage(new Point(0, 960), Resources.MainFrame);

            var letter = new TextBox();
            letter.Visible = false;
            letter.WordWrap = true;
            letter.Multiline = true;
            letter.ReadOnly = true;
            letter.Text = "Тестовый текст";
            letter.Font = new Font("Honey ScriptCYR", 48);
            letter.BackColor = Color.AntiqueWhite;
            letter.Location = new Point(450, 100);
            letter.ScrollBars = ScrollBars.Vertical;
            letter.ClientSize = new Size(1000, 840);
            letter.Click += (sender, args) => letter.Visible = false;
            gameForm.Controls.Add(letter);

            var shop = AddImage(new Point(450, 100), default);
            shop.Visible = false;
            shop.BackColor = Color.AntiqueWhite;
            shop.ClientSize = new Size(1000, 840);
            shop.Click += (sender, args) => shop.Visible = false;

            var lettersTable = AddImage(new Point(0, 550), Resources.Table);
            lettersTable.Click += (sender, args) => letter.Visible = true;

            var shopMenu = AddImage(new Point(900, 400), Resources.Boxes);
            shopMenu.Click += (sender, args) => shop.Visible = true;
        }

        public void UpdateLetters(int week, int route)
        {

        }
    }
}
