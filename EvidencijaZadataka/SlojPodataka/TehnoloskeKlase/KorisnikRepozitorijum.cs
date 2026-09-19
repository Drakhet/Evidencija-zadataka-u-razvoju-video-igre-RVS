using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlojPodataka.KlasePodataka;

namespace SlojPodataka.TehnoloskeKlase
{
    public class KorisnikRepozitorijum
    {
        private readonly EvidencijaDbContext _kontekst;

        public KorisnikRepozitorijum(EvidencijaDbContext kontekst)
        {
            _kontekst = kontekst;
        }

        public Korisnik? DohvatiPoKorisnickomImenu(string korisnickoIme)
        {
            return _kontekst.Korisnici
                .FirstOrDefault(k => k.KorisnickoIme == korisnickoIme);
        }

        public bool PostojiKorisnickoIme(string korisnickoIme)
        {
            return _kontekst.Korisnici
                .Any(k => k.KorisnickoIme == korisnickoIme);
        }

        public bool PostojiEmail(string email)
        {
            return _kontekst.Korisnici
                .Any(k => k.Email == email);
        }

        public void Dodaj(Korisnik korisnik)
        {
            _kontekst.Korisnici.Add(korisnik);
            _kontekst.SaveChanges();
        }
    }
}