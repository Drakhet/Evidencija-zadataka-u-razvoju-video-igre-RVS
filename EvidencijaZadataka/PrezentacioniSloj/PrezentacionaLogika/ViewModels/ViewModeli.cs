using System.ComponentModel.DataAnnotations;
using SlojPodataka.KlasePodataka;

namespace PrezentacioniSloj.PrezentacionaLogika.ViewModels
{
    public class PrijavaViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; } = string.Empty;
    }

    public class RegistracijaViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Korisničko ime mora imati između 3 i 50 karaktera.")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email adresa je obavezna.")]
        [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu.")]
        [Display(Name = "Email adresa")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Lozinka mora imati najmanje 6 karaktera.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [Compare(nameof(Lozinka), ErrorMessage = "Lozinke se ne poklapaju.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrdi lozinku")]
        public string PotvrdaLozinke { get; set; } = string.Empty;
    }
    public class ZadatakViewModel
    {
        public int ZadatakID { get; set; }

        [Required(ErrorMessage = "Naziv zadatka je obavezan.")]
        [StringLength(150, MinimumLength = 5,
            ErrorMessage = "Naziv mora imati između 5 i 150 karaktera.")]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Opis")]
        public string? Opis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tip zadatka je obavezan.")]
        [Display(Name = "Tip zadatka")]
        public string TipZadatka { get; set; } = string.Empty;

        [Display(Name = "Prioritet")]
        public string Prioritet { get; set; } = "Srednji";

        [Display(Name = "Status")]
        public string Status { get; set; } = "Otvoren";

        [Range(0, 100, ErrorMessage = "Procenat mora biti između 0 i 100.")]
        [Display(Name = "Procenat Zavrsenosti (%)")]
        public int ProcenatZavrsenosti { get; set; } = 0;

        [Required(ErrorMessage = "Rok za završetak je obavezan.")]
        [DataType(DataType.Date)]
        [Display(Name = "Rok za završetak")]
        public DateTime RokZaZavrsetak { get; set; } = DateTime.Today.AddDays(7);

        [StringLength(500)]
        [Display(Name = "Napomena")]
        public string? Napomena { get; set; }

        [Required(ErrorMessage = "Ključna tačka razvoja je obavezna.")]
        [Display(Name = "Ključna tačka razvoja")]
        public int KljucnaTackaRazvojaID { get; set; }

        [Required(ErrorMessage = "Zaduženi član tima je obavezan.")]
        [Display(Name = "Zaduženi član tima")]
        public int ClanTimaID { get; set; }

        public string Sifra { get; set; } = string.Empty;
        public DateTime DatumKreiranja { get; set; }

        public List<StavkaViewModel> Stavke { get; set; } = new();

        public List<(int ID, string Naziv)> KljucneTacke { get; set; } = new();
        public List<(int ID, string Naziv)> Clanovi { get; set; } = new();
    }

    public class StavkaViewModel
    {
        public int StavkaID { get; set; }
        public int ZadatakID { get; set; }
        public int RedniBroj { get; set; }

        [Required(ErrorMessage = "Naziv podzadatka je obavezan.")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Naziv mora imati između 3 i 200 karaktera.")]
        [Display(Name = "Naziv podzadatka")]
        public string NazivPodzadatka { get; set; } = string.Empty;

        [Display(Name = "Zavrseno")]
        public bool Zavrseno { get; set; } = false;
    }
}