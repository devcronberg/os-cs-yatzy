using System;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// Repræsenterer et yatzy point (enere, toere, chancen, yatzy mv)
    /// </summary>
    public class YatzyPoint : IComparable<YatzyPoint>
    {
        /// <summary>
        /// Navn på pointtype
        /// </summary>
        public string Navn { get; set; }

        /// <summary>
        /// Det samlede maksimale point
        /// </summary>
        public int MaxPoint { get; set; }

        /// <summary>
        /// Antal point
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// Point type
        /// </summary>
        public YatzyPointType PointType { get; set; }
        /// <summary>
        /// Sortering (benyttes når der blandt andet skabes en streng i ToString)
        /// </summary>
        public int Sortering { get; set; }

        /// <summary>
        /// IComparable
        /// </summary>
        /// <param name="other">Point at sammenligne</param>
        /// <returns></returns>
        public int CompareTo(YatzyPoint other)
        {
            return this.Sortering.CompareTo(other.Sortering);
        }

        /// <summary>
        /// Point type som en streng
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Navn} {Point}";
        }
    }
}