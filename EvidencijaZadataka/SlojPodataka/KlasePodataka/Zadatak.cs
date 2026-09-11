using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka
{
    [Table("Zadatak")]
    public class Zadatak : OsnovniEntitet
    {
        [Key]
        public int ZadatakID { get; set; }

        [Required]
        [StringLength(20)]
        public string Sifra { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Naziv { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Opis { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string TipZadatka { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Prioritet { get; set; } = "Srednji";

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Otvoren";

        [Required]
        [Range(0, 100)]
        public int ProcenatZavrsenosti { get; set; } = 0;

        [Required]
        public DateTime RokZaZavrsetak { get; set; }

        [StringLength(500)]
        public string Napomena { get; set; } = string.Empty;

        [ForeignKey("KljucnaTackaRazvoja")]
        [Required]
        public int KljucnaTackaRazvojaID { get; set; }
        public KljucnaTackaRazvoja KljucnaTackaRazvoja { get; set; } = null!;

        [ForeignKey("ClanTima")]
        [Required]
        public int ClanTimaID { get; set; }
        public ClanTima ClanTima { get; set; } = null!;

        public ICollection<StavkaZadatka> Stavke { get; set; }
            = new List<StavkaZadatka>();
    }
}