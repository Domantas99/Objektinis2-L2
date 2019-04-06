using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboras2
{
    /// <summary>
    /// Klasė skirta aprašyti prenumeratoriaus duomenims.
    /// </summary>
    class Prenumeratorius
    {
        public string Pavarde { get; private set; }
        public string Adresas { get; private set; }
        public int PradziaL { get; private set; }                 // Laikotario pradžia
        public int TrukmeL { get; private set; }                  // Laikotarpio ilgis/trukmė
        public string LeidinioKodas { get; private set; }
        public int LeidiniuKiekis { get; private set; }
        public string PriklausantisAgentas { get; private set; }

        /// <summary>
        /// Tusčias konstruktorius su pradinėmis reikšmėmis.
        /// </summary>
        public Prenumeratorius()
        {
            Pavarde = String.Empty;
            Adresas = String.Empty;
            PradziaL = 0;
            TrukmeL = 0;
            LeidinioKodas = String.Empty;
            LeidiniuKiekis = 0;
            PriklausantisAgentas = String.Empty;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pavarde"></param>
        /// <param name="adresas"></param>
        /// <param name="pr"></param>
        /// <param name="trukme"></param>
        /// <param name="leidKodas"></param>
        /// <param name="leidKiekis"></param>
        /// <param name="agentokodas"></param>
        public Prenumeratorius(string pavarde, string adresas, int pr,
                               int trukme, string leidKodas, int leidKiekis,
                               string agentokodas)
        {
            Pavarde = pavarde;
            Adresas = adresas;
            PradziaL = pr;
            TrukmeL = trukme;
            LeidinioKodas = leidKodas;
            LeidiniuKiekis = leidKiekis;
            PriklausantisAgentas = agentokodas;

        }

        /// <summary>
        /// Funkcija, kuri pakeičia prenumeratoriaus kodą.
        /// </summary>
        /// <param name="kodas"></param>
        public void PriskirtiAgenta(string kodas)
        {
            PriklausantisAgentas = kodas;
        }

        /// <summary>
        /// Funkcija, kuri gražina prenumeratorių string formatu spausdinimui.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("| {0, 10} | {1, 15} |     {2, 7}         |{3, 11}        |{4, 10}      |{5, 9}        |  {6, 8}    |",
                                 Pavarde, Adresas, PradziaL,TrukmeL,LeidinioKodas,LeidiniuKiekis,PriklausantisAgentas);
        }       

    }

}