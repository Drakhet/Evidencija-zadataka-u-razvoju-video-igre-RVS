using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlojPodataka.KlasePodataka;

namespace SlojPodataka.TehnoloskeKlase
{
    public class ZadatakRepozitorijum
    {
        private readonly EvidencijaDbContext _kontekst;

        public ZadatakRepozitorijum(EvidencijaDbContext kontekst)
        {
            _kontekst = kontekst;
        }

        public List<Zadatak> DohvatiSve()
        {
            return _kontekst.Zadaci
                .AsNoTracking()
                .Include(z => z.KljucnaTackaRazvoja)
                .Include(z => z.ClanTima)
                .Include(z => z.Stavke)
                .OrderBy(z => z.RokZaZavrsetak)
                .ToList();
        }

        public Zadatak? DohvatiPoId(int id)
        {
            return _kontekst.Zadaci
                .Include(z => z.KljucnaTackaRazvoja)
                .Include(z => z.ClanTima)
                .Include(z => z.Stavke)
                .FirstOrDefault(z => z.ZadatakID == id);
        }

        public bool SifraPostoji(string sifra, int? izuzmiId = null)
        {
            return _kontekst.Zadaci.Any(z => z.Sifra == sifra &&
                (izuzmiId == null || z.ZadatakID != izuzmiId));
        }

        public List<KljucnaTackaRazvoja> DohvatiKljucneTacke()
        {
            return _kontekst.KljucneTacke
                .OrderBy(k => k.DatumPocetka)
                .ToList();
        }

        public List<ClanTima> DohvatiClanove()
        {
            return _kontekst.Clanovi
                .OrderBy(c => c.Prezime)
                .ToList();
        }

        public void Dodaj(Zadatak zadatak)
        {
            using var transakcija = _kontekst.Database.BeginTransaction();
            try
            {
                int rb = 1;
                foreach (var stavka in zadatak.Stavke)
                    stavka.RedniBroj = rb++;

                _kontekst.Zadaci.Add(zadatak);
                _kontekst.SaveChanges();
                transakcija.Commit();
            }
            catch
            {
                transakcija.Rollback();
                throw;
            }
        }
        public void AzurirajPrioritet(int zadatakId, string prioritet)
        {
            var zadatak = _kontekst.Zadaci.Find(zadatakId);
            if (zadatak == null) return;
            zadatak.Prioritet = prioritet;
            _kontekst.SaveChanges();
        }
        public void Izmeni(Zadatak zadatak)
        {
            using var transakcija = _kontekst.Database.BeginTransaction();
            try
            {
                var stareStavke = _kontekst.StavkeZadataka
                    .Where(s => s.ZadatakID == zadatak.ZadatakID)
                    .ToList();
                _kontekst.StavkeZadataka.RemoveRange(stareStavke);
                _kontekst.SaveChanges();

                var postojeci = _kontekst.Zadaci
                    .FirstOrDefault(z => z.ZadatakID == zadatak.ZadatakID);

                if (postojeci == null) throw new Exception("Zadatak nije pronađen.");

                postojeci.Sifra = zadatak.Sifra;
                postojeci.Naziv = zadatak.Naziv;
                postojeci.Opis = zadatak.Opis;
                postojeci.TipZadatka = zadatak.TipZadatka;
                postojeci.Prioritet = zadatak.Prioritet;
                postojeci.Status = zadatak.Status;
                postojeci.ProcenatZavrsenosti = zadatak.ProcenatZavrsenosti;
                postojeci.RokZaZavrsetak = zadatak.RokZaZavrsetak;
                postojeci.Napomena = zadatak.Napomena;
                postojeci.KljucnaTackaRazvojaID = zadatak.KljucnaTackaRazvojaID;
                postojeci.ClanTimaID = zadatak.ClanTimaID;

                int rb = 1;
                foreach (var stavka in zadatak.Stavke)
                {
                    stavka.ZadatakID = postojeci.ZadatakID;
                    stavka.StavkaID = 0;
                    stavka.RedniBroj = rb++;
                    _kontekst.StavkeZadataka.Add(stavka);
                }

                _kontekst.SaveChanges();
                transakcija.Commit();
            }
            catch
            {
                transakcija.Rollback();
                throw;
            }
        }

        public void Obrisi(int id)
        {
            using var transakcija = _kontekst.Database.BeginTransaction();
            try
            {
                var zadatak = _kontekst.Zadaci
                    .Include(z => z.Stavke)
                    .FirstOrDefault(z => z.ZadatakID == id);

                if (zadatak == null) throw new Exception("Zadatak nije pronađen.");

                _kontekst.StavkeZadataka.RemoveRange(zadatak.Stavke);
                _kontekst.Zadaci.Remove(zadatak);
                _kontekst.SaveChanges();
                transakcija.Commit();
            }
            catch
            {
                transakcija.Rollback();
                throw;
            }
        }

        public List<Zadatak> Filtriraj(string? status, string? tipZadatka,
                                        int? kljucnaTackaId)
        {
            var upit = _kontekst.Zadaci
                .Include(z => z.KljucnaTackaRazvoja)
                .Include(z => z.ClanTima)
                .Include(z => z.Stavke)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                upit = upit.Where(z => z.Status == status);

            if (!string.IsNullOrEmpty(tipZadatka))
                upit = upit.Where(z => z.TipZadatka == tipZadatka);

            if (kljucnaTackaId.HasValue)
                upit = upit.Where(z => z.KljucnaTackaRazvojaID == kljucnaTackaId.Value);

            return upit.OrderBy(z => z.RokZaZavrsetak).ToList();
        }

        public int DohvatiUkupanBrojZadatakaPrekoSP()
        {
            using var veza = new Microsoft.Data.SqlClient.SqlConnection(
                _kontekst.Database.GetConnectionString());
            var komanda = new Microsoft.Data.SqlClient.SqlCommand(
                "sp_DajUkupanBrojZadataka", veza);
            komanda.CommandType = System.Data.CommandType.StoredProcedure;
            veza.Open();
            var rezultat = komanda.ExecuteScalar();
            return Convert.ToInt32(rezultat);
        }
    }
}