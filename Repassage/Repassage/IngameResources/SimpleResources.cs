using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    public class AriaPower
    {
        public int Amount;
        public int GrowthRate;
        public int ConvertRate = 10;
    }

    public class Loyalty
    {
        public int Amount;
        public double SaleRate = 1;
    }

    public class Money
    {
        public int Amount;
    }

    public class Equipment
    {
        public int Amount;
        public readonly int Price = 150;
    }

    public class Medicine
    {
        public int Amount;
        public readonly int Price = 100;
    }
}
