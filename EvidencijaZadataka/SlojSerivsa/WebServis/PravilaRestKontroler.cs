using Microsoft.AspNetCore.Mvc;
using SlojPoslovneLogike.Ogranicenja;

namespace SlojServisa.WebServis
{
    [ApiController]
    [Route("api/PravilaRest")]
    public class PravilaRestKontroler : ControllerBase
    {
        private readonly CitacPravila _citacPravila;

        public PravilaRestKontroler(CitacPravila citacPravila)
        {
            _citacPravila = citacPravila;
        }

        [HttpGet]
        public ActionResult DohvatiSvePravilа()
        {
            return Ok(new
            {
                RokZaPrioritet = _citacPravila.DohvatiRokZaPrioritet(),
                MinimalniProcenatZaRad = _citacPravila.DohvatiMinimalniProcenatZaRad(),
                MaksimalniProcenat = _citacPravila.DohvatiMaksimalniProcenat()
            });
        }

        [HttpGet("rokzaprioritet")]
        public ActionResult<int> DohvatiRokZaPrioritet()
        {
            return Ok(_citacPravila.DohvatiRokZaPrioritet());
        }
    }
}