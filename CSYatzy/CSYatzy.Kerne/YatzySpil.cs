using System;
using System.Collections.Generic;
using System.Linq;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// Repræsenterer et spil med spillere
    /// </summary>
    public class YatzySpil
    {
        /// <summary>
        /// Opret et spil med et antal spillere
        /// </summary>
        /// <param name="navne">Array af navne</param>
        public YatzySpil(params string[] navne)
        {
            if (navne == null)
                throw new YatzyException("Mangler spillere");

            Spillere = new List<YatzySpiller>();
            foreach (var item in navne)
            {
                Spillere.Add(new YatzySpiller(item));
            }
            AktuelSpiller = Spillere[0];
            Bæger = new YatzyBæger();
            AktuelSpillerAntalSlag = 0;
        }

        private int antalSlagIAlt = 0;

        /// <summary>
        /// "Hjerteslaget" i spillet som smides hver kan en spiller ryster bægeret
        /// </summary>
        public event EventHandler<YatzySpilEventArgs> NæsteHændelse;

        /// <summary>
        /// Hændelse der smides når spillet er slut
        /// </summary>
        public event EventHandler<YatzySpilEventArgs> SlutHændelse;

        /// <summary>
        /// Hændelse der smides når spillet startes
        /// </summary>
        public event EventHandler<YatzySpilEventArgs> StartHændelse;

        /// <summary>
        /// Fortæller hvor mange slag den aktuelle spiller har slået
        /// </summary>
        public int AktuelSpillerAntalSlag { get; private set; }
        
        /// <summary>
        /// Spillets bæger med terninger som deles mellem spillerne
        /// </summary>
        public YatzyBæger Bæger { get; private set; }
        
        /// <summary>
        /// Den aktuelle spiller
        /// </summary>
        public YatzySpiller AktuelSpiller { get; private set; }
        /// <summary>
        /// Samtlige spillere i spillet
        /// </summary>
        public List<YatzySpiller> Spillere { get; }
        /// <summary>
        /// Spillere sorteret efter stilling (sum i pointtavle)
        /// </summary>
        public List<YatzySpiller> SpillereSorteretEfterStilling
        {
            get
            {
                return Spillere.OrderByDescending(i => i.PointTavle.Sum).ToList();
            }
        }

        /// <summary>
        /// "Hjerteslaget" - metoden som kaldet i et loop hver gang der skal slås med terningerne
        /// </summary>
        public void Næste()
        {

            if (antalSlagIAlt == 0)
                StartHændelse?.Invoke(this, new YatzySpilEventArgs { Spil = this });

            if (AktuelSpillerAntalSlag < 3)
            {
                AktuelSpillerAntalSlag++;
                antalSlagIAlt++;
                Bæger.Ryst();
            }
            else
            {
                Bæger.FjernHoldFraAlleTerninger();
                Bæger.Ryst();
                AktuelSpillerAntalSlag = 1;
                int index = Spillere.IndexOf(AktuelSpiller);
                if (index < Spillere.Count() - 1)
                    AktuelSpiller = Spillere[index + 1];
                else
                    AktuelSpiller = Spillere[0];
            }

            NæsteHændelse?.Invoke(this, new YatzySpilEventArgs { Spil = this });

            if (Slut())
            {
                SlutHændelse?.Invoke(this, new YatzySpilEventArgs { Spil = this });
            }
        }

        /// <summary>
        /// Metode som fortæller om spillet er slut
        /// </summary>
        /// <returns>true/false</returns>
        public bool Slut()
        {
            return Spillere.Where(i => i.PointTavle.FindIkkeBenyttedePoint().Count == 0).Count() == Spillere.Count();
        }
    }
}