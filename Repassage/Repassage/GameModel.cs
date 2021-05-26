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
        private bool isBattleToday = true;

        public GameModel(Form newForm)
        {
            gameForm = newForm;
            CountResources();

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
            var battleCounter = 0;
            var weekCounter = 10;
            var characterText = AddImage(new Point(500, 820), Resources.Textbox, true, Color.Transparent, Resources.Textbox.Size);
            var phrases = GetTextsFromFile(phrasesPath).Skip(0);
            var currentPhrase = AddTextBox(new Point(20, 20),
                new Size(Resources.Textbox.Width - 40, Resources.Textbox.Height - 40), true, phrases.First().Value);
            currentPhrase.BringToFront();
            currentPhrase.Parent = characterText;
            currentPhrase.ScrollBars = ScrollBars.None;
            currentPhrase.Font = new Font("Arial", 16);

            var weekend = AddImage(new Point(0, 0), default, false, Color.AntiqueWhite, new Size(1920, 1080));
            var endLabel = AddLabel(weekend, new Point(0, 0), "");
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

            var orderIcon = AddImage(new Point(1450, 960), Resources.OrderIcon, true, Color.Transparent, Resources.OrderIcon.Size);
            var archiveIcon = AddImage(new Point(300, 960), Resources.ArchiveIcon, true, Color.Transparent, Resources.ArchiveIcon.Size);
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

            var ariaResource = AddResource(new Point(20, -1), new Point(180, 10), Resources.AriaIcon, ariaPower.Amount);
            var loyaltyResource = AddResource(new Point(300, -1), new Point(450, 10), Resources.LoyaltyIcon, loyalty.Amount);
            var moneyResource = AddResource(new Point(580, -1), new Point(730, 10), Resources.MoneyIcon, money.Amount);
            var equipmentResource = AddResource(new Point(1020, -1), new Point(1170, 10), Resources.EquipmentIcon, equipment.Amount);
            var medicineResource = AddResource(new Point(1320, -1), new Point(1470, 10), Resources.MedicineIcon, medicine.Amount);
            var armyResource = AddResource(new Point(1580, -1), new Point(1730, 10), Resources.ArmyIcon, armyTotalAmount);
            AddImage(new Point(0, 0), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);
            AddImage(new Point(0, 960), Resources.MainFrame, true, Color.Transparent, Resources.MainFrame.Size);

            var letters = GetTextsFromFile(scenarioPath).Skip(0);
            var currentLetter = AddTextBox(new Point(450, 100), archive.Size, false, letters.First().Value);
            gameForm.Controls.Add(currentLetter);

            var shop = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, archive.Size);
            var equipmentBar = AddTrackBar(shop, new Point(50, 50), new Point(50, 100), "Количество покупаемого снаряжения: {0}",
                (int)Math.Floor(money.Amount / (equipment.Price * loyalty.SaleRate)));
            var medicineBar = AddTrackBar(shop, new Point(50, 150), new Point(50, 200), "Количество покупаемых лекарств: {0}",
                (int)Math.Floor(money.Amount / (medicine.Price * loyalty.SaleRate)));
            var peopleBar = AddTrackBar(shop, new Point(50, 250), new Point(50, 300), "Количество нанимаемых людей: {0}",
                ariaPower.Amount / ariaPower.ConvertRate);
            var ariaBar = AddTrackBar(shop, new Point(50, 350), new Point(50, 400), "Выделяемая Арии сумма: {0}", money.Amount);
            var armyBar = AddTrackBar(shop, new Point(50, 450), new Point(50, 500), "Выплачиваемая роте сумма: {0}", money.Amount);
            var battleBar = AddButton(shop, new Point(50, 580), "Принять участие в грядущей битве: Нет");

            var order = AddImage(new Point(450, 100), default, false, Color.AntiqueWhite, new Size(1000, 840));
            var orderText = AddTextBox(new Point(50, 50), new Size(900, 600), true, "");            
            var orderButton = AddButton(order, new Point(50, 680), "Отдать приказ и завершить неделю");
            orderText.Parent = order;
            OrderUPD();

            var lettersTable = AddImage(new Point(0, 550), Resources.Table, true, Color.Transparent, Resources.Table.Size);
            var shopMenu = AddImage(new Point(900, 400), Resources.Boxes, true, Color.Transparent, Resources.Boxes.Size);

            equipmentBar.Scroll += (sender, args) => OrderUPD();
            medicineBar.Scroll += (sender, args) => OrderUPD();
            peopleBar.Scroll += (sender, args) => OrderUPD();
            ariaBar.Scroll += (sender, args) => OrderUPD();
            armyBar.Scroll += (sender, args) => OrderUPD();
            battleBar.Click += (sender, args) =>
            {
                if (battleBar.Text.Split(' ').Last().Equals("Да"))
                    battleBar.Text = "Принять участие в грядущей битве: Нет";
                else if (isBattleToday)
                    battleBar.Text = "Принять участие в грядущей битве: Да";

                OrderUPD();
            };

            archive.Click += (sender, args) => archive.Visible = false;
            order.Click += (sender, args) => order.Visible = false;
            shop.Click += (sender, args) => shop.Visible = false;
            orderButton.Click += (sender, args) =>
            {
                var newReport = new Dictionary<string, string>();
                scenariosPaths = new List<string>();
                phrasesPaths = new List<string>();
                weekend.BringToFront();
                weekend.Visible = true;
                order.Visible = false;                             

                if (battleBar.Text.Contains("Да"))
                {
                    var battle = new Enemies.Battle();
                    battle.Start(ref riflemen, ref horsemen, ref infantrymen,
                        ref servicemen, ref medicine, ref equipment, weekCounter);
                    endLabel.Location = new Point(830, 630);
                    endLabel.Text = "В бой, за Бореалис!";
                    battleBar.Text = "Принять участие в грядущей битве: Нет";
                    battleCounter++;
                }

                else
                {
                    endLabel.Location = new Point(700, 630);
                    endLabel.Text = "Эта неделя обещает быть спокойной...";
                }

                newReport.Add("(Доклад)", String.Format("Численность стрелков: {0}\r\nЧисленность кавалерии: {1}" +
                    "\r\nЧисленность пехоты: {2}\r\nЧисленность обслуживающего персонала: {3}\r\nВыплачено: {4} валюты",
                    riflemen.Amount, horsemen.Amount, infantrymen.Amount, servicemen.Amount, armyBar.Value));
                letters = letters.Concat(newReport);
                currentLetter.Text = letters.First().Value;
                weekCounter += 10;
                ChangeRoute(ref weekCounter, battleCounter, ariaBar.Value, armyBar.Value);
                UpdateResources(riflemen.Amount + horsemen.Amount + infantrymen.Amount + servicemen.Amount);

                if (scenariosPaths.Count > 0)
                {
                    foreach (var path in scenariosPaths)
                      letters = letters.Concat(GetTextsFromFile(path));
                }                  

                if (phrasesPaths.Count > 0)
                {
                    phrases = GetTextsFromFile(phrasesPaths.First());
                    currentPhrase.Text = phrases.First().Value;
                    characterText.Visible = true;
                }             
            };
            currentLetter.Click += (sender, args) =>
            {
                currentLetter.Visible = false;
                if (letters.Count() > 0) AddLetterToArchive(letters.First().Key);
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

            void UpdateResources(int armyTotalAmount)
            {
                ariaResource.Text = ariaPower.Amount.ToString();
                ariaResource.Size = new Size(ariaResource.Text.Length * 35, 50);
                
                loyaltyResource.Text = loyalty.Amount.ToString();
                loyaltyResource.Size = new Size(loyaltyResource.Text.Length * 35, 50);
                
                moneyResource.Text = money.Amount.ToString();
                moneyResource.Size = new Size(moneyResource.Text.Length * 35, 50);
                
                equipmentResource.Text = equipment.Amount.ToString();
                equipmentResource.Size = new Size(equipmentResource.Text.Length * 35, 50);
                
                medicineResource.Text = medicine.Amount.ToString();
                medicineResource.Size = new Size(medicineResource.Text.Length * 35, 50);
                
                armyResource.Text = armyTotalAmount.ToString();
                armyResource.Size = new Size(armyResource.Text.Length * 35, 50);
            }

            void OrderUPD()
            {
                orderText.Text = String.Format("Количество заказанного снаряжения: {0} \r\nСтоимость: {1} валюты/шт." +
                    "\r\nКоличество заказанных лекарств: {2} \r\nСтоимость: {3} валюты/шт." +
                    "\r\nКоличество нанятых людей: {4} \r\nСтоимость найма: {5} ед.влияния/чел." +
                    "\r\nВыделенная Арии сумма: {6} \r\nВыплата роте: {7} \r\nПринимается ли участие в битве: {8}",
                    equipmentBar.Value, (int)Math.Round(equipment.Price * loyalty.SaleRate), medicineBar.Value,
                    (int)Math.Round(medicine.Price * loyalty.SaleRate), peopleBar.Value, ariaPower.ConvertRate,
                    ariaBar.Value, armyBar.Value, battleBar.Text.Split(' ').Last());               
            }

            void AddLetterToArchive(string letterKey)
            {
                switch (letterKey)
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

        private void CountResources()
        {
            var rnd = new Random();
            var totalArmyCount = rnd.Next(300, 600);

            ariaPower.Amount = 10;
            loyalty.Amount = 10;
            riflemen.Amount = rnd.Next(totalArmyCount);
            horsemen.Amount = rnd.Next(totalArmyCount - riflemen.Amount);
            infantrymen.Amount = rnd.Next(totalArmyCount - riflemen.Amount - horsemen.Amount);
            servicemen.Amount = totalArmyCount - riflemen.Amount - horsemen.Amount - infantrymen.Amount;

            var minMoney = riflemen.Amount * riflemen.Salary + horsemen.Amount * horsemen.Salary
                + infantrymen.Amount * infantrymen.Salary + servicemen.Amount * servicemen.Salary;
            money.Amount = rnd.Next((int)Math.Round(minMoney * 1.25), minMoney * 2);
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

        private TextBox AddResource(Point iconLocation, Point amountLocation, Image imageIcon, int amount)
        {
            var resourceAmount = new TextBox();
            
            resourceAmount.ReadOnly = true;
            resourceAmount.BackColor = Color.AntiqueWhite;
            resourceAmount.Location = amountLocation;
            resourceAmount.Text = amount.ToString();
            resourceAmount.Size = new Size(resourceAmount.Text.Length * 35, 50);
            resourceAmount.Font = new Font("Arial", 32);
            AddImage(iconLocation, imageIcon, true, Color.Transparent, imageIcon.Size);
            gameForm.Controls.Add(resourceAmount);

            return resourceAmount;
        }

        private void ChangeRoute(ref int week, int battleCounter, int ariaSpendings, int armySpendings)
        {
            switch (week)
            {
                case 20:
                    scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_AnotherLet.txt");
                    phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\SecondWeekPhrases.txt");

                    if (ariaSpendings == 0)
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_FirstLetVer1.txt");
                    else if (ariaSpendings < money.Amount / 4)
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_FirstLetVer2.txt");
                    else
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_FirstLetVer3.txt");
                    if (armySpendings < riflemen.Amount * riflemen.Salary + horsemen.Amount * horsemen.Salary
                        + infantrymen.Amount * infantrymen.Salary + servicemen.Amount * servicemen.Salary)
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_ThirdLet.txt");
                    if (battleCounter > 0)
                    {
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_SecondLetVer1.txt");
                        isBattleToday = false;
                    }

                    else
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_SecondLetVer2.txt");
                    break;

                case 30:
                    isBattleToday = true;

                    if (battleCounter > 0)
                    {
                        week += 1;
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Third Week\ThirdWeek_SecondRoute.txt");
                        phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\ThirdWeekSecondRoutePhrases.txt");
                    }

                    else
                    {
                        week += 2;
                        scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Third Week\ThirdWeek_FirstRoute.txt");
                        phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\ThirdWeekFirstRoutePhrases.txt");
                    }

                    break;
            }
        }

        private Label AddLabel(Control parent, Point labelLocation, string phrase)
        {
            var newLabel = new Label();
            newLabel.Parent = parent;
            newLabel.Location = labelLocation;
            newLabel.ClientSize = new Size(900, 50);
            newLabel.Font = new Font("Arial", 18);
            newLabel.Text = String.Format(phrase, 0);

            return newLabel;
        }

        private TrackBar AddTrackBar(Control parent, Point labelLocation, Point trackBarLocation, string phrase, int maxAmount)
        {
            var trackLabel = AddLabel(parent, labelLocation, phrase);
            var trackBar = new TrackBar();

            trackBar.Parent = parent;
            trackBar.Location = trackBarLocation;
            trackBar.ClientSize = new Size(900, 100);
            trackBar.Maximum = maxAmount;
            trackBar.Scroll += (sender, args) =>
            {
                trackLabel.Text = String.Format(phrase, trackBar.Value);
            };

            return trackBar;
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

        private Dictionary<string, string> GetTextsFromFile(string scenarioPath)
        {
            var text = File.ReadAllText(scenarioPath).Split(' ');
            var texts = new Dictionary<string, string>();
            var isLetterFirst = true;
            var newAuthor = "";
            var newText = new StringBuilder();

            foreach (var word in text)
            {
                if (word.Length > 0 && word[0].Equals('('))
                {
                    if (isLetterFirst)
                    {
                        newAuthor = word;
                        isLetterFirst = false;
                        continue;
                    }

                    texts.Add(newAuthor, newText.ToString());
                    newText = new StringBuilder();
                    newAuthor = word;
                }

                else newText.Append(word + " ");
            }

            return texts;
        }
    }
}