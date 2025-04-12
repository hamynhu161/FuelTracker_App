using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankkaussovellus
{
    public class Tankkaus
    {
        public string Pvm {  get; set; }
        public int Kilometrit { get; set; }
        public int Litraa { get; set; }
        public double Summa { get; set; }

        public static double LaskeKeskikulutus (Tankkaus edellinen, Tankkaus nykyinen)
        {
            int kilemetritEro = nykyinen.Kilometrit - edellinen.Kilometrit;
            int litratEro = nykyinen.Litraa;
            double keskiKulutus = (double)litratEro / kilemetritEro;

            if(kilemetritEro > 0)
            {
                return keskiKulutus;
            }
            else
            {
                return 0;
            }
        }
    }
}
