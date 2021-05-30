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
        private Riflemen riflemen = new Riflemen();
        private Horsemen horsemen = new Horsemen();
        private Infantrymen infantrymen = new Infantrymen();
        private Servicemen servicemen = new Servicemen();
        private List<String> scenariosPaths = new List<String>();
        private List<String> phrasesPaths = new List<String>();
        private int salary;

        public GameModel(Form newForm)
        {
            var counter = new Counter();
            gameForm = newForm;
            counter.CountResources(ref ariaPower, ref loyalty, ref riflemen, ref horsemen,
                ref infantrymen, ref servicemen, ref money, ref salary);
            scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\First Week\FirstWeek.txt");
            phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\FirstWeekPhrases.txt");
        }

        public void Start()
        {
            DrawGame(riflemen.Amount + horsemen.Amount + infantrymen.Amount + servicemen.Amount,
                scenariosPaths.First(), phrasesPaths.First());
        }

        private void DrawGame(int armyTotalAmount, string scenarioPath, string phrasesPath)
        {
            var end = new Endings();
            var routes = new Routes();
            var adder = new Adder();
            var updater = new Updater();
            var counter = new Counter();
            var orderMaker = new OrderMaker();
            var fileReader = new FileReader();
            var isBattleToday = true;
            var isNotEnded = true;
            var weekCounter = 10;
            var battleCounter = 0;

            var characterText = adder.AddImage(gameForm, new Point(500, 820), Resources.Textbox,
                true, Color.Transparent, Resources.Textbox.Size);

            var phrases = fileReader.GetTexts(phrasesPath).Skip(0);
            var currentPhrase = adder.AddTextBox(gameForm, new Point(20, 20),
                new Size(Resources.Textbox.Width - 40, Resources.Textbox.Height - 40), true, phrases.First().Value);
            currentPhrase.BringToFront();
            currentPhrase.Parent = characterText;
            currentPhrase.ScrollBars = ScrollBars.None;

            if (phrases.First().Key.Equals("(System)"))
                currentPhrase.Font = new Font("Arial", 14);
            else
                currentPhrase.Font = new Font("Arial", 16);

            var weekend = adder.AddImage(gameForm, new Point(0, 0), default, false, Color.AntiqueWhite, new Size(1920, 1080));
            var endLabel = adder.AddLabel(weekend, new Point(0, 0), "");
            var endProgress = new ProgressBar();
            endProgress.Parent = weekend;
            endProgress.Size = new Size(750, 50);
            endProgress.Location = new Point(600, 700);

            var endTimer = new Timer();
            endTimer.Interval = 1000;
            endTimer.Enabled = true;
            endTimer.Tick += (sender, args) =>
            {
                if (endProgress.Value == endProgress.Maximum)
                {
                    weekend.Visible = false;
                    endProgress.Value = 0;
                }
                endProgress.PerformStep();
            };

            var orderIcon = adder.AddImage(gameForm, new Point(1450, 960), Resources.OrderIcon,
                true, Color.Transparent, Resources.OrderIcon.Size);
            var archiveIcon = adder.AddImage(gameForm, new Point(300, 960), Resources.ArchiveIcon,
                true, Color.Transparent, Resources.ArchiveIcon.Size);
            var archive = adder.AddImage(gameForm, new Point(450, 100), default, false, Color.AntiqueWhite, new Size(1000, 840));
            var ariaArchive = adder.AddImage(gameForm, new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            adder.AddArchiveButton(archive, ariaArchive, new Point(50, 50), "Письма Арии");
            var ariaLettersCounter = 1;

            var warArchive = adder.AddImage(gameForm, new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            adder.AddArchiveButton(archive, warArchive, new Point(50, 175), "Приказы Военного Совета");
            var warLettersCounter = 1;

            var scoutArchive = adder.AddImage(gameForm, new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            adder.AddArchiveButton(archive, scoutArchive, new Point(50, 300), "Информация от разведки");
            var scoutLettersCounter = 1;

            var friendsArchive = adder.AddImage(gameForm, new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            adder.AddArchiveButton(archive, friendsArchive, new Point(50, 425), "Личные сообщения");
            var friendsLettersCounter = 1;

            var infoArchive = adder.AddImage(gameForm, new Point(0, 0), default, false, Color.AntiqueWhite, archive.Size);
            adder.AddArchiveButton(archive, infoArchive, new Point(50, 550), "Доклады о состоянии роты");
            var infoLettersCounter = 1;

            var ariaResource = adder.AddResource(gameForm, new Point(20, -1),
                new Point(180, 10), Resources.AriaIcon, ariaPower.Amount);
            var loyaltyResource = adder.AddResource(gameForm, new Point(300, -1),
                new Point(450, 10), Resources.LoyaltyIcon, loyalty.Amount);
            var moneyResource = adder.AddResource(gameForm, new Point(580, -1),
                new Point(730, 10), Resources.MoneyIcon, money.Amount);
            var equipmentResource = adder.AddResource(gameForm, new Point(1020, -1),
                new Point(1170, 10), Resources.EquipmentIcon, equipment.Amount);
            var medicineResource = adder.AddResource(gameForm, new Point(1320, -1),
                new Point(1470, 10), Resources.MedicineIcon, medicine.Amount);
            var armyResource = adder.AddResource(gameForm, new Point(1580, -1),
                new Point(1730, 10), Resources.ArmyIcon, armyTotalAmount);
            adder.AddImage(gameForm, new Point(0, 0), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);
            adder.AddImage(gameForm, new Point(0, 960), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);

            var letters = fileReader.GetTexts(scenarioPath).Skip(0);

            if (weekCounter / 10 == 1)
            {
                var firstReport = new Dictionary<string, string>();
                firstReport.Add("(Доклад)", String.Format("Доклад о составе роты \r\nЧисленность стрелков: {0}" +
                    "\r\nЧисленность кавалерии: {1}\r\nЧисленность пехоты: {2}" +
                    "\r\nЧисленность обслуживающего персонала: {3}",
                    riflemen.Amount, horsemen.Amount, infantrymen.Amount, servicemen.Amount));
                letters = letters.Concat(firstReport);
            }

            var currentLetter = adder.AddTextBox(gameForm, new Point(450, 100), archive.Size, false, letters.First().Value);
            gameForm.Controls.Add(currentLetter);

            var shop = adder.AddImage(gameForm, new Point(450, 100), default, false, Color.AntiqueWhite, archive.Size);
            var equipmentBar = adder.AddTrackBar(shop, new Point(50, 50), new Point(50, 100), "Количество покупаемого снаряжения: {0}",
                (int)Math.Floor(money.Amount / (equipment.Price * loyalty.SaleRate)));
            var medicineBar = adder.AddTrackBar(shop, new Point(50, 150), new Point(50, 200), "Количество покупаемых лекарств: {0}",
                (int)Math.Floor(money.Amount / (medicine.Price * loyalty.SaleRate)));
            var peopleBar = adder.AddTrackBar(shop, new Point(50, 250), new Point(50, 300), "Количество нанимаемых людей: {0}",
                50 * ariaPower.ConvertRate);
            var ariaBar = adder.AddTrackBar(shop, new Point(50, 350), new Point(50, 400), "Выделяемая Арии сумма: {0}", money.Amount);
            var armyBar = adder.AddTrackBar(shop, new Point(50, 450), new Point(50, 500), "Выплачиваемая роте сумма: {0}", money.Amount);
            var battleBar = adder.AddButton(shop, new Point(50, 580), "Принять участие в грядущей битве: Нет");
            peopleBar.TickFrequency = ariaPower.ConvertRate;

            var order = adder.AddImage(gameForm, new Point(450, 100), default, false, Color.AntiqueWhite, new Size(1000, 840));
            var orderText = adder.AddTextBox(gameForm, new Point(50, 50), new Size(900, 600), true, "");
            var orderButton = adder.AddButton(order, new Point(50, 680), "Отдать приказ и завершить неделю");
            orderText.Parent = order;
            updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower, ref medicine,
                ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);

            var lettersTable = adder.AddImage(gameForm, new Point(0, 550), Resources.Table, true, Color.Transparent, Resources.Table.Size);
            var shopMenu = adder.AddImage(gameForm, new Point(900, 400), Resources.Boxes, true, Color.Transparent, Resources.Boxes.Size);

            equipmentBar.Scroll += (sender, args) => updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower,
                ref medicine, ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            medicineBar.Scroll += (sender, args) => updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower,
                ref medicine, ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            peopleBar.Scroll += (sender, args) => updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower,
                ref medicine, ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            ariaBar.Scroll += (sender, args) => updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower,
                ref medicine, ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            armyBar.Scroll += (sender, args) => updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower,
                ref medicine, ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            battleBar.Click += (sender, args) =>
            {
                if (battleBar.Text.Split(' ').Last().Equals("Да"))
                    battleBar.Text = battleBar.Text.Replace("Да", "Нет");
                else if (isBattleToday)
                    battleBar.Text = battleBar.Text.Replace("Нет", "Да");

                updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower,
                ref medicine, ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            };

            archive.Click += (sender, args) => archive.Visible = false;
            order.Click += (sender, args) => order.Visible = false;
            shop.Click += (sender, args) => shop.Visible = false;
            orderButton.Click += (sender, args) => orderMaker.MakeOrder(gameForm, ref scenariosPaths, ref phrasesPaths, ref routes,
            ref end, ref letters, ref phrases, ref loyalty, ref equipment, ref ariaPower, ref ariaBar, ref equipmentBar, ref medicineBar,
            ref armyBar, ref peopleBar, ref battleBar, ref weekend, ref order, ref counter, ref updater, ref endLabel, ref medicine,
            ref money, ref ariaResource, ref riflemen, ref horsemen, ref infantrymen, ref servicemen, ref loyaltyResource, ref moneyResource,
            ref equipmentResource, ref medicineResource, ref armyResource, ref currentPhrase, ref currentLetter, ref orderText,
            ref characterText, ref isNotEnded, ref isBattleToday, ref weekCounter, ref salary, ref battleCounter);
            currentLetter.Click += (sender, args) =>
            {
                currentLetter.Visible = false;
                if (letters.Count() > 0) adder.AddLetterToArchive(ref ariaArchive, ref warArchive, ref scoutArchive,
                    ref friendsArchive, ref infoArchive, ref ariaLettersCounter, ref warLettersCounter,
                    ref scoutLettersCounter, ref friendsLettersCounter, ref infoLettersCounter, gameForm,
                    letters.First().Key, currentLetter.Text);
                letters = letters.Skip(1);
                if (letters.Count() > 0)
                    currentLetter.Text = letters.First().Value;
                else
                {
                    currentLetter.Text = default;

                    if (phrases.Count() > 0)
                    {
                        currentPhrase.Text = phrases.First().Value;
                        characterText.Visible = true;
                    }
                }
            };
            currentPhrase.Click += (sender, args) =>
            {
                currentPhrase.Font = new Font("Arial", 16);
                phrases = phrases.Skip(1);

                if (phrases.Count() > 0 && letters.Count() == 0)
                {
                    currentPhrase.Text = phrases.First().Value;

                    if (phrases.Count() == 1)
                        phrases = phrases.Skip(1);
                }

                else
                    characterText.Visible = false;
            };

            archiveIcon.Click += (sender, args) => OtherBoxesUnvisible(archive, default);
            orderIcon.Click += (sender, args) => OtherBoxesUnvisible(order, default);
            shopMenu.Click += (sender, args) => OtherBoxesUnvisible(shop, default);
            lettersTable.Click += (sender, args) =>
            {
                currentLetter.BringToFront();
                if (currentLetter.Text.Length > 0) OtherBoxesUnvisible(default, currentLetter);
            };

            void OtherBoxesUnvisible(PictureBox exceptPictureBox, TextBox exceptTextBox)
            {
                archive.Visible = false;
                order.Visible = false;
                currentLetter.Visible = false;
                shop.Visible = false;

                if (exceptPictureBox != null)
                {
                    exceptPictureBox.BringToFront();
                    exceptPictureBox.Visible = true;
                }

                if (exceptTextBox != null)
                {
                    exceptTextBox.BringToFront();
                    exceptTextBox.Visible = true;
                }
            }
        }
    }
}