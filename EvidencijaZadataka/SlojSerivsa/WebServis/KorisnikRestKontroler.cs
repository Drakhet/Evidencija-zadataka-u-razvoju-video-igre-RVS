using Microsoft.AspNetCore.Mvc;
using SlojPodataka.KlasePodataka;
using SlojPodataka.TehnoloskeKlase;

namespace SlojServisa.WebServis
{
    [ApiController]
    [Route("api/KorisnikRest")]
    public class KorisnikRestKontroler : ControllerBase
    {
        private readonly KorisnikRepozitorijum _repo;

        public KorisnikRestKontroler(KorisnikRepozitorijum repo)
        {
            _repo = repo;
        }

        [HttpPost("prijava")]
        public ActionResult<string> Prijava([FromBody] PrijavaZahtev zahtev)
        {
            var korisnik = _repo.DohvatiPoKorisnickomImenu(zahtev.KorisnickoIme);

            if (korisnik == null)
                return Unauthorized("Korisničko ime nije pronađeno.");

            bool ispravnaLozinka = FunkcijeLozinke.ProveriLozinku(
                zahtev.Lozinka, korisnik.Salt, korisnik.LozinkaHes);

            if (!ispravnaLozinka)
                return Unauthorized("Lozinka nije ispravna.");

            return Ok(korisnik.KorisnickoIme);
        }

        [HttpPost("registracija")]
        public ActionResult Registracija([FromBody] RegistracijaZahtev zahtev)
        {
            if (_repo.PostojiKorisnickoIme(zahtev.KorisnickoIme))
                return Conflict("Korisničko ime je zauzeto.");

            if (_repo.PostojiEmail(zahtev.Email))
                return Conflict("Email adresa je već registrovana.");

            var salt = FunkcijeLozinke.GenerisiSalt();
            var korisnik = new Korisnik
            {
                KorisnickoIme = zahtev.KorisnickoIme,
                Email = zahtev.Email,
                Salt = salt,
                LozinkaHes = FunkcijeLozinke.IzracunajHash(zahtev.Lozinka, salt)
            };

            _repo.Dodaj(korisnik);
            return Ok("Registracija je uspešna.");
        }
    }

    public class PrijavaZahtev
    {
        public string KorisnickoIme { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
    }

    public class RegistracijaZahtev
    {
        public string KorisnickoIme { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
    }
}