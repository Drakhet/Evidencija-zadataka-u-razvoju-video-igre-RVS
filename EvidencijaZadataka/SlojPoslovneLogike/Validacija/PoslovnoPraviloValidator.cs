using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlojPodataka.KlasePodataka;
using SlojPoslovneLogike.Ogranicenja;

namespace SlojPoslovneLogike.Validacija
{
    public class PoslovnoPraviloValidator
    {
        private readonly CitacPravila _citacPravila;

        public PoslovnoPraviloValidator(CitacPravila citacPravila)
        {
            _citacPravila = citacPravila;
        }

        public bool TrebaEskaliratiPrioritet(Zadatak zadatak)
        {
            if (zadatak.Status == "Završeno") return false;
            if (zadatak.Prioritet == "Visok") return false;

            int prag = _citacPravila.DohvatiRokZaPrioritet();
            int daniDoRoka = (zadatak.RokZaZavrsetak - DateTime.Today).Days;

            return daniDoRoka <= prag;
        }

        public (bool Uspesno, string Poruka) ValidirajProcenatIStatus(
            int procenat, string status)
        {
            int min = _citacPravila.DohvatiMinimalniProcenatZaRad();
            int max = _citacPravila.DohvatiMaksimalniProcenat();

            if (procenat < 0 || procenat > max)
                return (false, $"Procenat mora biti između 0 i {max}.");

            if (status == "U radu" && procenat < min)
                return (false,
                    $"Za status 'U radu' procenat mora biti najmanje {min}%.");

            if (status == "Završeno" && procenat != 100)
                return (false, "Za status 'Završeno' procenat mora biti 100%.");

            return (true, "Validacija uspešna.");
        }
    }
}