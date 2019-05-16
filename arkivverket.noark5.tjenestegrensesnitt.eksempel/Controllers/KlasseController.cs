using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using arkivverket.noark5tj.models;
using arkivverket.noark5tj.webapi.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    public class KlasseController : ControllerBase
    {
        [Route("api/arkivstruktur/Klasse")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<ActionResult<KlasseType>> KlasserIndex()
        {
            List<ActionResult<KlasseType>> testdata = new List<ActionResult<KlasseType>>();

            testdata.Add(GetKlasse(Guid.NewGuid().ToString()));
            testdata.Add(GetKlasse(Guid.NewGuid().ToString()));

            return testdata.AsQueryable();
        }

        [Route("api/arkivstruktur/Klasse/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(KlasseType), 200)]
        public ActionResult<KlasseType> GetKlasse(string id)
        {
            KlasseType klasse = new KlasseType();
            klasse.tittel = "test" + id;
            klasse.systemID = id + "_sysId";
            klasse.beskrivelse = "Dette er en beskrivelse av" + id;
            klasse.klasseID = id;

            List<string> noekkelordList = new List<string>();
            string n1 = "en";
            noekkelordList.Add(n1);
            string n2 = "to";
            noekkelordList.Add(n2);
            string n3 = "tre";
            noekkelordList.Add(n3);
            klasse.noekkelord = noekkelordList.ToArray();

            klasse.oppdatertDato = DateTime.Now;
            klasse.oppdatertDatoSpecified = true;
            klasse.oppdatertAv = "Ola";
            klasse.referanseOppdatertAv = "TestReferanseOppdatert";
            klasse.opprettetDato = DateTime.Now;
            klasse.opprettetDatoSpecified = true;
            klasse.opprettetAv = "Per";
            klasse.referanseOpprettetAv = "testReferanseOpprettet";

            return Ok(klasse);
        }

        [Route("api/arkivstruktur/Klasse/{id}")]
        [HttpPost]
        public HttpResponseMessage OppdaterKlasse(string id)
        {
            return null;
        }

        [Route("api/arkivstruktur/ny-klasse")]
        [HttpGet]
        public ActionResult<KlasseType> InitialiserKlasse()
        {
            KlasseType klasse = new KlasseType();
            klasse.tittel = "angi tittel på klassen";
            klasse.beskrivelse = "Angi beskrivelse av klassen";

            return Ok(klasse);
        }

        [Route("api/arkivstruktur/ny-klasse")]
        [HttpPost]
        public ActionResult PostKlasse(KlasseType klasse)
        {
            if (klasse == null)
                return BadRequest();

            klasse.systemID = Guid.NewGuid().ToString();
            klasse.oppdatertDato = DateTime.Now;

            var createdUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/klasse/" + klasse.systemID);
            return Created(createdUri, klasse);
        }
    }
}
