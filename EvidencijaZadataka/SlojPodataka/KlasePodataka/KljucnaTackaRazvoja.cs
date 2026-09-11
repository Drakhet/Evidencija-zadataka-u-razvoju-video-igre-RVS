using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka
{
    [Table("KljucnaTackaRazvoja")]
    public class KljucnaTackaRazvoja
    {
        [Key]
        public int KljucnaTackaRazvojaID { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Opis { get; set; } = string.Empty;

        [Required]
        public DateTime DatumPocetka { get; set; }

        [Required]
        public DateTime DatumZavrsetka { get; set; }

        public ICollection<Zadatak> Zadaci { get; set; } = new List<Zadatak>();
    }
}