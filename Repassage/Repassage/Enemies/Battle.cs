using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Repassage.Enemies
{
    class Battle
    {
        public void Start(ref Riflemen playerRiflemen, ref Horsemen playerHorsemen, ref Infantrymen playerInfantrymen,
            ref Servicemen playerServicemen, ref Medicine playerMedicine, ref Equipment playerEquipment, int level)
        {
            var enemyRiflemen = new Riflemen();
            var enemyHorsemen = new Horsemen();
            var enemyInfantrymen = new Infantrymen();
            var enemyServicemen = new Servicemen();

            BuildEnemyArmy(ref enemyRiflemen, ref enemyHorsemen, ref enemyInfantrymen, ref enemyServicemen, level);
            var enemyATK = enemyRiflemen.ATK * enemyRiflemen.Amount + enemyHorsemen.ATK * enemyHorsemen.Amount
                + enemyInfantrymen.ATK * enemyInfantrymen.Amount + enemyServicemen.ATK * enemyServicemen.Amount;
            var enemyHP = enemyRiflemen.HP * enemyRiflemen.Amount + enemyHorsemen.HP * enemyHorsemen.Amount
                + enemyInfantrymen.HP * enemyInfantrymen.Amount + enemyServicemen.HP * enemyServicemen.Amount;
            var playerATK = playerRiflemen.ATK * playerRiflemen.Amount + playerHorsemen.ATK * playerHorsemen.Amount
                + playerInfantrymen.ATK * playerInfantrymen.Amount + playerServicemen.ATK * playerServicemen.Amount + playerEquipment.Amount;
            var playerHP = playerRiflemen.HP * playerRiflemen.Amount + playerHorsemen.HP * playerHorsemen.Amount
                + playerInfantrymen.HP * playerInfantrymen.Amount + playerServicemen.HP * playerServicemen.Amount + playerMedicine.Amount;

            if (playerATK >= enemyHP) enemyHP = 0;
            DealResources(ref playerMedicine, ref playerEquipment, playerHP, enemyATK);

            if (playerMedicine.Amount == 0)
                KillPlayerArmy(ref playerRiflemen, ref playerHorsemen,
                    ref playerInfantrymen, ref playerServicemen, enemyATK);
        }

        private void BuildEnemyArmy(ref Riflemen enemyRiflemen, ref Horsemen enemyHorsemen,
            ref Infantrymen enemyInfantrymen, ref Servicemen enemyServicemen, int level)
        {
            var allLevels = ReadLevelsFile(@"C:\Users\User\Documents\GitHub\Repassage\Repassage\Repassage\Enemies\LevelsEnemies.txt").Skip(0);

            switch (level)
            {
                case 20:
                    allLevels = allLevels.Skip(1);
                    break;

                case 31:
                    allLevels = allLevels.Skip(2);
                    break;

                case 32:
                    allLevels = allLevels.Skip(3);
                    break;
            }

            var enemyArmy = allLevels.First().Value;
            enemyRiflemen.Amount = enemyArmy[0];
            enemyHorsemen.Amount = enemyArmy[1];
            enemyInfantrymen.Amount = enemyArmy[2];
            enemyServicemen.Amount = enemyArmy[3];
        }

        private void DealResources(ref Medicine playerMedicine, ref Equipment playerEquipment, int playerHP, int enemyATK)
        {
            var rnd = new Random();
            playerEquipment.Amount *= rnd.Next(0, 1);
            if (playerHP - enemyATK < playerMedicine.Amount)
                playerMedicine.Amount = Math.Max(0, playerHP - enemyATK);
        }

        private void KillPlayerArmy(ref Riflemen playerRiflemen, ref Horsemen playerHorsemen,
            ref Infantrymen playerInfantrymen, ref Servicemen playerServicemen, int damage)
        {
            var rnd = new Random();
            var riflemenDamage = 0;
            var horsemenDamage = 0;
            var infantrymenDamage = 0;
            var servicemenDamage = 0;
            UpdateDamage(ref playerRiflemen, ref playerHorsemen,
                        ref playerInfantrymen, ref playerServicemen);

            while (damage > 0 && playerRiflemen.Amount + playerHorsemen.Amount 
                + playerInfantrymen.Amount + playerServicemen.Amount != 0)
            {
                playerRiflemen.Amount = KillArmyPart(ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                                ref playerServicemen, playerRiflemen.Amount, ref damage, riflemenDamage);
                playerHorsemen.Amount = KillArmyPart(ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                    ref playerServicemen, playerHorsemen.Amount, ref damage, horsemenDamage);
                playerInfantrymen.Amount = KillArmyPart(ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                    ref playerServicemen, playerInfantrymen.Amount, ref damage, infantrymenDamage);
                playerServicemen.Amount = KillArmyPart(ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                    ref playerServicemen, playerServicemen.Amount, ref damage, servicemenDamage);
            }

            void UpdateDamage(ref Riflemen playerRiflemen, ref Horsemen playerHorsemen,
            ref Infantrymen playerInfantrymen, ref Servicemen playerServicemen)
            {
                if (damage > playerRiflemen.HP) 
                        riflemenDamage = rnd.Next(1, damage / playerRiflemen.HP);

                if (damage > playerHorsemen.HP)
                    horsemenDamage = rnd.Next(1, damage / playerHorsemen.HP);

                if (damage > playerInfantrymen.HP)
                    infantrymenDamage = rnd.Next(1, damage / playerInfantrymen.HP);

                if (damage > playerServicemen.HP)
                    servicemenDamage = rnd.Next(1, damage / playerServicemen.HP);
            }

            int KillArmyPart(ref Riflemen playerRiflemen, ref Horsemen playerHorsemen, ref Infantrymen playerInfantrymen,
                ref Servicemen playerServicemen, int amount, ref int damage, int partDamage)
            {
                if (damage > 0 && partDamage > 0 && amount > 0)
                {
                    var corpses = rnd.Next(0, partDamage);
                    if (amount - corpses > 0) 
                        amount = Math.Max(0, amount - corpses);
                    damage -= partDamage;
                    UpdateDamage(ref playerRiflemen, ref playerHorsemen,
                        ref playerInfantrymen, ref playerServicemen);
                }

                return amount;
            }
        }

        private Dictionary<int, int[]> ReadLevelsFile(string levelsPath)
        {
            var rawLevels = File.ReadAllText(levelsPath).Split(' ');
            var allLevels = new Dictionary<int, int[]>();
            var isFirstLevel = true;
            var army = new List<int>();
            var route = new StringBuilder();

            foreach (var word in rawLevels)
            {
                if (word.Length > 0 && word[0].Equals('('))
                {
                    if (isFirstLevel)
                    {
                        foreach (var letter in word)
                            if (Char.IsDigit(letter)) route.Append(letter);
                        isFirstLevel = false;
                        continue;
                    }

                    if (route.Length > 0 && Int32.Parse(route.ToString()) > 0)
                        allLevels.Add(Int32.Parse(route.ToString()), army.ToArray());

                    route = new StringBuilder();
                    foreach (var letter in word)
                        if (Char.IsDigit(letter)) route.Append(letter);
                    army = new List<int>();
                }

                else if (word.Length > 0 && Char.IsDigit(word[0]))
                    army.Add(Int32.Parse(word));
            }

            return allLevels;
        }
    }
}
