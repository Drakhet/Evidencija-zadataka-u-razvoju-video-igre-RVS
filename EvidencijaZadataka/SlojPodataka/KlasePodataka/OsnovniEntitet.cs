using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.KlasePodataka
{
    public abstract class OsnovniEntitet
    {
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
    }
}