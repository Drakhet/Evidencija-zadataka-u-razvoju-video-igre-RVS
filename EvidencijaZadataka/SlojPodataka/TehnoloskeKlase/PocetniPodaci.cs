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
            Console.WriteLine($"PopuniSve pozvan, fajl postoji: {File.Exists(putanjaXml)}");
            if (!File.Exists(putanjaXml)) return;

            var xml = XDocument.Load(putanjaXml);

            PopuniKljucneTacke(kontekst, xml);
            PopuniClanove(kontekst, xml);
            PopuniKorisnike(kontekst, xml);
            PopuniZadatke(kontekst, xml);
            PopuniStavke(kontekst, xml);
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
        private static void PopuniZadatke(EvidencijaDbContext kontekst, XDocument xml)
        {
            if (kontekst.Zadaci.Any()) return;

            var zadaci = xml.Descendants("Zadatak").Select(z => new Zadatak
            {
                Sifra = z.Element("Sifra")?.Value ?? string.Empty,
                Naziv = z.Element("Naziv")?.Value ?? string.Empty,
                Opis = z.Element("Opis")?.Value ?? string.Empty,
                TipZadatka = z.Element("TipZadatka")?.Value ?? string.Empty,
                Prioritet = z.Element("Prioritet")?.Value ?? "Srednji",
                Status = z.Element("Status")?.Value ?? "Otvoren",
                ProcenatZavrsenosti = int.Parse(
                    z.Element("ProcenatZavrsenosti")?.Value ?? "0"),
                RokZaZavrsetak = DateTime.Parse(
                    z.Element("RokZaZavrsetak")?.Value
                    ?? DateTime.Now.AddMonths(1).ToString()),
                Napomena = z.Element("Napomena")?.Value ?? string.Empty,
                KljucnaTackaRazvojaID = int.Parse(
                    z.Element("KljucnaTackaRazvojaID")?.Value ?? "1"),
                ClanTimaID = int.Parse(
                    z.Element("ClanTimaID")?.Value ?? "1"),
                DatumKreiranja = DateTime.Now
            }).ToList();

            if (zadaci.Any())
            {
                kontekst.Zadaci.AddRange(zadaci);
                kontekst.SaveChanges();
            }
        }

        private static void PopuniStavke(EvidencijaDbContext kontekst, XDocument xml)
        {
            if (kontekst.StavkeZadataka.Any()) return;

            var zadaci = kontekst.Zadaci
                .OrderBy(z => z.ZadatakID)
                .ToList();

            var stavke = xml.Descendants("StavkaZadatka").Select(s =>
            {
                int redosled = int.Parse(s.Element("ZadatakID")?.Value ?? "1");
                var zadatak = zadaci.ElementAtOrDefault(redosled - 1);

                return new StavkaZadatka
                {
                    ZadatakID = zadatak?.ZadatakID ?? 0,
                    RedniBroj = int.Parse(s.Element("RedniBroj")?.Value ?? "1"),
                    NazivPodzadatka = s.Element("NazivPodzadatka")?.Value ?? string.Empty,
                    Zavrseno = bool.Parse(s.Element("Zavrseno")?.Value ?? "false"),
                    DatumKreiranja = DateTime.Now
                };
            }).Where(s => s.ZadatakID > 0).ToList();

            if (stavke.Any())
            {
                kontekst.StavkeZadataka.AddRange(stavke);
                kontekst.SaveChanges();
            }
        }
    }
}