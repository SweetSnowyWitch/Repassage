using System;
using System.Collections.Generic;
using System.Text;

namespace Repassage
{
    class Army
    {
        public class Riflemen : ISoldier
        {
            public int Amount { get; set; }
            public int Corpses { get; set; }
            public int Salary { get; }
            public int ATK { get; }
            public int HP { get; }

            public void Die()
            {
                throw new NotImplementedException();
            }

            public void Recover()
            {
                throw new NotImplementedException();
            }
        }

        public class Horsemen : ISoldier
        {
            public int Amount { get; set; }
            public int Corpses { get; set; }
            public int Salary { get ; }
            public int ATK { get ; }
            public int HP { get ; }           

            public void Die()
            {
                throw new NotImplementedException();
            }

            public void Recover()
            {
                throw new NotImplementedException();
            }
        }

        public class Infantrymen : ISoldier
        {
            public int Amount { get; set; }
            public int Corpses { get; set; }
            public int Salary { get ; }
            public int ATK { get ; }
            public int HP { get ; }

            public void Die()
            {
                throw new NotImplementedException();
            }

            public void Recover()
            {
                throw new NotImplementedException();
            }
        }

        public class Servicemen : ISoldier
        {
            public int Amount { get; set; }
            public int Corpses { get; set; }
            public int Salary { get ; }
            public int ATK { get ; }
            public int HP { get ; }

            public void Die()
            {
                throw new NotImplementedException();
            }

            public void Recover()
            {
                throw new NotImplementedException();
            }
        }
    }
}