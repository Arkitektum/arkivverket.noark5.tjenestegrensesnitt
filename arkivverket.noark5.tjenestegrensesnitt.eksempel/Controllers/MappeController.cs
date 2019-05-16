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
    public class MappeController : ControllerBase
    {
        [Route("api/arkivstruktur/Mappe")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IQueryable<MappeType> MapperIndex()
        {
            return MockNoarkDatalayer.Mapper.AsQueryable();
        }

        /// <summary>
        /// Returns a single mappe by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/arkivstruktur/Mappe/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(MappeType), 200)]
        public ActionResult<MappeType> GetMappe(string id)
        {
            var mappe = MockNoarkDatalayer.GetMappeById(id);

            if (mappe == null)
                return NotFound();

            return Ok(mappe);
        }

        [Route("api/arkivstruktur/Mappe/{id}")]
        [HttpPut]
        public ActionResult OppdaterMappe(MappeType mappe)
        {
            if (mappe == null)
                return BadRequest();

            mappe.oppdatertAv = "pålogget bruker";
            mappe.oppdatertDato = DateTime.Now;
            mappe.oppdatertDatoSpecified = true;
            mappe.referanseOppdatertAv = Guid.NewGuid().ToString();
            mappe.LinkList.Clear();
            mappe.RepopulateHyperMedia();

            var uri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Mappe/" + mappe.systemID);
            return Ok(uri);

        }

        [Route("api/arkivstruktur/ny-mappe")]
        [HttpGet]
        public MappeType InitialiserMappe()
        {
            return null;
        }

        [Route("api/arkivstruktur/ny-mappe")]
        [HttpPost]
        public HttpResponseMessage PostMappe(MappeType mappe)
        {
            return null;
        }

        [Route("api/arkivstruktur/Mappe/{Id}/avslutt-mappe")]
        [HttpGet]
        public ActionResult<MappeType> AvsluttMappe(string Id)
        {
            MappeType avslutteMappe = MockNoarkDatalayer.GetMappeById(Id);
            avslutteMappe.avsluttetAv = "tor";
            avslutteMappe.avsluttetDatoSpecified = true;
            avslutteMappe.avsluttetDato = DateTime.Now;
            return Ok(avslutteMappe);
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/kryssreferanse")]
        [HttpGet]
        public IEnumerable<MappeType> GetKryssreferanserFraMappe(string id)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/ny-kryssreferanse")]
        [HttpGet]
        public KryssreferanseType InitialiserFraMappeKryssreferanse(string Id)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/ny-kryssreferanse")]
        [HttpPost]
        public HttpResponseMessage PostKryssreferanseFraMappe(KryssreferanseType kryssreferanse)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/undermappe")]
        [HttpGet]
        public IEnumerable<MappeType> GetUndermapper(string Id)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/ny-undermappe")]
        [HttpGet]
        public MappeType InitialiserMappe(string arkivdelid)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/ny-undermappe")]
        [HttpPost]
        public HttpResponseMessage PostUndermapper(MappeType undermappe)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/undermappe/{undermappeId}")]
        [HttpGet]
        public MappeType GetUndermappe(string Id, string undermappeId)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/undermappe/{undermappeId}")]
        [HttpPost]
        public HttpResponseMessage OppdaterUndermappe(MappeType undermappe)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/merknad")]
        [HttpGet]
        public IEnumerable<MerknadType> GetMerknaderIMappe(string id)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/merknad/{merknadId}")]
        [HttpGet]
        public MerknadType GetMerknadIMappe(string id, string merknadId)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/merknad/{merknadId}")]
        [HttpPost]
        public HttpResponseMessage OppdaterMerknadIMappe(MerknadType merknad)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/ny-merknad")]
        [HttpGet]
        public MerknadType InitialiserMerknadIMappe(string Id)
        {
            return null;
        }

        // NY
        [Route("api/arkivstruktur/Mappe/{Id}/ny-merknad")]
        [HttpPost]
        public HttpResponseMessage PostMerknadIMappe(MerknadType merknad)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/mappe")]
        [HttpGet]
        public IQueryable<ActionResult<MappeType>> GetMapperFraArkivdel(string Id)
        {
            List<ActionResult<MappeType>> mappeTyper = new List<ActionResult<MappeType>>();

            mappeTyper.Add(MockNoarkDatalayer.Mapper.First());
            mappeTyper.Add(MockNoarkDatalayer.Saksmapper.First());

            return mappeTyper.AsQueryable();
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/mappe/{mappeId}")]
        [HttpGet]
        public MappeType GetMappeFraArkivdel(string Id, string mappeId)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/mappe/{mappeId}")]
        [HttpPost]
        public MappeType OppdaterMappeFraArkivdel(MappeType mappe)
        {
            return null;
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/ny-mappe")]
        [HttpGet]
        public ActionResult<MappeType> InitialiserMappeIArkivdel(string Id)
        {
            MappeType mappeType = new MappeType();
            mappeType.tittel = "angi tittel på mappe";
            mappeType.dokumentmedium = new DokumentmediumType() { kode = "E", beskrivelse = "Elektronisk arkiv" };
            mappeType.mappetype = new MappetypeType() { kode = "BYGG", beskrivelse = "Byggesak" };

            mappeType.LinkList.Clear();
            mappeType.LinkList.Add(new LinkType("http://rel.arkivverket.no/noark5/v4/api/administrasjon/dokumentmedium", BaseUrlResolver.GetBaseUrl() + "api/kodelister/Dokumentmedium"));
            mappeType.LinkList.Add(new LinkType("http://rel.arkivverket.no/noark5/v4/api/administrasjon/mappetype", BaseUrlResolver.GetBaseUrl() + "api/kodelister/Mappetype"));

            return Ok(mappeType);
        }


        [Route("api/arkivstruktur/Arkivdel/{Id}/ny-mappe")]
        [HttpPost]
        public ActionResult<MappeType> PostMappeIArkivdel(MappeType mappe)
        {
            if (mappe == null)
                return BadRequest();

            mappe.systemID = Guid.NewGuid().ToString();
            mappe.opprettetAv = "pålogget bruker";
            mappe.opprettetDato = DateTime.Now;
            mappe.opprettetDatoSpecified = true;
            mappe.referanseOpprettetAv = Guid.NewGuid().ToString();
            mappe.mappeID = "123456/2016";
            mappe.LinkList.Clear();
            mappe.RepopulateHyperMedia();

            var uriCreated = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Mappe/" + mappe.systemID);
            return Created(uriCreated, mappe);
        }


        [Route("api/arkivstruktur/Klasse/{Id}/mappe")]
        [HttpGet]
        [ListWithLinksResult]
        [EnableQuery()]
        public IEnumerable<ActionResult<MappeType>> GetMapperIKlasse(string Id)
        {
            List<ActionResult<MappeType>> mappeTyper = new List<ActionResult<MappeType>>();

            mappeTyper.Add(MockNoarkDatalayer.Mapper.First());
            mappeTyper.Add(MockNoarkDatalayer.Saksmapper.First());

            return mappeTyper.AsQueryable();
        }


        [Route("api/arkivstruktur/Klasse/{Id}/mappe/{mappeId}")]
        [HttpGet]
        public MappeType GetMappeFraKlasse(string Id, string mappeId)
        {
            return null;
        }


        [Route("api/arkivstruktur/Klasse/{Id}/mappe/{mappeId}")]
        [HttpPost]
        public MappeType OppdaterMappeFraKlasse(MappeType mappe)
        {
            return null;
        }


        [Route("api/arkivstruktur/Klasse/{Id}/ny-mappe")]
        [HttpGet]
        public ActionResult<MappeType> InitialiserMappeIKlasse(string Id)
        {
            MappeType mappeType = new MappeType();
            mappeType.tittel = "angi tittel på mappe";
            mappeType.dokumentmedium = new DokumentmediumType() { kode = "E", beskrivelse = "Elektronisk arkiv" };

            return mappeType;
        }


        [Route("api/arkivstruktur/Klasse/{Id}/ny-mappe")]
        [HttpPost]
        public ActionResult PostMappeIKlasse(MappeType mappe)
        {
            if (mappe == null)
                return BadRequest();

            var createdUri = new Uri(BaseUrlResolver.GetBaseUrl() + "api/arkivstruktur/Arkiv/" + mappe.systemID);
            return Created(createdUri, mappe);

        }

        // GET api/Mappe/ND 234234
        [Route("api/arkivstruktur/Klasse/{Id}/ny-mappe")]
        [HttpGet]
        public IQueryable<ActionResult<MappeType>> GetMappeByTittel(string tittel)
        {
            List<ActionResult<MappeType>> testdata = new List<ActionResult<MappeType>>();

            MappeType mappeType = MockNoarkDatalayer.Mapper.First();
            mappeType.tittel = tittel;
            testdata.Add(mappeType);
            return testdata.AsQueryable();
        }
    }
}
