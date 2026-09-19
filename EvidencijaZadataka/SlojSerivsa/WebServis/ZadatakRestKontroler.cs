using Microsoft.AspNetCore.Mvc;
using SlojPodataka.KlasePodataka;
using SlojPodataka.TehnoloskeKlase;

namespace SlojServisa.WebServis
{
    [ApiController]
    [Route("api/ZadatakRest")]
    public class ZadatakRestKontroler : ControllerBase
    {
        private readonly ZadatakRepozitorijum _repo;
        private readonly SlojPoslovneLogike.Stanje.PrikupljanjeStanja _prikupljanje;

        public ZadatakRestKontroler(
            ZadatakRepozitorijum repo,
            SlojPoslovneLogike.Stanje.PrikupljanjeStanja prikupljanje)
        {
            _repo = repo;
            _prikupljanje = prikupljanje;
        }

        [HttpGet]
        public ActionResult<List<Zadatak>> DohvatiSve()
        {
            Console.WriteLine("DohvatiSve pozvan");
            _prikupljanje.PrimeniPravilo();
            var zadaci = _repo.DohvatiSve();
            return Ok(zadaci);
        }

        [HttpGet("{id}")]
        public ActionResult<Zadatak> DohvatiPoId(int id)
        {
            var zadatak = _repo.DohvatiPoId(id);
            if (zadatak == null)
                return NotFound($"Zadatak sa ID={id} nije pronađen.");
            return Ok(zadatak);
        }

        [HttpGet("filtriraj")]
        public ActionResult<List<Zadatak>> Filtriraj(
      [FromQuery] string? status,
      [FromQuery] string? tipZadatka,
      [FromQuery] int? kljucnaTackaId)
        {
            Console.WriteLine("Filtriraj pozvan");
            _prikupljanje.PrimeniPravilo();
            var zadaci = _repo.Filtriraj(status, tipZadatka, kljucnaTackaId);
            return Ok(zadaci);
        }

        [HttpGet("ukupno")]
        public ActionResult<int> DohvatiUkupanBroj()
        {
            return Ok(_repo.DohvatiUkupanBrojZadatakaPrekoSP());
        }

        [HttpGet("kljucnetacke")]
        public ActionResult<List<KljucnaTackaRazvoja>> DohvatiKljucneTacke()
        {
            return Ok(_repo.DohvatiKljucneTacke());
        }

        [HttpGet("clanovi")]
        public ActionResult<List<ClanTima>> DohvatiClanove()
        {
            return Ok(_repo.DohvatiClanove());
        }

        [HttpPost]
        public ActionResult Dodaj([FromBody] Zadatak zadatak)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_repo.SifraPostoji(zadatak.Sifra))
                return Conflict($"Zadatak sa šifrom '{zadatak.Sifra}' već postoji.");

            if (zadatak.RokZaZavrsetak < DateTime.Today)
                return BadRequest("Rok za završetak ne može biti u prošlosti.");

            if (string.IsNullOrWhiteSpace(zadatak.Sifra))
                zadatak.Sifra = $"ZAD-{DateTime.Now:yyyy}-{DateTime.Now.Ticks % 10000:D4}";

            zadatak.DatumKreiranja = DateTime.Now;

            int rb = 1;
            foreach (var s in zadatak.Stavke)
                s.RedniBroj = rb++;

            _repo.Dodaj(zadatak);
            _prikupljanje.PrimeniPraviloNaJedan(zadatak);

            return Ok(zadatak);
        }

        [HttpPut("{id}")]
        public ActionResult Izmeni(int id, [FromBody] Zadatak zadatak)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != zadatak.ZadatakID)
                return BadRequest("ID se ne poklapa.");

            if (_repo.SifraPostoji(zadatak.Sifra, id))
                return Conflict($"Drugi zadatak sa šifrom '{zadatak.Sifra}' već postoji.");

            _repo.Izmeni(zadatak);
            return Ok(zadatak);
        }

        [HttpDelete("{id}")]
        public ActionResult Obrisi(int id)
        {
            var zadatak = _repo.DohvatiPoId(id);
            if (zadatak == null)
                return NotFound($"Zadatak sa ID={id} nije pronađen.");

            _repo.Obrisi(id);
            return Ok();
        }
    }
}