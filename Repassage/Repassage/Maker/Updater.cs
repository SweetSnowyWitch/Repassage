using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Repassage
{
    public class Updater
    {
        public void ResourcesUPD(ref Loyalty loyalty, ref Equipment equipment, ref AriaPower ariaPower,
                ref Medicine medicine, ref Money money, ref TextBox ariaResource, ref TextBox loyaltyResource, ref TextBox moneyResource,
                ref TextBox equipmentResource, ref TextBox medicineResource, ref TextBox armyResource, int armyTotalAmount)
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

        public void ValuesUPD(ref Loyalty loyalty, ref Equipment equipment, ref AriaPower ariaPower, 
            ref Medicine medicine, ref Money money, ref TrackBar ariaBar, ref TrackBar equipmentBar, ref TrackBar medicineBar, 
            ref TrackBar armyBar, ref TrackBar peopleBar, ref int salary)
        {
            if (armyBar.Value > 0)
                loyalty.Amount += 10;
            salary -= armyBar.Value;
            money.Amount -= (int)(equipmentBar.Value * equipment.Price * loyalty.SaleRate +
                medicineBar.Value * medicine.Price * loyalty.SaleRate + ariaBar.Value + armyBar.Value);
            ariaPower.Amount -= (int)Math.Round((double)peopleBar.Value / ariaPower.ConvertRate);
            equipment.Amount = equipmentBar.Value;
            medicine.Amount = medicineBar.Value;
        }

        public void OrderUPD(ref TextBox orderText, ref Loyalty loyalty, ref Equipment equipment, ref AriaPower ariaPower,
            ref Medicine medicine, ref TrackBar ariaBar, ref TrackBar equipmentBar, ref TrackBar medicineBar,
            ref TrackBar armyBar, ref TrackBar peopleBar, ref Button battleBar)
        {
            orderText.Text = String.Format("Количество заказанного снаряжения: {0} \r\nСтоимость: {1} валюты/шт." +
                "\r\nКоличество заказанных лекарств: {2} \r\nСтоимость: {3} валюты/шт." +
                "\r\nКоличество нанятых людей: {4} \r\nСтоимость найма: {5} чел./ед.влияния" +
                "\r\nВыделенная Арии сумма: {6} \r\nВыплата роте: {7} \r\nПринимается ли участие в битве: {8}",
                equipmentBar.Value, (int)Math.Round(equipment.Price * loyalty.SaleRate), medicineBar.Value,
                (int)Math.Round(medicine.Price * loyalty.SaleRate), peopleBar.Value, ariaPower.ConvertRate,
                ariaBar.Value, armyBar.Value, battleBar.Text.Split(' ').Last());
        }
    }
}