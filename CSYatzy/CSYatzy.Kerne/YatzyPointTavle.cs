using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSYatzy.Kerne
{
    /// <summary>
    /// Repræsentere en enkelt pointtavle (enere, toere, chancen, yatzy mv)
    /// </summary>
    public class YatzyPointTavle
    {
        private readonly Dictionary<YatzyPointType, int?> point;

        /// <summary>
        /// Opret pointtavle
        /// </summary>
        public YatzyPointTavle()
        {
            point = new Dictionary<YatzyPointType, int?>();
            foreach (var item in Enum.GetValues(typeof(YatzyPointType)))
            {
                point.Add((YatzyPointType)item, null);
            }
        }

        /// <summary>
        /// Er der bonus (enkelt terninger skal have en værdi over 63)
        /// </summary>
        public int Bonus
        {
            get
            {
                return SumEnkeltTerninger >= 63 ? 50 : 0;
            }
        }

        /// <summary>
        /// Den samlede sum
        /// </summary>
        public int Sum
        {
            get
            {
                int sum = 0;
                foreach (var item in point)
                {
                    sum += item.Value.GetValueOrDefault(0);
                }
                return sum + Bonus;
            }
        }

        /// <summary>
        /// Beregner summen af enkelt terninger
        /// </summary>
        public int SumEnkeltTerninger
        {
            get
            {
                return point[YatzyPointType.Enere].GetValueOrDefault(0) +
                    point[YatzyPointType.Toere].GetValueOrDefault(0) +
                    point[YatzyPointType.Treere].GetValueOrDefault(0) +
                    point[YatzyPointType.Fireere].GetValueOrDefault(0) +
                    point[YatzyPointType.Femere].GetValueOrDefault(0) +
                    point[YatzyPointType.Seksere].GetValueOrDefault(0);
            }
        }

        /// <summary>
        /// Er der nogle pointtyper som ikke er benyttet
        /// </summary>
        /// <returns>true/false</returns>
        public List<YatzyPointType> FindIkkeBenyttedePoint()
        {
            List<YatzyPointType> res = new List<YatzyPointType>();
            foreach (var item in point)
            {
                if (!item.Value.HasValue)
                    res.Add(item.Key);
            }
            return res;
        }

        /// <summary>
        /// Finder de konkrete mulige typer af point udfra et bæger med terninger
        /// </summary>
        /// <param name="bæger">Bæger</param>
        /// <returns>Liste af mulige point</returns>
        public List<YatzyPoint> FindMuligePoint(YatzyBæger bæger)
        {
            List<YatzyPoint> lst = FindAlleMuligePoint(bæger);
            List<YatzyPoint> res = new List<YatzyPoint>();
            foreach (var item in lst)
            {
                if (!point[item.PointType].HasValue)
                    res.Add(item);
            }
            return res;
        }

        /// <summary>
        /// Tilføj point
        /// </summary>
        /// <param name="type">Den konkret point type</param>
        /// <param name="samletPoint">De ønskede antal point</param>
        public void TilføjPoint(YatzyPointType type, int samletPoint)
        {
            if (point[type] != null)
                throw new YatzyException("Der er allerede en værdi til " + type);
            point[type] = samletPoint;
        }

        /// <summary>
        /// Streng der viser point tavlen
        /// </summary>
        /// <returns>Tekst</returns>
        public override string ToString()
        {
            System.Text.StringBuilder b = new StringBuilder();
            foreach (var item in point)
            {
                b.AppendLine(item.Key.ToString().PadRight(15) + item.Value.ToString().PadLeft(5));
                if (item.Key == YatzyPointType.Seksere)
                {
                    b.AppendLine("=Sum".PadRight(15) + SumEnkeltTerninger.ToString().PadLeft(5));
                    b.AppendLine("Bonus".PadRight(15) + Bonus.ToString().PadLeft(5));
                }
            }
            b.AppendLine("=Sum".PadRight(15) + Sum.ToString().PadLeft(5));
            return b.ToString();
        }

        /// <summary>
        /// Hvilke point typer er mulige med et bæger af terninger
        /// </summary>
        /// <param name="bæger">Bæger med terninger</param>
        /// <returns>Liste af point</returns>
        private List<YatzyPoint> FindAlleMuligePoint(YatzyBæger bæger)
        {
            List<YatzyPoint> lst = new List<YatzyPoint>();
            for (int i = 1; i < 7; i++)
            {
                if (bæger.FindAntalEnsTerninger(i) > 0)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        Navn = $"{bæger.FindAntalEnsTerninger(i)} stk. af {i}'ere",
                        PointType = (YatzyPointType)(i - 1),
                        Point = bæger.FindAntalEnsTerninger(i) * i,
                        MaxPoint = i * 5,
                        Sortering = i
                    };
                    lst.Add(p);
                }

                if (bæger.FindAntalEnsTerninger(i) >= 2)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        Navn = "Et par i " + i + "'ere",
                        PointType = YatzyPointType.EtPar,
                        MaxPoint = 12,
                        Point = i * 2,
                        Sortering = 7
                    };
                    lst.Add(p);
                }

                if (bæger.FindAntalEnsTerninger(i) >= 3)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        Navn = "Tre ens af " + i + "'ere",
                        PointType = YatzyPointType.TreEns,
                        MaxPoint = 18,
                        Point = i * 3,
                        Sortering = 9
                    };
                    lst.Add(p);
                }

                if (bæger.FindAntalEnsTerninger(i) >= 4)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        Navn = "Fire ens af " + i + "'ere",
                        MaxPoint = 24,
                        PointType = YatzyPointType.FireEns,
                        Point = i * 4,
                        Sortering = 10
                    };
                    lst.Add(p);
                }

                if (bæger.FindAntalEnsTerninger(i) == 5)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        Navn = "Yatzy (5 ens) med " + i + "'ere",
                        MaxPoint = 50,
                        PointType = YatzyPointType.Yatzy,
                        Point = 50,
                        Sortering = 15
                    };
                    lst.Add(p);

                }
            }

            var d = bæger.FindAntalEnsTerninger();
            var tmp = d.Where(i => i.Value >= 1).Select(i => new { i.Key, i.Value }).ToArray();

            if (tmp.Count() == 5)
            {
                if (d[1] > 0)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        Navn = "Lille (1,2,3,4,5)",
                        MaxPoint = 1 + 2 + 3 + 4 + 5,
                        PointType = YatzyPointType.Lille,
                        Sortering = 11,
                        Point = 1 + 2 + 3 + 4 + 5
                    };
                    lst.Add(p);
                }
                else
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        PointType = YatzyPointType.Stor,
                        Navn = "Stor (2,3,4,5,6)",
                        Sortering = 12,
                        Point = 2 + 3 + 4 + 5 + 6,
                        MaxPoint = 2 + 3 + 4 + 5 + 6,
                    };
                    lst.Add(p);
                }
            }

            tmp = d.Where(i => i.Value >= 2).Select(i => new { i.Key, i.Value }).ToArray();
            if (tmp.Count() == 2)
            {
                if (tmp[0].Value >= 2 && tmp[1].Value >= 2)
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        PointType = YatzyPointType.ToPar,
                        Navn = $"2 par af {tmp[0].Key}'ere og {tmp[1].Key}'ere",
                        Sortering = 8,
                        Point = (tmp[0].Key * 2) + (tmp[1].Key * 2),
                        MaxPoint = 22
                    };
                    lst.Add(p);
                }
            }

            tmp = d.Where(i => i.Value >= 2).Select(i => new { i.Key, i.Value }).ToArray();
            if (tmp.Count() == 2)
            {
                if ((tmp[0].Value >= 3 && tmp[1].Value >= 2) || (tmp[0].Value >= 2 && tmp[1].Value >= 3))
                {
                    YatzyPoint p = new YatzyPoint
                    {
                        PointType = YatzyPointType.FuldtHus,
                        Navn = $"Fuldt hus med {tmp[0].Key}'ere og {tmp[1].Key}'ere",
                        Sortering = 13,
                        Point = (tmp[0].Key * tmp[0].Value) + (tmp[1].Key * tmp[1].Value),
                        MaxPoint = 28
                    };
                    lst.Add(p);
                }
            }

            {
                YatzyPoint p = new YatzyPoint
                {
                    PointType = YatzyPointType.Chancen,
                    Navn = "Chancen",
                    Sortering = 14,
                    MaxPoint = 30,
                    Point = bæger.Sum
                };
                lst.Add(p);
            }

            lst.Sort();
            return lst;
        }
    }
}