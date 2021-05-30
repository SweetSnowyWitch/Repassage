using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    public class Routes
    {
        public void ChangeRoute(ref List<String> scenariosPaths, ref List<String> phrasesPaths, ref bool isNotEnded, 
            ref bool isBattleToday, ref int week, Form gameForm, Endings end, Money money, AriaPower ariaPower, 
            Riflemen riflemen, Horsemen horsemen, Infantrymen infantrymen, Servicemen servicemen, int battleCounter, 
            int ariaSpendings, int armySpendings, int salary)
        {
            switch (week)
            {
                case 20:
                    CreateSecondWeek(ref scenariosPaths, ref phrasesPaths, ref isBattleToday, money, ariaPower, 
                        riflemen, horsemen, infantrymen, servicemen, battleCounter, ariaSpendings, armySpendings);
                    break;

                case 30:
                    if (isNotEnded && salary > 0)
                        end.Riot(ref isNotEnded, gameForm);

                    if (battleCounter > 0)
                        CreateThirdWeekFirst(ref scenariosPaths, ref phrasesPaths, ref isBattleToday, ref week, money);

                    else
                        CreateThirdWeekSecond(ref scenariosPaths, ref phrasesPaths, ref week);

                    isBattleToday = true;
                    break;
            }
        }

        private void CreateSecondWeek(ref List<String> scenariosPaths, ref List<String> phrasesPaths, ref bool isBattleToday, 
            Money money, AriaPower ariaPower, Riflemen riflemen, Horsemen horsemen, Infantrymen infantrymen, 
            Servicemen servicemen, int battleCounter, int ariaSpendings, int armySpendings)
        {
            scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_AnotherLet.txt");
            phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\SecondWeekPhrases.txt");

            if (ariaSpendings == 0)
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_FirstLetVer1.txt");

            else if (ariaSpendings < money.Amount / 4)
            {
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_FirstLetVer2.txt");
                ariaPower.GrowthRate += 5;
            }

            else
            {
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_FirstLetVer3.txt");
                ariaPower.GrowthRate += 15;
            }

            if (armySpendings < riflemen.Amount * riflemen.Salary + horsemen.Amount * horsemen.Salary
                + infantrymen.Amount * infantrymen.Salary + servicemen.Amount * servicemen.Salary)
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_ThirdLet.txt");

            if (battleCounter > 0)
            {
                money.Amount += 1000;
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_SecondLetVer1.txt");
                isBattleToday = false;
            }

            else
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Second Week\SecondWeek_SecondLetVer2.txt");          
        }

        private void CreateThirdWeekFirst(ref List<String> scenariosPaths, ref List<String> phrasesPaths,
            ref bool isBattleToday, ref int week, Money money)
        {
            if (isBattleToday)
            {
                money.Amount += 10000;
                scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Third Week\ThirdWeek_Prize.txt");
            }

            week += 1;
            scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Third Week\ThirdWeek_SecondRoute.txt");
            phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\ThirdWeekSecondRoutePhrases.txt");
        }

        private void CreateThirdWeekSecond(ref List<String> scenariosPaths, ref List<String> phrasesPaths, ref int week)
        {
            week += 2;
            scenariosPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Scenario\Third Week\ThirdWeek_FirstRoute.txt");
            phrasesPaths.Add(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Phrases\ThirdWeekFirstRoutePhrases.txt");
        }
    }
}
