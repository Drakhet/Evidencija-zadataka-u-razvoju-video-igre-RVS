using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka
{
    [Table("StavkaZadatka")]
    public class StavkaZadatka : OsnovniEntitet
    {
        [Key]
        public int StavkaID { get; set; }

        [ForeignKey("Zadatak")]
        [Required]
        public int ZadatakID { get; set; }
        public Zadatak Zadatak { get; set; } = null!;

        [Required]
        [Range(1, 50)]
        public int RedniBroj { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string NazivPodzadatka { get; set; } = string.Empty;

        [Required]
        public bool Zavrseno { get; set; } = false;
    }
}