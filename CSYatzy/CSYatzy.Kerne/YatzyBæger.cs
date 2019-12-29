using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// Bæger til at opbevare 5 terninger
    /// </summary>
    public class YatzyBæger : IEnumerable<YatzyTerning>
    {
        private readonly Dictionary<int, int> ens;
        private readonly List<YatzyTerning> terninger;

        /// <summary>
        /// Opretter bæger med 5 terninger
        /// </summary>
        public YatzyBæger()
        {
            terninger = new List<YatzyTerning>();
            ens = new Dictionary<int, int>();
            for (int i = 0; i < 5; i++)
            {
                terninger.Add(new YatzyTerning());
            }
            FindEns();
        }

        /// <summary>
        /// Opretter bæger med 5 konkrete terninger 
        /// </summary>
        /// <param name="værdier">array af terninger (skal bestå af fem tal)</param>
        public YatzyBæger(params int[] værdier)
        {
            if (værdier.Length != 5)
                throw new YatzyException("Forkert antal terninger til Yatzybæger");
            terninger = new List<YatzyTerning>();
            ens = new Dictionary<int, int>();
            for (int i = 0; i < værdier.Length; i++)
            {
                terninger.Add(new YatzyTerning(værdier[i]));
            }
            FindEns();
        }
        /// <summary>
        /// Hændelse der smides når terningerne er rystet
        /// </summary>
        public event EventHandler ErRystet;

        /// <summary>
        /// Returnerer den samlede sum af terninger i bægeret
        /// </summary>
        public int Sum
        {
            get
            {
                int sum = terninger.Select(i => i.Værdi).Sum();
                return sum;
            }
        }

        /// <summary>
        /// Sikrer, at man kan tilgå terninger ved hjælp af et indeks
        /// </summary>
        /// <param name="index">Konkrete indeks mellem 0-4</param>
        /// <returns></returns>
        public YatzyTerning this[int index]
        {
            get
            {
                if (index < 0 || index > 4)
                    throw new YatzyException("Forkert indeks " + index);
                return terninger[index];
            }
        }

        /// <summary>
        /// Beregner hvor mange ens terninger der findes i bægeret af en konkret værdi
        /// </summary>
        /// <param name="værdi">Terningens værdi</param>
        /// <returns></returns>
        public int FindAntalEnsTerninger(int værdi)
        {
            if (værdi < 1 || værdi > 6)
                throw new YatzyException("Forkert værdi " + værdi);
            // ens er en dictionary af værdier/antal
            return ens[værdi];
        }

        /// <summary>
        /// Returnerer en dictionary der viser hvor mange ens terninger der findes i bægeret af en konkret værdi
        /// </summary>
        public Dictionary<int, int> FindAntalEnsTerninger()
        {
            return ens.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
        }

        /// <summary>
        /// Fjerner hold (sætter til false) på samtlige terninger
        /// </summary>
        public void FjernHoldFraAlleTerninger()
        {
            foreach (var item in terninger)
            {
                item.Hold(false);
            }
        }

        /// <summary>
        /// Til IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerator<YatzyTerning> GetEnumerator()
        {
            return terninger.GetEnumerator();
        }

        /// <summary>
        /// Til IEnumerable
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return terninger.GetEnumerator();
        }

        /// <summary>
        /// Sætter hold (kan ikke rystes) på en konkret terning
        /// </summary>
        /// <param name="indeks">0-5</param>
        /// <param name="holdVærdi">true/false - default er true</param>
        public void HoldTerning(int indeks, bool holdVærdi = true)
        {
            if (indeks < 0 || indeks > terninger.Count - 1)
                throw new YatzyException("Forkert terning angivet ved hold");
            terninger[indeks].Hold(holdVærdi);
        }

        /// <summary>
        /// Finder en tilfældig værdi til alle terninger som ikke har hold=true
        /// </summary>
        public void Ryst()
        {
            for (int i = 0; i < terninger.Count; i++)
            {
                if (!terninger[i].ErHoldt)
                {
                    terninger[i].Ryst();
                }
                ErRystet?.Invoke(this, new EventArgs());
            }
            FindEns();
        }

        /// <summary>
        /// Sorterer terninger
        /// </summary>
        public void Sorter()
        {
            terninger.Sort();
        }

        /// <summary>
        /// Skaber en streng repræsentation af bægeret
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(" ", terninger.Select(i => i.ToString()));
        }

        /// <summary>
        /// Opretter en dictionary med ens terninger (key=1-6, value=antal ens)
        /// </summary>
        private void FindEns()
        {
            ens.Clear();
            for (int i = 1; i < 7; i++)
            {
                ens.Add(i, terninger.Where(x => x.Værdi == i).Count());
            }
        }
    }
}