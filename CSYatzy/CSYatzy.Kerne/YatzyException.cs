using System;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// YatzyException der benyttes som exception af alle klasser
    /// </summary>
    public class YatzyException : Exception
    {
        /// <summary>
        /// Opret exception med en meddelelse
        /// </summary>
        /// <param name="message">Meddelelse</param>
        public YatzyException(string message) : base(message)
        {
        }
    }
}