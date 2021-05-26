using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    public class Riflemen : ISoldier
    {
        public int Amount { get; set; }
        public int Salary { get => 50; }
        public int ATK { get => 50; }
        public int HP { get => 20; }
    }

    public class Horsemen : ISoldier
    {
        public int Amount { get; set; }
        public int Salary { get => 50; }
        public int ATK { get => 20; }
        public int HP { get => 50; }
    }

    public class Infantrymen : ISoldier
    {
        public int Amount { get; set; }
        public int Salary { get => 20; }
        public int ATK { get => 20; }
        public int HP { get => 20; }
    }

    public class Servicemen : ISoldier
    {
        public int Amount { get; set; }
        public int Salary { get => 10; }
        public int ATK { get => 5; }
        public int HP { get => 15; }
    }
}