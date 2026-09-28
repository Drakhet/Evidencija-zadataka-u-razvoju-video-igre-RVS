using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SlojPodataka.TehnoloskeKlase
{
    public class ZadatakSPRepozitorijum
    {
        private readonly EvidencijaDbContext _kontekst;

        public ZadatakSPRepozitorijum(EvidencijaDbContext kontekst)
        {
            _kontekst = kontekst;
        }

        public int DohvatiUkupanBrojZadataka()
        {
            using var veza = new SqlConnection(
                _kontekst.Database.GetConnectionString());
            var komanda = new SqlCommand(
                "sp_DajUkupanBrojZadataka", veza);
            komanda.CommandType = CommandType.StoredProcedure;
            veza.Open();
            var rezultat = komanda.ExecuteScalar();
            return Convert.ToInt32(rezultat);
        }

        public int DohvatiBrojZadatakaPoStatusu(string status)
        {
            using var veza = new SqlConnection(
                _kontekst.Database.GetConnectionString());
            var komanda = new SqlCommand(
                "sp_DajBrojZadatakaPoStatusu", veza);
            komanda.CommandType = CommandType.StoredProcedure;
            komanda.Parameters.AddWithValue("@Status", status);
            veza.Open();
            var rezultat = komanda.ExecuteScalar();
            return Convert.ToInt32(rezultat);
        }
    }
}