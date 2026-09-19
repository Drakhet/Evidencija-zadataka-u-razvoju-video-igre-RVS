using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SlojPodataka.TehnoloskeKlase
{
    public class EtapaRepoDBUtils : Tabela
    {
        public int IzbrojKljucneTacke()
        {
            DataTable tabela = IzvrsiUpit(
                "SELECT COUNT(*) FROM KljucnaTackaRazvoja");
            return (int)tabela.Rows[0][0];
        }

        public List<string> DohvatiNaziveKljucnihTacaka()
        {
            DataTable tabela = IzvrsiUpit(
                "SELECT Naziv FROM KljucnaTackaRazvoja ORDER BY DatumPocetka");

            var rezultat = new List<string>();
            foreach (DataRow red in tabela.Rows)
                rezultat.Add(red[0].ToString()!);

            return rezultat;
        }

        public int IzbrojZadatkePoStatusu(string status)
        {
            DataTable tabela = IzvrsiUpit(
                $"SELECT COUNT(*) FROM Zadatak WHERE Status = '{status}'");
            return (int)tabela.Rows[0][0];
        }
    }
}