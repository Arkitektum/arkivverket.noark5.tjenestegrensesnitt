using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using arkivverket.noark5.tjenestegrensesnitt.eksempel.Services;
using arkivverket.noark5tj.models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    public class DokumentbeskrivelseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/arkivstruktur/Dokumentbeskrivelse")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<DokumentbeskrivelseType> DokumentbeskrivelseIndex()
        {
            return MockNoarkDatalayer.Dokumentbeskrivelser.AsQueryable();
        }

        [Route("api/arkivstruktur/Dokumentbeskrivelse/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ArkivType), 200)]
        public ActionResult GetDokumentbeskrivelse(string id)
        {
            DokumentbeskrivelseType dokumentbeskrivelseType = MockNoarkDatalayer.GetDokumentbeskrivelseById(id);

            if (dokumentbeskrivelseType == null)
            {
                return NotFound();
            }
            return Ok(dokumentbeskrivelseType);
        }

        [Route("api/arkivstruktur/Dokumentbeskrivelse/{id}")]
        [HttpPost]
        public HttpResponseMessage OppdaterDokumentbeskrivelse(string id)
        {
            return null;
        }

        [Route("api/arkivstruktur/ny-dokumentbeskrivelse")]
        [HttpGet]
        public DokumentbeskrivelseType InitialiserDokumentbeskrivelse(string id)
        {
            return null;
        }

        [Route("api/arkivstruktur/ny-dokumentbeskrivelse")]
        [HttpPost]
        public HttpResponseMessage PostDokumentbeskrivelse(string id)
        {
            return null;
        }

        
        [Route("api/arkivstruktur/Registrering/{Id}/dokumentbeskrivelse")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<DokumentbeskrivelseType> GetDokumentbeskrivelserFraRegistrering()
        {
            return MockNoarkDatalayer.Dokumentbeskrivelser.AsQueryable();
        }

        
        //NY
        [Route("api/arkivstruktur/Registrering/{id}/ny-dokumentbeskrivelse")]
        [HttpPost]
        public HttpResponseMessage PostReferansefilIRegistrering()
        {
            return null;
        }

        [Route("api/arkivstruktur/Registrering/{Id}/dokumentbeskrivelse/{dokumentbeskrivelseId}")]
        [HttpPost]
        public HttpResponseMessage PostRegistreringAvDokumentbeskrivelse(string Id, DokumentbeskrivelseType dokumentbeskrivelse, string dokumentbeskrivelseId) {
            return null;
        }
    }
}
