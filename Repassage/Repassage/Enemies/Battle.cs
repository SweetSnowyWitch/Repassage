using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Repassage.Enemies
{
    class Battle
    {
        public void Start(Form gameForm, ref bool isNotEnded, ref int totalCorpses, ref Riflemen playerRiflemen, 
            ref Horsemen playerHorsemen, ref Infantrymen playerInfantrymen, ref Servicemen playerServicemen, 
            ref Medicine playerMedicine, ref Equipment playerEquipment, int level)
        {
            var enemyRiflemen = new Riflemen();
            var enemyHorsemen = new Horsemen();
            var enemyInfantrymen = new Infantrymen();
            var enemyServicemen = new Servicemen();
            var end = new Endings();

            BuildEnemyArmy(ref enemyRiflemen, ref enemyHorsemen, ref enemyInfantrymen, ref enemyServicemen, level);
            var enemyATK = enemyRiflemen.ATK * enemyRiflemen.Amount + enemyHorsemen.ATK * enemyHorsemen.Amount
                + enemyInfantrymen.ATK * enemyInfantrymen.Amount + enemyServicemen.ATK * enemyServicemen.Amount;
            var enemyHP = enemyRiflemen.HP * enemyRiflemen.Amount + enemyHorsemen.HP * enemyHorsemen.Amount
                + enemyInfantrymen.HP * enemyInfantrymen.Amount + enemyServicemen.HP * enemyServicemen.Amount;
            var playerATK = playerRiflemen.ATK * playerRiflemen.Amount + playerHorsemen.ATK * playerHorsemen.Amount
                + playerInfantrymen.ATK * playerInfantrymen.Amount + playerServicemen.ATK * playerServicemen.Amount 
                + playerEquipment.Amount;

            DealResources(ref playerMedicine, ref playerEquipment, ref enemyHP, ref enemyATK);
            if (playerMedicine.Amount == 0 && enemyATK > 0)
                KillPlayerArmy(ref totalCorpses, ref playerRiflemen, ref playerHorsemen,
                    ref playerInfantrymen, ref playerServicemen, enemyATK);

            if (playerATK < enemyHP) 
                end.DeathInBatte(ref isNotEnded, gameForm, "Силы атаки армии оказалось недостаточно для победы над врагом");

            if (playerRiflemen.Amount + playerHorsemen.Amount 
                + playerInfantrymen.Amount + playerServicemen.Amount <= 0) 
                end.DeathInBatte(ref isNotEnded, gameForm, "Армия была полностью истреблена");
        }

        private void BuildEnemyArmy(ref Riflemen enemyRiflemen, ref Horsemen enemyHorsemen,
            ref Infantrymen enemyInfantrymen, ref Servicemen enemyServicemen, int level)
        {
            var fileReader = new FileReader(); 
            var allLevels = fileReader.GetLevels(fileReader.GetFilePath(@"Enemies\LevelsEnemies.txt")).Skip(0);
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

        private void DealResources(ref Medicine playerMedicine, ref Equipment playerEquipment, ref int enemyHP, ref int enemyATK)
        {
            var medicineAmount = playerMedicine.Amount;
            var equipmentAmount = playerEquipment.Amount;

            playerEquipment.Amount = Math.Max(0, equipmentAmount - enemyHP);
            enemyHP -= equipmentAmount;

            playerMedicine.Amount = Math.Max(0, medicineAmount - enemyATK);
            enemyATK -= medicineAmount;
        }

        private void KillPlayerArmy(ref int totalCorpses, ref Riflemen playerRiflemen, ref Horsemen playerHorsemen,
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
                playerRiflemen.Amount = KillArmyPart(ref totalCorpses, ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                                ref playerServicemen, playerRiflemen.Amount, ref damage, riflemenDamage, playerRiflemen.HP);
                playerHorsemen.Amount = KillArmyPart(ref totalCorpses, ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                    ref playerServicemen, playerHorsemen.Amount, ref damage, horsemenDamage, playerHorsemen.HP);
                playerInfantrymen.Amount = KillArmyPart(ref totalCorpses, ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                    ref playerServicemen, playerInfantrymen.Amount, ref damage, infantrymenDamage, playerInfantrymen.HP);
                playerServicemen.Amount = KillArmyPart(ref totalCorpses, ref playerRiflemen, ref playerHorsemen, ref playerInfantrymen,
                    ref playerServicemen, playerServicemen.Amount, ref damage, servicemenDamage, playerServicemen.HP);
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

            int KillArmyPart(ref int totalCorpses, ref Riflemen playerRiflemen, ref Horsemen playerHorsemen, 
                ref Infantrymen playerInfantrymen, ref Servicemen playerServicemen, int amount, 
                ref int damage, int partDamage, int partHP)
            {
                if (damage > 0 && partDamage > 0 && amount > 0)
                {
                    var corpses = rnd.Next(1, partDamage);
                    if (amount - corpses >= 0)
                    {
                        amount -= corpses;
                        totalCorpses += corpses;
                        damage -= partHP * corpses;

                        UpdateDamage(ref playerRiflemen, ref playerHorsemen,
                        ref playerInfantrymen, ref playerServicemen);
                    }
                }

                return amount;
            }
        }
    }
}