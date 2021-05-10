using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    class AriaPower
    {
        public int Amount;
        public int GrowthRate;
        public int ConvertRate = 1;
    }

    class Loyalty
    {
        public int Amount;
        public double SaleRate = 1;
    }

    class Money
    {
        public int Amount;
    }

    class Equipment
    {
        public int Amount;
        public readonly int Price = 150;
    }

    class Medicine
    {
        public int Amount;
        public readonly int Price = 100;
    }
}
