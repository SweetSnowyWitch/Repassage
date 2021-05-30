using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    public class Counter
    {
        public void CountResources(ref AriaPower ariaPower, ref Loyalty loyalty, ref Riflemen riflemen,
            ref Horsemen horsemen, ref Infantrymen infantrymen, ref Servicemen servicemen, ref Money money, ref int salary)
        {
            var rnd = new Random();
            var totalArmyCount = rnd.Next(500, 800);

            ariaPower.Amount = 10;
            loyalty.Amount = 10;
            CountArmy(ref riflemen, ref horsemen, ref infantrymen, ref servicemen, totalArmyCount);

            salary = riflemen.Amount * riflemen.Salary + horsemen.Amount * horsemen.Salary
                + infantrymen.Amount * infantrymen.Salary + servicemen.Amount * servicemen.Salary;
            money.Amount = rnd.Next((int)Math.Round(salary * 2.5), salary * 4);
        }

        public void CountArmy(ref Riflemen riflemen, 
            ref Horsemen horsemen, ref Infantrymen infantrymen, ref Servicemen servicemen, int totalCount)
        {
            var rnd = new Random();
            var newRifleAmount = rnd.Next(totalCount / 2);
            var newHorseAmount = rnd.Next((totalCount - newRifleAmount) / 2);
            var newInfantryAmount = rnd.Next(totalCount - newRifleAmount - newHorseAmount);
            var newServiceAmount = totalCount - newRifleAmount - newHorseAmount - newInfantryAmount;

            riflemen.Amount += newRifleAmount;
            horsemen.Amount += newHorseAmount;
            infantrymen.Amount += newInfantryAmount;
            servicemen.Amount += newServiceAmount;
        }
    }
}
