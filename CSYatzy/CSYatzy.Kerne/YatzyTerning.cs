using System;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// Klassen repræsenterer en (sorterbar) terning
    /// </summary>
    public class YatzyTerning : IComparable<YatzyTerning>
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Opretter en terning og skaber en tilfældig værdi
        /// </summary>
        public YatzyTerning()
        {
            Ryst();
        }

        /// <summary>
        /// Opretter en terning med en konkret værdi (brugt i test)
        /// </summary>
        /// <param name="værdi">Værdi mellem 1-6</param>
        public YatzyTerning(int værdi)
        {
            if (værdi < 1 || værdi > 6)
                throw new YatzyException($"Forkert værdi {værdi} ved oprettelse af terning");
            this.Værdi = værdi;
        }

        /// <summary>
        /// Hændelse der smides når terning er rystet
        /// </summary>
        public event EventHandler ErRystet;

        /// <summary>
        /// Benyttes i et bæger af terninger til at indikere, at terningen skal bibeholde sin værdi ved ryst
        /// </summary>
        public bool ErHoldt { get; private set; }
        /// <summary>
        /// Konkret værdi (1-6)
        /// </summary>
        public int Værdi { get; private set; }

        /// <summary>
        /// Fra IComparable - sikrer at en terning kan sorteres efter værdi
        /// </summary>
        /// <param name="other">Terning til sammenligning</param>
        /// <returns>Værdi mellem -1 til 1</returns>
        public int CompareTo(YatzyTerning other)
        {
            return Værdi.CompareTo(other.Værdi);
        }

        /// <summary>
        /// Sætter værdi til hold så en terning kan/ikke kan rystes 
        /// </summary>
        /// <param name="værdi">true = hold, false = ikke hold</param>
        public void Hold(bool værdi)
        {
            ErHoldt = værdi;
        }

        /// <summary>
        /// Finder en tilfældig værdi til terning mellem 1-6 
        /// </summary>
        public void Ryst()
        {
            Værdi = random.Next(1, 7);
            ErRystet?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Viser en terning som en streng - [værdi] eller *værdi* (* betyder, at terning ikke for en ny værdi ved ryst)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (ErHoldt)
                return $"*{Værdi}*";
            else
                return $"[{Værdi}]";
        }
    }
}