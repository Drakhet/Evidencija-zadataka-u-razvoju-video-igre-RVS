using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlojPodataka.KlasePodataka;
using SlojPodataka.TehnoloskeKlase;
using SlojPoslovneLogike.Validacija;

namespace SlojPoslovneLogike.Stanje
{
    public class PrikupljanjeStanja
    {
        private readonly ZadatakRepozitorijum _repo;
        private readonly PoslovnoPraviloValidator _validator;

        public PrikupljanjeStanja(
            ZadatakRepozitorijum repo,
            PoslovnoPraviloValidator validator)
        {
            _repo = repo;
            _validator = validator;
        }
        public List<Zadatak> PrimeniPravilo()
        {
            var sviZadaci = _repo.DohvatiSve();
            Console.WriteLine($"Ukupno zadataka: {sviZadaci.Count}");

            var izmenjeni = new List<Zadatak>();

            foreach (var zadatak in sviZadaci)
            {
                bool trebaEskalirati =
                    _validator.TrebaEskaliratiPrioritet(zadatak);

                Console.WriteLine($"Zadatak: {zadatak.Naziv}, " +
                                  $"Prioritet: {zadatak.Prioritet}, " +
                                  $"DaniDoRoka: {(zadatak.RokZaZavrsetak - DateTime.Today).Days}, " +
                                  $"TrebaEskalirati: {trebaEskalirati}");

                if (trebaEskalirati)
                {
                    _repo.AzurirajPrioritet(zadatak.ZadatakID, "Visok");
                    zadatak.Prioritet = "Visok";
                    izmenjeni.Add(zadatak);
                }
            }

            return izmenjeni;
        }

        public bool PrimeniPraviloNaJedan(Zadatak zadatak)
        {
            bool trebaEskalirati =
                _validator.TrebaEskaliratiPrioritet(zadatak);

            if (!trebaEskalirati) return false;

            _repo.AzurirajPrioritet(zadatak.ZadatakID, "Visok");
            zadatak.Prioritet = "Visok";
            return true;
        }
        public int DaniDoRoka(Zadatak zadatak)
        {
            return (zadatak.RokZaZavrsetak - DateTime.Today).Days;
        }
    }
}