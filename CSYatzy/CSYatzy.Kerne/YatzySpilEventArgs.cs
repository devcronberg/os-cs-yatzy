using System;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// EventsArgs der benyttes af yatzy hændelser
    /// </summary>
    public class YatzySpilEventArgs : EventArgs
    {
        /// <summary>
        /// Det spil som hændelsen hører til
        /// </summary>
        public YatzySpil Spil { get; set; }
    }
}