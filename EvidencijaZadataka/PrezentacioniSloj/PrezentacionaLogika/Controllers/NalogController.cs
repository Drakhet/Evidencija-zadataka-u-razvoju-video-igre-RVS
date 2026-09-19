using Microsoft.AspNetCore.Mvc;
using PrezentacioniSloj.PrezentacionaLogika.ViewModels;
using System.Net.Http.Json;

namespace PrezentacioniSloj.PrezentacionaLogika.Controllers
{
    public class NalogController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string _urlApi = "https://localhost:5072/api/KorisnikRest";

        public NalogController(IHttpClientFactory fabrika)
        {
            _httpClient = fabrika.CreateClient("default");
        }

        [HttpGet]
        public IActionResult Prijava()
        {
            if (HttpContext.Session.GetString("KorisnickoIme") != null)
                return RedirectToAction("Spisak", "Zadatak");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prijava(PrijavaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var odgovor = await _httpClient.PostAsJsonAsync(
                $"{_urlApi}/prijava", new
                {
                    model.KorisnickoIme,
                    model.Lozinka
                });

            if (odgovor.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString(
                    "KorisnickoIme", model.KorisnickoIme);
                return RedirectToAction("Spisak", "Zadatak");
            }

            ModelState.AddModelError(string.Empty,
                "Korisničko ime ili lozinka nisu ispravni.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registracija(RegistracijaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var odgovor = await _httpClient.PostAsJsonAsync(
                $"{_urlApi}/registracija", new
                {
                    model.KorisnickoIme,
                    model.Email,
                    model.Lozinka
                });

            if (odgovor.IsSuccessStatusCode)
            {
                TempData["Poruka"] = "Registracija uspešna. Možete se prijaviti.";
                return RedirectToAction(nameof(Prijava));
            }

            var greska = await odgovor.Content.ReadAsStringAsync();
            var statusKod = odgovor.StatusCode;
            ModelState.AddModelError(string.Empty, $"Greška {statusKod}: {greska}");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Odjava()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Prijava));
        }
    }
}