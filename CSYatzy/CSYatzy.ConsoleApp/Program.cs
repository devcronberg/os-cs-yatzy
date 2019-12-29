using CSYatzy.Kerne;
using System;
using System.Linq;

namespace CSYatzy.ConsoleApp
{
    /// <summary>
    /// Selve programmet
    /// </summary>
    class Program
    {

        /// <summary>
        /// Main er start metoden og indeholder brugerflade til hele spillet. Al logik findes i separat assembly
        /// </summary>
        static void Main()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Velkommen til Yatzy");
                Console.WriteLine("-------------------");
                Console.WriteLine();
                Console.WriteLine("Hvem skal spille (navn1,navn2,navn3 mv)");
                string[] navne = Console.ReadLine().Split(',').Select(i => i.Trim()).ToArray();

                YatzySpil spil = new YatzySpil(navne);

                spil.StartHændelse += (s, e) =>
                {
                    Console.WriteLine();
                    Console.WriteLine($"Nu starter spillet med {e.Spil.Spillere.Count} spiller{(e.Spil.Spillere.Count > 1 ? "e" : "")} - Tryk en tast for at starte!");
                    Console.ReadKey();
                };

                spil.NæsteHændelse += (s, e) =>
                {

                    if (e.Spil.AktuelSpillerAntalSlag < 3)
                    {
                        Console.Clear();
                        Console.WriteLine($"Nu slår {e.Spil.AktuelSpiller.Navn} som her har slået sit {e.Spil.AktuelSpillerAntalSlag}. slag:");
                        Console.WriteLine();
                        Console.WriteLine(e.Spil.Bæger);
                        Console.WriteLine();
                        Console.WriteLine(e.Spil.AktuelSpiller.PointTavle);
                        Console.WriteLine();
                        Console.WriteLine("Angiv hvilke terninger du ønsker at holde på/frigive (feks. 12, 15, 135 mv)");
                        int[] hold = HentVærdierTilHold();
                        if (hold != null)
                            foreach (var item in hold)
                            {
                                e.Spil.Bæger.HoldTerning(item - 1, !e.Spil.Bæger[item - 1].ErHoldt);
                            }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{e.Spil.AktuelSpiller.Navn} har nu slået sit 3. slag og har følgende terninger");
                        Console.WriteLine();
                        Console.WriteLine(e.Spil.Bæger);
                        Console.WriteLine();
                        Console.WriteLine("Her er dine point");
                        Console.WriteLine(e.Spil.AktuelSpiller.PointTavle);
                        Console.WriteLine();
                        Console.WriteLine("Vælg hvordag du vil bruge terninger - du kan vælge mellem følgende:");
                        Console.WriteLine();
                        var muligePoint = e.Spil.AktuelSpiller.PointTavle.FindMuligePoint(e.Spil.Bæger);
                        if (muligePoint.Count > 0)
                        {
                            for (int i = 0; i < muligePoint.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}: {muligePoint[i].Navn} (du får {muligePoint[i].Point} point - max point er {muligePoint[i].MaxPoint})");
                            }
                            int valg = HentVærdierTilPoint(muligePoint.Count()) - 1;
                            e.Spil.AktuelSpiller.PointTavle.TilføjPoint(muligePoint[valg].PointType, muligePoint[valg].Point);
                        }
                        else
                        {
                            Console.WriteLine("Der er ikke nogle mulige point - du bliver nødt til at strege. Du kan vælge mellem følgende:");
                            var ip = e.Spil.AktuelSpiller.PointTavle.FindIkkeBenyttedePoint();
                            for (int i = 0; i < ip.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}: {ip[i].ToString()}");
                            }
                            int valg = Convert.ToInt32(Console.ReadLine()) - 1;
                            e.Spil.AktuelSpiller.PointTavle.TilføjPoint(ip[valg], 0);
                        }
                    }
                };

                spil.SlutHændelse += (s, e) =>
                {
                    Console.Clear();
                    Console.WriteLine("Spillet er nu slut - her er den samlede stilling");
                    Console.WriteLine();
                    foreach (var item in e.Spil.SpillereSorteretEfterStilling)
                    {
                        Console.WriteLine($"{item.Navn.PadRight(20)} {item.PointTavle.Sum}");
                    }
                    Console.WriteLine();
                    foreach (var item in e.Spil.SpillereSorteretEfterStilling)
                    {
                        Console.WriteLine("Point for " + item.Navn);
                        Console.WriteLine();
                        Console.WriteLine(item.PointTavle);
                    }
                };

                do
                {
                    spil.Næste();
                } while (!spil.Slut());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Der er desværre sket en fejl");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Programmet afsluttes");
            }

        }

        /// <summary>
        /// Henter et tal mellem 1 og max
        /// </summary>
        /// <param name="max">Det største tilladte tal</param>
        /// <returns>Tal</returns>
        private static int HentVærdierTilPoint(int max)
        {
            string tmp = "";
            while (true)
            {
                try
                {
                    tmp = Console.ReadLine().Trim();
                    int res = Convert.ToInt32(tmp);
                    if (res < 1 || res > max)
                        throw new ApplicationException();
                    return res;
                }
                catch (Exception)
                {
                    Console.WriteLine("Forstår ikke '" + tmp + "' - prøv igen");
                }
            }
        }

        /// <summary>
        /// Henter et array af tal mellem 1-5 uden dubletter
        /// </summary>
        /// <returns>Array af tal</returns>
        private static int[] HentVærdierTilHold()
        {
            string tmp = "";
            while (true)
            {
                try
                {
                    tmp = Console.ReadLine();
                    if (tmp.Trim() == "")
                        return null;

                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (!"12345".Contains(tmp[i]))
                        {
                            throw new ApplicationException();
                        }
                    }

                    var stringArray = tmp.ToCharArray().Select(i => i.ToString()).ToArray();
                    var intArray = Array.ConvertAll(stringArray, int.Parse);
                    var distrinctArray = intArray.Distinct().ToArray();
                    if (distrinctArray.Count() != intArray.Count())
                        throw new ApplicationException();

                    return intArray.OrderBy(i => i).ToArray();

                }
                catch (Exception)
                {
                    Console.WriteLine("Forstår ikke '" + tmp + "' - prøv igen");
                }
            }
        }
    }
}