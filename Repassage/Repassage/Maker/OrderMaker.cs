using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    public class OrderMaker
    {
        public void MakeOrder(Form gameForm, ref List<String> scenariosPaths, ref List<String> phrasesPaths, ref Routes routes, 
            ref Endings end, ref IEnumerable<KeyValuePair<string, string>> letters, ref IEnumerable<KeyValuePair<string, string>> phrases,
            ref Loyalty loyalty, ref Equipment equipment, ref AriaPower ariaPower, ref TrackBar ariaBar, ref TrackBar equipmentBar, 
            ref TrackBar medicineBar, ref TrackBar armyBar, ref TrackBar peopleBar, ref Button battleBar, ref PictureBox weekend, 
            ref PictureBox order, ref Counter counter, ref Updater updater, ref Label endLabel, ref Medicine medicine, ref Money money, 
            ref TextBox ariaResource, ref Riflemen riflemen, ref Horsemen horsemen, ref Infantrymen infantrymen, ref Servicemen servicemen,
            ref TextBox loyaltyResource, ref TextBox moneyResource, ref TextBox equipmentResource, ref TextBox medicineResource, 
            ref TextBox armyResource, ref TextBox currentPhrase, ref TextBox currentLetter, ref TextBox orderText,
            ref PictureBox characterText, ref bool isNotEnded, ref bool isBattleToday, 
            ref int week, ref int salary, ref int battleCounter)
        {
            var fileReader = new FileReader();
            var newReport = new Dictionary<string, string>();
            var totalCorpses = 0;
            scenariosPaths = new List<string>();
            phrasesPaths = new List<string>();
            weekend.Visible = true;
            order.Visible = false;
            weekend.BringToFront();
            counter.CountArmy(ref riflemen, ref horsemen, ref infantrymen, ref servicemen, peopleBar.Value);
            updater.ValuesUPD(ref loyalty, ref equipment, ref ariaPower, ref medicine, ref money,
                ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref salary);

            if (battleBar.Text.Contains("Да"))
            {
                var battle = new Enemies.Battle();
                battle.Start(gameForm, ref isNotEnded, ref totalCorpses, ref riflemen, ref horsemen, ref infantrymen,
                    ref servicemen, ref medicine, ref equipment, week);
                endLabel.Location = new Point(830, 630);
                endLabel.Text = "В бой, за Бореалис!";
                battleBar.Text = battleBar.Text.Replace("Да", "Нет");
                battleCounter++;
                loyalty.Amount += 10;
                loyalty.SaleRate = 1 - (loyalty.Amount / 100.0);
                updater.OrderUPD(ref orderText, ref loyalty, ref equipment, ref ariaPower, ref medicine,
                    ref ariaBar, ref equipmentBar, ref medicineBar, ref armyBar, ref peopleBar, ref battleBar);
            }

            else
            {
                endLabel.Location = new Point(700, 630);
                endLabel.Text = "Эта неделя обещает быть спокойной...";
            }

            newReport.Add("(Доклад)", String.Format("Неделя {0} \r\nЧисленность стрелков: {1}" +
                "\r\nЧисленность кавалерии: {2}\r\nЧисленность пехоты: {3}" +
                "\r\nЧисленность обслуживающего персонала: {4}\r\nПотери: {5} \r\nВыплачено: {6} валюты",
                week / 10, riflemen.Amount, horsemen.Amount, infantrymen.Amount,
                servicemen.Amount, totalCorpses, armyBar.Value));
            letters = letters.Concat(newReport);
            currentLetter.Text = letters.First().Value;
            week += 10;
            routes.ChangeRoute(ref scenariosPaths, ref phrasesPaths, ref isNotEnded,
            ref isBattleToday, ref week, gameForm, end, money, ariaPower, riflemen, horsemen,
            infantrymen, servicemen, battleCounter, ariaBar.Value, armyBar.Value, salary);
            ariaPower.Amount += ariaPower.GrowthRate;

            updater.ResourcesUPD(ref loyalty, ref equipment, ref ariaPower, ref medicine, ref money, ref ariaResource,
            ref loyaltyResource, ref moneyResource, ref equipmentResource, ref medicineResource, ref armyResource,
            riflemen.Amount + horsemen.Amount + infantrymen.Amount + servicemen.Amount);

            if (isNotEnded && (ariaPower.Amount <= 0 || money.Amount < 0))
                end.Criminal(ref isNotEnded, gameForm);

            if (isNotEnded && battleCounter > 1)
                end.Triumph(ref isNotEnded, gameForm);

            else if (isNotEnded && week > 40)
                end.DeathInBatte(ref isNotEnded, gameForm);

            if (scenariosPaths.Count > 0)
            {
                foreach (var path in scenariosPaths)
                    letters = letters.Concat(fileReader.GetTexts(path));
            }

            if (phrasesPaths.Count > 0)
            {
                phrases = fileReader.GetTexts(phrasesPaths.First());
                currentPhrase.Text = phrases.First().Value;
                currentPhrase.Font = new Font("Arial", 16);
                characterText.Visible = true;
            }
        }
    }
}
