using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.TehnoloskeKlase
{
    public static class Konekcija
    {
     public static string NizKonekcije { get; set; } =
     "Server=localhost;Database=EvidencijaZadatakaDB;" +
     "Trusted_Connection=True;TrustServerCertificate=True;";
    }
}