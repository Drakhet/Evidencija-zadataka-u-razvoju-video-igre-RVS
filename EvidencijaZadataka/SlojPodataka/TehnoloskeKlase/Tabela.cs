using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.TehnoloskeKlase
{
    public abstract class Tabela
    {
        protected string _stringKonekcije = Konekcija.NizKonekcije;

        public DataTable IzvrsiUpit(string sql)
        {
            using (SqlConnection veza = new SqlConnection(_stringKonekcije))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, veza);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                return tabela;
            }
        }
    }
}