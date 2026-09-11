using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka
{
    [Table("ClanTima")]
    public class ClanTima
    {
        [Key]
        public int ClanTimaID { get; set; }

        [Required]
        [StringLength(50)]
        public string Ime { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Prezime { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Uloga { get; set; } = string.Empty;

        public ICollection<Zadatak> Zadaci { get; set; } = new List<Zadatak>();

        [NotMapped]
        public string PunoIme => $"{Ime} {Prezime}";
    }
}