using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlojPodataka.KlasePodataka;
using System.Xml.Linq;

namespace SlojPodataka.TehnoloskeKlase
{
    public static class PocetniPodaci
    {
        public static void PopuniSve(EvidencijaDbContext kontekst, string putanjaXml)
        {
            if (!File.Exists(putanjaXml)) return;

            var xml = XDocument.Load(putanjaXml);

            PopuniKljucneTacke(kontekst, xml);
            PopuniClanove(kontekst, xml);
            PopuniKorisnike(kontekst, xml);
        }

        private static void PopuniKljucneTacke(EvidencijaDbContext kontekst, XDocument xml)
        {
            if (kontekst.KljucneTacke.Any()) return;

            var tacke = xml.Descendants("KljucnaTackaRazvoja").Select(e => new KljucnaTackaRazvoja
            {
                Naziv = e.Element("Naziv")?.Value ?? string.Empty,
                Opis = e.Element("Opis")?.Value ?? string.Empty,
                DatumPocetka = DateTime.Parse(e.Element("DatumPocetka")?.Value ?? DateTime.Now.ToString()),
                DatumZavrsetka = DateTime.Parse(e.Element("DatumZavrsetka")?.Value ?? DateTime.Now.AddMonths(3).ToString())
            }).ToList();

            if (tacke.Any())
            {
                kontekst.KljucneTacke.AddRange(tacke);
                kontekst.SaveChanges();
            }
        }

        private static void PopuniClanove(EvidencijaDbContext kontekst, XDocument xml)
        {
            if (kontekst.Clanovi.Any()) return;

            var clanovi = xml.Descendants("ClanTima").Select(c => new ClanTima
            {
                Ime = c.Element("Ime")?.Value ?? string.Empty,
                Prezime = c.Element("Prezime")?.Value ?? string.Empty,
                Email = c.Element("Email")?.Value ?? string.Empty,
                Uloga = c.Element("Uloga")?.Value ?? string.Empty
            }).ToList();

            if (clanovi.Any())
            {
                kontekst.Clanovi.AddRange(clanovi);
                kontekst.SaveChanges();
            }
        }

        private static void PopuniKorisnike(EvidencijaDbContext kontekst, XDocument xml)
        {
            if (kontekst.Korisnici.Any()) return;

            var korisnici = xml.Descendants("Korisnik").Select(k =>
            {
                var lozinka = k.Element("Lozinka")?.Value ?? "";
                var salt = FunkcijeLozinke.GenerisiSalt();
                return new Korisnik
                {
                    KorisnickoIme = k.Element("KorisnickoIme")?.Value ?? "",
                    Email = k.Element("Email")?.Value ?? "",
                    Salt = salt,
                    LozinkaHes = FunkcijeLozinke.IzracunajHash(lozinka, salt)
                };
            }).ToList();

            if (korisnici.Any())
            {
                kontekst.Korisnici.AddRange(korisnici);
                kontekst.SaveChanges();
            }
        }
    }
}