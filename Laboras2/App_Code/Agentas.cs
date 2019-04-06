using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboras2
{
    /// <summary>
    /// Klasė, kurioje aprašoma agento duomenys.
    /// </summary>
    class Agentas
    {
        public string AgentoKodas { get; private set; }
        public string Pavarde { get; private set; }
        public string Vardas { get; private set; }
        public string Adresas { get; private set; }
        public string Telefonas { get; private set; }

        public int Kruvis { get; private set; }

        /// <summary>
        /// Tusčias agento konstruktorius su pradinėmis reikšmėmis.
        /// </summary>
        public Agentas()
        {
            AgentoKodas = String.Empty;
            Pavarde = String.Empty;
            Vardas = String.Empty;
            Adresas = String.Empty;
            Telefonas = String.Empty;
        }

        /// <summary>
        /// Agento konstruktorius su paramentrais.
        /// </summary>
        /// <param name="ak">Agento kodas</param>
        /// <param name="pav">Pavardė</param>
        /// <param name="vard">Vardas</param>
        /// <param name="adresas">Adresas</param>
        /// <param name="telefonas">Telefonas</param>
        public Agentas(string ak, string pav, string vard, string adresas, string telefonas)
        {
            AgentoKodas = ak;
            Pavarde = pav;
            Vardas = vard;
            Adresas = adresas;
            Telefonas = telefonas;
            Kruvis = 0;
        }

        /// <summary>
        /// Funkcija, kuri nustato krūvį.
        /// </summary>
        /// <param name="kruvis">Krūvis</param>
        public void SetKruvis(int kruvis)
        {
            Kruvis = kruvis;
        }

        /// <summary>
        /// Funkcija, kuri gražina agentą string formatu spausdinimui.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return String.Format("|{0, 11} | {1, 10} | {2, 10} | {3, 10} | {4, 10} |", AgentoKodas, 
                                 Pavarde, Vardas, Adresas, Telefonas);
        }

        /// <summary>
        /// Funkcija, kuri padidina krūvį.
        /// </summary>
        public void AddKruvis()
        {
            Kruvis++;
        }

        /// <summary>
        /// Palyginimo operatorius > , kuris lygina pagal
        /// pavardę ir vardą.
        /// </summary>
        /// <param name="lhs">Agento objektas kairėje</param>
        /// <param name="rhs">Agento objektas dešinėje</param>
        /// <returns>true/false</returns>
        public static bool operator >(Agentas lhs, Agentas rhs)
        {
            if (String.Compare(lhs.Pavarde, rhs.Pavarde) == 0)
            {
                if (String.Compare(lhs.Vardas, rhs.Vardas) > 0)
                {
                    return true;
                }
                return false;
            }
            if (String.Compare(lhs.Pavarde, rhs.Pavarde) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Palyginimo operatorius,  kuris lygina pagal
        /// pavardę ir vardą.
        /// </summary>
        /// <param name="lhs">Agento objektas kairėje</param>
        /// <param name="rhs">Agento objektas dešinėje</param>
        /// <returns>true/false</returns>
        public static bool operator <(Agentas lhs, Agentas rhs)
        {
            if (String.Compare(lhs.Pavarde, rhs.Pavarde) == 0)
            {
                if (String.Compare(lhs.Vardas, rhs.Vardas) < 0)
                {
                    return true;
                }
                return false;
            }
            if (String.Compare(lhs.Pavarde, rhs.Pavarde) < 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Palyginimo operatorius
        /// </summary>
        /// <param name="lhs">Agento objektas kairėje</param>
        /// <param name="rhs">Agento objektas dešinėje</param>
        /// <returns>true/false</returns>
        public static bool operator ==(Agentas lhs, Agentas rhs)
        {
            if (lhs.Pavarde == rhs.Pavarde && lhs.Vardas == rhs.Vardas)
            {
                return true;
            }
            return false;
        }

        public bool Equals(Agentas agentas)
        {
            if (Object.ReferenceEquals(agentas, null))
            {
                return false;
            }
            if (this.GetType() != agentas.GetType())
            {
                return false;
            }
            return (Pavarde == agentas.Pavarde) && (Vardas == agentas.Vardas);
        }


        public override int GetHashCode()
        {
            return Pavarde.GetHashCode() ^ Vardas.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Palyginimo operatorius
        /// </summary>
        /// <param name="lhs">Agento objektas kairėje</param>
        /// <param name="rhs">Agento objektas dešinėje</param>
        /// <returns>true/false</returns>
        public static bool operator !=(Agentas lhs, Agentas rhs)
        {       
                if (lhs.Pavarde != rhs.Pavarde && lhs.Vardas != rhs.Pavarde)
                {
                    return true;
                }
            
            return false;
        }

    }

}