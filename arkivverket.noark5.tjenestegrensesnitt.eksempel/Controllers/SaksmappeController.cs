using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using arkivverket.noark5.tjenestegrensesnitt.eksempel.Services;
using arkivverket.noark5tj.models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;

namespace arkitektum.kommit.noark5.api.Controllers
{
    /// <summary>
    /// Provides api methods for saksmappe - api/sakarkiv/Saksmappe
    /// </summary>
    public class SaksmappeController : ControllerBase
    {
        /// <summary>
        /// Returns all Saksmappe
        /// </summary>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<SaksmappeType> SaksmapperIndex()
        {
            return MockNoarkDatalayer.Saksmapper.AsQueryable();
        }

        /// <summary>
        /// Expand Mappe to Saksmappe
        /// Required fields:
        /// * saksdato
        /// * saksansvarlig
        /// * saksstatus
        /// 
        /// Implementation of this method should take care of extra fields provided by the client that exists in saksmappe and save them as well.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saksmappe"></param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/utvid-til-saksmappe")]
        [HttpPut]
        [ProducesResponseType(typeof(SaksmappeType), 200)]
        public ActionResult UtvidTilSaksmappe(string id, SaksmappeType saksmappeOppdatert)
        {
            MappeType mappe = MockNoarkDatalayer.GetMappeById(id);

            if (mappe == null)
            {
                return BadRequest("Invalid saksmappe id, saksmappe could not be found");
            }
            if (saksmappeOppdatert.saksdato == DateTime.MinValue)
            {
                return BadRequest("saksdato is required to upgrade mappe to saksmappe.");
            }
            if (string.IsNullOrWhiteSpace(saksmappeOppdatert.saksansvarlig))
            {
                return BadRequest("saksansvarlig is required to upgrade mappe to saksmappe.");
            }
            if (string.IsNullOrWhiteSpace(saksmappeOppdatert.saksstatus?.kode))
            {
                return BadRequest("saksstatus is required to upgrade mappe to saksmappe.");
            }

            var saksmappe = new SaksmappeType();
            saksmappe.saksdato = saksmappeOppdatert.saksdato;
            saksmappe.saksansvarlig = saksmappeOppdatert.saksansvarlig;
            saksmappe.saksstatus = saksmappeOppdatert.saksstatus;

            saksmappe.oppdatertDato = DateTime.Now;
            saksmappe.oppdatertDatoSpecified = true;

            // copy fields from mappe
            saksmappe.tittel = mappe.tittel;
            saksmappe.offentligTittel = mappe.offentligTittel;
            saksmappe.systemID = mappe.systemID;
            saksmappe.opprettetDato = mappe.opprettetDato;
            saksmappe.opprettetDatoSpecified = mappe.opprettetDatoSpecified;
            saksmappe.oppdatertAv = mappe.oppdatertAv;
            saksmappe.mappeID = mappe.mappeID;
            saksmappe.gradering = mappe.gradering;
            saksmappe.klasse = mappe.klasse;
            saksmappe.merknad = mappe.merknad;

            saksmappe.RepopulateHyperMedia();

            MockNoarkDatalayer.Saksmapper.RemoveAll(x => x.systemID == id);
            MockNoarkDatalayer.Saksmapper.Add(saksmappe);

            return Ok(saksmappe);
        }

        /// <summary>
        /// Return a single saksmappe
        /// </summary>
        /// <param name="id">Saksmappe id</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(SaksmappeType), 200)]
        public ActionResult GetSaksmappe(string id)
        {
            var saksmappe = MockNoarkDatalayer.GetSaksmappeById(id);

            if (saksmappe == null)
            {
                return NotFound();
            }

            return Ok(saksmappe);
        }


        /// <summary>
        /// Returns all klasse coupled to the given saksmappe
        /// </summary>
        /// <param name="queryOptions"></param>
        /// <param name="id">Saksmappe Id</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/sekundaerklassifikasjoner")]
        [HttpGet]
        public ActionResult<SaksmappeType> GetSekundaerklassifikasjoner(ODataQueryOptions<KlasseType> queryOptions, string id)
        {
            var sekundaerklassifikasjoner = MockNoarkDatalayer.GetSekundaerklassifikasjonerBySaksmappeId(id);

            return Ok(sekundaerklassifikasjoner);
        }



        /// <summary>
        /// Sletter sekundærklassifikasjon
        /// </summary>
        /// <param name="id">Saksmappe Id</param>
        /// <param name="systemId">Systemid til en sekundærklassifikasjon</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/sekundaerklassifikasjon/{systemId}")]
        [HttpDelete]
        public ActionResult SlettSekundaerklassifikasjon(string id, string systemId)
        {
            if (id == null)
                return BadRequest();

            MockNoarkDatalayer.DeleteSekundaerklassifikasjonFromSaksmappe(id, systemId);

            return Ok();
        }

