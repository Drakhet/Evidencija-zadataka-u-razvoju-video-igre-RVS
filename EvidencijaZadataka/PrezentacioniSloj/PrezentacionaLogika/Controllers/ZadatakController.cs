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
    }
}