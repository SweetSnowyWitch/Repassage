using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    public interface ISoldier
    {
        int Amount { get; set; }
        int Corpses { get; set; }
        int Salary { get; }
        int ATK { get; }
        int HP { get; }
        void Recover();
        void Die();
    }
}