        /// <summary>
        /// Sletter sekundærklassifikasjoner
        /// </summary>
        /// <param name="id">Saksmappe Id</param>
        /// <param name="klasseTyper">En tabell med sekundærklassifikasjoner</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/sekundaerklassifikasjoner")]
        [HttpDelete]
        public ActionResult SlettSekundaerklassifikasjoner(string id, KlasseType[] klasseTyper)
        {
            if (id == null)
                return BadRequest();

            if (klasseTyper == null) klasseTyper = CreateNewKlasseTypeArray();
            
            MockNoarkDatalayer.DeleteSekundaerklassifikasjonFromSaksmappe(id, klasseTyper);

            return Ok();
        }


        /// <summary>
        /// Legg til sekundærklassifikasjon
        /// </summary>
        /// <param name="id">Saksmappe Id</param>
        /// <param name="klasseType">Klassetype</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/sekundaerklassifikasjoner")]
        [HttpPost]
        [ProducesResponseType(typeof(SaksmappeType), 200)]
        public ActionResult NyeSekundaerklassifikasjoner(string id, KlasseType klasseType)
        {
            if (klasseType == null) klasseType = CreateKlasseTypeExample();

            if (id == null) return BadRequest("Invalid saksmappe id, saksmappe could not be found");

            MockNoarkDatalayer.AddSekundaerklassifikasjonToSaksmappe(id, klasseType);

            return Ok(klasseType);
        }


        /// <summary>
        /// Erstatte sekundærklassifikasjoner
        /// </summary>
        /// <param name="id">Saksmappe Id</param>
        /// <param name="klasseType">En tabell med klassetyper</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/sekundaerklassifikasjoner")]
        [HttpPut]
        [ProducesResponseType(typeof(SaksmappeType), 200)]
        public ActionResult SettSekundaerklassifikasjon(string id, KlasseType[] klasseType)
        {
            if (klasseType == null) klasseType = CreateNewKlasseTypeArray();
            
            if (id == null) return BadRequest("Invalid saksmappe id, saksmappe could not be found");

            MockNoarkDatalayer.SetSekundaerklassifikasjonerToSaksmappe(id, klasseType);

            return Ok(klasseType);
        }


        /// <summary>
        /// Opprette ny sekundærklasse
        /// </summary>
        /// <param name="id">Saksmappe id</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("api/sakarkiv/Saksmappe/{id}/ny-sekundaerklassifikasjon")]
        [HttpGet]
        public ActionResult<KlasseType> InitialiserSekundaerklasse(string id)
        {
            var klasseType = new KlasseType();
            klasseType.tittel = "angi tittel på mappe";
            klasseType.opprettetAv = "Innlogget bruker";
            klasseType.RepopulateHyperMedia();

            return Ok(klasseType);
        }

        /// <summary>
        /// Legg til ny sekundærklassifikasjon i saksmappen
        /// </summary>
        /// <param name="id">Saksmappe id</param>
        /// <param name="klasseType">klassetype</param>
        /// <returns></returns>
        [Route("api/sakarkiv/Saksmappe/{id}/ny-sekundaerklassifikasjon")]
        [HttpPost]
        public ActionResult<KlasseType> InitialiserSekundaerklasse(string id, KlasseType klasseType)
        {
            if (klasseType == null) klasseType = CreateKlasseTypeExample();
            
            if (id == null) return BadRequest("Invalid saksmappe id, saksmappe could not be found");

            MockNoarkDatalayer.AddSekundaerklassifikasjonToSaksmappe(id, klasseType);

            return Ok(klasseType);
        }



        private KlasseType[] CreateNewKlasseTypeArray()
        {
            var klasseTypeArray = new KlasseType[3];
            klasseTypeArray[0] = CreateKlasseTypeExample(1);
            klasseTypeArray[1] = CreateKlasseTypeExample(2);
            klasseTypeArray[2] = CreateKlasseTypeExample(3);

            return klasseTypeArray;
        }


        private KlasseType CreateKlasseTypeExample(int i = 1)
        {
            return new KlasseType()
            {
                tittel = "Tittel " + i,
                systemID = "syst_" + i,
                beskrivelse = "Dette er en beskrivelse av " + i,
                klasseID = "KlasseId" + i,
                oppdatertDato = DateTime.Now,
                oppdatertDatoSpecified = true,
                oppdatertAv = "Test navn" + i,
                referanseOppdatertAv = "",
                opprettetDato = DateTime.Now,
                opprettetDatoSpecified = true,
                opprettetAv = "Test navn" + i,
                referanseOpprettetAv = "Test navn" + i,
            };
        }
    }
}
