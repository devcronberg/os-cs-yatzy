namespace CSYatzy.Kerne
{
    /// <summary>
    /// Repræsenterer en spiller
    /// </summary>
    public class YatzySpiller
    {
        /// <summary>
        /// Spillerens navn
        /// </summary>
        public string Navn { get; private set; }

        /// <summary>
        /// Spillerens pointtavle
        /// </summary>
        public YatzyPointTavle PointTavle { get; set; } = new YatzyPointTavle();

        /// <summary>
        /// Opret en spiller med et konkret navn
        /// </summary>
        /// <param name="navn">Navn</param>
        public YatzySpiller(string navn)
        {
            if (string.IsNullOrEmpty(navn))
                throw new YatzyException("Forkert navn");
            Navn = navn;
        }
    }
}