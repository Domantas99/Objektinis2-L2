using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboras2
{
    /// <summary>
    /// Mazgo sąrašo klasė.
    /// </summary>
    /// <typeparam name="T">Tipas</typeparam>
    public class NodeList<T>
    {
        /// <summary>
        /// Mazgo klasė.
        /// </summary>
        public sealed class Node
        {
            public T Data;
            public Node Next;

            /// <summary>
            /// Tusčias konstruktorius
            /// </summary>
            public Node()
            {

            }

            /// <summary>
            /// Konstruktorius su parametrais.
            /// </summary>
            /// <param name="data">Objekto data</param>
            public Node(T data)
            {
                this.Data = data;
            }

        }

        private Node Start;             // Sąrašo pirmas elementas.
        private Node End;               // Sąrašo paskutinis elementas.
        private Node Current;           // Sąrašo tuo metu einamas elementas.

        public NodeList()
        {
            Start = null;
            End = null;
            Current = null;
        }


        /// <summary>
        /// Funkcija, kuri ištrina esamą(Current) sąrašo elementą.
        /// </summary>
        public void Delete()
        {
            if (Start == null) return;

            if (Start.Next == null)
            {
                Start = null;
                return;
            }

            // Ištrina jeigu pirmas elementas.
            if (Current == Start)
            {
                Current = Start;
                Start = Start.Next;
                return;
            }

            // Ištrina jeigu paskutinis elementas.
            if (Current == End)
            {
                Node node = Start;
                while (node.Next != End)
                {                 
                    node = node.Next;
                }
                node.Next = null;
                End = node;
                return;
                
            }
            
            // Ištrina vidinį elementą.
            Node temp = Start;
            while (temp.Next.Next != null)
            {
                if (temp.Next == Current)
                {
                    temp.Next = Current.Next;
                    Current = temp;
                    return;
                }
                temp = temp.Next;
            }

        }

        /// <summary>
        /// Funkcija, kuri sunaikina sąrašą.
        /// </summary>
        public void NaikintiSarasa()
        {
            while (Start != null)
            {
                Node d = Start;
                Start = Start.Next;
                d.Next = null;
            }
        }

        /// <summary>
        /// Funkcija, kuri rikiuoja sąrašą(pagal pavardę ir vardą)
        /// </summary>
        public void Rikiuoti()
        {
            Pradzia();
            for (Node first = FirstNode(); first != null; first = first.Next)
            {
                Node max = first;
                for (Node second = first; second != null; second = second.Next)
                {
                    if ((second.Data as Agentas) < (max.Data as Agentas))
                        {
                        max = second;
                    }
                    T temp = first.Data;
                    first.Data = max.Data;
                    max.Data = temp;
                }
            }
        }

        /// <summary>
        /// Funkcija, kuri grąžina pirmą sąrašo elementą.
        /// </summary>
        /// <returns>Start node</returns>
        private Node FirstNode()
        {
            return Start;
        }

        /// <summary>
        /// Funkcija, kuri prideda elementą į sąrašą.
        /// </summary>
        /// <param name="data">Objekto data</param>
        public void AddData(T data)
        {
            Node node = new Node(data);
            if (Start == null)
            {
                Start = node;
                End = node;
            }
            else
            {
                End.Next = node;
                End = node;
            }

        }

        /// <summary>
        /// Funkcija, kuri grąžina objektą.
        /// </summary>
        /// <returns>Objektas</returns>
        public T GautiT()
        {
            return Current.Data;
        }

        /// <summary>
        /// Funkcija, kuri pradeda ciklą.
        /// </summary>
        public void Pradzia()
        {
            Current = Start;
        }

        /// <summary>
        /// Funkciją, kuri pereina į sekantį sąrašo elementą.
        /// </summary>
        public void Sekantis()
        {
            if (Current != null)
            {
                Current = Current.Next;
            }
        }

        /// <summary>
        /// Funkcija, kuri tikrina ar yra dar elementų sąraše.
        /// </summary>
        /// <returns></returns>
        public bool ArYra()
        {
            return Current != null;
        }

    }

}