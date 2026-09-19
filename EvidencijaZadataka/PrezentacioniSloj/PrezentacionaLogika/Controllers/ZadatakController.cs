using Microsoft.AspNetCore.Mvc;
using PrezentacioniSloj.PrezentacionaLogika.ViewModels;
using SlojPodataka.KlasePodataka;
using System.Net.Http.Json;

namespace PrezentacioniSloj.PrezentacionaLogika.Controllers
{
    public class ZadatakController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string _urlZadatak = "https://localhost:5072/api/ZadatakRest";

        public ZadatakController(IHttpClientFactory fabrika)
        {
            _httpClient = fabrika.CreateClient("default");
        }

        private bool KorisnikPrijavljen()
            => HttpContext.Session.GetString("KorisnickoIme") != null;

        public async Task<IActionResult> Spisak(
            string? status, string? tipZadatka, int? kljucnaTackaId)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var url = $"{_urlZadatak}/filtriraj?" +
                      $"status={status}&tipZadatka={tipZadatka}" +
                      (kljucnaTackaId.HasValue ? $"&kljucnaTackaId={kljucnaTackaId}" : "");

            var zadaci = await _httpClient
                .GetFromJsonAsync<List<Zadatak>>(url)
                ?? new List<Zadatak>();

            var kljucneTacke = await _httpClient
                .GetFromJsonAsync<List<KljucnaTackaRazvoja>>(
                    $"{_urlZadatak}/kljucnetacke")
                ?? new List<KljucnaTackaRazvoja>();

            ViewBag.TrenutniStatus = status;
            ViewBag.TrenutniTip = tipZadatka;
            ViewBag.TrenutnaKljucnaTacka = kljucnaTackaId;
            ViewBag.KljucneTacke = kljucneTacke;

