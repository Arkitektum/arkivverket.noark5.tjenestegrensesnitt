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
    public class RegistreringController : ControllerBase
    {
        [Route("api/arkivstruktur/registrering")]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<RegistreringType> RegistreringerIndex()
        {
            return MockNoarkDatalayer.Registreringer.AsQueryable();
        }


        [Route("api/arkivstruktur/ny-registrering")]
        [HttpGet]
        public ActionResult<RegistreringType> InitialiserRegistrering()
        {
            RegistreringType registreringsType = new RegistreringType();
            registreringsType.arkivertDato = DateTime.Now;
            registreringsType.arkivertAv = "Pålogget bruker 2";
            registreringsType.referanseArkivdel = null;

            return Ok(registreringsType);
        }


        [Route("api/arkivstruktur/ny-registrering")]
        [HttpPost]
        public ActionResult PostRegistrering(RegistreringType registrering)
        {
            if (registrering == null)
                return BadRequest();

            registrering.systemID = Guid.NewGuid().ToString();
            registrering.opprettetDato = DateTime.Now.AddDays(-2);
            registrering.opprettetDatoSpecified = true;
            registrering.opprettetAv = "pålogget bruker";

            registrering.RepopulateHyperMedia();

            var createdUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/registrering/" + registrering.systemID);
            return Created(createdUri, registrering);

        }


        [Route("api/arkivstruktur/Registrering/{id}")]
        [HttpGet]
        public ActionResult<RegistreringType> GetRegistrering(string id)
        {
            RegistreringType registreringType = new RegistreringType();
            registreringType.systemID = id;
            registreringType.opprettetDato = DateTime.Now;
            registreringType.opprettetDatoSpecified = true;
            registreringType.oppdatertDato = DateTime.Now;
            registreringType.oppdatertAv = "bruker";

            registreringType.RepopulateHyperMedia();

            return Ok(registreringType);
        }


        [Route("api/arkivstruktur/Registrering/{id}")]
        [HttpPost]
        public ActionResult OppdaterRegistrering(RegistreringType registrering)
        {
            if (registrering == null)
                return BadRequest();

            var createdUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/registrering/" + registrering.systemID);
            return Created(createdUri, registrering);
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/registrering")]
        [HttpGet]
        public IQueryable<ActionResult<RegistreringType>> GetRegistreringerIArkivdel(string Id)
        {
            List<ActionResult<RegistreringType>> testdata = new List<ActionResult<RegistreringType>>();

            testdata.Add(GetRegistrering(Guid.NewGuid().ToString()));

            return testdata.AsQueryable();
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/ny-registrering")]
        [HttpGet]
        public RegistreringType InitialiserRegistreringerIArkivdel(string Id)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/ny-registrering")]
        [HttpPost]
        public HttpResponseMessage PostRegistreringerIArkivdel(RegistreringType registrering)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/registrering/{registreringsId}")]
        [HttpGet]
        public RegistreringType GetRegistreringIArkivdel(string Id, string registreringsId)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/registrering/{registreringsId}")]
        [HttpPost]
        public HttpResponseMessage OppdaterRegistreringIArkivdel(RegistreringType registrering)
        {
            return null;
        }


        [Route("api/arkivstruktur/Mappe/{Id}/registrering")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<ActionResult<RegistreringType>> GetRegistreringerIMappe(string Id)
        {
            List<ActionResult<RegistreringType>> registreringTyper = new List<ActionResult<RegistreringType>>();

            registreringTyper.Add(GetRegistrering(Guid.NewGuid().ToString()));

            return registreringTyper.AsQueryable();
        }


        [Route("api/arkivstruktur/Mappe/{Id}/ny-registrering")]
        [HttpGet]
        public RegistreringType InitialiserRegistreringerIMappe(string Id)
        {
            return null;
        }


        [Route("api/arkivstruktur/Mappe/{Id}/ny-registrering")]
        [HttpPost]
        public HttpResponseMessage PostRegistreringerIMappe(RegistreringType registrering)
        {
            return null;
        }


        [Route("api/arkivstruktur/Mappe/{Id}/registrering/{registreringsId}")]
        [HttpGet]
        public RegistreringType GetRegistreringIMappe(string Id, string registreringsId)
        {
            return null;
        }


        [Route("api/arkivstruktur/Mappe/{Id}/registrering/{registreringsId}")]
        [HttpPost]
        public HttpResponseMessage OppdaterRegistreringIMappe(RegistreringType registrering)
        {
            return null;
        }

    }
}
