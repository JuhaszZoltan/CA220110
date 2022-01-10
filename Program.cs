using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA220110
{
    internal class Program
    {
        static Dictionary<string, List<Jatekos>> csapatok 
            = new Dictionary<string, List<Jatekos>>();
        static int jatekosokSzama = 0;
        static void Main()
        {
            Beolvasas();
            OsszesCsapatNeve();
            AtlantaHawksJatekosai();
            JatekosonkentOsszJovedelem();
            LegjobbKereset();
            CsapatonkentKoltottPenz();
            LegnagyobbKulonbseg();
            LegalacsonyabbAtlag();
            LegdragabbJatekos();
            Atlagfozetes();
            CsapatonkentAtlagFelett();
            EvesSzerintCsokkenobe();

            Console.ReadKey(true);
        }

        private static void EvesSzerintCsokkenobe()
        {
            var model = new List<JatekosForOrder>();

            foreach (var cs in csapatok)
            {
                foreach (var j in cs.Value)
                {
                    model.Add(new JatekosForOrder() { CsapatNev = cs.Key, Jatekos = j });
                }
            }
            var rendezett = model
                .OrderByDescending(x => x.Jatekos.EvesFizetes)
                .ToList();

            foreach (var j in rendezett)
            {
                Console.WriteLine($"{j.CsapatNev} - {j.Jatekos.Nev} - ${j.Jatekos.EvesFizetes}");
            }
        }

        private static void CsapatonkentAtlagFelett()
        {
            foreach (var cs in csapatok)
            {
                var csapatAtlag = cs.Value.Average(x => x.EvesFizetes);
                var atlagFelettDb = cs.Value.Count(x => x.EvesFizetes > csapatAtlag);

                Console.WriteLine($"{cs.Key}: {atlagFelettDb} db");
            }
        }

        private static void Atlagfozetes()
        {
            var atlag_1 = csapatok.Values.Average(x => x.Average(y => y.EvesFizetes));

            long sum = 0;

            foreach (var cs in csapatok.Values)
            {
                foreach (var j in cs)
                {
                    sum += j.EvesFizetes;
                }
            }

            //Console.WriteLine($"átlag v1: ${atlag_1}");
            Console.WriteLine($"átlag v2: ${sum / (float)jatekosokSzama}");
        }

        private static void LegdragabbJatekos()
        {
            foreach (var cs in csapatok)
            {
                Console.WriteLine(
                    $"{cs.Key}: " +
                    $"${cs.Value.OrderBy(x => x.EvesFizetes).Last().Nev}");
            }
        }

        private static void LegalacsonyabbAtlag()
        {
            (string Nev, double Avg) minAvg = ("", double.MaxValue);

            foreach (var cs in csapatok)
            {
                double avg = cs.Value.Average(x => x.EvesFizetes);
                if (avg < minAvg.Avg) minAvg = (cs.Key, avg);
            }
            Console.WriteLine(
                $"legalacsonyabb átlagfizetés: {minAvg.Nev}, " +
                $"(${minAvg.Avg})");
        }

        private static void LegnagyobbKulonbseg()
        {
            (string Nev, int Dif) legnagyobbKulonbseg = ("", 0);

            foreach (var csapat in csapatok)
            {
                int dif = csapat.Value.Max(x => x.EvesFizetes) - csapat.Value.Min(x => x.EvesFizetes);
                if (dif > legnagyobbKulonbseg.Dif)
                    legnagyobbKulonbseg = (csapat.Key, dif);
            }

            Console.WriteLine(
                $"A legnagyobb különbség a {legnagyobbKulonbseg.Nev} " +
                $"cspatban van: ${legnagyobbKulonbseg.Dif}");
        }

        private static void CsapatonkentKoltottPenz()
        {
            foreach (var csapat in csapatok)
            {
                Console.WriteLine($"{csapat.Key} : ${csapat.Value.Sum(x => x.EvesFizetes)}");
            }
        }

        private static void LegjobbKereset()
        {
            var max = new Jatekos("", "0", "0");

            foreach (var csapat in csapatok.Values)
            {
                foreach (var jatekos in csapat)
                {
                    if (jatekos.EvesFizetes > max.EvesFizetes)
                        max = jatekos;
                }
            }

            Console.WriteLine("Legnagyobb jövedelem a szezobnan:");
            Console.WriteLine($"{max.Nev} (${max.EvesFizetes})");
        }

        private static void JatekosonkentOsszJovedelem()
        {
            foreach (var csapat in csapatok)
            {
                Console.WriteLine(csapat.Key);
                foreach (var jatekos in csapat.Value)
                {
                    Console.WriteLine($"\t{jatekos.Nev} - ${jatekos.EvesFizetes * jatekos.Szerzodes}");
                }
            }
        }

        private static void AtlantaHawksJatekosai()
        {

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Atlanta Hawks játékosai: ");

            foreach (var jatekos in csapatok["Atlanta Hawks"])
            {
                Console.WriteLine("\t" + jatekos.Nev);
            }
        }

        private static void OsszesCsapatNeve()
        {
            foreach (var csapatNev in csapatok.Keys)
            {
                Console.WriteLine(csapatNev);
            }
        }

        private static void Beolvasas()
        {
            using (var sr = new StreamReader(@"..\..\res\NBA2003.csv", Encoding.UTF8))
            {
                jatekosokSzama = int.Parse(sr.ReadLine());

                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');

                    string csapatNev = sor[0].Trim('\"');

                    var jatekos = new Jatekos(
                        nev: sor[1].Trim('\"'),
                        evesFizetes: sor[2],
                        szerzodes: sor[3]);


                    if (!csapatok.ContainsKey(csapatNev))
                    {
                        csapatok.Add(csapatNev, new List<Jatekos>());
                    }
                    csapatok[csapatNev].Add(jatekos);
                }
            }
        }
    }
}
