using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using arkivverket.noark5.tjenestegrensesnitt.eksempel.Services;
using arkivverket.noark5tj.models;
using arkivverket.noark5tj.webapi.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    public class ArkivdelController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOptions"></param>
        /// <returns></returns>
        [Route("api/arkivstruktur/Arkivdel")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IEnumerable<ArkivdelType> Index()
        {

            return MockNoarkDatalayer.Arkivdeler.AsQueryable();

        }


        [Route("api/arkivstruktur/Arkivdel/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ArkivdelType), 200)]
        public ActionResult GetArkivdel(string id)
        {

            ArkivdelType arkivdel = MockNoarkDatalayer.Arkivdeler.FirstOrDefault(i => i.systemID == id);

            if (arkivdel == null)
                return NotFound();

            return Ok(arkivdel);
        }


        // NY
        [Route("api/arkivstruktur/Arkivdel/{id}")]
        [HttpPost]
        public ActionResult OppdaterArkivdel(ArkivdelType arkivdel)
        {
            if (arkivdel == null)
                return BadRequest();

            arkivdel.oppdatertDato = DateTime.Now;
            arkivdel.oppdatertDatoSpecified = true;

            return Ok(arkivdel);
        }


        [Route("api/arkivstruktur/ny-arkivdel")]
        [HttpGet]
        public ActionResult<ArkivdelType> InitialiserArkivdel()
        {
            //Legger på standardtekster feks for pålogget bruker
            ArkivdelType arkivdel = new ArkivdelType();
            arkivdel.tittel = "angi tittel på arkiv";
            arkivdel.dokumentmedium = new DokumentmediumType();
            arkivdel.dokumentmedium.kode = "Elektronisk arkiv";
            arkivdel.arkivdelstatus = new ArkivdelstatusType();
            arkivdel.arkivdelstatus.kode = "O";

            // objektet finnes ikke ennå og lenkelista er derfor ikke gyldig
            arkivdel.LinkList.Clear();

            return arkivdel;
        }


        [Route("api/arkivstruktur/ny-arkivdel")]
        [HttpPost]
        public ActionResult NyArkivdel(ArkivdelType arkivdel)
        {
            if (arkivdel == null)
                return BadRequest();

            arkivdel.systemID = Guid.NewGuid().ToString();
            arkivdel.opprettetDato = DateTime.Now;
            arkivdel.opprettetDatoSpecified = true;
            arkivdel.opprettetAv = "pålogget bruker";
            
            MockNoarkDatalayer.Arkivdeler.Add(arkivdel);

            var createdUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Arkivdel/" + arkivdel.systemID);
           
            return Created(createdUri, arkivdel);
        }


        [Route("api/arkivstruktur/Arkiv/{Id}/arkivdel")]
        [HttpGet]
        public IEnumerable<ArkivdelType> GetArkivdelerFraArkiv(string Id)
        {
            IEnumerable<ArkivdelType> arkivdel = MockNoarkDatalayer.Arkivdeler.Where(i => i.arkiv.systemID == Id);
            return arkivdel;
        }


        // NY
        [Route("api/arkivstruktur/Arkiv/{arkivId}/Arkivdel/{ArkivdelId}")]
        [HttpGet]
        public ArkivdelType GetArkivdelIArkiv()
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Arkiv/{arkivId}/Arkivdel/{ArkivdelId}")]
        [HttpPost]
        public HttpResponseMessage OppdaterArkivdelIArkiv()
        {
            return null;
        }

        //NY
        [Route("api/arkivstruktur/Arkiv/{arkivId}/ny-arkivdel")]
        [HttpGet]
        public ArkivdelType InitialiserArkivdelIArkiv()
        {
            return null;
        }

        //NY
        [Route("api/arkivstruktur/Arkiv/{arkivId}/ny-arkivdel")]
        [HttpPost]
        public HttpResponseMessage PostArkivdel()
        {
            return null;
        }

    }
}
