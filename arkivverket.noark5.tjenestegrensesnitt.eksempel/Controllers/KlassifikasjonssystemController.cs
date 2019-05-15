using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using arkivverket.noark5tj.models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    public class KlassifikasjonssystemController : ControllerBase
    {
        [Route("api/arkivstruktur/Klassifikasjonssystem")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<ActionResult<KlassifikasjonssystemType>> GetKlassifikasjonssystemer()
        {
            List<ActionResult<KlassifikasjonssystemType>> klassifikasjonssystemer = new List<ActionResult<KlassifikasjonssystemType>>();

            klassifikasjonssystemer.Add(new KlassifikasjonssystemType() { systemID = Guid.NewGuid().ToString(), tittel = "Ordningsprisnsipp 1" });
            klassifikasjonssystemer.Add(new KlassifikasjonssystemType() { systemID = Guid.NewGuid().ToString(), tittel = "Ordningsprisnsipp 2" });

            return klassifikasjonssystemer.AsQueryable();
        }


        [Route("api/arkivstruktur/Klassifikasjonssystem/{id}")]
        [HttpGet]
        public KlassifikasjonssystemType GetKlassifikasjonssystem(string id)
        {
            return null;
        }


        [Route("api/arkivstruktur/Klassifikasjonssystem/{id}")]
        [HttpPost]
        public KlassifikasjonssystemType OppdaterKlassifikasjonssystem(KlassifikasjonssystemType klassifikasjonssystem)
        {
            return null;
        }


        [Route("api/arkivstruktur/ny-klassifikasjonssystem")]
        [HttpGet]
        public KlassifikasjonssystemType InitialiserKlassifikasjonssystem()
        {
            return null;
        }


        [Route("api/arkivstruktur/ny-klassifikasjonssystem")]
        [HttpPost]
        public HttpResponseMessage PostKlassifikasjonssystem(KlassifikasjonssystemType klassifikasjonssystem)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/klassifikasjonssystem")]
        [HttpGet]
        public IEnumerable<KlassifikasjonssystemType> GetKlassifikasjonssystemerIArkivdel()
        {
            return null;
        }

        [Route("api/arkivstruktur/Arkivdel/{Id}/klassifikasjonssystem/{klassifikasjonssystemId}")]
        [HttpGet]
        public KlassifikasjonssystemType GetKlassifikasjonssystemIArkivdel()
        {
            return null;
        }

        [Route("api/arkivstruktur/Arkivdel/{Id}/klassifikasjonssystem/{klassifikasjonssystemId}")]
        [HttpPost]
        public HttpResponseMessage OppdaterKlassifikasjonssystemIArkivdel(KlassifikasjonssystemType klassifikasjonssystem)
        {
            return null;
        }

        [Route("api/arkivstruktur/Arkivdel/{arkivdelId}/ny-klassifikasjonssystem")]
        [HttpGet]
        public KlassifikasjonssystemType InitialiserKlassifikasjonssystemIArkivdel()
        {
            return null;
        }

        [Route("api/arkivstruktur/Arkivdel/{arkivdelId}/ny-klassifikasjonssystem")]
        [HttpPost]
        public HttpResponseMessage PostKlassifikasjonssystemIArkivdel(KlassifikasjonssystemType klassifikasjonssystem)
        {
            return null;
        }
    }
}
