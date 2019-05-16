using System;
using System.Collections.Generic;
using System.Linq;
using arkivverket.noark5.tjenestegrensesnitt.eksempel.Services;
using arkivverket.noark5tj.models;
using arkivverket.noark5tj.webapi.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    /// <summary>
    /// rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv og rel.arkivverket.no/noark5/v4/arkivstruktur/ny-arkiv
    /// </summary>
    public class ArkivController : ControllerBase
    {
        
        /// <summary>
        /// Henter tilgjengelige arkiv
        /// </summary>
        /// <returns>en liste med arkiv</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        /// <remarks>relasjonsnøkkel <a href="https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/">https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/</a>,
        /// og dokumentasjon av <a href="http://arkivverket.metakat.no/Objekttype/Index/EAID_C24AA8BC_2F54_4277_AA3E_54644165DBD6">datamodell, restriksjoner og mulige relasjonsnøkler</a></remarks>
        [Route("api/arkivstruktur/arkiv")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<ArkivType> ArkivIndex()
        {
            return MockNoarkDatalayer.Arkiver.AsQueryable();
        }

        /// <summary>
        /// Henter et arkiv med gitt id
        /// </summary>
        /// <param name="id">systemid for arkiv</param>
        /// <returns>et arkiv eller 404 hvis det ikke finnes</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        /// <remarks>relasjonsnøkkel <a href="https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/">https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/</a>,
        /// og dokumentasjon av <a href="http://arkivverket.metakat.no/Objekttype/Index/EAID_C24AA8BC_2F54_4277_AA3E_54644165DBD6">datamodell,
        /// restriksjoner og mulige relasjonsnøkler</a></remarks>
        [Route("api/arkivstruktur/Arkiv/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ArkivType), 200)]
        public ActionResult<ArkivType> GetArkiv(string id)
        {
            ArkivType arkiv = MockNoarkDatalayer.GetArkivById(id);

            if (arkiv == null)
            {
                return NotFound();
            }
            return Ok(arkiv);
        }

        /// <summary>
        /// Oppdaterer arkiv
        /// </summary>
        /// <param name="id"></param>
        /// <param name="arkiv"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="409">Conflict - objektet kan være endret av andre</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        /// <remarks>relasjonsnøkkel <a href="https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/">https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/</a>,
        /// og dokumentasjon av <a href="http://arkivverket.metakat.no/Objekttype/Index/EAID_C24AA8BC_2F54_4277_AA3E_54644165DBD6">datamodell, restriksjoner og mulige relasjonsnøkler</a></remarks>
        [Route("api/arkivstruktur/Arkiv/{id}")]
        [HttpPut]
        public ActionResult OppdaterArkiv(string id, ArkivType arkiv)
        {
            if (arkiv == null)
                return BadRequest();

            var url = BaseUrlResolver.GetBaseUrl();

            ArkivType existing = MockNoarkDatalayer.Arkiver.FirstOrDefault(i => i.systemID == arkiv.systemID);

            if (existing == null)
                return NotFound();

            // run update on object
            existing.oppdatertDato = DateTime.Now;
            existing.oppdatertDatoSpecified = true;
            existing.oppdatertAv = "bruker";
            existing.referanseOppdatertAv = Guid.NewGuid().ToString();
            existing.tittel = arkiv.tittel;
            existing.beskrivelse = arkiv.beskrivelse;
            

            var createdUri = new Uri(url + "api/arkivstruktur/Arkiv/" + existing.systemID);
            return Created(createdUri, existing);
        }

        /// <summary>
        /// Sletter arkiv
        /// </summary>
        /// <param name="id">systemid for gitt arkiv</param>
        /// <returns>statuskode</returns>
        /// <response code="204">NoContent - Slettet ok</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="409">Conflict - objektet kan være endret av andre</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        /// <remarks>relasjonsnøkkel <a href="https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/">https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/</a>,
        /// og dokumentasjon av <a href="http://arkivverket.metakat.no/Objekttype/Index/EAID_C24AA8BC_2F54_4277_AA3E_54644165DBD6">datamodell,
        /// restriksjoner og mulige relasjonsnøkler</a></remarks>
        [Route("api/arkivstruktur/Arkiv/{id}")]
        [HttpDelete]
        public ActionResult SlettArkiv(string id)
        {
            if (id == null)
                return BadRequest();

            //Kan slettes? Har rettighet? Logges mm..
            //sjekke etag om objektet er endret av andre?
            ArkivType m = MockNoarkDatalayer.Arkiver.FirstOrDefault(i => i.systemID == id);

            if (m == null)
                return NotFound();

            // remove object
            MockNoarkDatalayer.Arkiver.Remove(m);

            return NoContent();
        }



        /// <summary>
        /// Preutfylling av et nytt arkiv
        /// </summary>
        /// <returns>et arkiv</returns>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        /// <remarks>relasjonsnøkkel <a href="https://rel.arkivverket.no/noark5/v4/arkivstruktur/ny-arkiv/">https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/</a>,
        /// og dokumentasjon av <a href="http://arkivverket.metakat.no/Objekttype/Index/EAID_C24AA8BC_2F54_4277_AA3E_54644165DBD6">datamodell,
        /// restriksjoner og mulige relasjonsnøkler</a></remarks>
        [Route("api/arkivstruktur/nytt-arkiv")]
        [HttpGet]
        public ArkivType InitialiserArkiv()
        {
            // Legger på standardtekster feks for pålogget bruker

            ArkivType arkiv = new ArkivType();
            arkiv.tittel = "angi tittel på arkiv";
            arkiv.dokumentmedium = new DokumentmediumType();
            arkiv.dokumentmedium.kode = "E";
            arkiv.arkivstatus = new ArkivstatusType();
            arkiv.arkivstatus.kode = "O";
            
            // objektet finnes ikke ennå og lenkelista er derfor ikke gyldig
            arkiv.LinkList.Clear();

            return arkiv;
        }

        /// <summary>
        /// Oppretter et nytt arkiv
        /// </summary>
        /// <returns>url til nytt arkiv i location header</returns>
        /// <response code="200">ok</response>
        /// <response code="201">Created - opprettet</response>
        /// <response code="400">BadRequest - ugyldig forespørsel</response>
        /// <response code="403">Forbidden - ingen tilgang</response>
        /// <response code="404">NotFound - ikke funnet</response>
        /// <response code="409">Conflict - objektet kan være endret av andre</response>
        /// <response code="501">NotImplemented - ikke implementert</response>
        /// <remarks>relasjonsnøkkel <a href="https://rel.arkivverket.no/noark5/v4/arkivstruktur/ny-arkiv/">https://rel.arkivverket.no/noark5/v4/arkivstruktur/arkiv/</a>,
        /// og dokumentasjon av <a href="http://arkivverket.metakat.no/Objekttype/Index/EAID_C24AA8BC_2F54_4277_AA3E_54644165DBD6">datamodell,
        /// restriksjoner og mulige relasjonsnøkler</a></remarks>
        [Route("api/arkivstruktur/nytt-arkiv")]
        [HttpPost]
        [ProducesResponseType(typeof(ArkivType), 200)]
        public ActionResult NyttArkiv(ArkivType arkiv)
        {
            if (arkiv == null)
                return BadRequest();

            arkiv.systemID = Guid.NewGuid().ToString();
            arkiv.opprettetDato = DateTime.Now;
            arkiv.opprettetDatoSpecified = true;
            arkiv.opprettetAv = "pålogget bruker";

            MockNoarkDatalayer.Arkiver.Add(arkiv);

            var createdUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Arkiv/" + arkiv.systemID);

            return Created(createdUri, arkiv);
        }


        // ****    ARKIVSKAPER

        /// <summary>
        /// Henter arkivskapere innenfor arkiv
        /// </summary>
        /// <param name="arkivId"></param>
        /// <returns></returns>
        [Route("api/arkivstruktur/Arkiv/{arkivId}/arkivskaper")]
        [HttpGet]
        public ActionResult<IQueryable<ArkivskaperType>> GetArkivskapereIArkiv(string arkivId)
        {
            List<ArkivskaperType> list = new List<ArkivskaperType>();

            ArkivType arkiv = MockNoarkDatalayer.GetArkivById(arkivId);

            if (arkiv == null)
                return NotFound();

            foreach (var arkivskaper in arkiv.arkivskaper)
            {
                list.Add(arkivskaper);
            }

           return Ok(list.AsQueryable());
        }

        /// <summary>
        /// Henter arkivskaper
        /// </summary>
        /// <param name="arkivskaperId"></param>
        /// <returns></returns>
        [Route("api/arkivstruktur/arkivskaper/{arkivskaperId}")]
        [HttpGet]
        public ActionResult GetArkivskaper(string arkivskaperId)
        {
            ArkivskaperType arkivskaper = MockNoarkDatalayer.GetArkivskaperById(arkivskaperId);
            if (arkivskaper == null)
                return NotFound();

            return Ok(arkivskaper);
        }

        /// <summary>
        /// Preutfylling av arkivskaper
        /// </summary>
        /// <returns></returns>
        [Route("api/arkivstruktur/ny-arkivskaper")]
        [HttpGet]
        public ActionResult InitialiserArkivskaper()
        {
            ArkivskaperType arkivskaper = new ArkivskaperType();
            arkivskaper.arkivskaperNavn = "angi navn på arkivskaper";
            arkivskaper.beskrivelse = "angi beskrivelse";
           
            return Ok(arkivskaper);
        }
        
        /// <summary>
        /// Opprett ny arkivskaper
        /// </summary>
        /// <param name="arkivskaper"></param>
        /// <returns></returns>
        [Route("api/arkivstruktur/ny-arkivskaper")]
        [HttpPost]
        public ActionResult PostArkivskaper(ArkivskaperType arkivskaper)
        {
            if (arkivskaper == null)
                return BadRequest();

            arkivskaper.systemID = Guid.NewGuid().ToString();
            arkivskaper.opprettetAv = "pålogget bruker";
            arkivskaper.opprettetDato = DateTime.Now;
            arkivskaper.opprettetDatoSpecified = true;

            MockNoarkDatalayer.Arkivskaper.Add(arkivskaper);

            arkivskaper.RepopulateHyperMedia();

            var createUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Arkivdel/" + arkivskaper.systemID);

            return Created(createUri, arkivskaper);
        }
    }
}
