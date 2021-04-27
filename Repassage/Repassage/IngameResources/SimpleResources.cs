using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    class AriaPower
    {
        public int Amount;
        public double GrowthRate;
        public double ConvertRate;
    }

    class Loyalty
    {
        public int Amount;
        public double SaleRate;
    }

    class Money
    {
        public int Amount;
    }

    class Equipment
    {
        public int Amount;
        public readonly int Price;
    }

    class Medicine
    {
        public int Amount;
        public readonly int Price;
    }
}
