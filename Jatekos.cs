using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA220110
{
    internal class Jatekos
    {
        public string Nev { get; set; }
        public int EvesFizetes { get; set; }
        public int Szerzodes { get; set; }

        public Jatekos(string nev, string evesFizetes, string szerzodes)
        {
            Nev = nev;
            EvesFizetes = int.Parse(evesFizetes);
            Szerzodes = int.Parse(szerzodes);
        }
    }
}
