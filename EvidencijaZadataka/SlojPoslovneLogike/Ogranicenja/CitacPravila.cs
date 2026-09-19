using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlojPoslovneLogike.Ogranicenja
{
    public class CitacPravila
    {
        private readonly string _putanjaXml;

        public CitacPravila(string putanjaXml)
        {
            _putanjaXml = putanjaXml;
        }

        public int DohvatiRokZaPrioritet()
        {
            try
            {
                var xml = XDocument.Load(_putanjaXml);
                var vrednost = xml.Root?.Element("RokZaPrioritet")?.Value;

                if (int.TryParse(vrednost, out int rok) && rok >= 1 && rok <= 30)
                    return rok;

                return 3;
            }
            catch
            {
                return 3;
            }
        }

        public int DohvatiMinimalniProcenatZaRad()
        {
            try
            {
                var xml = XDocument.Load(_putanjaXml);
                var vrednost = xml.Root?.Element("MinimalniProcenatZaRad")?.Value;

                if (int.TryParse(vrednost, out int min))
                    return min;

                return 1;
            }
            catch
            {
                return 1;
            }
        }

        public int DohvatiMaksimalniProcenat()
        {
            try
            {
                var xml = XDocument.Load(_putanjaXml);
                var vrednost = xml.Root?.Element("MaksimalniProcenat")?.Value;

                if (int.TryParse(vrednost, out int max))
                    return max;

                return 100;
            }
            catch
            {
                return 100;
            }
        }
    }
}