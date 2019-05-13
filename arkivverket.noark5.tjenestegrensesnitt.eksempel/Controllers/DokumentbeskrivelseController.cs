using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using arkivverket.noark5tj.models;
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
        /// <param name="queryOptions"></param>
        /// <returns></returns>
        [Route("api/arkivstruktur/Dokumentbeskrivelse")]
        [HttpGet]
        public IEnumerable<DokumentbeskrivelseType> DokumentbeskrivelseIndex(ODataQueryOptions<DokumentbeskrivelseType> queryOptions)
        {
            List<DokumentbeskrivelseType> testdata = new List<DokumentbeskrivelseType>();

            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));

            return testdata.AsEnumerable();
        }

        [Route("api/arkivstruktur/Dokumentbeskrivelse/{id}")]
        [HttpGet]
        public DokumentbeskrivelseType GetDokumentbeskrivelse(string id)
        {
          
            DokumentbeskrivelseType dokumentbeskrivelse = new DokumentbeskrivelseType();
            dokumentbeskrivelse.systemID = id;
            dokumentbeskrivelse.tittel = "angitt tittel " + id;
            dokumentbeskrivelse.beskrivelse = "beskrivelse";
            dokumentbeskrivelse.opprettetDato = DateTime.Now;
            dokumentbeskrivelse.RepopulateHyperMedia();
         
            return dokumentbeskrivelse;
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
        public IEnumerable<DokumentbeskrivelseType> GetDokumentbeskrivelserFraRegistrering(string Id)
        {
            List<DokumentbeskrivelseType> testdata = new List<DokumentbeskrivelseType>();

            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));
            testdata.Add(GetDokumentbeskrivelse(Guid.NewGuid().ToString()));

            return testdata.AsEnumerable();
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