            return View(zadaci);
        }

        public async Task<IActionResult> Detalji(int id)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var zadatak = await _httpClient
                .GetFromJsonAsync<Zadatak>($"{_urlZadatak}/{id}");

            if (zadatak == null) return NotFound();

            ViewBag.DaniDoRoka = (zadatak.RokZaZavrsetak - DateTime.Today).Days;
            return View(zadatak);
        }

        public async Task<IActionResult> Obrisi(int id)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var zadatak = await _httpClient
                .GetFromJsonAsync<Zadatak>($"{_urlZadatak}/{id}");

            if (zadatak == null) return NotFound();
            return View(zadatak);
        }

        [HttpPost, ActionName("Obrisi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObrisiPotvrdjen(int id)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            await _httpClient.DeleteAsync($"{_urlZadatak}/{id}");
            TempData["Poruka"] = "Zadatak je uspešno obrisan.";
            return RedirectToAction(nameof(Spisak));
        }
        public async Task<IActionResult> Stampaj(int id)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var zadatak = await _httpClient
                .GetFromJsonAsync<Zadatak>($"{_urlZadatak}/{id}");

            if (zadatak == null) return NotFound();

            ViewBag.DaniDoRoka = (zadatak.RokZaZavrsetak - DateTime.Today).Days;
            return View(zadatak);
        }

        public async Task<IActionResult> StampajSpisak(
            string? status, string? tipZadatka, int? kljucnaTackaId)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var url = $"{_urlZadatak}/filtriraj?" +
                      $"status={status}&tipZadatka={tipZadatka}" +
                      (kljucnaTackaId.HasValue ? $"&kljucnaTackaId={kljucnaTackaId}" : "");

            var zadaci = await _httpClient
                .GetFromJsonAsync<List<Zadatak>>(url)
                ?? new List<Zadatak>();

            ViewBag.FilterStatus = status ?? "Svi";
            ViewBag.FilterTip = tipZadatka ?? "Svi";
            ViewBag.DatumStampe = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            return View(zadaci);
        }
        private async Task<List<(int ID, string Naziv)>> DohvatiKljucneTackeZaMeni()
        {
            var lista = await _httpClient
                .GetFromJsonAsync<List<KljucnaTackaRazvoja>>(
                    $"{_urlZadatak}/kljucnetacke")
                ?? new List<KljucnaTackaRazvoja>();

            return lista.Select(k => (k.KljucnaTackaRazvojaID, k.Naziv)).ToList();
        }

        private async Task<List<(int ID, string Naziv)>> DohvatiClanovZaMeni()
        {
            var lista = await _httpClient
                .GetFromJsonAsync<List<ClanTima>>(
                    $"{_urlZadatak}/clanovi")
                ?? new List<ClanTima>();

            return lista.Select(c => (c.ClanTimaID, c.PunoIme)).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Unos()
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var model = new ZadatakViewModel
            {
                KljucneTacke = await DohvatiKljucneTackeZaMeni(),
                Clanovi = await DohvatiClanovZaMeni(),
                Stavke = new List<StavkaViewModel> { new() }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unos(ZadatakViewModel model)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            model.Stavke = model.Stavke
                .Where(s => !string.IsNullOrWhiteSpace(s.NazivPodzadatka))
                .ToList();
            var kljuceviZaBrisanje = ModelState.Keys.Where(k => k.StartsWith("Stavke")).ToList();
            foreach (var kljuc in kljuceviZaBrisanje)
                ModelState.Remove(kljuc);

            if (!ModelState.IsValid)
            {
                model.KljucneTacke = await DohvatiKljucneTackeZaMeni();
                model.Clanovi = await DohvatiClanovZaMeni();
                return View(model);
            }

            var zadatak = MapirajUEntitet(model);
            var odgovor = await _httpClient.PostAsJsonAsync(_urlZadatak, zadatak);

            if (odgovor.IsSuccessStatusCode)
            {
                TempData["Poruka"] = $"Zadatak '{model.Naziv}' je uspešno dodat.";
                return RedirectToAction(nameof(Spisak));
            }

            var greska = await odgovor.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, greska.Trim('"'));
            model.KljucneTacke = await DohvatiKljucneTackeZaMeni();
            model.Clanovi = await DohvatiClanovZaMeni();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Izmena(int id)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            var zadatak = await _httpClient
                .GetFromJsonAsync<Zadatak>($"{_urlZadatak}/{id}");

            if (zadatak == null) return NotFound();

            var model = MapirajUViewModel(zadatak);
            model.KljucneTacke = await DohvatiKljucneTackeZaMeni();
            model.Clanovi = await DohvatiClanovZaMeni();

            if (!model.Stavke.Any())
                model.Stavke.Add(new StavkaViewModel());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Izmena(int id, ZadatakViewModel model)
        {
            if (!KorisnikPrijavljen())
                return RedirectToAction("Prijava", "Nalog");

            model.Stavke = model.Stavke
                .Where(s => !string.IsNullOrWhiteSpace(s.NazivPodzadatka))
                .ToList();

            if (!ModelState.IsValid)
            {
                model.KljucneTacke = await DohvatiKljucneTackeZaMeni();
                model.Clanovi = await DohvatiClanovZaMeni();
                return View(model);
            }

            var zadatak = MapirajUEntitet(model);
            zadatak.ZadatakID = id;
            var odgovor = await _httpClient
                .PutAsJsonAsync($"{_urlZadatak}/{id}", zadatak);

            if (odgovor.IsSuccessStatusCode)
            {
                TempData["Poruka"] = $"Zadatak '{model.Naziv}' je uspešno izmenjen.";
                return RedirectToAction(nameof(Spisak));
            }

            var greska = await odgovor.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, greska.Trim('"'));
            model.KljucneTacke = await DohvatiKljucneTackeZaMeni();
            model.Clanovi = await DohvatiClanovZaMeni();
            return View(model);
        }

        private static Zadatak MapirajUEntitet(ZadatakViewModel m)
        {
            return new Zadatak
            {
                ZadatakID = m.ZadatakID,
                Sifra = m.Sifra,
                Naziv = m.Naziv,
                Opis = m.Opis,
                TipZadatka = m.TipZadatka,
                Prioritet = m.Prioritet,
                Status = m.Status,
                ProcenatZavrsenosti = m.ProcenatZavrsenosti,
                RokZaZavrsetak = m.RokZaZavrsetak,
                Napomena = m.Napomena,
                KljucnaTackaRazvojaID = m.KljucnaTackaRazvojaID,
                ClanTimaID = m.ClanTimaID,
                DatumKreiranja = m.DatumKreiranja == default
                                        ? DateTime.Now
                                        : m.DatumKreiranja,
                Stavke = m.Stavke.Select((s, i) => new StavkaZadatka
                {
                    StavkaID = s.StavkaID,
                    RedniBroj = i + 1,
                    NazivPodzadatka = s.NazivPodzadatka,
                    Zavrseno = s.Zavrseno
                }).ToList()
            };
        }

        private static ZadatakViewModel MapirajUViewModel(Zadatak z)
        {
            return new ZadatakViewModel
            {
                ZadatakID = z.ZadatakID,
                Sifra = z.Sifra,
                Naziv = z.Naziv,
                Opis = z.Opis,
                TipZadatka = z.TipZadatka,
                Prioritet = z.Prioritet,
                Status = z.Status,
                ProcenatZavrsenosti = z.ProcenatZavrsenosti,
                RokZaZavrsetak = z.RokZaZavrsetak,
                Napomena = z.Napomena,
                KljucnaTackaRazvojaID = z.KljucnaTackaRazvojaID,
                ClanTimaID = z.ClanTimaID,
                DatumKreiranja = z.DatumKreiranja,
                Stavke = z.Stavke.Select(s => new StavkaViewModel
                {
                    StavkaID = s.StavkaID,
                    ZadatakID = s.ZadatakID,
                    RedniBroj = s.RedniBroj,
                    NazivPodzadatka = s.NazivPodzadatka,
                    Zavrseno = s.Zavrseno
                }).ToList()
            };
        }
    }
}